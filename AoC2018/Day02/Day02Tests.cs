using NUnit.Framework;

namespace Day02
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"abcdef",
"bababc",
"abbcde",
"abcccd",
"aabcdd",
"abcdee",
"ababab"
        }, 12, TestName = "Checksum 12")]
        public void Checksum(string[] input, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.Checksum(), Is.EqualTo(expected));
        }

        [TestCase(new string[] {
"abcde",
"fghij",
"klmno",
"pqrst",
"fguij",
"axcye",
"wvxyz"
        }, "fgij", TestName = "CommonChars")]
        public void CommonChars(string[] input, string expected)
        {
            Program.Parse(input);
            Assert.That(Program.CommonChars(), Is.EqualTo(expected));
        }
    }
}
