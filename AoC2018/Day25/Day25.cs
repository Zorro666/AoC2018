using System;

/*

--- Day 25: Four-Dimensional Adventure ---

The reindeer's symptoms are getting worse, and neither you nor the white-bearded man have a solution.
At least the reindeer has a warm place to rest: a small bed near where you're sitting.

As you reach down, the reindeer looks up at you, accidentally bumping a button on your wrist-mounted device with its nose in the process - a button labeled "help".

"Hello, and welcome to the Time Travel Support Hotline! If you are lost in time and space, press 1.
If you are trapped in a time paradox, press 2.
If you need help caring for a sick reindeer, press 3.
If you--"

Beep.

A few seconds later, you hear a new voice.
"Hello; please state the nature of your reindeer." You try to describe the situation.

"Just a moment, I think I can remotely run a diagnostic scan." A beam of light projects from the device and sweeps over the reindeer a few times.

"Okay, it looks like your reindeer is very low on magical energy; it should fully recover if we can fix that.
Let me check your timeline for a source.... Got one.
There's actually a powerful source of magical energy about 1000 years forward from you, and at roughly your position, too! 
It looks like... hot chocolate? 
Anyway, you should be able to travel there to pick some up; just don't forget a mug! 
Is there anything else I can help you with today?"

You explain that your device isn't capable of going forward in time.
"I... see.
That's tricky.
Well, according to this information, your device should have the necessary hardware to open a small portal and send some hot chocolate back to you.
You'll need a list of fixed points in spacetime; I'm transmitting it to you now."

"You just need to align your device to the constellations of fixed points so that it can lock on to the destination and open the portal.
Let me look up how much hot chocolate that breed of reindeer needs."

"It says here that your particular reindeer is-- this can't be right, it says there's only one like that in the universe! But THAT means that you're--" You disconnect the call.

The list of fixed points in spacetime (your puzzle input) is a set of four-dimensional coordinates.
To align your device, acquire the hot chocolate, and save the reindeer, you just need to find the number of constellations of points in the list.

Two points are in the same constellation if their manhattan distance apart is no more than 3 or if they can form a chain of points, 
each a manhattan distance no more than 3 from the last, between the two of them.
(That is, if a point is close enough to a constellation, it "joins" that constellation.) 
For example:

 0,0,0,0
 3,0,0,0
 0,3,0,0
 0,0,3,0
 0,0,0,3
 0,0,0,6
 9,0,0,0
12,0,0,0

In the above list, the first six points form a single constellation: 0,0,0,0 is exactly distance 3 from the next four, 
and the point at 0,0,0,6 is connected to the others by being 3 away from 0,0,0,3, which is already in the constellation.
The bottom two points, 9,0,0,0 and 12,0,0,0 are in a separate constellation because no point is close enough to connect them to the first constellation.
So, in the above list, the number of constellations is 2.
(If a point at 6,0,0,0 were present, it would connect 3,0,0,0 and 9,0,0,0, merging all of the points into a single giant constellation instead.)

In this example, the number of constellations is 4:

-1,2,2,0
0,0,2,-2
0,0,0,-2
-1,2,0,0
-2,-2,-2,2
3,0,2,-1
-1,3,2,2
-1,0,-1,0
0,2,1,-2
3,0,0,0

In this one, it's 3:

1,-1,0,1
2,0,-1,0
3,2,-1,0
0,0,3,1
0,0,-1,-1
2,3,-2,0
-2,2,0,0
2,-2,0,-1
1,-1,0,-1
3,2,0,2

Finally, in this one, it's 8:

1,-1,-1,-2
-2,-2,0,1
0,2,1,3
-2,3,-2,1
0,2,3,-2
-1,-1,1,-2
0,-2,-1,0
-2,2,3,-1
1,2,2,0
-1,-2,0,-2

The portly man nervously strokes his white beard.
It's time to get that hot chocolate.

How many constellations are formed by the fixed points in spacetime?

*/

namespace Day25
{
    class Program
    {
        const int MAX_NUM_STARS = 2048;
        const int MAX_NUM_CONSTELLATIONS = 1024;
        readonly private static int[] sStarsX = new int[MAX_NUM_STARS];
        readonly private static int[] sStarsY = new int[MAX_NUM_STARS];
        readonly private static int[] sStarsZ = new int[MAX_NUM_STARS];
        readonly private static int[] sStarsW = new int[MAX_NUM_STARS];
        readonly private static int[,] sConstellations = new int[MAX_NUM_CONSTELLATIONS, MAX_NUM_STARS];
        readonly private static int[] sConstellationStarCounts = new int[MAX_NUM_CONSTELLATIONS];
        private static int sCountStars;
        private static int sCountConstellations;

        private Program(string inputFile)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            var result1 = CountConstellations();
            Console.WriteLine($"Day25 : Result1 {result1}");
            var expected = 399;
            if (result1 != expected)
            {
                throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
            }
        }

        public static void Parse(string[] lines)
        {
            sCountConstellations = 0;
            sCountStars = 0;
            if (lines.Length > MAX_NUM_STARS)
            {
                throw new InvalidProgramException($"Bad input too many stars {lines.Length} Max {MAX_NUM_STARS}");
            }
            foreach (var line in lines)
            {
                var tokens = line.Split(',');
                if (tokens.Length != 4)
                {
                    throw new InvalidProgramException($"Bad line '{line}' expected 4 tokens got {tokens.Length}");
                }
                sStarsX[sCountStars] = int.Parse(tokens[0]);
                sStarsY[sCountStars] = int.Parse(tokens[1]);
                sStarsZ[sCountStars] = int.Parse(tokens[2]);
                sStarsW[sCountStars] = int.Parse(tokens[3]);
                ++sCountStars;
            }
            for (var c = 0; c < MAX_NUM_CONSTELLATIONS; ++c)
            {
                sConstellationStarCounts[c] = 0;
            }
        }

        private static int FindConstellation(int star)
        {
            var myX = sStarsX[star];
            var myY = sStarsY[star];
            var myZ = sStarsZ[star];
            var myW = sStarsW[star];
            var myConstellation = -1;
            for (var c = 0; c < sCountConstellations; ++c)
            {
                var starCount = sConstellationStarCounts[c];
                for (var s = 0; s < starCount; ++s)
                {
                    var otherStar = sConstellations[c, s];
                    var otherX = sStarsX[otherStar];
                    var otherY = sStarsY[otherStar];
                    var otherZ = sStarsZ[otherStar];
                    var otherW = sStarsW[otherStar];
                    var distance = 0;
                    distance += Math.Abs(myX - otherX);
                    distance += Math.Abs(myY - otherY);
                    distance += Math.Abs(myZ - otherZ);
                    distance += Math.Abs(myW - otherW);
                    if (distance <= 3)
                    {
                        myConstellation = c;
                        break;
                    }
                }
                if (myConstellation != -1)
                {
                    break;
                }
            }
            return myConstellation;
        }

        private static int CountNonZeroConstellations()
        {
            var count = 0;
            for (var c = 0; c < sCountConstellations; ++c)
            {
                if (sConstellationStarCounts[c] != 0)
                {
                    ++count;
                }
            }
            return count;
        }

        public static int CountConstellations()
        {
            for (var star = 0; star < sCountStars; ++star)
            {
                var constellation = FindConstellation(star);
                if (constellation == -1)
                {
                    constellation = sCountConstellations;
                    ++sCountConstellations;
                }
                sConstellations[constellation, sConstellationStarCounts[constellation]] = star;
                ++sConstellationStarCounts[constellation];
            }

            // Merge constellations
            var count = CountNonZeroConstellations();
            int oldCount;
            do
            {
                oldCount = count;
                for (var c = 0; c < sCountConstellations; ++c)
                {
                    var starCount = sConstellationStarCounts[c];
                    var constellation = c;
                    for (var s = 0; s < starCount; ++s)
                    {
                        var star = sConstellations[c, s];
                        constellation = FindConstellation(star);
                        if (constellation != c)
                        {
                            break;
                        }
                    }
                    if (constellation != c)
                    {
                        for (var s = 0; s < starCount; ++s)
                        {
                            var star = sConstellations[c, s];
                            sConstellations[constellation, sConstellationStarCounts[constellation]] = star;
                            ++sConstellationStarCounts[constellation];
                            --sConstellationStarCounts[c];
                        }
                    }
                }
                count = CountNonZeroConstellations();
            }
            while (count != oldCount);
            return count;
        }

        public static void Run()
        {
            Console.WriteLine("Day25 : Start");
            _ = new Program("Day25/input.txt");
            Console.WriteLine("Day25 : End");
        }
    }
}
