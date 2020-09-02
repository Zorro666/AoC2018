using NUnit.Framework;

namespace Day25
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"0,0,0,0",
"3,0,0,0",
"0,3,0,0",
"0,0,3,0",
"0,0,0,3",
"0,0,0,6",
"9,0,0,0",
"12,0,0,0"
        }, 2, TestName = "CountConstellations A = 2")]
        [TestCase(new string[] {
"-1,2,2,0",
"0,0,2,-2",
"0,0,0,-2",
"-1,2,0,0",
"-2,-2,-2,2",
"3,0,2,-1",
"-1,3,2,2",
"-1,0,-1,0",
"0,2,1,-2",
"3,0,0,0"
        }, 4, TestName = "CountConstellations B = 4")]
        [TestCase(new string[] {
"1,-1,0,1",
"2,0,-1,0",
"3,2,-1,0",
"0,0,3,1",
"0,0,-1,-1",
"2,3,-2,0",
"-2,2,0,0",
"2,-2,0,-1",
"1,-1,0,-1",
"3,2,0,2"
        }, 3, TestName = "CountConstellations C = 3")]
        [TestCase(new string[] {
"1,-1,-1,-2",
"-2,-2,0,1",
"0,2,1,3",
"-2,3,-2,1",
"0,2,3,-2",
"-1,-1,1,-2",
"0,-2,-1,0",
"-2,2,3,-1",
"1,2,2,0",
"-1,-2,0,-2"
        }, 8, TestName = "CountConstellations D = 8")]
        public void CountConstellations(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.CountConstellations(), Is.EqualTo(expected));
        }
    }
}
