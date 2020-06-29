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
"After:  [3, 2, 2, 1]"
        }, 1, TestName = "CountThreeMoreOpcodes A = 1")]
        public void Day16(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.CountThreeOrMoreOpcodes(), Is.EqualTo(expected));
        }
    }
}
