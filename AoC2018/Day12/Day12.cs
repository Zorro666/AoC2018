using System;

/*

--- Day 12: Subterranean Sustainability ---

The year 518 is significantly more underground than your history books implied.
Either that, or you've arrived in a vast cavern network under the North Pole.

After exploring a little, you discover a long tunnel that contains a row of small pots as far as you can see to your left and right.
A few of them contain plants - someone is trying to grow things in these geothermally-heated caves.

The pots are numbered, with 0 in front of you.
To the left, the pots are numbered -1, -2, -3, and so on; to the right, 1, 2, 3....
Your puzzle input contains a list of pots from 0 to the right and whether they do (#) or do not (.) currently contain a plant, the initial state.
(No other pots currently contain plants.) 
For example, an initial state of #..##.... indicates that pots 0, 3, and 4 currently contain plants.

Your puzzle input also contains some notes you find on a nearby table: someone has been trying to figure out how these plants spread to nearby pots.
Based on the notes, for each generation of plants, a given pot has or does not have a plant based on whether that pot (and the two pots on either side of it) had a plant in the last generation.
These are written as LLCRR => N, where L are pots to the left, C is the current pot being considered, R are the pots to the right, and N is whether the current pot will have a plant in the next generation.
For example:

A note like ..#.. => . means that a pot that contains a plant but with no plants within two pots of it will not have a plant in it during the next generation.
A note like ##.## => . means that an empty pot with two plants on each side of it will remain empty in the next generation.
A note like .##.# => # means that a pot has a plant in a given generation if, in the previous generation, there were plants in that pot, the one immediately to the left, and the one two pots to the right, but not in the ones immediately to the right and two to the left.
It's not clear what these plants are for, but you're sure it's important, so you'd like to make sure the current configuration of plants is sustainable by determining what will happen after 20 generations.

For example, given the following input:

initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #
For brevity, in this example, only the combinations which do produce a plant are listed.
(Your input includes all possible combinations.) Then, the next 20 generations will look like this:

                 1         2         3     
       0         0         0         0     
 0: ...#..#.#..##......###...###...........
 1: ...#...#....#.....#..#..#..#...........
 2: ...##..##...##....#..#..#..##..........
 3: ..#.#...#..#.#....#..#..#...#..........
 4: ...#.#..#...#.#...#..#..##..##.........
 5: ....#...##...#.#..#..#...#...#.........
 6: ....##.#.#....#...#..##..##..##........
 7: ...#..###.#...##..#...#...#...#........
 8: ...#....##.#.#.#..##..##..##..##.......
 9: ...##..#..#####....#...#...#...#.......
10: ..#.#..#...#.##....##..##..##..##......
11: ...#...##...#.#...#.#...#...#...#......
12: ...##.#.#....#.#...#.#..##..##..##.....
13: ..#..###.#....#.#...#....#...#...#.....
14: ..#....##.#....#.#..##...##..##..##....
15: ..##..#..#.#....#....#..#.#...#...#....
16: .#.#..#...#.#...##...#...#.#..##..##...
17: ..#...##...#.#.#.#...##...#....#...#...
18: ..##.#.#....#####.#.#.#...##...##..##..
19: .#..###.#..#.#.#######.#.#.#..#.#...#..
20: .#....##....#####...#######....#.#..##.
The generation is shown along the left, where 0 is the initial state.
The pot numbers are shown along the top, where 0 labels the center pot, negative-numbered pots extend to the left, and positive pots extend toward the right.
Remember, the initial state begins at pot 0, which is not the leftmost pot used in this example.

After one generation, only seven plants remain.
The one in pot 0 matched the rule looking for ..#.., the one in pot 4 matched the rule looking for .#.#., pot 9 matched .##.., and so on.

In this example, after 20 generations, the pots shown as # contain plants, the furthest left of which is pot -2, and the furthest right of which is pot 34.
Adding up all the numbers of plant-containing pots after the 20th generation produces 325.

After 20 generations, what is the sum of the numbers of all pots which contain a plant?

Your puzzle answer was 4110.

--- Part Two ---

You realize that 20 generations aren't enough. After all, these plants will need to last another 1500 years to even reach your timeline, not to mention your future.

After fifty billion (50000000000) generations, what is the sum of the numbers of all pots which contain a plant?

*/

namespace Day12
{
    class Program
    {
        const int MAX_NUM_GENERATIONS = 1024;
        const int MAX_NUM_PLANTS = 1024 * 2;
        const int MAX_NUM_RULES = 1024;
        static int sRulesCount;
        readonly static byte[,] sMatches = new byte[MAX_NUM_RULES, 5];
        readonly static byte[] sOutputs = new byte[MAX_NUM_RULES];
        readonly static byte[] sNextState = new byte[MAX_NUM_PLANTS];
        readonly static byte[] sCurrentState = new byte[MAX_NUM_PLANTS];
        readonly static byte[] sInitialState = new byte[MAX_NUM_PLANTS];
        static int sInitialStateCount;
        static string sStableState;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = PlantSum(20);
                Console.WriteLine($"Day12 : Result1 {result1}");
                var expected = 3494;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = PredictPlantSum(50000000000);
                Console.WriteLine($"Day12 : Result2 {result2}");
                var expected = 2850000002454;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            if (lines.Length < 3)
            {
                throw new InvalidProgramException($"Invalid input need at least 3 lines got {lines.Length}");
            }
            if (!lines[0].StartsWith("initial state: "))
            {
                throw new InvalidProgramException($"Invalid first line '{lines[0]}' expected 'initial state: '");
            }
            //'initial state: #..#.#..##......###...###'
            var initialState = lines[0].Split(':')[1].Trim();
            //Console.WriteLine($"{initialState}");
            for (var c = 0; c < initialState.Length; ++c)
            {
                if ((initialState[c] != '.') && (initialState[c] != '#'))
                {
                    throw new InvalidProgramException($"Invalid initialState line '{initialState}' input must be '.' or '#' '{initialState[c]}'");
                }
                sInitialState[c] = (byte)((initialState[c] == '.') ? 0 : 1);
            }
            sInitialStateCount = initialState.Length;

            //''
            if (lines[1].Length != 0)
            {
                throw new InvalidProgramException($"Invalid second line '{lines[0]}' expected empty line got '{lines[1]}'");
            }

            sRulesCount = 0;
            for (var i = 2; i < lines.Length; ++i)
            {
                //'...## => #'
                //'..#.. => #'
                var ruleInput = lines[i].Trim();
                var tokens = ruleInput.Split();
                if (tokens.Length != 3)
                {
                    throw new InvalidProgramException($"Invalid rules line '{ruleInput}' expected 3 tokens got '{tokens.Length}'");
                }
                if (tokens[1] != "=>")
                {
                    throw new InvalidProgramException($"Invalid rules line '{ruleInput}' expected '=>' got '{tokens[1]}'");
                }
                var match = tokens[0];
                if (match.Length != 5)
                {
                    throw new InvalidProgramException($"Invalid rules line '{ruleInput}' match rule must be 5 characters long '{match}' {match.Length}");
                }
                var output = tokens[2];
                if (output.Length != 1)
                {
                    throw new InvalidProgramException($"Invalid rules line '{ruleInput}' output rule must be a single character '{output}' {output.Length}");
                }
                for (var c = 0; c < 5; ++c)
                {
                    if ((match[c] != '.') && (match[c] != '#'))
                    {
                        throw new InvalidProgramException($"Invalid rules line '{ruleInput}' match characters must be '.' or '#' '{match[c]}'");
                    }
                    sMatches[sRulesCount, c] = (byte)((match[c] == '.') ? 0 : 1);
                }
                if ((output[0] != '.') && (output[0] != '#'))
                {
                    throw new InvalidProgramException($"Invalid rules line '{ruleInput}' output must be '.' or '#' '{output[0]}'");
                }
                sOutputs[sRulesCount] = (byte)((output[0] == '.') ? 0 : 1);
                ++sRulesCount;
            }
        }

        public static int PlantSum(int generations)
        {
            Simulate(generations);
            var plantSum = 0;
            for (var i = 0; i < MAX_NUM_PLANTS; ++i)
            {
                if (sCurrentState[i] == 1)
                {
                    plantSum += i - (MAX_NUM_PLANTS / 2);
                }
            }

            return plantSum;
        }

        static int CountPlants()
        {
            var plantCount = 0;
            for (var i = 0; i < MAX_NUM_PLANTS; ++i)
            {
                plantCount += sCurrentState[i];
            }
            return plantCount;
        }

        public static int NumberOfPlants(int generations)
        {
            Simulate(generations);
            return CountPlants();
        }

        static void NextGeneration()
        {
            var (minPlantPos, maxPlantPos) = FindStartEnd();
            for (var i = 0; i < MAX_NUM_PLANTS; ++i)
            {
                sNextState[i] = 0;
            }

            minPlantPos -= 10;
            if (minPlantPos < 0)
            {
                throw new InvalidProgramException($"Plant array not big enough");
            }
            minPlantPos = Math.Max(minPlantPos, 2);
            maxPlantPos += 10;
            if (maxPlantPos >= MAX_NUM_PLANTS)
            {
                throw new InvalidProgramException($"Plant array not big enough");
            }
            maxPlantPos = Math.Min(maxPlantPos, MAX_NUM_PLANTS - 2);
            for (var i = minPlantPos; i < maxPlantPos; ++i)
            {
                byte output = 0;
                for (var r = 0; r < sRulesCount; ++r)
                {
                    bool match = true;
                    for (var c = 0; c < 5; ++c)
                    {
                        if (sMatches[r, c] != sCurrentState[i + c - 2])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        output = sOutputs[r];
                        break;
                    }
                }
                sNextState[i] = output;
            }
        }

        static void InitialiseGeneration()
        {
            for (var i = 0; i < MAX_NUM_PLANTS; ++i)
            {
                sCurrentState[i] = 0;
            }
            for (var i = 0; i < sInitialStateCount; ++i)
            {
                sCurrentState[i + MAX_NUM_PLANTS / 2] = sInitialState[i];
            }

        }

        static void UpdateCurrentState()
        {
            for (var i = 0; i < MAX_NUM_PLANTS; ++i)
            {
                sCurrentState[i] = sNextState[i];
            }
        }

        static void Simulate(int generations)
        {
            InitialiseGeneration();
            for (var g = 0; g < generations; ++g)
            {
                NextGeneration();
                UpdateCurrentState();
            }
        }

        static string OutputState()
        {
            var (start, end) = FindStartEnd();
            var state = "";
            for (var i = start; i <= end; ++i)
            {
                if (sCurrentState[i] == 1)
                {
                    state += '#';
                }
                else
                {
                    state += '.';
                }
            }
            return state;
        }

        static (int m, int c) FindStableState(int maxGenerations)
        {
            var numSameGenerations = 0;
            InitialiseGeneration();
            var prevNumPlants = int.MinValue;
            var prevState = "";
            var prevM = int.MaxValue;
            var prevC = int.MaxValue;
            var prevEndC = int.MaxValue;
            var prevStart = int.MaxValue;
            var prevEnd = int.MinValue;
            for (var g = 0; g < maxGenerations; ++g)
            {
                var thisState = "";
                NextGeneration();
                UpdateCurrentState();
                var numPlants = CountPlants();
                var thisM = prevM;
                var thisC = prevC;
                var thisEndC = prevEndC;
                var thisStart = prevStart;
                var thisEnd = prevEnd;
                bool theSame = false;
                if (numPlants == prevNumPlants)
                {
                    thisState = OutputState();
                    if (thisState == prevState)
                    {
                        (thisStart, thisEnd) = FindStartEnd();
                        thisStart -= MAX_NUM_PLANTS / 2;
                        thisEnd -= MAX_NUM_PLANTS / 2;
                        // yPrev = m * gPrev + c
                        // yThis = m * gThis + c
                        // gThis = gPrev + 1
                        // yThis = m * (gPrev + 1) + c
                        // yThis = m * gPrev + c + m
                        // yThis = yPrev + m
                        // m = yThis - yPrev
                        // c = yThis - m * gThis
                        // c = yPrev - m * gPrev
                        thisM = thisStart - prevStart;
                        if (thisM == prevM)
                        {
                            var endM = thisEnd - prevEnd;
                            if (thisM == endM)
                            {
                                thisC = thisStart - thisM * g;
                                if (thisC == prevC)
                                {
                                    thisEndC = thisEnd - endM * g;
                                    if (thisEndC == prevEndC)
                                    {
                                        theSame = true;
                                        ++numSameGenerations;
                                        if (numSameGenerations > 128)
                                        {
                                            sStableState = thisState;
                                            return (thisM, thisC);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!theSame)
                {
                    numSameGenerations = 0;
                }
                prevNumPlants = numPlants;
                prevState = thisState;
                prevM = thisM;
                prevC = thisC;
                prevEndC = thisEndC;
                prevStart = thisStart;
                prevEnd = thisEnd;
            }
            throw new InvalidProgramException($"Failed to find stable state after {maxGenerations} generations");
        }

        static (int start, int end) FindStartEnd()
        {
            var start = int.MaxValue;
            var end = int.MinValue;
            for (var i = 0; i < MAX_NUM_PLANTS; ++i)
            {
                if (sCurrentState[i] == 1)
                {
                    start = Math.Min(start, i);
                    end = Math.Max(end, i);
                }
            }
            return (start, end);
        }

        public static long PredictPlantSum(long generations)
        {
            var (m, c) = FindStableState(MAX_NUM_GENERATIONS);
            var (start, end) = FindStartEnd();
            start -= MAX_NUM_PLANTS / 2;
            end -= MAX_NUM_PLANTS / 2;
            Console.WriteLine($"m:{m} c:{c} {start}:{end}");
            Console.WriteLine($"Stable:`{sStableState}`");
            var plantSum = 0L;
            var startPosition = m * (generations - 1) + c;
            for (var i = 0; i < sStableState.Length; ++i)
            {
                if (sStableState[i] == '#')
                {
                    plantSum += startPosition + i;
                }
            }
            return plantSum;
        }

        public static void Run()
        {
            Console.WriteLine("Day12 : Start");
            _ = new Program("Day12/input.txt", true);
            _ = new Program("Day12/input.txt", false);
            Console.WriteLine("Day12 : End");
        }
    }
}
