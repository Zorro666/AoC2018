using NUnit.Framework;

namespace Day13
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
@"/->-\        ",
@"|   |  /----\",
@"| /-+--+-\  |",
@"| | |  | v  |",
@"\-+-/  \-+--/",
@"  \------/   ",
        }, 7, 3, TestName = "FirstCrash 7,3")]
        public void FirstCrash(string[] input, int expectedX, int expectedY)
        {
            Program.Parse(input);
            var (x, y) = Program.FirstCrash();
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }

        [Test]
        [TestCase(new string[] {
@"/>-<\  ",
@"|   |  ",
@"| /<+-\",
@"| | | v",
@"\>+</ |",
@"  |   ^",
@"  \<->/",
        }, 6, 4, TestName = "LastCart 6,4")]
        public void LastCart(string[] input, int expectedX, int expectedY)
        {
            Program.Parse(input);
            var (x, y) = Program.LastCart();
            Assert.That(x, Is.EqualTo(expectedX));
            Assert.That(y, Is.EqualTo(expectedY));
        }
    }
}
