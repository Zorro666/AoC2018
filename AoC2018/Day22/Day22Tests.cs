using NUnit.Framework;

namespace Day22
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"depth: 510",
"target: 10, 10"
        }, 114, TestName = "RiskLevel A = 114")]
        public void Risklevel(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.RiskLevel(), Is.EqualTo(expected));
        }
    }
}
