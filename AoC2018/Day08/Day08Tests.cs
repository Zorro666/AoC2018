using NUnit.Framework;

namespace Day08
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 138, TestName = "Sum 138")]
        public void Sum(string nodes, int expected)
        {
            Program.Parse(new string[] { nodes });
            Assert.That(Program.Sum(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 66, TestName = "RootNodeValue 66")]
        public void RootNodeValue(string nodes, int expected)
        {
            Program.Parse(new string[] { nodes });
            Assert.That(Program.RootNodeValue(), Is.EqualTo(expected));
        }
    }
}
