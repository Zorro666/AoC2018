using System;

/*

--- Day 10: The Stars Align ---

It's no use; your navigation system simply isn't capable of providing walking directions in the arctic circle, and certainly not in 1018.

The Elves suggest an alternative.
In times like these, North Pole rescue operations will arrange points of light in the sky to guide missing Elves back to base.
Unfortunately, the message is easy to miss: the points move slowly enough that it takes hours to align them, but have so much momentum that they only stay aligned for a second.
If you blink at the wrong time, it might be hours before another message appears.

You can see these points of light floating in the distance, and record their position in the sky and their velocity, the relative change in position per second (your puzzle input).
The coordinates are all given from your perspective; given enough time, those positions and velocities will move the points into a cohesive message!

Rather than wait, you decide to fast-forward the process and calculate what the points will eventually spell.

For example, suppose you note the following points:

position=< 9,  1> velocity=< 0,  2>
position=< 7,  0> velocity=<-1,  0>
position=< 3, -2> velocity=<-1,  1>
position=< 6, 10> velocity=<-2, -1>
position=< 2, -4> velocity=< 2,  2>
position=<-6, 10> velocity=< 2, -2>
position=< 1,  8> velocity=< 1, -1>
position=< 1,  7> velocity=< 1,  0>
position=<-3, 11> velocity=< 1, -2>
position=< 7,  6> velocity=<-1, -1>
position=<-2,  3> velocity=< 1,  0>
position=<-4,  3> velocity=< 2,  0>
position=<10, -3> velocity=<-1,  1>
position=< 5, 11> velocity=< 1, -2>
position=< 4,  7> velocity=< 0, -1>
position=< 8, -2> velocity=< 0,  1>
position=<15,  0> velocity=<-2,  0>
position=< 1,  6> velocity=< 1,  0>
position=< 8,  9> velocity=< 0, -1>
position=< 3,  3> velocity=<-1,  1>
position=< 0,  5> velocity=< 0, -1>
position=<-2,  2> velocity=< 2,  0>
position=< 5, -2> velocity=< 1,  2>
position=< 1,  4> velocity=< 2,  1>
position=<-2,  7> velocity=< 2, -2>
position=< 3,  6> velocity=<-1, -1>
position=< 5,  0> velocity=< 1,  0>
position=<-6,  0> velocity=< 2,  0>
position=< 5,  9> velocity=< 1, -2>
position=<14,  7> velocity=<-2,  0>
position=<-3,  6> velocity=< 2, -1>
Each line represents one point.
Positions are given as <X, Y> pairs: X represents how far left (negative) or right (positive) the point appears, while Y represents how far up (negative) or down (positive) the point appears.

At 0 seconds, each point has the position given.
Each second, each point's velocity is added to its position.
So, a point with velocity <1, -2> is moving to the right, but is moving upward twice as quickly.
If this point's initial position were <3, 9>, after 3 seconds, its position would become <6, 3>.

Over time, the points listed above would move like this:

Initially:
........#.............
................#.....
.........#.#..#.......
......................
#..........#.#.......#
...............#......
....#.................
..#.#....#............
.......#..............
......#...............
...#...#.#...#........
....#..#..#.........#.
.......#..............
...........#..#.......
#...........#.........
...#.......#..........

After 1 second:
......................
......................
..........#....#......
........#.....#.......
..#.........#......#..
......................
......#...............
....##.........#......
......#.#.............
.....##.##..#.........
........#.#...........
........#...#.....#...
..#...........#.......
....#.....#.#.........
......................
......................

After 2 seconds:
......................
......................
......................
..............#.......
....#..#...####..#....
......................
........#....#........
......#.#.............
.......#...#..........
.......#..#..#.#......
....#....#.#..........
.....#...#...##.#.....
........#.............
......................
......................
......................

After 3 seconds:
......................
......................
......................
......................
......#...#..###......
......#...#...#.......
......#...#...#.......
......#####...#.......
......#...#...#.......
......#...#...#.......
......#...#...#.......
......#...#..###......
......................
......................
......................
......................

After 4 seconds:
......................
......................
......................
............#.........
........##...#.#......
......#.....#..#......
.....#..##.##.#.......
.......##.#....#......
...........#....#.....
..............#.......
....#......#...#......
.....#.....##.........
...............#......
...............#......
......................
......................

After 3 seconds, the message appeared briefly: HI.
Of course, your message will be much longer and will take many more seconds to appear.

What message will eventually appear in the sky?

*/

namespace Day10
{
    class Program
    {
        const int MAX_NUM_POINTS = 1024;

        readonly static int[] sPosX = new int[MAX_NUM_POINTS];
        readonly static int[] sPosY = new int[MAX_NUM_POINTS];
        readonly static int[] sPosX0 = new int[MAX_NUM_POINTS];
        readonly static int[] sPosY0 = new int[MAX_NUM_POINTS];
        readonly static int[] sVelX0 = new int[MAX_NUM_POINTS];
        readonly static int[] sVelY0 = new int[MAX_NUM_POINTS];

        static int sPointsCount;
        static int sMinX;
        static int sMinY;
        static int sMaxX;
        static int sMaxY;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = -666;
                Console.WriteLine($"Day10 : Result1 {result1}");
                var expected = 280;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day10 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            sMinX = int.MaxValue;
            sMinY = int.MaxValue;
            sMaxX = int.MinValue;
            sMaxX = int.MinValue;
            // <position=< 9,  1> velocity=< 0,  2>
            foreach (var line in lines)
            {
                var tokens = line.Trim().Split('>');
                // tokens[0] = '<position=< 9,  1'
                // tokens[1] = 'velocity=< 0,  2'
                // tokens[2] = ''
                if (tokens.Length != 3)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 3 tokens after '>' split got {tokens.Length}");
                }
                if (tokens[2].Length != 0)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected final token to be '' got '{tokens[2]}'");
                }
                if (!tokens[0].Trim().StartsWith("<position=<"))
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected line to start with '<position=<' got '{tokens[0]}'");
                }
                if (!tokens[1].Trim().StartsWith("velocity=<"))
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected line to start with 'velocity=<' got '{tokens[1]}'");
                }
                var positionTokens = tokens[0].Trim().Split(',');
                if (positionTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 2 tokens for position got {positionTokens.Length}");
                }
                var velocityTokens = tokens[1].Trim().Split(',');
                if (velocityTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Unexpected line '{line}' expected 2 tokens for velocity got {velocityTokens.Length}");
                }
                var posXToken = positionTokens[0].Trim().Split('<')[2];
                var posYToken = positionTokens[1].Trim();
                sPosX0[sPointsCount] = int.Parse(posXToken);
                sPosY0[sPointsCount] = int.Parse(posYToken);
                var velXToken = velocityTokens[0].Trim().Split('<')[1];
                var velYToken = velocityTokens[1].Trim();
                sVelX0[sPointsCount] = int.Parse(velXToken);
                sVelY0[sPointsCount] = int.Parse(velYToken);

                sMinX = Math.Min(sMinX, sPosX0[sPointsCount]);
                sMaxX = Math.Max(sMaxX, sPosX0[sPointsCount]);
                sMinY = Math.Min(sMinY, sPosY0[sPointsCount]);
                sMaxY = Math.Max(sMaxY, sPosY0[sPointsCount]);
                ++sPointsCount;
            }
        }

        public static void Simulate(int nSteps)
        {
            for (var i = 0; i < sPointsCount; ++i)
            {
                sPosX[i] = sPosX0[i] + sVelX0[i] * nSteps;
                sPosY[i] = sPosY0[i] + sVelY0[i] * nSteps;
            }
        }

        public static string[] MapOutput()
        {
            var height = sMaxY - sMinY + 1;
            var width = sMaxX - sMinX + 1;
            var output = new string[height];
            for (var i = 0; i < height; ++i)
            {
                var y = sMinY + i;
                var line = "";
                for (var j = 0; j < width; ++j)
                {
                    var x = sMinX + j;
                    bool found = false;
                    for (var p = 0; p < sPointsCount; ++p)
                    {
                        if ((sPosX[p] == x) && (sPosY[p] == y))
                        {
                            found = true;
                            break;
                        }

                    }
                    if (found)
                    {
                        line += "#";
                    }
                    else
                    {
                        line += ".";
                    }
                }
                output[i] = line;
            }
            return output;
        }

        public static void Run()
        {
            Console.WriteLine("Day10 : Start");
            _ = new Program("Day10/input.txt", true);
            _ = new Program("Day10/input.txt", false);
            Console.WriteLine("Day10 : End");
        }
    }
}
