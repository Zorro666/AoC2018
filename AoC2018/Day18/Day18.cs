using System;

/*

--- Day 18: Settlers of The North Pole ---

On the outskirts of the North Pole base construction project, many Elves are collecting lumber.

The lumber collection area is 50 acres by 50 acres; each acre can be either open ground (.), trees (|), or a lumberyard (#).
You take a scan of the area (your puzzle input).

Strange magic is at work here: each minute, the landscape looks entirely different.
In exactly one minute, an open acre can fill with trees, a wooded acre can be converted to a lumberyard, or a lumberyard can be cleared to open ground (the lumber having been sent to other projects).

The change to each acre is based entirely on the contents of that acre as well as the number of open, wooded, or lumberyard acres adjacent to it at the start of each minute.
Here, "adjacent" means any of the eight acres surrounding that acre.
(Acres on the edges of the lumber collection area might have fewer than eight adjacent acres; the missing acres aren't counted.)

In particular:

An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees. Otherwise, it becomes open.

These changes happen across all acres simultaneously, each of them using the state of all acres at the beginning of the minute and changing to their new form by the end of that same minute.
Changes that happen during the minute don't affect each other.

For example, suppose the lumber collection area is instead only 10 by 10 acres with this initial configuration:

Initial state:
.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.

After 1 minute:
.......##.
......|###
.|..|...#.
..|#||...#
..##||.|#|
...#||||..
||...|||..
|||||.||.|
||||||||||
....||..|.

After 2 minutes:
.......#..
......|#..
.|.|||....
..##|||..#
..###|||#|
...#|||||.
|||||||||.
||||||||||
||||||||||
.|||||||||

After 3 minutes:
.......#..
....|||#..
.|.||||...
..###|||.#
...##|||#|
.||##|||||
||||||||||
||||||||||
||||||||||
||||||||||

After 4 minutes:
.....|.#..
...||||#..
.|.#||||..
..###||||#
...###||#|
|||##|||||
||||||||||
||||||||||
||||||||||
||||||||||

After 5 minutes:
....|||#..
...||||#..
.|.##||||.
..####|||#
.|.###||#|
|||###||||
||||||||||
||||||||||
||||||||||
||||||||||

After 6 minutes:
...||||#..
...||||#..
.|.###|||.
..#.##|||#
|||#.##|#|
|||###||||
||||#|||||
||||||||||
||||||||||
||||||||||

After 7 minutes:
...||||#..
..||#|##..
.|.####||.
||#..##||#
||##.##|#|
|||####|||
|||###||||
||||||||||
||||||||||
||||||||||

After 8 minutes:
..||||##..
..|#####..
|||#####|.
||#...##|#
||##..###|
||##.###||
|||####|||
||||#|||||
||||||||||
||||||||||

After 9 minutes:
..||###...
.||#####..
||##...##.
||#....###
|##....##|
||##..###|
||######||
|||###||||
||||||||||
||||||||||

After 10 minutes:
.||##.....
||###.....
||##......
|##.....##
|##.....##
|##....##|
||##.####|
||#####|||
||||#|||||
||||||||||

After 10 minutes, there are 37 wooded acres and 31 lumberyards.
Multiplying the number of wooded acres by the number of lumberyards gives the total resource value after ten minutes: 37 * 31 = 1147.

What will the total resource value of the lumber collection area be after 10 minutes?

Your puzzle answer was 604884.

--- Part Two ---

This important natural resource will need to last for at least thousands of years. Are the Elves collecting this lumber sustainably?

What will the total resource value of the lumber collection area be after 1000000000 minutes?

*/

namespace Day18
{
    class Program
    {
        const int MAX_MAP_WIDTH = 50;
        const int MAX_MAP_HEIGHT = 50;
        readonly static private char[,] sMapInitial = new char[MAX_MAP_WIDTH, MAX_MAP_HEIGHT];
        readonly static private char[,] sMapCurrent = new char[MAX_MAP_WIDTH, MAX_MAP_HEIGHT];
        readonly static private char[,] sMapNext = new char[MAX_MAP_WIDTH, MAX_MAP_HEIGHT];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                Simulate(10);
                var result1 = TotalResource();
                Console.WriteLine($"Day18 : Result1 {result1}");
                var expected = 604884;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = PredictTotalResource(1000000000);
                Console.WriteLine($"Day18 : Result2 {result2}");
                var expected = 190820;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_MAP_HEIGHT; ++x)
                {
                    sMapInitial[x, y] = ' ';
                }
            }
            var height = lines.Length;
            if (height < 1)
            {
                throw new InvalidProgramException($"Invalid input need at least one line got {height}");
            }
            var width = lines[0].Trim().Length;
            for (var y = 0; y < height; ++y)
            {
                var line = lines[y].Trim();
                if (line.Length != width)
                {
                    throw new InvalidProgramException($"Invalid line '{line}' width {line.Length} expected to be {width}");
                }
                for (var x = 0; x < width; ++x)
                {
                    var cell = line[x];
                    if ((cell != '.') && (cell != '|') && (cell != '#'))
                    {
                        throw new InvalidProgramException($"Invalid line '{line}' unknown cell '{cell}' expected '.' or '|' or '#'");
                    }
                    sMapInitial[x, y] = cell;
                }
            }
        }

        private static void CopyInitialToCurrent()
        {
            for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                {
                    sMapCurrent[x, y] = sMapInitial[x, y];
                }
            }
        }

        private static void SimulateLoop()
        {
            for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                {
                    var cell = sMapCurrent[x, y];
                    var newCell = cell;
                    if (cell == ' ')
                    {
                    }
                    else
                    {
                        var countAdjacentTrees = 0;
                        var countAdjacentLumberyards = 0;
                        for (var y2 = y - 1; y2 <= y + 1; ++y2)
                        {
                            for (var x2 = x - 1; x2 <= x + 1; ++x2)
                            {
                                if ((y2 != y) || (x2 != x))
                                {
                                    var adjacentCell = ' ';
                                    if ((x2 >= 0) && (x2 < MAX_MAP_WIDTH) && (y2 >= 0) && (y2 < MAX_MAP_HEIGHT))
                                    {
                                        adjacentCell = sMapCurrent[x2, y2];
                                    }
                                    if (adjacentCell == '|')
                                    {
                                        ++countAdjacentTrees;
                                    }
                                    else if (adjacentCell == '#')
                                    {
                                        ++countAdjacentLumberyards;
                                    }
                                }
                            }
                        }
                        if (cell == '.')
                        {
                            // become trees if three or more adjacent acres contained trees.
                            // Otherwise, nothing happens.
                            if (countAdjacentTrees >= 3)
                            {
                                newCell = '|';
                            }
                        }
                        else if (cell == '|')
                        {
                            // become lumberyard if three or more adjacent acres were lumberyards.
                            // Otherwise, nothing happens.
                            if (countAdjacentLumberyards >= 3)
                            {
                                newCell = '#';
                            }
                        }
                        else if (cell == '#')
                        {
                            // remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees.
                            // Otherwise, it becomes open.
                            if ((countAdjacentLumberyards == 0) || (countAdjacentTrees == 0))
                            {
                                newCell = '.';
                            }
                        }
                        else
                        {
                            throw new InvalidProgramException($"Unknown cell '{cell}' at {x}, {y}");
                        }
                    }
                    sMapNext[x, y] = newCell;
                }
            }
        }

        public static void Simulate(long minutes)
        {
            CopyInitialToCurrent();
            for (var m = 0; m < minutes; ++m)
            {
                SimulateLoop();
                for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
                {
                    for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                    {
                        sMapCurrent[x, y] = sMapNext[x, y];
                    }
                }
            }
        }

        public static int PredictTotalResource(long minutes)
        {
            (long loopStart, long loopEnd) = FindLoopPoint();
            var loopLength = loopEnd - loopStart;
            var wrappedMinutes = minutes - loopStart;
            wrappedMinutes %= loopLength;
            wrappedMinutes += loopStart;
            Simulate(wrappedMinutes);
            return TotalResource();
        }

        private static (int loopStart, int loopEnd) FindLoopPoint()
        {
            const int MAX_NUM_MINUTES = 1024;
            var pastGrids = new char[MAX_NUM_MINUTES, MAX_MAP_WIDTH, MAX_MAP_HEIGHT];
            var pastTotals = new int[MAX_NUM_MINUTES];
            CopyInitialToCurrent();
            for (var m = 0; m < MAX_NUM_MINUTES; ++m)
            {
                SimulateLoop();
                for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
                {
                    for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                    {
                        sMapCurrent[x, y] = sMapNext[x, y];
                        pastGrids[m, x, y] = sMapCurrent[x, y];
                    }
                }
                var totalResource = TotalResource();
                pastTotals[m] = totalResource;
                for (var i = 0; i < m; ++i)
                {
                    if (pastTotals[i] != totalResource)
                    {
                        continue;
                    }
                    var match = true;
                    for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
                    {
                        for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                        {
                            if (pastGrids[i, x, y] != sMapCurrent[x, y])
                            {
                                match = false;
                                break;
                            }
                        }
                    }
                    if (match)
                    {
                        return (i, m);
                    }
                }
            }
            throw new InvalidProgramException($"Stable point not found");
        }

        public static int TotalResource()
        {
            var countTrees = 0;
            var countLumberyards = 0;
            for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                {
                    var cell = sMapCurrent[x, y];
                    if (cell == '|')
                    {
                        ++countTrees;
                    }
                    else if (cell == '#')
                    {
                        ++countLumberyards;
                    }
                }
            }
            return countTrees * countLumberyards;
        }

        public static string[] GetMap()
        {
            var width = int.MinValue;
            var height = int.MinValue;
            for (var y = 0; y < MAX_MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_MAP_WIDTH; ++x)
                {
                    if (sMapCurrent[x, y] != ' ')
                    {
                        width = Math.Max(width, x);
                        height = Math.Max(height, y);
                    }
                }
            }
            ++width;
            ++height;

            var lines = new string[height];
            for (var y = 0; y < height; ++y)
            {
                var line = "";
                for (var x = 0; x < width; ++x)
                {
                    line += sMapCurrent[x, y];
                }
                lines[y] = line;
            }
            return lines;
        }

        public static void Run()
        {
            Console.WriteLine("Day18 : Start");
            _ = new Program("Day18/input.txt", true);
            _ = new Program("Day18/input.txt", false);
            Console.WriteLine("Day18 : End");
        }
    }
}
