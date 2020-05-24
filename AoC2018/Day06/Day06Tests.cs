using NUnit.Framework;

namespace Day06
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"1, 1",
"1, 6",
"8, 3",
"3, 4",
"5, 5",
"8, 9"
        }, 17, TestName = "LargestFiniteArea 17")]
        public void LargestFiniteArea(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.LargestFiniteArea(), Is.EqualTo(expected));
        }
    }
}
