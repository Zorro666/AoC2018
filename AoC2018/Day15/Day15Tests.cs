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

        [TestCase(
            new string[] {
"#######",
"#E..G.#",
"#...#.#",
"#.G.#G#",
"#######"
                }, 1, 1, 4, 1, TestName = "ClosestTarget (4,1)")]
        public void ClosestTarget(string[] map, int fromX, int fromY, int expectedX, int expectedY)
        {
            Program.Parse(map);
            (var x, var y) = Program.ClosestTarget(fromX, fromY);
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }

        [TestCase(
            new string[] {
"#######",
"#E..G.#",
"#...#.#",
"#.G.#G#",
"#######"
                }, 1, 1, 2, 1, TestName = "MoveTowardsTarget (2,1)")]
        [TestCase(
            new string[] {
"#######",
"#.E...#",
"#.....#",
"#...G.#",
"#######"
                }, 2, 1, 3, 1, TestName = "MoveTowardsTarget (3,1)")]
        [TestCase(
            new string[] {
"#########",
"#.G...G.#",
"#...G...#",
"#.......#",
"#.G.E..G#",
"#.......#",
"#.......#",
"#G..G..G#",
"#########"
                }, 4, 4, 4, 3, TestName = "MoveTowardsTarget (4,3)")]
        [TestCase(
            new string[] {
"#########",
"#..G.G..#",
"#...G...#",
"#.G.E.G.#",
"#.......#",
"#G..G..G#",
"#.......#",
"#.......#",
"#########"
                }, 4, 3, 4, 3, TestName = "MoveTowardsTarget Adjacent")]
        public void MoveTowardsTarget(string[] map, int fromX, int fromY, int expectedX, int expectedY)
        {
            Program.Parse(map);
            (var x, var y) = Program.MoveTowardsTarget(fromX, fromY);
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }

        [TestCase(
            new string[] {
"#########",
"#G..G..G#",
"#.......#",
"#.......#",
"#G..E..G#",
"#.......#",
"#.......#",
"#G..G..G#",
"#########"
                }, 0,
            new string[] {
"#########",
"#G..G..G#",
"#.......#",
"#.......#",
"#G..E..G#",
"#.......#",
"#.......#",
"#G..G..G#",
"#########"
                }, TestName = "SimulateRound 0")]
        [TestCase(
            new string[] {
"#########",
"#G..G..G#",
"#.......#",
"#.......#",
"#G..E..G#",
"#.......#",
"#.......#",
"#G..G..G#",
"#########"
                }, 1,
            new string[] {
"#########",
"#.G...G.#",
"#...G...#",
"#...E..G#",
"#.G.....#",
"#.......#",
"#G..G..G#",
"#.......#",
"#########"
                }, TestName = "SimulateRound 1")]
        [TestCase(
            new string[] {
"#########",
"#G..G..G#",
"#.......#",
"#.......#",
"#G..E..G#",
"#.......#",
"#.......#",
"#G..G..G#",
"#########"
                }, 2,
            new string[] {
"#########",
"#..G.G..#",
"#...G...#",
"#.G.E.G.#",
"#.......#",
"#G..G..G#",
"#.......#",
"#.......#",
"#########"
                }, TestName = "SimulateRound 2")]
        [TestCase(
            new string[] {
"#########",
"#G..G..G#",
"#.......#",
"#.......#",
"#G..E..G#",
"#.......#",
"#.......#",
"#G..G..G#",
"#########"
                }, 3,
            new string[] {
"#########",
"#.......#",
"#..GGG..#",
"#..GEG..#",
"#G..G...#",
"#......G#",
"#.......#",
"#.......#",
"#########"
                }, TestName = "SimulateRound 3")]
        public void SimulateRound(string[] map, int rounds, string[] expected)
        {
            Program.Parse(map);
            Program.Simulate(rounds);
            Assert.That(Program.GetMap(), Is.EqualTo(expected));
        }
    }
}
