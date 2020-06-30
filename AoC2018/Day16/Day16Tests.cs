using NUnit.Framework;

namespace Day16
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"Before: [3, 2, 1, 1]",
"9 2 1 2",
"After:  [3, 2, 2, 1]",
"",
"",
"14 3 3 2",
"14 3 3 0",
"14 2 2 1",
"13 0 2 1"

        }, 1, TestName = "CountThreeMoreOpcodes A = 1")]
        public void CountThreeOrMoreOpcodes(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.CountThreeOrMoreOpcodes(), Is.EqualTo(expected));
        }
    }
}
