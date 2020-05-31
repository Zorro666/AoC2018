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
        [TestCase(42, 21, 61)]
        public void FindLargest3x3(int serialNumber, int expectedX, int expectedY)
        {
            Program.ComputePowerLevels(serialNumber);
            (int x, int y) = Program.FindLargest3x3();
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }

        [Test]
        [TestCase(18, 90, 269, 16)]
        [TestCase(42, 232, 251, 12)]
        public void FindLargestSquare(int serialNumber, int expectedX, int expectedY, int expectedSize)
        {
            Program.ComputePowerLevels(serialNumber);
            (int x, int y, int size) = Program.FindLargestSquare();
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
            Assert.That(size, Is.EqualTo(expectedSize));
        }
    }
}
