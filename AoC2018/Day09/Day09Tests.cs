using NUnit.Framework;

namespace Day09
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("9 players; last marble is worth 25 points", 32L)]
        [TestCase("10 players; last marble is worth 1618 points", 8317L)]
        [TestCase("13 players; last marble is worth 7999 points", 146373L)]
        [TestCase("17 players; last marble is worth 1104 points", 2764L)]
        [TestCase("21 players; last marble is worth 6111 points", 54718L)]
        [TestCase("30 players; last marble is worth 5807 points", 37305L)]
        public void HighScore(string input, long expected)
        {
            Program.Parse(new string[] { input });
            Assert.That(Program.HighScore(), Is.EqualTo(expected));
        }
    }
}
