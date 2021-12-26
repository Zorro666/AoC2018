using System;
using System.IO;

/*

--- Day 17: Reservoir Research ---

You arrive in the year 18.
If it weren't for the coat you got in 1018, you would be very cold: the North Pole base hasn't even been constructed.

Rather, it hasn't been constructed yet.
The Elves are making a little progress, but there's not a lot of liquid water in this climate, so they're getting very dehydrated.
Maybe there's more underground?

You scan a two-dimensional vertical slice of the ground nearby and discover that it is mostly sand with veins of clay.
The scan only provides data with a granularity of square meters, but it should be good enough to determine how much water is trapped there.
In the scan, x represents the distance to the right, and y represents the distance down.
There is also a spring of water near the surface at x=500, y=0.
The scan identifies which square meters are clay (your puzzle input).

For example, suppose your scan shows the following veins of clay:

x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504
Rendering clay as #, sand as ., and the water spring as +, and with x increasing to the right and y increasing downward, this becomes:

   44444455555555
   99999900000000
   45678901234567
 0 ......+.......
 1 ............#.
 2 .#..#.......#.
 3 .#..#..#......
 4 .#..#..#......
 5 .#.....#......
 6 .#.....#......
 7 .#######......
 8 ..............
 9 ..............
10 ....#.....#...
11 ....#.....#...
12 ....#.....#...
13 ....#######...

The spring of water will produce water forever.
Water can move through sand, but is blocked by clay.
Water always moves down when possible, and spreads to the left and right otherwise, filling space that has clay on both sides and falling out otherwise.

For example, if five squares of water are created, they will flow downward until they reach the clay and settle there.
Water that has come to rest is shown here as ~, while sand through which water has passed (but which is now dry again) is shown as |:

......+.......
......|.....#.
.#..#.|.....#.
.#..#.|#......
.#..#.|#......
.#....|#......
.#~~~~~#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...

Two squares of water can't occupy the same location.
If another five squares of water are created, they will settle on the first five, filling the clay reservoir a little more:

......+.......
......|.....#.
.#..#.|.....#.
.#..#.|#......
.#..#.|#......
.#~~~~~#......
.#~~~~~#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...

Water pressure does not apply in this scenario.
If another four squares of water are created, they will stay on the right side of the barrier, and no water will reach the left side:

......+.......
......|.....#.
.#..#.|.....#.
.#..#~~#......
.#..#~~#......
.#~~~~~#......
.#~~~~~#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...

At this point, the top reservoir overflows.
While water can reach the tiles above the surface of the water, it cannot settle there, and so the next five squares of water settle like this:

......+.......
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
........|.....
....#...|.#...
....#...|.#...
....#~~~~~#...
....#######...

Note especially the leftmost |: the new squares of water can reach this tile, but cannot stop there.
Instead, eventually, they all fall to the right and settle in the reservoir below.

After 10 more squares of water, the bottom reservoir is also full:

......+.......
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
........|.....
....#~~~~~#...
....#~~~~~#...
....#~~~~~#...
....#######...

Finally, while there is nowhere left for the water to settle, it can reach a few more tiles before overflowing beyond the bottom of the scanned data:

......+.......    (line not counted: above minimum y value)
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
...|||||||||..
...|#~~~~~#|..
...|#~~~~~#|..
...|#~~~~~#|..
...|#######|..
...|.......|..    (line not counted: below maximum y value)
...|.......|..    (line not counted: below maximum y value)
...|.......|..    (line not counted: below maximum y value)

How many tiles can be reached by the water? 
To prevent counting forever, ignore tiles with a y coordinate smaller than the smallest y coordinate in your scan data or larger than the largest one.

Any x coordinate is valid.

In this example, the lowest y coordinate given is 1, and the highest is 13, causing the water spring (in row 0) and the water falling off the bottom of the render (in rows 14 through infinity) to be ignored.

So, in the example above, counting both water at rest (~) and other sand tiles the water can hypothetically reach (|), the total number of tiles the water can reach is 57.

How many tiles can the water reach within the range of y values in your scan?

Your puzzle answer was 29802.

--- Part Two ---

After a very long time, the water spring will run dry. How much water will be retained?

In the example above, water that won't eventually drain out is shown as ~, a total of 29 tiles.

How many water tiles are left after the water spring stops producing water and all remaining water not at rest has drained?

*/

namespace Day17
{
    class Program
    {
        const int MAX_GRID_WIDTH = 1024;
        const int MAX_GRID_HEIGHT = 2048;
        const int MAX_NUM_SPRINGS = 64;
        readonly static char[,] sMapInitial = new char[MAX_GRID_WIDTH, MAX_GRID_HEIGHT];
        readonly static char[,] sMapCurrent = new char[MAX_GRID_WIDTH, MAX_GRID_HEIGHT];
        readonly static int[] sSpringsDownX = new int[MAX_NUM_SPRINGS];
        readonly static int[] sSpringsDownY = new int[MAX_NUM_SPRINGS];
        readonly static int[] sSpringsLeftX = new int[MAX_NUM_SPRINGS];
        readonly static int[] sSpringsLeftY = new int[MAX_NUM_SPRINGS];
        readonly static int[] sSpringsRightX = new int[MAX_NUM_SPRINGS];
        readonly static int[] sSpringsRightY = new int[MAX_NUM_SPRINGS];
        static int sNumSpringsDown;
        static int sNumSpringsLeft;
        static int sNumSpringsRight;

        static int sMinX = int.MaxValue;
        static int sMaxX = int.MinValue;
        static int sMinY = int.MaxValue;
        static int sMaxY = int.MinValue;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);
            SimulateWater();
            (var wet, var settled) = CountWater();

            if (part1)
            {
                var result1 = wet + settled;
                Console.WriteLine($"Day17 : Result1 {result1}");
                var expected = 30384;

                if (result1 != expected)
                {
                    OutputMap();
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = settled;
                Console.WriteLine($"Day17 : Result2 {result2}");
                var expected = 24479;
                if (result2 != expected)
                {
                    OutputMap();
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            for (var y = 0; y < MAX_GRID_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_GRID_WIDTH; ++x)
                {
                    sMapInitial[x, y] = '.';
                }
            }
            sMapInitial[500, 0] = '+';
            foreach (var line in lines)
            {
                // "x=495, y=2..7",
                // "y=7, x=495..501",
                var tokens = line.Trim().Split(',');
                if (tokens.Length != 2)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 2 , separated tokens got {tokens.Length}");
                }
                var valueOneTokens = tokens[0].Trim().Split('=');
                if (valueOneTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 2 = separated tokens got {valueOneTokens.Length}");
                }
                var valueTwoTokens = tokens[1].Trim().Split('=');
                if (valueTwoTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 2 = separated tokens got {valueTwoTokens.Length}");
                }

                var xMin = int.MaxValue;
                var xMax = int.MinValue;
                var yMin = int.MaxValue;
                var yMax = int.MinValue;
                var axisOne = valueOneTokens[0].Trim()[0];
                var valueOne = int.Parse(valueOneTokens[1]);
                var expectedAxisTwo = '?';
                if (axisOne == 'x')
                {
                    xMin = valueOne;
                    xMax = valueOne;
                    expectedAxisTwo = 'y';
                }
                else if (axisOne == 'y')
                {
                    yMin = valueOne;
                    yMax = valueOne;
                    expectedAxisTwo = 'x';
                }
                else
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected first value to be 'x' or 'y' got {valueOneTokens[0]}");
                }
                var axisTwo = valueTwoTokens[0].Trim()[0];
                if (axisTwo != expectedAxisTwo)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected second value to be '{expectedAxisTwo}' got {axisTwo}");
                }
                var valueTwoMinMaxTokens = valueTwoTokens[1].Trim().Split("..");
                if (valueTwoMinMaxTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 2 values separated by '..' got '{valueTwoMinMaxTokens.Length}'");
                }
                var valueTwoMin = int.Parse(valueTwoMinMaxTokens[0]);
                var valueTwoMax = int.Parse(valueTwoMinMaxTokens[1]);
                if (axisTwo == 'x')
                {
                    xMin = valueTwoMin;
                    xMax = valueTwoMax;
                }
                else if (axisTwo == 'y')
                {
                    yMin = valueTwoMin;
                    yMax = valueTwoMax;
                }
                else
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected second value to be 'x' or 'y' got {valueOneTokens[0]}");
                }
                if ((xMin < 0) || (xMin > MAX_GRID_WIDTH))
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' xMin {xMin} out-of-range {0} -> {MAX_GRID_WIDTH}");
                }
                if ((xMax < 0) || (xMax > MAX_GRID_WIDTH))
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' xMax {xMax} out-of-range {0} -> {MAX_GRID_WIDTH}");
                }
                if ((yMin < 0) || (yMin > MAX_GRID_HEIGHT))
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' yMin {yMin} out-of-range {0} -> {MAX_GRID_HEIGHT}");
                }
                if ((yMax < 0) || (yMax > MAX_GRID_HEIGHT))
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' yMax {yMax} out-of-range {0} -> {MAX_GRID_HEIGHT}");
                }
                for (var y = yMin; y <= yMax; ++y)
                {
                    for (var x = xMin; x <= xMax; ++x)
                    {
                        sMapInitial[x, y] = '#';
                    }
                }

                sMinX = Math.Min(sMinX, xMin);
                sMaxX = Math.Max(sMaxX, xMax);

                sMinY = Math.Min(sMinY, yMin);
                sMaxY = Math.Max(sMaxY, yMax);
            }
            // Water can go one cell either side of the walls
            --sMinX;
            ++sMaxX;
        }

        public static (int wet, int settled) CountWater()
        {
            int wet = 0;
            int settled = 0;
            for (var y = sMinY; y <= sMaxY; ++y)
            {
                for (var x = sMinX; x <= sMaxX; ++x)
                {
                    if (sMapCurrent[x, y] == '|')
                    {
                        ++wet;
                    }
                    else if (sMapCurrent[x, y] == '~')
                    {
                        ++settled;
                    }
                }
            }
            return (wet, settled);
        }

        private static void DropWater()
        {
            //Console.WriteLine($"B:{sNumSpringsDown} {sNumSpringsLeft} {sNumSpringsRight}");
            while (sNumSpringsDown > 0)
            {
                var startX = sSpringsDownX[0];
                var startY = sSpringsDownY[0];
                RemoveSpringDown();
                var x = startX;
                var y = startY;
                while (y <= sMaxY)
                {
                    var cell = sMapCurrent[x, y];
                    var below = sMapCurrent[x, y + 1];
                    var left = sMapCurrent[x - 1, y];
                    var right = sMapCurrent[x + 1, y];
                    if (cell == '.')
                    {
                        sMapCurrent[x, y] = '|';
                    }
                    else if (cell == '|')
                    {
                        if (below == '.')
                        {
                            ++y;
                        }
                        else if (below == '|')
                        {
                            ++y;
                        }
                        else if ((below == '#') || (below == '~'))
                        {
                            if ((left == '.') || (left == '|'))
                            {
                                AddSpringLeft(x, y);
                            }
                            if ((right == '.') || (right == '|'))
                            {
                                AddSpringRight(x, y);
                            }
                            if ((left == '#') && (right == '#'))
                            {
                                AddSpringLeft(x, y);
                                //AddSpringRight(x, y);
                            }
                            break;
                        }
                        else
                        {
                            throw new NotImplementedException($"Down: Unknown cell '{cell}' below:'{below}' left:'{left}' right:'{right}'");
                        }
                    }
                    else
                    {
                        throw new NotImplementedException($"Down: Unknown cell '{cell}' below:'{below}' left:'{left}' right:'{right}'");
                    }
                }
            }
            while (sNumSpringsLeft > 0)
            {
                var startX = sSpringsLeftX[0];
                var startY = sSpringsLeftY[0];
                RemoveSpringLeft();
                var x = startX - 1;
                var y = startY;
                while (x >= sMinX)
                {
                    var cell = sMapCurrent[x, y];
                    var below = sMapCurrent[x, y + 1];
                    var left = sMapCurrent[x - 1, y];
                    var right = sMapCurrent[x + 1, y];
                    if ((below == '#') || (below == '~'))
                    {
                        if (cell == '.')
                        {
                            sMapCurrent[x, y] = '|';
                        }
                        else if (cell == '|')
                        {
                            --x;
                        }
                        else if (cell == '#')
                        {
                            var xLeft = x + 1;
                            var xRight = int.MinValue;
                            for (var x2 = xLeft; x2 <= sMaxX; ++x2)
                            {
                                if (sMapCurrent[x2, y] == '#')
                                {
                                    xRight = x2;
                                    break;
                                }
                                if (sMapCurrent[x2, y] != '|')
                                {
                                    break;
                                }
                            }
                            if (xRight > xLeft)
                            {
                                for (var x2 = xLeft; x2 < xRight; ++x2)
                                {
                                    if (sMapCurrent[x2, y] != '|')
                                    {
                                        throw new NotImplementedException($"Left Settle: Unknown cell '{sMapCurrent[x2, y]}'");
                                    }
                                    sMapCurrent[x2, y] = '~';
                                }
                            }
                            break;
                        }
                        else if ((cell == '~') && (left == '~') && (right == '~'))
                        {
                            break;
                        }
                        else
                        {
                            throw new NotImplementedException($"Left: Unknown cell '{cell}' left '{left}' right '{right}' below '{below};");
                        }
                    }
                    else if ((below == '.') || (below == '|'))
                    {
                        AddSpringDown(x, y);
                        break;
                    }
                    else if ((cell == '~') && (left == '~') && (right == '~'))
                    {
                        break;
                    }
                    else
                    {
                        throw new NotImplementedException($"Left: Unknown cell '{cell}' left '{left}' right '{right}' below '{below};");
                    }
                }
            }
            while (sNumSpringsRight > 0)
            {
                var startX = sSpringsRightX[0];
                var startY = sSpringsRightY[0];
                RemoveSpringRight();
                var x = startX + 1;
                var y = startY;
                while (x <= sMaxX)
                {
                    var cell = sMapCurrent[x, y];
                    var below = sMapCurrent[x, y + 1];
                    var left = sMapCurrent[x - 1, y];
                    var right = sMapCurrent[x + 1, y];
                    if ((below == '#') || (below == '~'))
                    {
                        if (cell == '.')
                        {
                            sMapCurrent[x, y] = '|';
                        }
                        else if (cell == '|')
                        {
                            ++x;
                        }
                        else if (cell == '#')
                        {
                            var xLeft = int.MaxValue;
                            var xRight = x;
                            for (var x2 = xRight - 1; x2 >= sMinX; --x2)
                            {
                                if (sMapCurrent[x2, y] == '#')
                                {
                                    xLeft = x2 + 1;
                                    break;
                                }
                                if (sMapCurrent[x2, y] != '|')
                                {
                                    break;
                                }
                            }
                            if (xRight > xLeft)
                            {
                                for (var x2 = xLeft; x2 < xRight; ++x2)
                                {
                                    if (sMapCurrent[x2, y] != '|')
                                    {
                                        throw new NotImplementedException($"Right Settle: Unknown cell '{sMapCurrent[x2, y]}'");
                                    }
                                    sMapCurrent[x2, y] = '~';
                                }
                            }
                            break;
                        }
                        else if ((cell == '~') && (left == '~') && (right == '~'))
                        {
                            break;
                        }
                        else if ((cell == '~') && (left == '~') && (right == '#'))
                        {
                            break;
                        }
                        else
                        {
                            throw new NotImplementedException($"Right: Unknown cell '{cell}' left '{left}' right '{right}' below '{below};");
                        }
                    }
                    else if ((below == '.') || (below == '|'))
                    {
                        AddSpringDown(x, y);
                        break;
                    }
                    else
                    {
                        throw new NotImplementedException($"Right: Unknown cell '{cell}' left '{left}' right '{right}' below '{below};");
                    }
                }
            }
        }

        private static void RemoveSpringDown()
        {
            if (sNumSpringsDown == 0)
            {
                return;
            }
            for (var i = 1; i < sNumSpringsDown; ++i)
            {
                sSpringsDownX[i - 1] = sSpringsDownX[i];
                sSpringsDownY[i - 1] = sSpringsDownY[i];
            }
            --sNumSpringsDown;
        }

        private static void RemoveSpringLeft()
        {
            if (sNumSpringsLeft == 0)
            {
                return;
            }
            for (var i = 1; i < sNumSpringsLeft; ++i)
            {
                sSpringsLeftX[i - 1] = sSpringsLeftX[i];
                sSpringsLeftY[i - 1] = sSpringsLeftY[i];
            }
            --sNumSpringsLeft;
        }

        private static void RemoveSpringRight()
        {
            if (sNumSpringsRight == 0)
            {
                return;
            }
            for (var i = 1; i < sNumSpringsRight; ++i)
            {
                sSpringsRightX[i - 1] = sSpringsRightX[i];
                sSpringsRightY[i - 1] = sSpringsRightY[i];
            }
            --sNumSpringsRight;
        }

        private static void AddSpringDown(int x, int y)
        {
            for (var i = 0; i < sNumSpringsDown; ++i)
            {
                if ((sSpringsDownX[i] == x) && (sSpringsDownY[i] == y))
                {
                    return;
                }
            }
            sSpringsDownX[sNumSpringsDown] = x;
            sSpringsDownY[sNumSpringsDown] = y;
            ++sNumSpringsDown;
        }

        private static void AddSpringLeft(int x, int y)
        {
            for (var i = 0; i < sNumSpringsLeft; ++i)
            {
                if ((sSpringsLeftX[i] == x) && (sSpringsLeftY[i] == y))
                {
                    return;
                }
            }
            sSpringsLeftX[sNumSpringsLeft] = x;
            sSpringsLeftY[sNumSpringsLeft] = y;
            ++sNumSpringsLeft;
        }

        private static void AddSpringRight(int x, int y)
        {
            for (var i = 0; i < sNumSpringsRight; ++i)
            {
                if ((sSpringsRightX[i] == x) && (sSpringsRightY[i] == y))
                {
                    return;
                }
            }
            sSpringsRightX[sNumSpringsRight] = x;
            sSpringsRightY[sNumSpringsRight] = y;
            ++sNumSpringsRight;
        }

        private static bool IsSpringDown(int x, int y)
        {
            for (var i = 0; i < sNumSpringsDown; ++i)
            {
                if ((sSpringsDownX[i] == x) && (sSpringsDownY[i] == y))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsSpringLeft(int x, int y)
        {
            for (var i = 0; i < sNumSpringsLeft; ++i)
            {
                if ((sSpringsLeftX[i] == x) && (sSpringsLeftY[i] == y))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsSpringRight(int x, int y)
        {
            for (var i = 0; i < sNumSpringsRight; ++i)
            {
                if ((sSpringsRightX[i] == x) && (sSpringsRightY[i] == y))
                {
                    return true;
                }
            }
            return false;
        }

        private static void CopyInitialSetup()
        {
            for (var y = 0; y < MAX_GRID_HEIGHT; ++y)
            {
                for (var x = 0; x < MAX_GRID_WIDTH; ++x)
                {
                    sMapCurrent[x, y] = sMapInitial[x, y];
                }
            }

            sNumSpringsDown = 0;
            sNumSpringsLeft = 0;
            sNumSpringsRight = 0;
        }

        private static void OutputMap()
        {
#if false
            Console.WriteLine($"'v' {sNumSpringsDown} '<' {sNumSpringsLeft} '>' {sNumSpringsRight}");
            for (var y = 0; y <= sMaxY; ++y)
            {
                var line = "";
                for (var x = sMinX; x <= sMaxX; ++x)
                {
                    if (IsSpringDown(x, y) == true)
                    {
                        line += 'v';
                    }
                    else if (IsSpringLeft(x, y) == true)
                    {
                        line += '<';
                    }
                    else if (IsSpringRight(x, y) == true)
                    {
                        line += '>';
                    }
                    else
                    {
                        line += sMapCurrent[x, y];
                    }
                }
                Console.WriteLine(line);
            }
#endif // #if false
            var lines = new string[sMaxY - 0 + 1];
            for (var y = 0; y <= sMaxY; ++y)
            {
                var line = "";
                for (var x = sMinX; x <= sMaxX; ++x)
                {
                    if (IsSpringDown(x, y) == true)
                    {
                        line += 'v';
                    }
                    else if (IsSpringLeft(x, y) == true)
                    {
                        line += '<';
                    }
                    else if (IsSpringRight(x, y) == true)
                    {
                        line += '>';
                    }
                    else
                    {
                        line += sMapCurrent[x, y];
                    }
                }
                lines[y] = line;
            }
            File.WriteAllLines("/tmp/image0.txt", lines);
        }

        public static void SimulateWater()
        {
            CopyInitialSetup();
            (var wet, var settled) = CountWater();
            if (wet + settled != 0)
            {
                OutputMap();
                throw new InvalidProgramException($"Expected WaterCount to be 0 {wet + settled}");
            }
            var same = 0;
            do
            {
                var oldWet = wet;
                var oldSettled = settled;
                AddSpringDown(500, 1);
                if ((sNumSpringsDown == 0) && (sNumSpringsLeft == 0) && (sNumSpringsRight == 0))
                {
                }
                do
                {
                    DropWater();
                } while ((sNumSpringsDown != 0) || (sNumSpringsLeft != 0) || (sNumSpringsRight != 0));

                (wet, settled) = CountWater();
                if ((wet == oldWet) && (settled == oldSettled))
                {
                    ++same;
                }
                else
                {
                    same = 0;
                }
            }
            while (same < 8);
            //OutputMap();
        }

        public static void Run()
        {
            Console.WriteLine("Day17 : Start");
            _ = new Program("Day17/input.txt", true);
            _ = new Program("Day17/input.txt", false);
            Console.WriteLine("Day17 : End");
        }
    }
}

