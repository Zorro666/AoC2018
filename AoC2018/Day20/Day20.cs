using System;
using System.Collections.Generic;

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

Your puzzle answer was 4274.

--- Part Two ---

Okay, so the facility is big.

How many rooms have a shortest path from your current location that pass through at least 1000 doors?

*/

namespace Day20
{
    class Program
    {
        const int MAX_MAP_SIZE = 2048;
        const int sOriginX = MAX_MAP_SIZE / 2;
        const int sOriginY = MAX_MAP_SIZE / 2;
        const int MAX_NUM_ROOMS = 32768;

        private static readonly char[,] sMap = new char[MAX_MAP_SIZE, MAX_MAP_SIZE];

        public struct Room
        {
            public int x;
            public int y;
            public int distance;
        };

        private static readonly List<Room> sVisitedRooms = new List<Room>(MAX_NUM_ROOMS);

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            GenerateMap(lines[0]);
            OutputMap();

            if (part1)
            {
                var result1 = FurthestRoom();
                Console.WriteLine($"Day20 : Result1 {result1}");
                var expected = 4274;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = AtLeastNDoorsRooms(1000);
                Console.WriteLine($"Day20 : Result2 {result2}");
                var expected = 8547;
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

        private static void SetMap(int x, int y, char c)
        {
            if (sMap[x, y] == c)
            {
                return;
            }
            if (sMap[x, y] == '?')
            {
                sMap[x, y] = c;
                return;
            }
            throw new InvalidProgramException($"Unexpected map '{sMap[x, y]}' at [{x},{y}] new c '{c}'");
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

        private static Room Move(Room from, char direction)
        {
            var x = from.x;
            var y = from.y;

            var doorX = x;
            var doorY = y;
            char door;
            if (direction == 'N')
            {
                door = '-';
                doorY = y - 1;
                y -= 2;
            }
            else if (direction == 'S')
            {
                door = '-';
                doorY = y + 1;
                y += 2;
            }
            else if (direction == 'W')
            {
                door = '|';
                doorX = x - 1;
                x -= 2;
            }
            else if (direction == 'E')
            {
                door = '|';
                doorX = x + 1;
                x += 2;
            }
            else
            {
                throw new InvalidProgramException($"Invalid movement direction '{direction}'");
            }

            SetMap(x, y, '.');
            SetMap(doorX, doorY, door);

            if (door == '|')
            {
                SetMap(doorX, doorY - 1, '#');
                SetMap(doorX, doorY + 1, '#');
            }
            else if (door == '-')
            {
                SetMap(doorX - 1, doorY, '#');
                SetMap(doorX + 1, doorY, '#');
            }
            else
            {
                throw new InvalidProgramException($"Invalid door character '{door}'");
            }
            return new Room() { x = x, y = y, distance = from.distance + 1 };
        }

        private static Room Walk(Room start, string movement)
        {
            var countChars = movement.Length;
            if (countChars > 0)
            {
                var from = start;
                for (var i = 0; i < countChars; ++i)
                {
                    var move = movement[i];
                    var to = Move(from, move);
                    bool found = false;
                    var visitedRoomsCount = sVisitedRooms.Count;
                    for (var vri = 0; vri < visitedRoomsCount; ++vri)
                    {
                        var vr = sVisitedRooms[vri];
                        if ((vr.x == to.x) && (vr.y == to.y))
                        {
                            vr.distance = Math.Min(vr.distance, to.distance);
                            sVisitedRooms[vri] = vr;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        sVisitedRooms.Add(to);
                    }

                    from = to;
                }
                return from;
            }
            else
            {
                return start;
            }
        }

        private static List<Room> ApplyMovement(string movement, List<Room> originRooms)
        {
            var uniqueRooms = new List<Room>(originRooms.Count);
            foreach (var r in originRooms)
            {
                bool found = false;
                var uniqueRoomsCount = uniqueRooms.Count;
                for (var uri = 0; uri < uniqueRoomsCount; ++uri)
                {
                    var ur = uniqueRooms[uri];
                    if ((ur.x == r.x) && (ur.y == r.y))
                    {
                        ur.distance = Math.Min(ur.distance, r.distance);
                        uniqueRooms[uri] = ur;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    uniqueRooms.Add(r);
                }
            }

            if (movement.Length > 0)
            {
                var endingRooms = new List<Room>(MAX_NUM_ROOMS);
                foreach (var room in uniqueRooms)
                {
                    endingRooms.Add(Walk(room, movement));
                }
                return endingRooms;
            }
            else
            {
                return uniqueRooms;
            }
        }

        public static void GenerateMap(string line)
        {
            List<(int pathStart, List<Room> rooms)> currentRooms = new List<(int, List<Room>)>(MAX_NUM_ROOMS);
            List<List<Room>> futureRooms = new List<List<Room>>(MAX_NUM_ROOMS);

            sVisitedRooms.Clear();

            if (line[0] != '^')
            {
                throw new InvalidProgramException($"Invalid regexp first character '{line[0]}' expected '^'");
            }

            if (line[^1] != '$')
            {
                throw new InvalidProgramException($"Invalid regexp last character '{line[^1]}' expected '$'");
            }
            var regexp = line.Replace('^', '(').Replace('$', ')');
            ClearMap();

            // StartX,Y is depth = 0, otion = 0
            //NEWS(N|S(S|N)E)NEWS
            // ( : increase depth : num routes at depth should equal 0 : add route : movement on child node = ""
            // | : add route : movement on child node = ""
            // ) : decrease depth : append and add routes at parent depth from child depth : num routes at child depth = 0
            // NEWS : append movement to all routes at current depth and option
            for (var i = 0; i < regexp.Length; ++i)
            {
                if (i % 2000 == 0)
                {
                    Console.WriteLine($"{i}/{regexp.Length} {currentRooms.Count} {sVisitedRooms.Count}");
                }
                var c = regexp[i];
                // ( : increase depth : start new stack level
                if (c == '(')
                {
                    List<Room> newRooms = null;
                    if (currentRooms.Count > 0)
                    {
                        var (pathStart, originRooms) = currentRooms[^1];
                        var movement = regexp[pathStart..i];
                        newRooms = ApplyMovement(movement, originRooms);
                        //Console.WriteLine($"'(' OriginRoomsCount:{originRooms.Count} x:{originRooms[0].x} y:{originRooms[0].y} movement:'{movement}'");
                    }
                    else
                    {
                        newRooms = new List<Room>(1)
                        {
                            new Room() { x = sOriginX,  y = sOriginY }
                        };
                    }
                    // Append new rooms for this stack level
                    currentRooms.Add((i + 1, newRooms));
                    // Start empty list for future rooms at this stack level
                    futureRooms.Add(new List<Room>());
                    // Append new rooms for the first option at this stack level
                    currentRooms.Add((i + 1, newRooms));
                }
                // ) : decrease depth : end current stack level
                else if (c == ')')
                {
                    // Complete current option
                    var (pathStart, originRooms) = currentRooms[^1];
                    currentRooms.RemoveAt(currentRooms.Count - 1);
                    var movement = regexp[pathStart..i];
                    var newRooms = ApplyMovement(movement, originRooms);
                    //Console.WriteLine($"')' OriginRoomsCount:{originRooms.Count} x:{originRooms[0].x} y:{originRooms[0].y} movement:'{movement}'");
                    // Add the new rooms to future rooms of parent block
                    var newFutureRooms = futureRooms[^1];
                    newFutureRooms.AddRange(newRooms);
                    futureRooms.RemoveAt(futureRooms.Count - 1);
                    var uniqueRooms = new List<Room>(newFutureRooms.Count);
                    foreach (var r in newFutureRooms)
                    {
                        bool found = false;
                        var uniqueRoomsCount = uniqueRooms.Count;
                        for (var uri = 0; uri < uniqueRoomsCount; ++uri)
                        {
                            var ur = uniqueRooms[uri];
                            if ((ur.x == r.x) && (ur.y == r.y))
                            {
                                ur.distance = Math.Min(ur.distance, r.distance);
                                uniqueRooms[uri] = ur;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            uniqueRooms.Add(r);
                        }
                    }

                    if (futureRooms.Count > 0)
                    {
                        futureRooms[^1].AddRange(uniqueRooms);
                    }
                    // Add start room after parenthesis block
                    currentRooms.RemoveAt(currentRooms.Count - 1);
                    if (currentRooms.Count > 0)
                    {
                        currentRooms[^1] = (i + 1, newFutureRooms);
                    }
                }
                // | : end current option in current stack level, start new empty option
                else if (c == '|')
                {
                    // Complete current option and append to the future rooms of this stack level
                    var (pathStart, originRooms) = currentRooms[^1];
                    currentRooms.RemoveAt(currentRooms.Count - 1);
                    var movement = regexp[pathStart..i];
                    var newRooms = ApplyMovement(movement, originRooms);
                    //Console.WriteLine($"'|' OriginRoomsCount:{originRooms.Count} x:{originRooms[0].x} y:{originRooms[0].y} movement:'{movement}'");
                    futureRooms[^1].AddRange(newRooms);
                    // Add new start rooms for the next option
                    currentRooms.Add((i + 1, currentRooms[^1].rooms));
                }
            }
            sMap[sOriginX, sOriginY] = 'X';
            FillInWalls();
        }

        public static int FurthestRoom()
        {
            var maxDistance = int.MinValue;
            foreach (var r in sVisitedRooms)
            {
                maxDistance = Math.Max(maxDistance, r.distance);
            }
            return maxDistance;
        }

        private static int AtLeastNDoorsRooms(int countDoors)
        {
            var countRooms = 0;
            foreach (var r in sVisitedRooms)
            {
                if (r.distance >= countDoors)
                {
                    ++countRooms;
                }
            }
            return countRooms;
        }

        public static void OutputMap()
        {
            var mapLines = GetMap();
            foreach (var line in mapLines)
            {
                Console.WriteLine(line);
            }
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
