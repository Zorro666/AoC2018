﻿using NUnit.Framework;

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
        }, 0, 11, TestName = "NumberOfPlants 0 = 11")]
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
        }, 1, 7, TestName = "NumberOfPlants 1 = 7")]
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
        }, 2, 11, TestName = "NumberOfPlants 2 = 11")]
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
        }, 3, 9, TestName = "NumberOfPlants 3 = 9")]
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
        }, 5, 9, TestName = "NumberOfPlants 5 = 9")]
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
        }, 10, 14, TestName = "NumberOfPlants 10 = 14")]
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
        }, 15, 11, TestName = "NumberOfPlants 15 = 11")]
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
        }, 18, 18, TestName = "NumberOfPlants 18 = 18")]
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
        }, 19, 20, TestName = "NumberOfPlants 19 = 20")]
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
        }, 20, 19, TestName = "NumberOfPlants 20 = 19")]
        public void NumberOfPlants(string[] input, int generations, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.NumberOfPlants(generations), Is.EqualTo(expected));
        }

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
        }, 20, 325, TestName = "PlantSum 20 = 325")]
        public void PlantSum(string[] input, int generations, int expected)
        {
            Program.Parse(input);
            Assert.That(Program.PlantSum(generations), Is.EqualTo(expected));
        }
    }
}
