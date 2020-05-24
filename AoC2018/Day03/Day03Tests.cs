using NUnit.Framework;

namespace Day03
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"#1 @ 1,3: 4x4",
"#2 @ 3,1: 4x4",
"#3 @ 5,5: 2x2"
        }, 4, TestName = "OverClaimed 4")]
        public void OverClaimed(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.OverClaimed(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] {
"#1 @ 1,3: 4x4",
"#2 @ 3,1: 4x4",
"#3 @ 5,5: 2x2"
        }, 3, TestName = "SingleClaimID 3")]
        public void SingleClaimID(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.SingleClaimID(), Is.EqualTo(expected));
        }
    }
}
