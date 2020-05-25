using System;

/*

--- Day 6: Chronal Coordinates ---

The device on your wrist beeps several times, and once again you feel like you're falling.

"Situation critical," the device announces.
"Destination indeterminate.
Chronal interference detected.
Please specify new target coordinates."

The device then produces a list of coordinates (your puzzle input).
Are they places it thinks are safe or dangerous? It recommends you check manual page 729.
The Elves did not give you a manual.

If they're dangerous, maybe you can minimize the danger by finding the coordinate that gives the largest distance from the other points.

Using only the Manhattan distance, determine the area around each coordinate by counting the number of integer X,Y locations that are closest to that coordinate (and aren't tied in distance to any other coordinate).

Your goal is to find the size of the largest area that isn't infinite.
For example, consider the following list of coordinates:

1, 1
1, 6
8, 3
3, 4
5, 5
8, 9

If we name these coordinates A through F, we can draw them on a grid, putting 0,0 at the top left:

..........
.A........
..........
........C.
...D......
.....E....
.B........
..........
..........
........F.
This view is partial - the actual grid extends infinitely in all directions.
Using the Manhattan distance, each location's closest coordinate can be determined, shown here in lowercase:

aaaaa.cccc
aAaaa.cccc
aaaddecccc
aadddeccCc
..dDdeeccc
bb.deEeecc
bBb.eeee..
bbb.eeefff
bbb.eeffff
bbb.ffffFf
Locations shown as . are equally far from two or more coordinates, and so they don't count as being closest to any.

In this example, the areas of coordinates A, B, C, and F are infinite - while not shown here, their areas extend forever outside the visible grid.
However, the areas of coordinates D and E are finite: D is closest to 9 locations, and E is closest to 17 (both including the coordinate's location itself).
Therefore, in this example, the size of the largest area is 17.

What is the size of the largest area that isn't infinite?

Your puzzle answer was 3722.

--- Part Two ---

On the other hand, if the coordinates are safe, maybe the best you can do is try to find a region near as many coordinates as possible.

For example, suppose you want the sum of the Manhattan distance to all of the coordinates to be less than 32.
For each location, add up the distances to all of the given coordinates; if the total of those distances is less than 32, that location is within the desired region.
Using the same coordinates as above, the resulting region looks like this:

..........
.A........
..........
...###..C.
..#D###...
..###E#...
.B.###....
..........
..........
........F.
x
In particular, consider the highlighted location 4,3 located at the top middle of the region.
Its calculation is as follows, where abs() is the absolute value function:

Distance to coordinate A: abs(4-1) + abs(3-1) =  5
Distance to coordinate B: abs(4-1) + abs(3-6) =  6
Distance to coordinate C: abs(4-8) + abs(3-3) =  4
Distance to coordinate D: abs(4-3) + abs(3-4) =  2
Distance to coordinate E: abs(4-5) + abs(3-5) =  3
Distance to coordinate F: abs(4-8) + abs(3-9) = 10
Total distance: 5 + 6 + 4 + 2 + 3 + 10 = 30
Because the total distance to all coordinates (30) is less than 32, the location is within the region.

This region, which also includes coordinates D and E, has a total size of 16.

Your actual region will need to be much larger than this example, though, instead including all locations with a total distance of less than 10000.

What is the size of the region containing all locations which have a total distance to all given coordinates of less than 10000?

*/

namespace Day06
{
    class Program
    {
        const int MAX_GRID_SIZE = 1024;
        const int ORIGIN_OFFSET = MAX_GRID_SIZE / 2;
        const int MAX_LOCATION_COUNT = 1024;

        readonly static int[,] sClosestPoints = new int[MAX_GRID_SIZE, MAX_GRID_SIZE];
        readonly static int[,] sClosestDistance = new int[MAX_GRID_SIZE, MAX_GRID_SIZE];

        static int sLocationCount;
        readonly static int[] sX0s = new int[MAX_LOCATION_COUNT];
        readonly static int[] sY0s = new int[MAX_LOCATION_COUNT];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = LargestFiniteArea();
                Console.WriteLine($"Day06 : Result1 {result1}");
                var expected = 3722;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = AreaWithin(10000);
                Console.WriteLine($"Day06 : Result2 {result2}");
                var expected = 44634;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            if (lines.Length >= MAX_LOCATION_COUNT)
            {
                throw new InvalidProgramException($"Too many locations {lines.Length} MAX:{MAX_LOCATION_COUNT}");
            }
            sLocationCount = 0;

            var l = 0;
            foreach (var line in lines)
            {
                //`X0, Y0`
                var xyTokens = line.Trim().Split(',');
                sX0s[l] = int.Parse(xyTokens[0]) + ORIGIN_OFFSET;
                sY0s[l] = int.Parse(xyTokens[1]) + ORIGIN_OFFSET;

                if ((sX0s[l] < 0) || (sX0s[l] >= MAX_GRID_SIZE))
                {
                    throw new InvalidProgramException($"Grid not large enough for x0 {sX0s[l] - ORIGIN_OFFSET} MAX:{MAX_GRID_SIZE - ORIGIN_OFFSET}");
                }
                if ((sY0s[l] < 0) || (sY0s[l] >= MAX_GRID_SIZE))
                {
                    throw new InvalidProgramException($"Grid not large enough for y0 {sY0s[l] - ORIGIN_OFFSET} MAX:{MAX_GRID_SIZE - ORIGIN_OFFSET}");
                }
                ++l;
            }

            sLocationCount = l;
        }

        static void LogClosestPoints()
        {
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    Console.Write($"{(sClosestPoints[x, y] >= 0 ? (char)('A' + sClosestPoints[x, y]) : '.')}");
                }
                Console.WriteLine($"");
            }
        }

        public static int LargestFiniteArea()
        {
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    sClosestDistance[x, y] = int.MaxValue;
                    sClosestPoints[x, y] = int.MaxValue;
                }
            }

            for (var l = 0; l < sLocationCount; ++l)
            {
                var x0 = sX0s[l];
                var y0 = sY0s[l];
                for (var y = 0; y < MAX_GRID_SIZE; ++y)
                {
                    for (var x = 0; x < MAX_GRID_SIZE; ++x)
                    {
                        var distance = Math.Abs(x - x0) + Math.Abs(y - y0);
                        var oldDistance = sClosestDistance[x, y];
                        if (distance < oldDistance)
                        {
                            sClosestPoints[x, y] = l;
                            sClosestDistance[x, y] = distance;
                        }
                        if (distance == oldDistance)
                        {
                            sClosestPoints[x, y] = -l;
                        }
                    }
                }
            }

            var maxFiniteCount = int.MinValue;
            for (var l = 0; l < sLocationCount; ++l)
            {
                var finiteCount = 0;
                var x0 = sX0s[l];
                var y0 = sY0s[l];
                bool infinite = false;
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    var y = 0;
                    if (sClosestPoints[x, y] < 0)
                    {
                        continue;
                    }
                    if (sClosestPoints[x, y] == l)
                    {
                        infinite = true;
                        break;
                    }
                    var distance = Math.Abs(x - x0) + Math.Abs(y - y0);
                    var oldDistance = sClosestDistance[x, y];
                    if (distance == oldDistance)
                    {
                        infinite = true;
                        break;
                    }

                    y = MAX_GRID_SIZE - 1;
                    if (sClosestPoints[x, y] < 0)
                    {
                        continue;
                    }
                    if (sClosestPoints[x, y] == l)
                    {
                        infinite = true;
                        break;
                    }
                    distance = Math.Abs(x - x0) + Math.Abs(y - y0);
                    oldDistance = sClosestDistance[x, y];
                    if (distance == oldDistance)
                    {
                        infinite = true;
                        break;
                    }
                }
                if (infinite)
                {
                    continue;
                }
                for (var y = 0; y < MAX_GRID_SIZE; ++y)
                {
                    var x = 0;
                    if (sClosestPoints[x, y] < 0)
                    {
                        continue;
                    }
                    if (sClosestPoints[x, y] == l)
                    {
                        infinite = true;
                        break;
                    }
                    var distance = Math.Abs(x - x0) + Math.Abs(y - y0);
                    var oldDistance = sClosestDistance[x, y];
                    if (distance == oldDistance)
                    {
                        infinite = true;
                        break;
                    }

                    x = MAX_GRID_SIZE - 1;
                    if (sClosestPoints[x, y] < 0)
                    {
                        continue;
                    }
                    if (sClosestPoints[x, y] == l)
                    {
                        infinite = true;
                        break;
                    }
                    distance = Math.Abs(x - x0) + Math.Abs(y - y0);
                    oldDistance = sClosestDistance[x, y];
                    if (distance == oldDistance)
                    {
                        infinite = true;
                        break;
                    }
                }
                if (infinite)
                {
                    continue;
                }
                for (var y = 1; y < MAX_GRID_SIZE - 1; ++y)
                {
                    for (var x = 1; x < MAX_GRID_SIZE - 1; ++x)
                    {
                        if (sClosestPoints[x, y] == l)
                        {
                            ++finiteCount;
                        }
                    }
                }
                maxFiniteCount = Math.Max(maxFiniteCount, finiteCount);
            }
            return maxFiniteCount;
        }

        public static int AreaWithin(int maxTotalDistance)
        {
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    sClosestDistance[x, y] = int.MaxValue;
                    sClosestPoints[x, y] = int.MaxValue;
                }
            }

            var areaWithin = 0;
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    var totalDistance = 0;
                    for (var l = 0; l < sLocationCount; ++l)
                    {
                        var x0 = sX0s[l];
                        var y0 = sY0s[l];
                        var distance = Math.Abs(x - x0) + Math.Abs(y - y0);
                        totalDistance += distance;
                        if (totalDistance >= maxTotalDistance)
                        {
                            break;
                        }
                    }
                    if (totalDistance < maxTotalDistance)
                    {
                        sClosestDistance[x, y] = 1;
                        ++areaWithin;
                    }
                    else
                    {
                        sClosestDistance[x, y] = 0;
                    }
                }
            }
            return areaWithin;
        }

        public static void Run()
        {
            Console.WriteLine("Day06 : Start");
            _ = new Program("Day06/input.txt", true);
            _ = new Program("Day06/input.txt", false);
            Console.WriteLine("Day06 : End");
        }
    }
}
