using NUnit.Framework;

namespace Day01
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"+1",
"-2",
"+3",
"+1"
        }, 3, TestName = "FinalFrequency A = 3")]
        [TestCase(new string[] {
"+1",
"+1",
"+1"
        }, 3, TestName = "FinalFrequency B = 3")]
        [TestCase(new string[] {
"+1",
"+1",
"-2"
        }, 0, TestName = "FinalFrequency C = 0")]
        [TestCase(new string[] {
"-1",
"-2",
"-3"
        }, -6, TestName = "FinalFrequency D = -6")]
        public void FinalFrequency(string[] input, long expected)
        {
            Program.Parse(input);
            Assert.That(Program.FindFinalFrequency(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] {
"+1",
"-2",
"+3",
"+1"
        }, 2, TestName = "RepeatFrequency A = 2")]
        [TestCase(new string[] {
"+1",
"-1"
        }, 0, TestName = "RepeatFrequency B = 0")]
        [TestCase(new string[] {
"+3",
"+3",
"+4",
"-2",
"-4"
        }, 10, TestName = "RepeatFrequency C = 10")]
        [TestCase(new string[] {
"-6",
"+3",
"+8",
"+5",
"-6"
        }, 5, TestName = "RepeatFrequency D = 5")]
        [TestCase(new string[] {
"+7",
"+7",
"-2",
"-7",
"-4"
        }, 14, TestName = "RepeatFrequency E = 14")]
        public void FindFirstRepeatFrequency(string[] input, long expected)
        {
            Program.Parse(input);
            Assert.That(Program.FindFirstRepeatFrequency(), Is.EqualTo(expected));
        }
    }
}
