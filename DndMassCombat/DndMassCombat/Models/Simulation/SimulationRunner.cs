using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var sb = new StringBuilder();
            
            if (simulationViewModel.UnitDescription1.IsAttacking == true)
            {
                Simulate(simulationViewModel.UnitDescription1,
                    simulationViewModel.Group1,
                    simulationViewModel.UnitDescription2,
                    simulationViewModel.Group2,
                    sb);
            }
            else
            {
                Simulate(simulationViewModel.UnitDescription2,
                    simulationViewModel.Group2,
                    simulationViewModel.UnitDescription1,
                    simulationViewModel.Group1,
                    sb);
            }

            sb.AppendLine("----------------");
            simulationViewModel.Description += sb.ToString();
        }

        private void Simulate(UnitDescriptionViewModel attackingUnit, GroupViewModel attackingGroup,
            UnitDescriptionViewModel defendingUnit, GroupViewModel defendingGroup, StringBuilder description)
        {
            if (defendingGroup.UnitCount == 0)
                return;
            
            var defendingUnitHitPointList = string.IsNullOrEmpty(defendingGroup.UnitsHpJson) 
                ? new List<int>() 
                : JsonSerializer.Deserialize<List<int>>(defendingGroup.UnitsHpJson);

            UpdateDefendingListHitPoint(defendingUnit.HitPoint, defendingGroup.UnitCount, defendingUnitHitPointList);

            var attackResult = Attack(attackingUnit.HitBonus,
                attackingUnit.DamageDice,
                attackingUnit.DamageBonus,
                attackingGroup.UnitCount,
                defendingUnit.ArmorClass,
                defendingUnitHitPointList);

            var totalDamageKilled = attackResult.TotalUnitKilled * defendingUnit.HitPoint;
            var totalDamageWounded = defendingUnitHitPointList.Sum(hp => defendingUnit.HitPoint - hp);
            var totalDamage = totalDamageKilled + totalDamageWounded;
 
            defendingGroup.HitPoint -= totalDamage;
            defendingGroup.UnitCount -= attackResult.TotalUnitKilled;
            defendingGroup.UnitsHpJson = JsonSerializer.Serialize(defendingUnitHitPointList);

            description.AppendLine($"{attackingUnit.Name} attacks {defendingUnit.Name}");
            if (totalDamage == 0)
            {
                description.AppendLine("All missed!");
            }
            else
            {
                description.AppendLine($"Total damage: {totalDamage}");
                description.AppendLine($"Total unit killed: {attackResult.TotalUnitKilled}");
            }
        }

        private AttackResult Attack(
            int attackingUnitHitBonus,
            Dice attackingUnitDamageDice,
            int attackingUnitDamageBonus,
            int attackingGroupUnitCount,
            int defendingUnitArmorClass,
            List<int> defendingUnitHitPointList)
        {
            int totalKilledCount = 0;

            for (int i = 0; i < attackingGroupUnitCount; i++)
            {
                if (defendingUnitHitPointList.Count == 0)
                    break;
                
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

                    ApplyDamage(defendingUnitHitPointList, damage, targetIndex, ref totalKilledCount);
                }
                else
                {
                    var hitScore = hitRoll + attackingUnitHitBonus;
                    if (hitScore >= defendingUnitArmorClass)
                    {
                        var damageRoll = _diceRoller.Roll(attackingUnitDamageDice);
                        var damageScore = damageRoll + attackingUnitDamageBonus;

                        ApplyDamage(defendingUnitHitPointList, damageScore, targetIndex, ref totalKilledCount);
                    }
                }
            }

            return new AttackResult
            {
                TotalUnitKilled = totalKilledCount,
            };
        }

        private static void ApplyDamage(List<int> defendingUnitHitPointList, int damageScore, int targetIndex, ref int totalKilledCount)
        {
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
            if (defendingUnitHitPointList.Count > defendingGroupUnitCount)
            {
                var diff = defendingUnitHitPointList.Count - defendingGroupUnitCount;
                defendingUnitHitPointList.RemoveRange(defendingUnitHitPointList.Count - diff, diff);
            }
        }
    }
}