using NUnit.Framework;

namespace Day20
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("^WNE$", new string[] {
"#####",
"#.|.#",
"#-###",
"#.|X#",
"#####"
        }, TestName = "GenerateMap A")]
        [TestCase("^ENWWW(NEEE|SSE(EE|N))$", new string[] {
"#########",
"#.|.|.|.#",
"#-#######",
"#.|.|.|.#",
"#-#####-#",
"#.#.#X|.#",
"#-#-#####",
"#.|.|.|.#",
"#########"
        }, TestName = "GenerateMap B")]
        [TestCase("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", new string[] {
"###########",
"#.|.#.|.#.#",
"#-###-#-#-#",
"#.|.|.#.#.#",
"#-#####-#-#",
"#.#.#X|.#.#",
"#-#-#####-#",
"#.#.|.|.|.#",
"#-###-###-#",
"#.|.|.#.|.#",
"###########"
        }, TestName = "GenerateMap C")]
        [TestCase("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", new string[] {
"#############",
"#.|.|.|.|.|.#",
"#-#####-###-#",
"#.#.|.#.#.#.#",
"#-#-###-#-#-#",
"#.#.#.|.#.|.#",
"#-#-#-#####-#",
"#.#.#.#X|.#.#",
"#-#-#-###-#-#",
"#.|.#.|.#.#.#",
"###-#-###-#-#",
"#.|.#.|.|.#.#",
"#############"
        }, TestName = "GenerateMap D")]
        [TestCase("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", new string[] {
"###############",
"#.|.|.|.#.|.|.#",
"#-###-###-#-#-#",
"#.|.#.|.|.#.#.#",
"#-#########-#-#",
"#.#.|.|.|.|.#.#",
"#-#-#########-#",
"#.#.#.|X#.|.#.#",
"###-#-###-#-#-#",
"#.|.#.#.|.#.|.#",
"#-###-#####-###",
"#.|.#.|.|.#.#.#",
"#-#-#####-#-#-#",
"#.#.|.|.|.#.|.#",
"###############"
        }, TestName = "GenerateMap E")]
        public void GenerateMap(string regexp, string[] expected)
        {
            Program.GenerateMap(regexp);
            Program.OutputMap();
            Assert.That(Program.GetMap(), Is.EqualTo(expected));
        }

        [TestCase("^WNE$", 3, TestName = "FurthestRoom A")]
        [TestCase("^ENWWW(NEEE|SSE(EE|N))$", 10, TestName = "FurthestRoom B")]
        [TestCase("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18, TestName = "FurthestRoom C")]
        [TestCase("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", 23, TestName = "FurthestRoom D")]
        [TestCase("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", 31, TestName = "FurthestRoom E")]
        public void FurthestRoom(string regexp, int expected)
        {
            Program.GenerateMap(regexp);
            Program.OutputMap();
            Assert.That(Program.FurthestRoom(), Is.EqualTo(expected));
        }
    }
}
