using NUnit.Framework;

namespace Day07
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"Step C must be finished before step A can begin.",
"Step C must be finished before step F can begin.",
"Step A must be finished before step B can begin.",
"Step A must be finished before step D can begin.",
"Step B must be finished before step E can begin.",
"Step D must be finished before step E can begin.",
"Step F must be finished before step E can begin."
        }, "CABDFE", TestName = "ConstructionOrder CABDFE")]
        public void ConstructionOrder(string[] input, string expected)
        {
            Program.Parse(input);
            Assert.That(Program.ConstructionOrder(), Is.EqualTo(expected));
        }
    }
}
