using NUnit.Framework;

namespace Day05
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("dabAcCaCBAcCcaDA", 10)]
        [TestCase("aA", 0)]
        [TestCase("abBA", 0)]
        [TestCase("abAB", 4)]
        [TestCase("aabAAB", 6)]
        public void Reduce(string input, int expected)
        {
            Assert.That(Program.Reduce(input), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("dabAcCaCBAcCcaDA", 4)]
        [TestCase("abAB", 0)]
        [TestCase("aabAAB", 0)]
        public void Shortest(string input, int expected)
        {
            Assert.That(Program.Shortest(input), Is.EqualTo(expected));
        }
    }
}
