using System.Collections.Generic;
using System.Text.Json;
using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public class SimulationRunner : ISimulationRunner
    {
        private readonly IDiceRoller _diceRoller;

        public SimulationRunner(IDiceRoller diceRoller)
        {
            _diceRoller = diceRoller;
        }

        public void Simulate(SimulationViewModel simulationViewModel)
        {
            if (simulationViewModel.UnitDescription1.IsAttacking == true)
                Simulate(simulationViewModel.UnitDescription1,
                    simulationViewModel.Group1,
                    simulationViewModel.UnitDescription2,
                    simulationViewModel.Group2);
            else
                Simulate(simulationViewModel.UnitDescription2,
                    simulationViewModel.Group2,
                    simulationViewModel.UnitDescription1,
                    simulationViewModel.Group1);
        }

        private void Simulate(UnitDescriptionViewModel attackingUnit, GroupViewModel attackingGroup,
            UnitDescriptionViewModel defendingUnit, GroupViewModel defendingGroup)
        {
            var defendingUnitHitPointList = string.IsNullOrEmpty(defendingGroup.UnitsHpJson) 
                ? new List<int>() 
                : JsonSerializer.Deserialize<List<int>>(defendingGroup.UnitsHpJson);

            UpdateDefendingListHitPoint(defendingUnit.HitPoint, defendingGroup.UnitCount, defendingUnitHitPointList);

            var (totalDamage, totalKilledCount) = Attack(attackingUnit.HitBonus,
                attackingUnit.DamageDice,
                attackingUnit.DamageBonus,
                attackingGroup.UnitCount,
                defendingUnit.ArmorClass,
                defendingUnitHitPointList);

            defendingGroup.HitPoint -= totalDamage;
            defendingGroup.UnitCount -= totalKilledCount;
            defendingGroup.UnitsHpJson = JsonSerializer.Serialize(defendingUnitHitPointList);
        }

        private (int totalDamage, int unitKilledCount) Attack(
            int attackingUnitHitBonus,
            Dice attackingUnitDamageDice,
            int attackingUnitDamageBonus,
            int attackingGroupUnitCount,
            int defendingUnitArmorClass,
            List<int> defendingUnitHitPointList)
        {
            int totalDamage = 0;
            int totalKilledCount = 0;

            for (int i = 0; i < attackingGroupUnitCount; i++)
            {
                var defendingGroupUnitCount = defendingUnitHitPointList.Count;
                var targetIndex = i % defendingGroupUnitCount;

                // Roll attack damage
                var hitRoll = _diceRoller.Roll(Dice.D20);

                // Fumble
                if (hitRoll == 1)
                    continue;

                // Critic!
                if (hitRoll == 20)
                {
                    var damage = (int) attackingUnitDamageDice + attackingUnitDamageBonus;
                    int damageRoll;
                    do
                    {
                        damageRoll = _diceRoller.Roll(attackingUnitDamageDice);
                        damage += damageRoll + attackingUnitDamageBonus;
                    } while (damageRoll == (int) attackingUnitDamageDice);

                    ApplyDamage(defendingUnitHitPointList, damage, targetIndex, ref totalDamage, ref totalKilledCount);
                }
                else
                {
                    var hitScore = hitRoll + attackingUnitHitBonus;
                    if (hitScore >= defendingUnitArmorClass)
                    {
                        var damageRoll = _diceRoller.Roll(attackingUnitDamageDice);
                        var damageScore = damageRoll + attackingUnitDamageBonus;

                        ApplyDamage(defendingUnitHitPointList, damageScore, targetIndex, ref totalDamage, ref totalKilledCount);
                    }
                }
            }

            return (totalDamage, totalKilledCount);
        }

        private static void ApplyDamage(List<int> defendingUnitHitPointList,
            int damageScore,
            int targetIndex,
            ref int totalDamage,
            ref int totalKilledCount)
        {
            totalDamage += damageScore;
            defendingUnitHitPointList[targetIndex] -= damageScore;
            if (defendingUnitHitPointList[targetIndex] <= 0)
            {
                defendingUnitHitPointList.RemoveAt(targetIndex);
                totalKilledCount++;
            }
        }

        private static void UpdateDefendingListHitPoint(int defendingUnitHitPoint, int defendingGroupUnitCount, List<int> defendingUnitHitPointList)
        {
            // Add new units
            for (int i = defendingUnitHitPointList.Count; i < defendingGroupUnitCount; i++)
            {
                defendingUnitHitPointList.Add(defendingUnitHitPoint);
            }

            // Remove old units
            for (int i = 0; i < defendingUnitHitPointList.Count - defendingGroupUnitCount; i++)
            {
                defendingUnitHitPointList.RemoveAt(defendingUnitHitPointList.Count - 1);
            }
        }
    }
}