using System;

/*

--- Day 3: No Matter How You Slice It ---

The Elves managed to locate the chimney-squeeze prototype fabric for Santa's suit (thanks to someone who helpfully wrote its box IDs on the wall of the warehouse in the middle of the night).
Unfortunately, anomalies are still affecting them - nobody can even agree on how to cut the fabric.

The whole piece of fabric they're working on is a very large square - at least 1000 inches on each side.

Each Elf has made a claim about which area of fabric would be ideal for Santa's suit.
All claims have an ID and consist of a single rectangle with edges parallel to the edges of the fabric.
Each claim's rectangle is defined as follows:

The number of inches between the left edge of the fabric and the left edge of the rectangle.
The number of inches between the top edge of the fabric and the top edge of the rectangle.
The width of the rectangle in inches.
The height of the rectangle in inches.
A claim like #123 @ 3,2: 5x4 means that claim ID 123 specifies a rectangle 3 inches from the left edge, 2 inches from the top edge, 5 inches wide, and 4 inches tall.
Visually, it claims the square inches of fabric represented by # (and ignores the square inches of fabric represented by .) in the diagram below:

...........
...........
...#####...
...#####...
...#####...
...#####...
...........
...........
...........
The problem is that many of the claims overlap, causing two or more claims to cover part of the same areas.
For example, consider the following claims:

#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2
Visually, these claim the following areas:

........
...2222.
...2222.
.11XX22.
.11XX22.
.111133.
.111133.
........
The four square inches marked with X are claimed by both 1 and 2.
(Claim 3, while adjacent to the others, does not overlap either of them.)

If the Elves all proceed with their own plans, none of them will have enough fabric.
How many square inches of fabric are within two or more claims?
*/

namespace Day03
{
    class Program
    {
        const int MAX_GRID_SIZE = 1024;
        readonly static byte[,] sUsedCount = new byte[MAX_GRID_SIZE, MAX_GRID_SIZE];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = OverClaimed();
                Console.WriteLine($"Day03 : Result1 {result1}");
                var expected = 115242;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day03 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    sUsedCount[x, y] = 0;
                }
            }

            foreach (var line in lines)
            {
                var tokensAt = line.Trim().Split('@');
                var tokensColon = tokensAt[1].Trim().Split(':');
                var originToken = tokensColon[0];
                var dimensionsToken = tokensColon[1];

                var x0y0Tokens = originToken.Trim().Split(',');
                var x0Token = x0y0Tokens[0].Trim();
                var y0Token = x0y0Tokens[1].Trim();

                var x0 = int.Parse(x0Token);
                var y0 = int.Parse(y0Token);

                var widthHeightTokens = dimensionsToken.Trim().Split('x');
                var widthToken = widthHeightTokens[0].Trim();
                var heightToken = widthHeightTokens[1].Trim();
                var width = int.Parse(widthToken);
                var height = int.Parse(heightToken);

                for (var y = y0; y < y0 + height; ++y)
                {
                    for (var x = x0; x < x0 + width; ++x)
                    {
                        var count = sUsedCount[x, y];
                        if (count < 100)
                        {
                            ++count;
                        }
                        sUsedCount[x, y] = count;
                    }
                }
            }
        }

        public static long OverClaimed()
        {
            var count = 0L;
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    if (sUsedCount[x, y] >= 2)
                    {
                        ++count;
                    }
                }
            }
            return count;
        }

        public static void Run()
        {
            Console.WriteLine("Day03 : Start");
            _ = new Program("Day03/input.txt", true);
            _ = new Program("Day03/input.txt", false);
            Console.WriteLine("Day03 : End");
        }
    }
}
