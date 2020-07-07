using System;

/*

--- Day 20: A Regular Map ---

While you were learning about instruction pointers, the Elves made considerable progress.
When you look up, you discover that the North Pole base construction project has completely surrounded you.

The area you are in is made up entirely of rooms and doors.
The rooms are arranged in a grid, and rooms only connect to adjacent rooms when a door is present between them.

For example, drawing rooms as ., walls as #, doors as | or -, your current position as X, and where north is up, the area you're in might look like this:

#####
#.|.#
#-###
#.|X#
#####

You get the attention of a passing construction Elf and ask for a map.
"I don't have time to draw out a map of this place - it's huge. Instead, I can give you directions to every room in the facility!" 
He writes down some directions on a piece of parchment and runs off.
In the example above, the instructions might have been ^WNE$, a regular expression or "regex" (your puzzle input).

The regex matches routes (like WNE for "west, north, east") that will take you from your current room through various doors in the facility.
In aggregate, the routes will take you through every door in the facility at least once; mapping out all of these routes will let you build a proper map and find your way around.

^ and $ are at the beginning and end of your regex; these just mean that the regex doesn't match anything outside the routes it describes.
(Specifically, ^ matches the start of the route, and $ matches the end of it.) These characters will not appear elsewhere in the regex.

The rest of the regex matches various sequences of the characters N (north), S (south), E (east), and W (west).
In the example above, ^WNE$ matches only one route, WNE, which means you can move west, then north, then east from your current position.
Sequences of letters like this always match that exact route in the same order.

Sometimes, the route can branch.
A branch is given by a list of options separated by pipes (|) and wrapped in parentheses.
So, ^N(E|W)N$ contains a branch: after going north, you must choose to go either east or west before finishing your route by going north again.
By tracing out the possible routes after branching, you can determine where the doors are and, therefore, where the rooms are in the facility.

For example, consider this regex: ^ENWWW(NEEE|SSE(EE|N))$

This regex begins with ENWWW, which means that from your current position, all routes must begin by moving east, north, and then west three times, in that order.
After this, there is a branch.
Before you consider the branch, this is what you know about the map so far, with doors you aren't sure about marked with a ?:

#?#?#?#?#
?.|.|.|.?
#?#?#?#-#
    ?X|.?
    #?#?#

After this point, there is (NEEE|SSE(EE|N)).
This gives you exactly two options: NEEE and SSE(EE|N).
By following NEEE, the map now looks like this:

#?#?#?#?#
?.|.|.|.?
#-#?#?#?#
?.|.|.|.?
#?#?#?#-#
    ?X|.?
    #?#?#

Now, only SSE(EE|N) remains.
Because it is in the same parenthesized group as NEEE, it starts from the same room NEEE started in.
It states that starting from that point, there exist doors which will allow you to move south twice, then east; this ends up at another branch.
After that, you can either move east twice or north once.
This information fills in the rest of the doors:

#?#?#?#?#
?.|.|.|.?
#-#?#?#?#
?.|.|.|.?
#-#?#?#-#
?.?.?X|.?
#-#-#?#?#
?.|.|.|.?
#?#?#?#?#

Once you've followed all possible routes, you know the remaining unknown parts are all walls, producing a finished map of the facility:

#########
#.|.|.|.#
#-#######
#.|.|.|.#
#-#####-#
#.#.#X|.#
#-#-#####
#.|.|.|.#
#########

Sometimes, a list of options can have an empty option, like (NEWS|WNSE|).
This means that routes at this point could effectively skip the options in parentheses and move on immediately.
For example, consider this regex and the corresponding map:

^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$

###########
#.|.#.|.#.#
#-###-#-#-#
#.|.|.#.#.#
#-#####-#-#
#.#.#X|.#.#
#-#-#####-#
#.#.|.|.|.#
#-###-###-#
#.|.|.#.|.#
###########

This regex has one main route which, at three locations, can optionally include additional detours and be valid: (NEWS|), (WNSE|), and (SWEN|).
Regardless of which option is taken, the route continues from the position it is left at after taking those steps.
So, for example, this regex matches all of the following routes (and more that aren't listed here):

ENNWSWWSSSEENEENNN
ENNWSWWNEWSSSSEENEENNN
ENNWSWWNEWSSSSEENEESWENNNN
ENNWSWWSSSEENWNSEEENNN

By following the various routes the regex matches, a full map of all of the doors and rooms in the facility can be assembled.

To get a sense for the size of this facility, you'd like to determine which room is furthest from you: specifically, you would like to find the room for which the shortest path to that room would require passing through the most doors.

In the first example (^WNE$), this would be the north-east corner 3 doors away.
In the second example (^ENWWW(NEEE|SSE(EE|N))$), this would be the south-east corner 10 doors away.
In the third example (^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$), this would be the north-east corner 18 doors away.
Here are a few more examples:

Regex: ^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$
Furthest room requires passing 23 doors

#############
#.|.|.|.|.|.#
#-#####-###-#
#.#.|.#.#.#.#
#-#-###-#-#-#
#.#.#.|.#.|.#
#-#-#-#####-#
#.#.#.#X|.#.#
#-#-#-###-#-#
#.|.#.|.#.#.#
###-#-###-#-#
#.|.#.|.|.#.#
#############

Regex: ^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$
Furthest room requires passing 31 doors

###############
#.|.|.|.#.|.|.#
#-###-###-#-#-#
#.|.#.|.|.#.#.#
#-#########-#-#
#.#.|.|.|.|.#.#
#-#-#########-#
#.#.#.|X#.|.#.#
###-#-###-#-#-#
#.|.#.#.|.#.|.#
#-###-#####-###
#.|.#.|.|.#.#.#
#-#-#####-#-#-#
#.#.|.|.|.#.|.#
###############

What is the largest number of doors you would be required to pass through to reach a room? 
That is, find the room for which the shortest path from your starting location to that room would require passing through the most doors; what is the fewest doors you can pass through to reach it?

*/

namespace Day20
{
    class Program
    {
        const int MAX_MAP_SIZE = 2048;
        const int sOriginX = MAX_MAP_SIZE / 2;
        const int sOriginY = MAX_MAP_SIZE / 2;

        private static readonly char[,] sMap = new char[MAX_MAP_SIZE, MAX_MAP_SIZE];
        private static int sCurrentX;
        private static int sCurrentY;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            GenerateMap(lines[0]);

            if (part1)
            {
                var result1 = FurthestRoom();
                Console.WriteLine($"Day20 : Result1 {result1}");
                var expected = 280;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day20 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        private static void ClearMap()
        {
            for (var y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    sMap[x, y] = '?';
                }
            }
        }

        private static void Move(string movement)
        {
            foreach (var c in movement)
            {
                var doorX = sCurrentX;
                var doorY = sCurrentY;
                var door = '?';
                if (c == 'N')
                {
                    door = '-';
                    doorY = sCurrentY - 1;
                    sCurrentY -= 2;
                }
                else if (c == 'S')
                {
                    door = '-';
                    doorY = sCurrentY + 1;
                    sCurrentY += 2;
                }
                else if (c == 'W')
                {
                    door = '|';
                    doorX = sCurrentX - 1;
                    sCurrentX -= 2;
                }
                else if (c == 'E')
                {
                    door = '|';
                    doorX = sCurrentX + 1;
                    sCurrentX += 2;
                }
                else
                {
                    throw new InvalidProgramException($"Invalid movement character '{c}'");
                }

                sMap[sCurrentX, sCurrentY] = '.';
                sMap[doorX, doorY] = door;

                if (door == '|')
                {
                    sMap[doorX, doorY - 1] = '#';
                    sMap[doorX, doorY + 1] = '#';
                }
                else if (door == '-')
                {
                    sMap[doorX - 1, doorY] = '#';
                    sMap[doorX + 1, doorY] = '#';
                }
                else
                {
                    throw new InvalidProgramException($"Invalid door character '{door}'");
                }
            }
        }

        private static void FillInWalls()
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            for (var y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] != '?')
                    {
                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
            for (var y = minY; y <= maxY; ++y)
            {
                for (var x = minX; x <= maxX; ++x)
                {
                    if ((sMap[x, y] == '.') || (sMap[x, y] == '|') || (sMap[x, y] == '-') || (sMap[x, y] == 'X'))
                    {
                        for (var y2 = y - 1; y2 <= y + 1; ++y2)
                        {
                            for (var x2 = x - 1; x2 <= x + 1; ++x2)
                            {
                                if (sMap[x2, y2] == '?')
                                {
                                    sMap[x2, y2] = '#';
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void GenerateMap(string line)
        {
            if (line[0] != '^')
            {
                throw new InvalidProgramException($"Invalid regexp first character '{line[0]}' expected '^'");
            }

            if (line[^1] != '$')
            {
                throw new InvalidProgramException($"Invalid regexp last character '{line[^1]}' expected '$'");
            }
            var regexp = line[1..^1];

            ClearMap();

            sCurrentX = sOriginX;
            sCurrentY = sOriginY;
            sMap[sCurrentX, sCurrentY] = 'X';
            // '(' : start sequence / set of options
            // ')' : end sequence / set of options
            // '|' : an optional branch
            var movement = "";
            for (var i = 0; i < regexp.Length; ++i)
            {
                var c = regexp[i];
                if (c == '(')
                {
                    //StartNewSequence();
                    // increase stack depth
                }
                else if (c == ')')
                {
                    //EndCurrentSequence();
                    // decrease stack depth
                }
                else if (c == '|')
                {
                    //StartBranch();
                    // finish current branch
                }
                else if (c == 'N')
                {
                    movement += c;
                }
                else if (c == 'S')
                {
                    movement += c;
                }
                else if (c == 'W')
                {
                    movement += c;
                }
                else if (c == 'E')
                {
                    movement += c;
                }
                else
                {
                    throw new InvalidProgramException($"Unknown character '{c}'");
                }
            }
            Move(movement);
            FillInWalls();
        }

        public static int FurthestRoom()
        {
            throw new NotImplementedException();
        }

        public static string[] GetMap()
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            for (var y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] == '#')
                    {
                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
            var height = maxY - minY + 1;
            var output = new string[height];
            var i = 0;
            for (var y = minY; y <= maxY; ++y)
            {
                var line = "";
                for (var x = minX; x <= maxX; ++x)
                {
                    line += sMap[x, y];
                }
                output[i] = line;
                Console.WriteLine(line);
                ++i;
            }
            return output;
        }

        public static void Run()
        {
            Console.WriteLine("Day20 : Start");
            _ = new Program("Day20/input.txt", true);
            _ = new Program("Day20/input.txt", false);
            Console.WriteLine("Day20 : End");
        }
    }
}
