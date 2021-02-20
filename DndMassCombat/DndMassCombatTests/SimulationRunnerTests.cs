using DndMassCombat.Models.Simulation;
using DndMassCombat.Models.ViewModels;
using Moq;
using NUnit.Framework;

namespace DndMassCombatTests
{
    public class Tests
    {
        private SimulationRunner _simulationRunner;
        private Mock<IDiceRoller> _diceRoller;
        private SimulationViewModel _simulationModel;

        #region fightvalues

        private const int _unitCount1 = 20;
        private const string _name1 = "HobGob1";
        private const int _armorClass1 = 17;
        private const int _damageBonus1 = 1;
        private const Dice _damageDice1 = Dice.D6;
        private const int _hitBonus1 = 2;
        private const int _hitPoint1 = 9;

        private const int _unitCount2 = 20;
        private const string _name2 = "HobGob2";
        private const int _armorClass2 = 16;
        private const int _damageBonus2 = 2;
        private const Dice _damageDice2 = Dice.D8;
        private const int _hitBonus2 = 3;
        private const int _hitPoint2 = 7;

        #endregion

        [SetUp]
        public void Setup()
        {
            _diceRoller = new Mock<IDiceRoller>();

            _simulationModel = new SimulationViewModel()
            {
                Group1 = new GroupViewModel()
                {
                    HitPoint = _unitCount1 * _hitPoint1,
                    UnitCount = _unitCount1
                },
                Group2 = new GroupViewModel()
                {
                    HitPoint = _unitCount2 * _hitPoint2,
                    UnitCount = _unitCount2
                },
                UnitDescription1 = new UnitDescriptionViewModel()
                {
                    Name = _name1,
                    ArmorClass = _armorClass1,
                    DamageBonus = _damageBonus1,
                    DamageDice = _damageDice1,
                    HitBonus = _hitBonus1,
                    HitPoint = _hitPoint1
                },
                UnitDescription2 = new UnitDescriptionViewModel()
                {
                    Name = _name2,
                    ArmorClass = _armorClass2,
                    DamageBonus = _damageBonus2,
                    DamageDice = _damageDice2,
                    HitBonus = _hitBonus2,
                    HitPoint = _hitPoint2
                },
            };

            _simulationRunner = new SimulationRunner(_diceRoller.Object);
        }

        [Test]
        public void Test_Simulate_Attack_From_First_Group_No_Deaths()
        {
            _diceRoller.Setup(x => x.Roll(_damageDice1)).Returns(4);
            _diceRoller.Setup(x => x.Roll(Dice.D20)).Returns(18);
            _simulationModel.UnitDescription1.IsAttacking = true;

            _simulationRunner.Simulate(_simulationModel);

            Assert.AreEqual(_unitCount2 * _hitPoint2 - _unitCount1 * (4 + _damageBonus1), _simulationModel.Group2.HitPoint);
            Assert.AreEqual(_unitCount2, _simulationModel.Group2.UnitCount);
            Assert.AreEqual(_name2, _simulationModel.UnitDescription2.Name);
            Assert.AreEqual(_armorClass2, _simulationModel.UnitDescription2.ArmorClass);
            Assert.AreEqual(_damageBonus2, _simulationModel.UnitDescription2.DamageBonus);
            Assert.AreEqual(_damageDice2, _simulationModel.UnitDescription2.DamageDice);
            Assert.AreEqual(_hitBonus2, _simulationModel.UnitDescription2.HitBonus);
            Assert.AreEqual(_hitPoint2, _simulationModel.UnitDescription2.HitPoint);
            Assert.AreEqual(null, _simulationModel.UnitDescription2.IsAttacking);

            Assert.AreEqual(_unitCount1 * _hitPoint1, _simulationModel.Group1.HitPoint);
            Assert.AreEqual(_unitCount1, _simulationModel.Group1.UnitCount);
            Assert.AreEqual(_name1, _simulationModel.UnitDescription1.Name);
            Assert.AreEqual(_armorClass1, _simulationModel.UnitDescription1.ArmorClass);
            Assert.AreEqual(_damageBonus1, _simulationModel.UnitDescription1.DamageBonus);
            Assert.AreEqual(_damageDice1, _simulationModel.UnitDescription1.DamageDice);
            Assert.AreEqual(_hitBonus1, _simulationModel.UnitDescription1.HitBonus);
            Assert.AreEqual(_hitPoint1, _simulationModel.UnitDescription1.HitPoint);
            Assert.AreEqual(true, _simulationModel.UnitDescription1.IsAttacking);
        }

        [Test]
        public void Test_Simulate_Attack_From_Second_Group_No_Deaths()
        {
            _diceRoller.Setup(x => x.Roll(_damageDice2)).Returns(5);
            _diceRoller.Setup(x => x.Roll(Dice.D20)).Returns(18);
            _simulationModel.UnitDescription2.IsAttacking = true;

            _simulationRunner.Simulate(_simulationModel);

            Assert.AreEqual(_unitCount2 * _hitPoint2, _simulationModel.Group2.HitPoint);
            Assert.AreEqual(_unitCount2, _simulationModel.Group2.UnitCount);
            Assert.AreEqual(_name2, _simulationModel.UnitDescription2.Name);
            Assert.AreEqual(_armorClass2, _simulationModel.UnitDescription2.ArmorClass);
            Assert.AreEqual(_damageBonus2, _simulationModel.UnitDescription2.DamageBonus);
            Assert.AreEqual(_damageDice2, _simulationModel.UnitDescription2.DamageDice);
            Assert.AreEqual(_hitBonus2, _simulationModel.UnitDescription2.HitBonus);
            Assert.AreEqual(_hitPoint2, _simulationModel.UnitDescription2.HitPoint);
            Assert.AreEqual(true, _simulationModel.UnitDescription2.IsAttacking);

            Assert.AreEqual(_unitCount1 * _hitPoint1 - _unitCount2 * (5 + _damageBonus2), _simulationModel.Group1.HitPoint);
            Assert.AreEqual(_unitCount1, _simulationModel.Group1.UnitCount);
            Assert.AreEqual(_name1, _simulationModel.UnitDescription1.Name);
            Assert.AreEqual(_armorClass1, _simulationModel.UnitDescription1.ArmorClass);
            Assert.AreEqual(_damageBonus1, _simulationModel.UnitDescription1.DamageBonus);
            Assert.AreEqual(_damageDice1, _simulationModel.UnitDescription1.DamageDice);
            Assert.AreEqual(_hitBonus1, _simulationModel.UnitDescription1.HitBonus);
            Assert.AreEqual(_hitPoint1, _simulationModel.UnitDescription1.HitPoint);
            Assert.AreEqual(null, _simulationModel.UnitDescription1.IsAttacking);
        }

        [Test]
        public void Test_Simulate_Attack_From_First_Group_With_Deaths()
        {
            // Only one unit will be killed
            int count = 0;
            _diceRoller.Setup(x => x.Roll(_damageDice1)).Returns(50);
            _diceRoller.Setup(x => x.Roll(Dice.D20)).Callback<Dice>(_ => count++).Returns(() => count == 1 ? 18 : 1);
            _simulationModel.UnitDescription1.IsAttacking = true;

            _simulationRunner.Simulate(_simulationModel);

            Assert.AreEqual(_unitCount2 * _hitPoint2 - _hitPoint2, _simulationModel.Group2.HitPoint); // Single unit killed
            Assert.AreEqual(_unitCount2 - 1, _simulationModel.Group2.UnitCount);
            Assert.AreEqual(_name2, _simulationModel.UnitDescription2.Name);
            Assert.AreEqual(_armorClass2, _simulationModel.UnitDescription2.ArmorClass);
            Assert.AreEqual(_damageBonus2, _simulationModel.UnitDescription2.DamageBonus);
            Assert.AreEqual(_damageDice2, _simulationModel.UnitDescription2.DamageDice);
            Assert.AreEqual(_hitBonus2, _simulationModel.UnitDescription2.HitBonus);
            Assert.AreEqual(_hitPoint2, _simulationModel.UnitDescription2.HitPoint);
            Assert.AreEqual(null, _simulationModel.UnitDescription2.IsAttacking);

            Assert.AreEqual(_unitCount1 * _hitPoint1, _simulationModel.Group1.HitPoint);
            Assert.AreEqual(_unitCount1, _simulationModel.Group1.UnitCount);
            Assert.AreEqual(_name1, _simulationModel.UnitDescription1.Name);
            Assert.AreEqual(_armorClass1, _simulationModel.UnitDescription1.ArmorClass);
            Assert.AreEqual(_damageBonus1, _simulationModel.UnitDescription1.DamageBonus);
            Assert.AreEqual(_damageDice1, _simulationModel.UnitDescription1.DamageDice);
            Assert.AreEqual(_hitBonus1, _simulationModel.UnitDescription1.HitBonus);
            Assert.AreEqual(_hitPoint1, _simulationModel.UnitDescription1.HitPoint);
            Assert.AreEqual(true, _simulationModel.UnitDescription1.IsAttacking);
        }

        [Test]
        public void Test_Simulate_Attack_From_Second_Group_With_Deaths()
        {
            // One unit will be killed, the second wounded
            int count = 0;
            _diceRoller.Setup(x => x.Roll(_damageDice2)).Returns<Dice>(_ =>
            {
                if (count == 1)
                    return 50;

                return 2;
            });
            // 2 first attack successful
            _diceRoller.Setup(x => x.Roll(Dice.D20)).Callback<Dice>(_ => count++).Returns(() => count <= 2 ? 18 : 1);
            _simulationModel.UnitDescription2.IsAttacking = true;

            _simulationRunner.Simulate(_simulationModel);

            Assert.AreEqual(_unitCount2 * _hitPoint2, _simulationModel.Group2.HitPoint);
            Assert.AreEqual(_unitCount2, _simulationModel.Group2.UnitCount);
            Assert.AreEqual(_name2, _simulationModel.UnitDescription2.Name);
            Assert.AreEqual(_armorClass2, _simulationModel.UnitDescription2.ArmorClass);
            Assert.AreEqual(_damageBonus2, _simulationModel.UnitDescription2.DamageBonus);
            Assert.AreEqual(_damageDice2, _simulationModel.UnitDescription2.DamageDice);
            Assert.AreEqual(_hitBonus2, _simulationModel.UnitDescription2.HitBonus);
            Assert.AreEqual(_hitPoint2, _simulationModel.UnitDescription2.HitPoint);
            Assert.AreEqual(true, _simulationModel.UnitDescription2.IsAttacking);

            Assert.AreEqual(_unitCount1 * _hitPoint1 - _hitPoint1 - 2 - _damageBonus2, _simulationModel.Group1.HitPoint);
            Assert.AreEqual(_unitCount1 - 1, _simulationModel.Group1.UnitCount);
            Assert.AreEqual(_name1, _simulationModel.UnitDescription1.Name);
            Assert.AreEqual(_armorClass1, _simulationModel.UnitDescription1.ArmorClass);
            Assert.AreEqual(_damageBonus1, _simulationModel.UnitDescription1.DamageBonus);
            Assert.AreEqual(_damageDice1, _simulationModel.UnitDescription1.DamageDice);
            Assert.AreEqual(_hitBonus1, _simulationModel.UnitDescription1.HitBonus);
            Assert.AreEqual(_hitPoint1, _simulationModel.UnitDescription1.HitPoint);
            Assert.AreEqual(null, _simulationModel.UnitDescription1.IsAttacking);
        }
    }
