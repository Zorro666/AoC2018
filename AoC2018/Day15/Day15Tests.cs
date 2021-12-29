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
                }, 1, 1, 3, 1, TestName = "ClosestTarget (3,1)")]
        [TestCase(
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
                }, 7, 5, int.MaxValue, int.MaxValue, TestName = "ClosestTarget (7,5) None")]
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
"########",
"#G....E#",
"#G.GE..#",
"#..#...#",
"#E....E#",
"########"
                }, 6, 4, 6, 3, TestName = "MoveTowardsTarget (6,3)")]
        [TestCase(
    new string[] {
"#############",
"#....##.##.##",
"##.GG....E..#",
"#...G.GE....#",
"#.....E.....#",
"##....#.....#",
"###GE.....E.#",
"#############"
                }, 10, 6, 10, 5, TestName = "MoveTowardsTarget (10,5)")]
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
                }, TestName = "Simulate Move 0")]
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
                }, TestName = "Simulate Move 1")]
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
                }, TestName = "Simulate Move 2")]
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
                }, TestName = "Simulate Move 3")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 1,
            new string[] {
"#######",
"#..G..#",
"#...EG#",
"#.#G#G#",
"#...#E#",
"#.....#",
"#######"
            }, TestName = "Simulate 1")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 2,
            new string[] {
"#######",
"#...G.#",
"#..GEG#",
"#.#.#G#",
"#...#E#",
"#.....#",
"#######"
            }, TestName = "Simulate 2")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 23,
            new string[] {
"#######",
"#...G.#",
"#..G.G#",
"#.#.#G#",
"#...#E#",
"#.....#",
"#######"
            }, TestName = "Simulate 23")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 24,
            new string[] {
"#######",
"#..G..#",
"#...G.#",
"#.#G#G#",
"#...#E#",
"#.....#",
"#######"
            }, TestName = "Simulate 24")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 25,
            new string[] {
"#######",
"#.G...#",
"#..G..#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
            }, TestName = "Simulate 25")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 26,
            new string[] {
"#######",
"#G....#",
"#.G...#",
"#.#.#G#",
"#...#E#",
"#..G..#",
"#######"
            }, TestName = "Simulate 26")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 27,
            new string[] {
"#######",
"#G....#",
"#.G...#",
"#.#.#G#",
"#...#E#",
"#...G.#",
"#######"
            }, TestName = "Simulate 27")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 28,
            new string[] {
"#######",
"#G....#",
"#.G...#",
"#.#.#G#",
"#...#E#",
"#....G#",
"#######"
            }, TestName = "Simulate 28")]
        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 47,
            new string[] {
"#######",
"#G....#",
"#.G...#",
"#.#.#G#",
"#...#.#",
"#....G#",
"#######",
            }, TestName = "Simulate 47")]
        public void Simulate(string[] map, int rounds, string[] expected)
        {
            Program.Parse(map);
            Program.Simulate(rounds, 3);
            Assert.That(Program.GetMap(), Is.EqualTo(expected));
        }

        [TestCase(
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
                }, 4, 2, 200 - 1 * 3, TestName = "ResolveCombat(4,2) 3")]
        [TestCase(
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
                }, 4, 3, 200 - 4 * 3, TestName = "ResolveCombat(4,3) 3")]
        public void ResolveCombat(string[] map, int x, int y, int expected)
        {
            Program.Parse(map);
            Program.Simulate(1, 3);
            Assert.That(Program.GetHP(x, y), Is.EqualTo(expected));
        }

        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 47 * 590, TestName = "BattleResult A")]
        [TestCase(
            new string[] {
"#######",
"#G..#E#",
"#E#E.E#",
"#G.##.#",
"#...#E#",
"#...E.#",
"#######",
                }, 37 * 982, TestName = "BattleResult B")]
        [TestCase(
            new string[] {
"#######",
"#E..EG#",
"#.#G.E#",
"#E.##E#",
"#G..#.#",
"#..E#.#",
"#######"
                }, 46 * 859, TestName = "BattleResult C")]
        [TestCase(
            new string[] {
"#######",
"#E.G#.#",
"#.#G..#",
"#G.#.G#",
"#G..#.#",
"#...E.#",
"#######"
                }, 35 * 793, TestName = "BattleResult D")]
        [TestCase(
            new string[] {
"#######",
"#.E...#",
"#.#..G#",
"#.###.#",
"#E#G#G#",
"#...#G#",
"#######"
                }, 54 * 536, TestName = "BattleResult E")]
        [TestCase(
            new string[] {
"#########",
"#G......#",
"#.E.#...#",
"#..##..G#",
"#...##..#",
"#...#...#",
"#.G...G.#",
"#.....G.#",
"#########"
                }, 20 * 937, TestName = "BattleResult F")]
        public void BattleResult(string[] map, int expected)
        {
            Program.Parse(map);
            Assert.That(Program.BattleResult(3), Is.EqualTo(expected));
        }

        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 15, TestName = "ElfAttackPower A = 15")]
        [TestCase(
            new string[] {
"#######",
"#E..EG#",
"#.#G.E#",
"#E.##E#",
"#G..#.#",
"#..E#.#",
"#######"
                }, 4, TestName = "ElfAttackPower B = 4")]
        [TestCase(
            new string[] {
"#######",
"#E.G#.#",
"#.#G..#",
"#G.#.G#",
"#G..#.#",
"#...E.#",
"#######"
                }, 15, TestName = "ElfAttackPower C = 15")]
        [TestCase(
            new string[] {
"#######",
"#.E...#",
"#.#..G#",
"#.###.#",
"#E#G#G#",
"#...#G#",
"#######"
                }, 12, TestName = "ElfAttackPower D = 12")]
        [TestCase(
            new string[] {
"#########",
"#G......#",
"#.E.#...#",
"#..##..G#",
"#...##..#",
"#...#...#",
"#.G...G.#",
"#.....G.#",
"#########"
                }, 34, TestName = "ElfAttackPower E = 34")]
        public void ElfAttackPower(string[] map, int expected)
        {
            Program.Parse(map);
            Assert.That(Program.ElfAttackPower(), Is.EqualTo(expected));
        }

        [TestCase(
            new string[] {
"#######",
"#.G...#",
"#...EG#",
"#.#.#G#",
"#..G#E#",
"#.....#",
"#######"
                }, 29 * 172, TestName = "ElfWinBattleResult A = 4988")]
        [TestCase(
            new string[] {
"#######",
"#E..EG#",
"#.#G.E#",
"#E.##E#",
"#G..#.#",
"#..E#.#",
"#######"
                }, 33 * 948, TestName = "ElfWinBattleResult B = 31284")]
        [TestCase(
            new string[] {
"#######",
"#E.G#.#",
"#.#G..#",
"#G.#.G#",
"#G..#.#",
"#...E.#",
"#######"
                }, 37 * 94, TestName = "ElfWinBattleResult C = 3478")]
        [TestCase(
            new string[] {
"#######",
"#.E...#",
"#.#..G#",
"#.###.#",
"#E#G#G#",
"#...#G#",
"#######"
                }, 39 * 166, TestName = "ElfWinBattleResult D = 6474")]
        [TestCase(
            new string[] {
"#########",
"#G......#",
"#.E.#...#",
"#..##..G#",
"#...##..#",
"#...#...#",
"#.G...G.#",
"#.....G.#",
"#########"
                }, 30 * 38, TestName = "ElfWinBattleResult E = 1140")]
        public void ElfWinBattleResult(string[] map, int expected)
        {
            Program.Parse(map);
            Assert.That(Program.ElfWinBattleResult(), Is.EqualTo(expected));
        }
    }
}
