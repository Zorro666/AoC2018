using NUnit.Framework;

namespace Day18
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
".#.#...|#.",
".....#|##|",
".|..|...#.",
"..|#.....#",
"#.#|||#|#|",
"...#.||...",
".|....|...",
"||...#|.#|",
"|.||||..|.",
"...#.|..|."
        }, 10, 37 * 31, TestName = "TotalResource A 10 = 1147")]
        public void TotalResource(string[] lines, int minutes, int expected)
        {
            Program.Parse(lines);
            Program.Simulate(minutes);
            Assert.That(Program.TotalResource(), Is.EqualTo(expected));
        }
    }
}
