using System;

/*

--- Day 22: Mode Maze ---

This is it, your final stop: the year -483.
It's snowing and dark outside; the only light you can see is coming from a small cottage in the distance.
You make your way there and knock on the door.

A portly man with a large, white beard answers the door and invites you inside.
For someone living near the North Pole in -483, he must not get many visitors, but he doesn't act surprised to see you.
Instead, he offers you some milk and cookies.

After talking for a while, he asks a favor of you.
His friend hasn't come back in a few hours, and he's not sure where he is.
Scanning the region briefly, you discover one life signal in a cave system nearby; his friend must have taken shelter there.
The man asks if you can go there to retrieve his friend.

The cave is divided into square regions which are either dominantly rocky, narrow, or wet (called its type).
Each region occupies exactly one coordinate in X,Y format where X and Y are integers and zero or greater. (Adjacent regions can be the same type.)

The scan (your puzzle input) is not very detailed: it only reveals the depth of the cave system and the coordinates of the target.
However, it does not reveal the type of each region.
The mouth of the cave is at 0,0.

The man explains that due to the unusual geology in the area, there is a method to determine any region's type based on its erosion level.
The erosion level of a region can be determined from its geologic index.
The geologic index can be determined using the first rule that applies from the list below:

The region at 0,0 (the mouth of the cave) has a geologic index of 0.
The region at the coordinates of the target has a geologic index of 0.
If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.
If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271.
Otherwise, the region's geologic index is the result of multiplying the erosion levels of the regions at X-1,Y and X,Y-1.
A region's erosion level is its geologic index plus the cave system's depth, all modulo 20183.
Then:

If the erosion level modulo 3 is 0, the region's type is rocky.
If the erosion level modulo 3 is 1, the region's type is wet.
If the erosion level modulo 3 is 2, the region's type is narrow.
For example, suppose the cave system's depth is 510 and the target's coordinates are 10,10.
Using % to represent the modulo operator, the cavern would look as follows:

At 0,0, the geologic index is 0.
The erosion level is (0 + 510) % 20183 = 510.
The type is 510 % 3 = 0, rocky.

At 1,0, because the Y coordinate is 0, the geologic index is 1 * 16807 = 16807.
The erosion level is (16807 + 510) % 20183 = 17317.
The type is 17317 % 3 = 1, wet.

At 0,1, because the X coordinate is 0, the geologic index is 1 * 48271 = 48271.
The erosion level is (48271 + 510) % 20183 = 8415.
The type is 8415 % 3 = 0, rocky.

At 1,1, neither coordinate is 0 and it is not the coordinate of the target, so the geologic index is the erosion level of 0,1 (8415) times the erosion level of 1,0 (17317), 8415 * 17317 = 145722555.
The erosion level is (145722555 + 510) % 20183 = 1805.
The type is 1805 % 3 = 2, narrow.

At 10,10, because they are the target's coordinates, the geologic index is 0.
The erosion level is (0 + 510) % 20183 = 510.
The type is 510 % 3 = 0, rocky.
Drawing this same cave system with rocky as ., wet as =, narrow as |, the mouth as M, the target as T, with 0,0 in the top-left corner, X increasing to the right, and Y increasing downward, the top-left corner of the map looks like this:

M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Before you go in, you should determine the risk level of the area.

For the rectangle that has a top-left corner of region 0,0 and a bottom-right corner of the region containing the target, add up the risk level of each individual region: 0 for rocky regions, 1 for wet regions, and 2 for narrow regions.

In the cave system above, because the mouth is at 0,0 and the target is at 10,10, adding up the risk level of all regions with an X coordinate from 0 to 10 and a Y coordinate from 0 to 10, this total is 114.

What is the total risk level for the smallest rectangle that includes 0,0 and the target's coordinates?

Your puzzle answer was 5637.

--- Part Two ---

Okay, it's time to go rescue the man's friend.

As you leave, he hands you some tools: a torch and some climbing gear.
You can't equip both tools at once, but you can choose to use neither.

Tools can only be used in certain regions:

In rocky regions, you can use the climbing gear or the torch.
You cannot use neither (you'll likely slip and fall).

In wet regions, you can use the climbing gear or neither tool.
You cannot use the torch (if it gets wet, you won't have a light source).

In narrow regions, you can use the torch or neither tool.
You cannot use the climbing gear (it's too bulky to fit).

You start at 0,0 (the mouth of the cave) with the torch equipped and must reach the target coordinates as quickly as possible.
The regions with negative X or Y are solid rock and cannot be traversed.
The fastest route might involve entering regions beyond the X or Y coordinate of the target.

You can move to an adjacent region (up, down, left, or right; never diagonally) if your currently equipped tool allows you to enter that region.
Moving to an adjacent region takes one minute.
(For example, if you have the torch equipped, you can move between rocky and narrow regions, but cannot enter wet regions.)

You can change your currently equipped tool or put both away if your new equipment would be valid for your current region.
Switching to using the climbing gear, torch, or neither always takes seven minutes, regardless of which tools you start with.
(For example, if you are in a rocky region, you can switch from the torch to the climbing gear, but you cannot switch to neither.)

Finally, once you reach the target, you need the torch equipped before you can find him in the dark.
The target is always in a rocky region, so if you arrive there with climbing gear equipped, you will need to spend seven minutes switching to your torch.

For example, using the same cave system as above, starting in the top left corner (0,0) and moving to the bottom right corner (the target, 10,10) as quickly as possible, one possible route is as follows, with your current position marked X:

Initially:
X=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Down:
M=.|=.|.|=.|=|=.
X|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Right:
M=.|=.|.|=.|=|=.
.X=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Switch from using the torch to neither tool:
M=.|=.|.|=.|=|=.
.X=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Right 3:
M=.|=.|.|=.|=|=.
.|=|X|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Switch from using neither tool to the climbing gear:
M=.|=.|.|=.|=|=.
.|=|X|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Down 7:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..X==..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Right:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..=X=..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Down 3:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||.X.|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Right:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||..X|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Down:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.X..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Right 4:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===T===||
=|||...|==..|=.|
=.=|=.=..=X||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Up 2:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===X===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

Switch from using the climbing gear to the torch:
M=.|=.|.|=.|=|=.
.|=|=|||..|.=...
.==|....||=..|==
=.|....|.==.|==.
=|..==...=.|==..
=||.=.=||=|=..|=
|.=.===|||..=..|
|..==||=.|==|===
.=..===..=|.|||.
.======|||=|=.|=
.===|=|===X===||
=|||...|==..|=.|
=.=|=.=..=.||==|
||=|=...|==.=|==
|=.=||===.|||===
||.|==.|.|.||=||

This is tied with other routes as the fastest way to reach the target: 45 minutes.

In it, 21 minutes are spent switching tools (three times, seven minutes each) and the remaining 24 minutes are spent moving.

What is the fewest number of minutes you can take to reach the target?

*/

namespace Day22
{
    class Program
    {
        public struct Node
        {
            public int x;
            public int y;
            public int time;
            public int torch;
            public int climbingGear;
        };

        const int MAX_MAP_SIZE = 1024;
        const int MAX_COUNT_NODES = 2 * MAX_MAP_SIZE * MAX_MAP_SIZE;
        const int SWITCH_GEAR_TIME = 7;
        const int MOVE_TIME = 1;
        readonly private static int[,] sErosion = new int[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly private static int[,,,] sVisited = new int[2, 2, MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly private static Node[] sNodes = new Node[MAX_COUNT_NODES];
        private static int sWidth;
        private static int sHeight;
        private static int sDepth;
        private static int sTargetX;
        private static int sTargetY;
        private static int sNodeStart;
        private static int sNodeCount;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = RiskLevel();
                Console.WriteLine($"Day22 : Result1 {result1}");
                var expected = 11810;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = ShortestTime();
                Console.WriteLine($"Day22 : Result2 {result2}");
                var expected = 1015;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            // "depth: 510",
            // "target: 10, 10"
            if (lines.Length != 2)
            {
                throw new InvalidProgramException($"Expected 2 lines of input got {lines.Length}");
            }
            if (!lines[0].StartsWith("depth: "))
            {
                throw new InvalidProgramException($"Expected first line to start with 'depth: ' line:'{lines[0]}'");
            }
            if (!lines[1].StartsWith("target: "))
            {
                throw new InvalidProgramException($"Expected second line to start with 'target: ' line:'{lines[1]}'");
            }
            sDepth = int.Parse(lines[0].Substring("depth: ".Length));
            var tokens = lines[1].Substring("target: ".Length).Split(',');
            sTargetX = int.Parse(tokens[0]);
            sTargetY = int.Parse(tokens[1]);

            Console.WriteLine($"Depth: {sDepth}");
            Console.WriteLine($"Target: {sTargetX},{sTargetY}");
        }

        private static void ComputeErosionMap(int width, int height)
        {
            sWidth = width;
            sHeight = height;
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    // The region at the coordinates of the target has a geologic index of 0.
                    // If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.
                    // If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271.
                    // Otherwise, the region's geologic index is the result of multiplying the erosion levels of the regions at X-1,Y and X,Y-1.
                    // A region's erosion level is its geologic index plus the cave system's depth, all modulo 20183.
                    int geologicIndex;
                    if (y == 0)
                    {
                        geologicIndex = x * 16807;
                    }
                    else if (x == 0)
                    {
                        geologicIndex = y * 48271;
                    }
                    else if ((x == sTargetX) && (y == sTargetY))
                    {
                        geologicIndex = 0;
                    }
                    else
                    {
                        geologicIndex = sErosion[x - 1, y] * sErosion[x, y - 1];
                    }
                    var erosionLevel = (geologicIndex + sDepth) % 20183;
                    sErosion[x, y] = erosionLevel;
                }
            }
        }

        private static int TimeToMove(ref int torch, ref int climbingGear, int fromX, int fromY, int toX, int toY)
        {
            var deltaTime = MOVE_TIME;
            var currentCellType = sErosion[fromX, fromY] % 3;
            var newCellType = sErosion[toX, toY] % 3;

            // The start is always in a rocky region
            if ((fromX == 0) && (fromY == 0))
            {
                currentCellType = 0;
            }
            if ((toX == 0) && (toY == 0))
            {
                newCellType = 0;
            }

            // The target is always in a rocky region
            if ((toX == sTargetX) && (toY == sTargetY))
            {
                newCellType = 0;
            }

            if (newCellType != currentCellType)
            {
                var needTorch = 0;
                var needClimbingGear = 0;
                // 0, the region's type is rocky.
                // In rocky regions, you can use the climbing gear or the torch.
                // You cannot use neither (you'll likely slip and fall).
                // 1, the region's type is wet.
                // In wet regions, you can use the climbing gear or neither tool.
                // You cannot use the torch (if it gets wet, you won't have a light source).
                // 2, the region's type is narrow.
                // In narrow regions, you can use the torch or neither tool.
                // You cannot use the climbing gear (it's too bulky to fit).
                if (newCellType == 0)
                {
                    if (currentCellType == 1)
                    {
                        needTorch = 0;
                        needClimbingGear = 1;
                    }
                    else if (currentCellType == 2)
                    {
                        needTorch = 1;
                        needClimbingGear = 0;
                    }
                }
                else if (newCellType == 1)
                {
                    if (currentCellType == 0)
                    {
                        needTorch = 0;
                        needClimbingGear = 1;
                    }
                    else if (currentCellType == 2)
                    {
                        needTorch = 0;
                        needClimbingGear = 0;
                    }
                }
                else if (newCellType == 2)
                {
                    if (currentCellType == 0)
                    {
                        needTorch = 1;
                        needClimbingGear = 0;
                    }
                    else if (currentCellType == 1)
                    {
                        needTorch = 0;
                        needClimbingGear = 0;
                    }
                }

                if ((needTorch == 1) && (needClimbingGear == 1))
                {
                    throw new InvalidProgramException($"Trying to equip both items at the same time");
                }
                else if ((needTorch != torch) || (needClimbingGear != climbingGear))
                {
                    deltaTime += SWITCH_GEAR_TIME;
                    torch = needTorch;
                    climbingGear = needClimbingGear;
                }
            }
            return deltaTime;
        }

        public static int FirstRoute()
        {
            var totalTime = 0;
            var x = 0;
            var y = 0;
            var torch = 1;
            var climbingGear = 0;
            while ((x != sTargetX) || (y != sTargetY))
            {
                var newX = x;
                var newY = y;
                if (sTargetY > y)
                {
                    newY = y + 1;
                }
                else if (sTargetX > x)
                {
                    newX = x + 1;
                }

                totalTime += TimeToMove(ref torch, ref climbingGear, x, y, newX, newY);
                x = newX;
                y = newY;
            }

            // Finally, once you reach the target, you need the torch equipped before you can find him in the dark.
            if (torch == 0)
            {
                totalTime += SWITCH_GEAR_TIME;
            }

            return totalTime;
        }

        private static void AddNode(int fromX, int fromY, int toX, int toY, int time, int torch, int climbingGear, int minTime)
        {
            var newTorch = torch;
            var newClimbingGear = climbingGear;
            var newTime = time + TimeToMove(ref newTorch, ref newClimbingGear, fromX, fromY, toX, toY);
            if (newTime <= minTime)
            {
                sNodes[sNodeCount] = new Node { x = toX, y = toY, time = newTime, torch = newTorch, climbingGear = newClimbingGear };
                ++sNodeCount;
                if (sNodeCount == MAX_COUNT_NODES)
                {
                    sNodeCount = 0;
                }
                if (sNodeCount == sNodeStart)
                {
                    throw new InvalidProgramException($"Count {sNodeCount} == Start {sNodeStart}");
                }
            }
        }

        private static void FindQuickestRoute(ref int minTime)
        {
            sNodeStart = 0;
            sNodeCount = 0;
            sNodes[sNodeCount] = new Node { x = 0, y = 0, time = 0, torch = 1, climbingGear = 0 };
            ++sNodeCount;
            while (sNodeStart != sNodeCount)
            {
                var node = sNodes[sNodeStart];
                ++sNodeStart;
                if (sNodeStart == MAX_COUNT_NODES)
                {
                    sNodeStart = 0;
                }
                if (node.time > minTime)
                {
                    continue;
                }
                StepInRoute(node.x, node.y, node.time, ref minTime, node.torch, node.climbingGear);
            }
        }

        private static void StepInRoute(int fromX, int fromY, int time, ref int minTime, int torch, int climbingGear)
        {
            if (sVisited[torch, climbingGear, fromX, fromY] <= time)
            {
                return;
            }
            if ((fromX == sTargetX) && (fromY == sTargetY))
            {
                if (torch == 0)
                {
                    time += SWITCH_GEAR_TIME;
                }
                minTime = Math.Min(time, minTime);
            }
            else
            {
                sVisited[torch, climbingGear, fromX, fromY] = time;
                if (fromY < (sHeight - 1))
                {
                    AddNode(fromX, fromY, fromX + 0, fromY + 1, time, torch, climbingGear, minTime);
                }
                if (fromX < (sWidth - 1))
                {
                    AddNode(fromX, fromY, fromX + 1, fromY + 0, time, torch, climbingGear, minTime);
                }
                if (fromX > 0)
                {
                    AddNode(fromX, fromY, fromX - 1, fromY + 0, time, torch, climbingGear, minTime);
                }
                if (fromY > 0)
                {
                    AddNode(fromX, fromY, fromX + 0, fromY - 1, time, torch, climbingGear, minTime);
                }
            }
        }

        public static int ShortestTime()
        {
            var width = sTargetX + 100;
            var height = sTargetY + 100;
            ComputeErosionMap(width, height);
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    sVisited[0, 0, x, y] = int.MaxValue;
                    sVisited[0, 1, x, y] = int.MaxValue;
                    sVisited[1, 0, x, y] = int.MaxValue;
                    sVisited[1, 1, x, y] = int.MaxValue;
                }
            }

            var minTime = FirstRoute();
            FindQuickestRoute(ref minTime);

            return minTime;
        }

        public static int RiskLevel()
        {
            var width = sTargetX + 10;
            var height = sTargetY + 10;
            ComputeErosionMap(width, height);

            // For the rectangle that has a top-left corner of region 0,0 and a bottom-right corner of the region containing the target, add up the risk level of each individual region: 0 for rocky regions, 1 for wet regions, and 2 for narrow regions.
            // If the erosion level modulo 3 is 0, the region's type is rocky.
            // If the erosion level modulo 3 is 1, the region's type is wet.
            // If the erosion level modulo 3 is 2, the region's type is narrow.
            var riskLevel = 0;
            for (var y = 0; y <= sTargetY; ++y)
            {
                for (var x = 0; x <= sTargetX; ++x)
                {
                    riskLevel += sErosion[x, y] % 3;
                }
            }
            return riskLevel;
        }

        public static void Run()
        {

            _ = new Program("Day22/input.txt", true);
            _ = new Program("Day22/input.txt", false);
            Console.WriteLine("Day22 : End");
        }
    }
}
