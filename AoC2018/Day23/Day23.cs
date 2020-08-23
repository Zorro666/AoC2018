using System;

/*

--- Day 23: Experimental Emergency Teleportation ---

Using your torch to search the darkness of the rocky cavern, you finally locate the man's friend: a small reindeer.

You're not sure how it got so far in this cave.
It looks sick - too sick to walk - and too heavy for you to carry all the way back.
Sleighs won't be invented for another 1500 years, of course.

The only option is experimental emergency teleportation.

You hit the "experimental emergency teleportation" button on the device and push I accept the risk on no fewer than 18 different warning messages.
Immediately, the device deploys hundreds of tiny nanobots which fly around the cavern, apparently assembling themselves into a very specific formation.
The device lists the X,Y,Z position (pos) for each nanobot as well as its signal radius (r) on its tiny screen (your puzzle input).

Each nanobot can transmit signals to any integer coordinate which is a distance away from it less than or equal to its signal radius (as measured by Manhattan distance).
Coordinates a distance away of less than or equal to a nanobot's signal radius are said to be in range of that nanobot.

Before you start the teleportation process, you should determine which nanobot is the strongest (that is, which has the largest signal radius) and then, for that nanobot, the total number of nanobots that are in range of it, including itself.

For example, given the following nanobots:

pos=<0,0,0>, r=4
pos=<1,0,0>, r=1
pos=<4,0,0>, r=3
pos=<0,2,0>, r=1
pos=<0,5,0>, r=3
pos=<0,0,3>, r=1
pos=<1,1,1>, r=1
pos=<1,1,2>, r=1
pos=<1,3,1>, r=1

The strongest nanobot is the first one (position 0,0,0) because its signal radius, 4 is the largest.
Using that nanobot's location and signal radius, the following nanobots are in or out of range:

The nanobot at 0,0,0 is distance 0 away, and so it is in range.
The nanobot at 1,0,0 is distance 1 away, and so it is in range.
The nanobot at 4,0,0 is distance 4 away, and so it is in range.
The nanobot at 0,2,0 is distance 2 away, and so it is in range.
The nanobot at 0,5,0 is distance 5 away, and so it is not in range.
The nanobot at 0,0,3 is distance 3 away, and so it is in range.
The nanobot at 1,1,1 is distance 3 away, and so it is in range.
The nanobot at 1,1,2 is distance 4 away, and so it is in range.
The nanobot at 1,3,1 is distance 5 away, and so it is not in range.

In this example, in total, 7 nanobots are in range of the nanobot with the largest signal radius.

Find the nanobot with the largest signal radius.
How many nanobots are in range of its signals?

Your puzzle answer was 652.

--- Part Two ---

Now, you just need to figure out where to position yourself so that you're actually teleported when the nanobots activate.

To increase the probability of success, you need to find the coordinate which puts you in range of the largest number of nanobots.
If there are multiple, choose one closest to your position (0,0,0, measured by manhattan distance).

For example, given the following nanobot formation:

pos=<10,12,12>, r=2
pos=<12,14,12>, r=2
pos=<16,12,12>, r=4
pos=<14,14,14>, r=6
pos=<50,50,50>, r=200
pos=<10,10,10>, r=5

Many coordinates are in range of some of the nanobots in this formation.
However, only the coordinate 12,12,12 is in range of the most nanobots: it is in range of the first five, but is not in range of the nanobot at 10,10,10.
(All other coordinates are in range of fewer than five nanobots.) This coordinate's distance from 0,0,0 is 36.

Find the coordinates that are in range of the largest number of nanobots.

What is the shortest manhattan distance between any of those points and 0,0,0?

*/

namespace Day23
{
    class Program
    {
        const int MAX_COUNT_BOTS = 1024;
        readonly private static int[] sBotsOriginX = new int[MAX_COUNT_BOTS];
        readonly private static int[] sBotsOriginY = new int[MAX_COUNT_BOTS];
        readonly private static int[] sBotsOriginZ = new int[MAX_COUNT_BOTS];
        readonly private static int[] sBotsRadius = new int[MAX_COUNT_BOTS];
        private static int sCountBots;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = CountInRange();
                Console.WriteLine($"Day23 : Result1 {result1}");
                var expected = 652;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = BestLocation();
                Console.WriteLine($"Day23 : Result2 {result2}");
                var expected = 164960498;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            sCountBots = 0;
            // pos=<0,0,0>, r=4
            foreach (var line in lines)
            {
                var tokens = line.Trim().Split(">,");
                if (tokens.Length != 2)
                {
                    throw new InvalidProgramException($"Bad input line {line} Expected 2 tokens separated by '>,' got {tokens.Length}");
                }
                var posToken = tokens[0].Trim();
                var posTokens = posToken.Split("=<");
                if (posTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Bad position token {posToken} Expected 2 tokens separated by '<=' got {posTokens.Length}");
                }
                var xyzToken = posTokens[1];
                var xyzTokens = xyzToken.Split(',');
                if (xyzTokens.Length != 3)
                {
                    throw new InvalidProgramException($"Bad xyz token {xyzToken} Expected 3 tokens separated by ',' got {xyzTokens.Length}");
                }
                var x = int.Parse(xyzTokens[0]);
                var y = int.Parse(xyzTokens[1]);
                var z = int.Parse(xyzTokens[2]);

                var radiusToken = tokens[1].Trim();
                var radiusTokens = radiusToken.Split("=");
                if (radiusTokens.Length != 2)
                {
                    throw new InvalidProgramException($"Bad radius token {radiusToken} Expected 2 tokens separated by '=' got {radiusTokens.Length}");
                }
                if (radiusTokens[0] != "r")
                {
                    throw new InvalidProgramException($"Bad radius token {radiusToken} Expected 'r' got {radiusTokens[0]}");
                }
                var r = int.Parse(radiusTokens[1]);

                sBotsOriginX[sCountBots] = x;
                sBotsOriginY[sCountBots] = y;
                sBotsOriginZ[sCountBots] = z;
                sBotsRadius[sCountBots] = r;
                ++sCountBots;
            }
        }

        private static int CountBotsInRange(int x, int y, int z, int radius)
        {
            var countInRange = 0;
            for (var b = 0; b < sCountBots; ++b)
            {
                var dx = Math.Abs(sBotsOriginX[b] - x);
                var dy = Math.Abs(sBotsOriginY[b] - y);
                var dz = Math.Abs(sBotsOriginZ[b] - z);
                if ((dx + dy + dz) <= radius)
                {
                    ++countInRange;
                }
            }

            return countInRange;
        }

        public static int CountInRange()
        {
            var maxRadius = int.MinValue;
            var maxRadiusBot = int.MinValue;
            for (var b = 0; b < sCountBots; ++b)
            {
                if (sBotsRadius[b] > maxRadius)
                {
                    maxRadiusBot = b;
                    maxRadius = sBotsRadius[b];
                }
            }
            if (maxRadiusBot == int.MinValue)
            {
                throw new InvalidProgramException($"Failed to find maximum radius bot");
            }

            var x0 = sBotsOriginX[maxRadiusBot];
            var y0 = sBotsOriginY[maxRadiusBot];
            var z0 = sBotsOriginZ[maxRadiusBot];
            var radius = sBotsRadius[maxRadiusBot];

            return CountBotsInRange(x0, y0, z0, radius);

        }

        private static int CountBotsOverlapping(int x0, int y0, int z0, int r0)
        {
            var countInRange = 0;
            for (var b = 0; b < sCountBots; ++b)
            {
                var dx = Math.Abs(sBotsOriginX[b] - x0);
                var dy = Math.Abs(sBotsOriginY[b] - y0);
                var dz = Math.Abs(sBotsOriginZ[b] - z0);
                var radius = sBotsRadius[b];
                if ((dx + dy + dz) < (radius + r0))
                {
                    ++countInRange;
                }
            }

            return countInRange;
        }

        public static int BestLocation()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var minZ = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;
            var maxZ = int.MinValue;

            for (var b = 0; b < sCountBots; ++b)
            {
                var x = sBotsOriginX[b];
                var y = sBotsOriginY[b];
                var z = sBotsOriginZ[b];
                minX = Math.Min(x, minX);
                minY = Math.Min(y, minY);
                minZ = Math.Min(z, minZ);
                maxX = Math.Max(x, maxX);
                maxY = Math.Max(y, maxY);
                maxZ = Math.Max(z, maxZ);
            }

            var step = maxX - minX;
            step = Math.Max(step, maxY - minY);
            step = Math.Max(step, maxZ - minZ);
            step = 1 << (Math.ILogB((double)step) + 1);
            var newX = 0;
            var newY = 0;
            var newZ = 0;
            var minDist = int.MaxValue;
            while (step > 0)
            {
                var maxCount = 0;
                minDist = int.MaxValue;
                for (var z = minZ; z <= maxZ; z += step)
                {
                    for (var y = minY; y <= maxY; y += step)
                    {
                        for (var x = minX; x <= maxX; x += step)
                        {
                            var count = CountBotsOverlapping(x, y, z, step);
                            if (count > maxCount)
                            {
                                var dist = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                                maxCount = count;
                                minDist = dist;
                                newX = x;
                                newY = y;
                                newZ = z;
                                //Console.WriteLine($"{x},{y},{z} dist {dist} count {count} step {step}");
                            }
                            else if (count == maxCount)
                            {
                                var dist = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    newX = x;
                                    newY = y;
                                    newZ = z;
                                }
                            }
                        }
                    }
                }
                step /= 2;
                minX = newX - step;
                maxX = newX + step;
                minY = newY - step;
                maxY = newY + step;
                minZ = newZ - step;
                maxZ = newZ + step;
            }
            return minDist;
        }

        public static void Run()
        {
            Console.WriteLine("Day23 : Start");
            _ = new Program("Day23/input.txt", true);
            _ = new Program("Day23/input.txt", false);
            Console.WriteLine("Day23 : End");
        }
    }
}
