using NUnit.Framework;

namespace Day24
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"Immune System:",
"17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
"989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3",
"",
"Infection:",
"801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
"4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
        }, 5216, TestName = "WinningArmyUnits A = 5216")]
        public void WinningArmyUnits(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.WinningArmyUnits(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] {
"Immune System:",
"17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
"989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3",
"",
"Infection:",
"801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
"4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
        }, 1570, 51, TestName = "WinningArmyUnits Boost 1570 = 51")]
        public void WinningArmyUnitsBoost(string[] input, int boost, int expected)
        {
            Program.Parse(input);
            Program.Boost(boost);
            Assert.That(Program.WinningArmyUnits(), Is.EqualTo(expected));
            Assert.That(Program.WinningSide(), Is.EqualTo(0));
        }
    }
}
