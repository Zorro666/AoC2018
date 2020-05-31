using NUnit.Framework;

namespace Day12
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"initial state: #..#.#..##......###...###",
"",
"...## => #",
"..#.. => #",
".#... => #",
".#.#. => #",
".#.## => #",
".##.. => #",
".#### => #",
"#.#.# => #",
"#.### => #",
"##.#. => #",
"##.## => #",
"###.. => #",
"###.# => #",
"####. => #"
        }, 20, 35)]
        public void NumberOfPlants(string[] input, int generations, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.NumberOfPlants(generations), Is.EqualTo(expected));
        }
    }
}
