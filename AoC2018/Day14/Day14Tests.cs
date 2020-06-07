using NUnit.Framework;

namespace Day14
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(9, "5158916779", TestName = "NextTenRecipes 9")]
        [TestCase(5, "0124515891", TestName = "NextTenRecipes 5")]
        [TestCase(18, "9251071085", TestName = "NextTenRecipes 18")]
        [TestCase(2018, "5941429882", TestName = "NextTenRecipes 2018")]
        public void NextTenRecipe(int numRecipes, string expected)
        {
            Assert.That(Program.NextTenRecipes(numRecipes), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("51589", 9, TestName = "HowManyRecipes 51589 = 9")]
        [TestCase("01245", 5, TestName = "HowManyRecipes 01245 = 5")]
        [TestCase("92510", 18, TestName = "HowManyRecipes 92510 = 18")]
        [TestCase("59414", 2018, TestName = "HowManyRecipes 59414= 2018")]
        public void HowManyRecipes(string pattern, int expected)
        {
            Assert.That(Program.HowManyRecipes(pattern), Is.EqualTo(expected));
        }
    }
}
