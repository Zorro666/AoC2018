using NUnit.Framework;

namespace Day15
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 1, 2, 1, TestName = "TurnOrder 1 = (2,1)")]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 2, 4, 1, TestName = "TurnOrder 2 = (4,1)")]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 3, 1, 2, TestName = "TurnOrder 3 = (1,2)")]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 4, 3, 2, TestName = "TurnOrder 4 = (3,2)")]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 5, 5, 2, TestName = "TurnOrder 5 = (5,2)")]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 6, 2, 3, TestName = "TurnOrder 6 = (2,3)")]
        [TestCase(
            new string[] {
"#######",
"#.G.E.#",
"#E.G.E#",
"#.G.E.#",
"#######"
                }, 7, 4, 3, TestName = "TurnOrder 7 = (4,3)")]
        public void TurnOrder(string[] map, int turn, int expectedX, int expectedY)
        {
            Program.Parse(map);
            (var x, var y) = Program.TurnOrder(turn - 1);
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }
    }
}
