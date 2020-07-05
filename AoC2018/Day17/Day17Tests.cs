using NUnit.Framework;

namespace Day17
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"x=495, y=2..7",
"y=7, x=495..501",
"x=501, y=3..7",
"x=498, y=2..4",
"x=506, y=1..2",
"x=498, y=10..13",
"x=504, y=10..13",
"y=13, x=498..504"
        }, 57, TestName = "CountWaterTotal A = 57")]
        public void CountWaterTotal(string[] input, int expected)
        {
            Program.Parse(input);
            Program.SimulateWater();
            (var wet, var settled) = Program.CountWater();
            Assert.That(wet + settled, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] {
"x=495, y=2..7",
"y=7, x=495..501",
"x=501, y=3..7",
"x=498, y=2..4",
"x=506, y=1..2",
"x=498, y=10..13",
"x=504, y=10..13",
"y=13, x=498..504"
        }, 29, TestName = "CountWaterSettled A = 29")]
        public void CountWater(string[] input, int expected)
        {
            Program.Parse(input);
            Program.SimulateWater();
            (var _, var settled) = Program.CountWater();
            Assert.That(settled, Is.EqualTo(expected));
        }
    }
}
