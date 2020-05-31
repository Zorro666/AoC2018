using NUnit.Framework;

namespace Day11
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(8, 3, 5, 4)]
        [TestCase(57, 122, 79, -5)]
        [TestCase(39, 217, 196, 0)]
        [TestCase(71, 101, 153, 4)]
        public void PowerLevel(int serialNumber, int x, int y, int expected)
        {
            Assert.That(Program.PowerLevel(serialNumber, x, y), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(18, 33, 45)]
        [TestCase(42, 21, 51)]
        public void FindLargest3x3(int serialNumber, int expectedX, int expectedY)
        {
            Program.ComputePowerLevels(serialNumber);
            (int x, int y) = Program.FindLargest3x3();
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }
    }
}
