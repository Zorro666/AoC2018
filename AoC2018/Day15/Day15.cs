using System;

/*

--- Day 15: Beverage Bandits ---

Having perfected their hot chocolate, the Elves have a new problem: the Goblins that live in these caves will do anything to steal it.
Looks like they're here for a fight.

You scan the area, generating a map of the walls (#), open cavern (.), and starting position of every Goblin (G) and Elf (E) (your puzzle input).

Combat proceeds in rounds; in each round, each unit that is still alive takes a turn, resolving all of its actions before the next unit's turn begins.
On each unit's turn, it tries to move into range of an enemy (if it isn't already) and then attack (if it is in range).

All units are very disciplined and always follow very strict combat rules.
Units never move or attack diagonally, as doing so would be dishonorable.
When multiple choices are equally valid, ties are broken in reading order: top-to-bottom, then left-to-right.
For instance, the order in which units take their turns within a round is the reading order of their starting positions in that round, regardless of the type of unit or whether other units have moved after the round started.
For example:

                 would take their
These units:   turns in this order:
  #######           #######
  #.G.E.#           #.1.2.#
  #E.G.E#           #3.4.5#
  #.G.E.#           #.6.7.#
  #######           #######
Each unit begins its turn by identifying all possible targets (enemy units).
If no targets remain, combat ends.

Then, the unit identifies all of the open squares (.) that are in range of each target; these are the squares which are adjacent (immediately up, down, left, or right) to any target and which aren't already occupied by a wall or another unit.
Alternatively, the unit might already be in range of a target.
If the unit is not already in range of a target, and there are no open squares which are in range of a target, the unit ends its turn.

If the unit is already in range of a target, it does not move, but continues its turn with an attack.
Otherwise, since it is not in range of a target, it moves.

To move, the unit first considers the squares that are in range and determines which of those squares it could reach in the fewest steps.
A step is a single movement to any adjacent (immediately up, down, left, or right) open (.) square.
Units cannot move into walls or other units.
The unit does this while considering the current positions of units and does not do any prediction about where units will be later.
If the unit cannot reach (find an open path to) any of the squares that are in range, it ends its turn.
If multiple squares are in range and tied for being reachable in the fewest steps, the square which is first in reading order is chosen.
For example:

Targets:      In range:     Reachable:    Nearest:      Chosen:
#######       #######       #######       #######       #######
#E..G.#       #E.?G?#       #E.@G.#       #E.!G.#       #E.+G.#
#...#.#  -->  #.?.#?#  -->  #.@.#.#  -->  #.!.#.#  -->  #...#.#
#.G.#G#       #?G?#G#       #@G@#G#       #!G.#G#       #.G.#G#
#######       #######       #######       #######       #######
In the above scenario, the Elf has three targets (the three Goblins):

Each of the Goblins has open, adjacent squares which are in range (marked with a ? on the map).
Of those squares, four are reachable (marked @); the other two (on the right) would require moving through a wall or unit to reach.
Three of these reachable squares are nearest, requiring the fewest steps (only 2) to reach (marked !).
Of those, the square which is first in reading order is chosen (+).
The unit then takes a single step toward the chosen square along the shortest path to that square.
If multiple steps would put the unit equally closer to its destination, the unit chooses the step which is first in reading order.
(This requires knowing when there is more than one shortest path so that you can consider the first step of each such path.) For example:

In range:     Nearest:      Chosen:       Distance:     Step:
#######       #######       #######       #######       #######
#.E...#       #.E...#       #.E...#       #4E212#       #..E..#
#...?.#  -->  #...!.#  -->  #...+.#  -->  #32101#  -->  #.....#
#..?G?#       #..!G.#       #...G.#       #432G2#       #...G.#
#######       #######       #######       #######       #######
The Elf sees three squares in range of a target (?), two of which are nearest (!), and so the first in reading order is chosen (+).
Under "Distance", each open square is marked with its distance from the destination square; the two squares to which the Elf could move on this turn (down and to the right) are both equally good moves and would leave the Elf 2 steps from being in range of the Goblin.
Because the step which is first in reading order is chosen, the Elf moves right one square.

Here's a larger example of movement:

Initially:
#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########

After 1 round:
#########
#.G...G.#
#...G...#
#...E..G#
#.G.....#
#.......#
#G..G..G#
#.......#
#########

After 2 rounds:
#########
#..G.G..#
#...G...#
#.G.E.G.#
#.......#
#G..G..G#
#.......#
#.......#
#########

After 3 rounds:
#########
#.......#
#..GGG..#
#..GEG..#
#G..G...#
#......G#
#.......#
#.......#
#########

Once the Goblins and Elf reach the positions above, they all are either in range of a target or cannot find any square in range of a target, and so none of the units can move until a unit dies.

After moving (or if the unit began its turn in range of a target), the unit attacks.

To attack, the unit first determines all of the targets that are in range of it by being immediately adjacent to it.
If there are no such targets, the unit ends its turn.
Otherwise, the adjacent target with the fewest hit points is selected; in a tie, the adjacent target with the fewest hit points which is first in reading order is selected.

The unit deals damage equal to its attack power to the selected target, reducing its hit points by that amount.
If this reduces its hit points to 0 or fewer, the selected target dies: its square becomes . and it takes no further turns.

Each unit, either Goblin or Elf, has 3 attack power and starts with 200 hit points.

For example, suppose the only Elf is about to attack:

       HP:            HP:
G....  9       G....  9  
..G..  4       ..G..  4  
..EG.  2  -->  ..E..     
..G..  2       ..G..  2  
...G.  1       ...G.  1  

The "HP" column shows the hit points of the Goblin to the left in the corresponding row.
The Elf is in range of three targets: the Goblin above it (with 4 hit points), the Goblin to its right (with 2 hit points), and the Goblin below it (also with 2 hit points).
Because three targets are in range, the ones with the lowest hit points are selected: the two Goblins with 2 hit points each (one to the right of the Elf and one below the Elf).
Of those, the Goblin first in reading order (the one to the right of the Elf) is selected.
The selected Goblin's hit points (2) are reduced by the Elf's attack power (3), reducing its hit points to -1, killing it.

After attacking, the unit's turn ends.
Regardless of how the unit's turn ends, the next unit in the round takes its turn.
If all units have taken turns in this round, the round ends, and a new round begins.

The Elves look quite outnumbered.
You need to determine the outcome of the battle: the number of full rounds that were completed (not counting the round in which combat ends) multiplied by the sum of the hit points of all remaining units at the moment combat ends.
(Combat only ends when a unit finds no targets during its turn.)

Below is an entire sample combat.
Next to each map, each row's units' hit points are listed from left to right.

Initially:
#######   
#.G...#   G(200)
#...EG#   E(200), G(200)
#.#.#G#   G(200)
#..G#E#   G(200), E(200)
#.....#   
#######   

After 1 round:
#######   
#..G..#   G(200)
#...EG#   E(197), G(197)
#.#G#G#   G(200), G(197)
#...#E#   E(197)
#.....#   
#######   

After 2 rounds:
#######   
#...G.#   G(200)
#..GEG#   G(200), E(188), G(194)
#.#.#G#   G(194)
#...#E#   E(194)
#.....#   
#######   

Combat ensues; eventually, the top Elf dies:

After 23 rounds:
#######   
#...G.#   G(200)
#..G.G#   G(200), G(131)
#.#.#G#   G(131)
#...#E#   E(131)
#.....#   
#######   

After 24 rounds:
#######   
#..G..#   G(200)
#...G.#   G(131)
#.#G#G#   G(200), G(128)
#...#E#   E(128)
#.....#   
#######   

After 25 rounds:
#######   
#.G...#   G(200)
#..G..#   G(131)
#.#.#G#   G(125)
#..G#E#   G(200), E(125)
#.....#   
#######   

After 26 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(122)
#...#E#   E(122)
#..G..#   G(200)
#######   

After 27 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(119)
#...#E#   E(119)
#...G.#   G(200)
#######   

After 28 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(116)
#...#E#   E(113)
#....G#   G(200)
#######   

More combat ensues; eventually, the bottom Elf dies:

After 47 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(59)
#...#.#   
#....G#   G(200)
#######   

Before the 48th round can finish, the top-left Goblin finds that there are no targets remaining, and so combat ends.
So, the number of full rounds that were completed is 47, and the sum of the hit points of all remaining units is 200+131+59+200 = 590.
From these, the outcome of the battle is 47 * 590 = 27730.

Here are a few example summarized combats:

#######       #######
#G..#E#       #...#E#   E(200)
#E#E.E#       #E#...#   E(197)
#G.##.#  -->  #.E##.#   E(185)
#...#E#       #E..#E#   E(200), E(200)
#...E.#       #.....#
#######       #######

Combat ends after 37 full rounds
Elves win with 982 total hit points left
Outcome: 37 * 982 = 36334
#######       #######   
#E..EG#       #.E.E.#   E(164), E(197)
#.#G.E#       #.#E..#   E(200)
#E.##E#  -->  #E.##.#   E(98)
#G..#.#       #.E.#.#   E(200)
#..E#.#       #...#.#   
#######       #######   

Combat ends after 46 full rounds
Elves win with 859 total hit points left
Outcome: 46 * 859 = 39514
#######       #######   
#E.G#.#       #G.G#.#   G(200), G(98)
#.#G..#       #.#G..#   G(200)
#G.#.G#  -->  #..#..#   
#G..#.#       #...#G#   G(95)
#...E.#       #...G.#   G(200)
#######       #######   

Combat ends after 35 full rounds
Goblins win with 793 total hit points left
Outcome: 35 * 793 = 27755
#######       #######   
#.E...#       #.....#   
#.#..G#       #.#G..#   G(200)
#.###.#  -->  #.###.#   
#E#G#G#       #.#.#.#   
#...#G#       #G.G#G#   G(98), G(38), G(200)
#######       #######   

Combat ends after 54 full rounds
Goblins win with 536 total hit points left
Outcome: 54 * 536 = 28944
#########       #########   
#G......#       #.G.....#   G(137)
#.E.#...#       #G.G#...#   G(200), G(200)
#..##..G#       #.G##...#   G(200)
#...##..#  -->  #...##..#   
#...#...#       #.G.#...#   G(200)
#.G...G.#       #.......#   
#.....G.#       #.......#   
#########       #########   

Combat ends after 20 full rounds
Goblins win with 937 total hit points left
Outcome: 20 * 937 = 18740
What is the outcome of the combat described in your puzzle input?

Your puzzle answer was 250594.

--- Part Two ---

According to your calculations, the Elves are going to lose badly.
Surely, you won't mess up the timeline too much if you give them just a little advanced technology, right?

You need to make sure the Elves not only win, but also suffer no losses: even the death of a single Elf is unacceptable.

However, you can't go too far: larger changes will be more likely to permanently alter spacetime.

So, you need to find the outcome of the battle in which the Elves have the lowest integer attack power (at least 4) that allows them to win without a single death.
The Goblins always have an attack power of 3.

In the first summarized example above, the lowest attack power the Elves need to win without losses is 15:

#######       #######
#.G...#       #..E..#   E(158)
#...EG#       #...E.#   E(14)
#.#.#G#  -->  #.#.#.#
#..G#E#       #...#.#
#.....#       #.....#
#######       #######

Combat ends after 29 full rounds
Elves win with 172 total hit points left
Outcome: 29 * 172 = 4988

In the second example above, the Elves need only 4 attack power:

#######       #######
#E..EG#       #.E.E.#   E(200), E(23)
#.#G.E#       #.#E..#   E(200)
#E.##E#  -->  #E.##E#   E(125), E(200)
#G..#.#       #.E.#.#   E(200)
#..E#.#       #...#.#
#######       #######

Combat ends after 33 full rounds
Elves win with 948 total hit points left
Outcome: 33 * 948 = 31284

In the third example above, the Elves need 15 attack power:

#######       #######
#E.G#.#       #.E.#.#   E(8)
#.#G..#       #.#E..#   E(86)
#G.#.G#  -->  #..#..#
#G..#.#       #...#.#
#...E.#       #.....#
#######       #######

Combat ends after 37 full rounds
Elves win with 94 total hit points left
Outcome: 37 * 94 = 3478

In the fourth example above, the Elves need 12 attack power:

#######       #######
#.E...#       #...E.#   E(14)
#.#..G#       #.#..E#   E(152)
#.###.#  -->  #.###.#
#E#G#G#       #.#.#.#
#...#G#       #...#.#
#######       #######

Combat ends after 39 full rounds
Elves win with 166 total hit points left
Outcome: 39 * 166 = 6474
In the last example above, the lone Elf needs 34 attack power:

#########       #########   
#G......#       #.......#   
#.E.#...#       #.E.#...#   E(38)
#..##..G#       #..##...#   
#...##..#  -->  #...##..#   
#...#...#       #...#...#   
#.G...G.#       #.......#   
#.....G.#       #.......#   
#########       #########   

Combat ends after 30 full rounds
Elves win with 38 total hit points left
Outcome: 30 * 38 = 1140
After increasing the Elves' attack power until it is just barely enough for them to win without any Elves dying, what is the outcome of the combat described in your puzzle input?00

*/

namespace Day15
{
    class Program
    {
        const int MAX_MAP_SIZE = 32;
        readonly static char[,] sMapStart = new char[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly static int[,] sHPStart = new int[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly static char[,] sMap = new char[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly static int[,] sHP = new int[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly static bool[,] sProcessed = new bool[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly static int[,] sDistances = new int[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static int sWidth;
        static int sHeight;
        static int sElfAttack;
        static int sRoundCount;
        const int sGoblinAttack = 3;
        const int sElfHP = 200;
        const int sGoblinHP = 200;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = BattleResult(3);
                Console.WriteLine($"Day15 : Result1 {result1}");
                var expected = 250648;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = ElfWinBattleResult();
                Console.WriteLine($"Day15 : Result2 {result2}");
                var expected = 42224;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            sHeight = lines.Length;
            if (sHeight == 0)
            {
                throw new InvalidProgramException($"Invalid map no input");
            }
            sWidth = lines[0].Trim().Length;
            for (var y = 0; y < sHeight; ++y)
            {
                var line = lines[y];
                var width = line.Trim().Length;
                if (width != sWidth)
                {
                    throw new InvalidProgramException($"Unexpected width at line[{y}] {width} Expected:{sWidth}");
                }
                for (var x = 0; x < sWidth; ++x)
                {
                    sMapStart[x, y] = line[x];
                    if (sMapStart[x, y] == 'E')
                    {
                        sHPStart[x, y] = sElfHP;
                    }
                    if (sMapStart[x, y] == 'G')
                    {
                        sHPStart[x, y] = sGoblinHP;
                    }
                }
            }
            SetupFromStart();
        }

        private static void SetupFromStart()
        {
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    sMap[x, y] = sMapStart[x, y];
                    sHP[x, y] = sHPStart[x, y];
                    sProcessed[x, y] = false;
                    sDistances[x, y] = 0;
                }
            }
            sRoundCount = 0;

        }

        private static void Visit(int x, int y, int endX, int endY, ref int steps)
        {
            if (sMap[x, y] == '#')
            {
                return;
            }
            if (sMap[x, y] != '.')
            {
                return;
            }
            ++steps;
            if (steps >= sDistances[x, y])
            {
                return;
            }
            sDistances[x, y] = steps;
            if ((x == endX) && (y == endY))
            {
                return;
            }
            int newSteps;

            newSteps = steps;
            Visit(x + 0, y - 1, endX, endY, ref newSteps);

            newSteps = steps;
            Visit(x - 1, y + 0, endX, endY, ref newSteps);

            newSteps = steps;
            Visit(x + 1, y + 0, endX, endY, ref newSteps);

            newSteps = steps;
            Visit(x + 0, y + 1, endX, endY, ref newSteps);
        }

        private static bool GenerateDistanceMap(int fromX, int fromY)
        {
            var oldCell = sMap[fromX, fromY];
            if (oldCell != '.')
            {
                return false;
            }
            int steps;
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    sDistances[x, y] = int.MaxValue;
                }
            }
            sMap[fromX, fromY] = '.';
            sDistances[fromX, fromY] = 0;

            steps = 0;
            Visit(fromX + 0, fromY - 1, MAX_MAP_SIZE, MAX_MAP_SIZE, ref steps);

            steps = 0;
            Visit(fromX - 1, fromY + 0, MAX_MAP_SIZE, MAX_MAP_SIZE, ref steps);

            steps = 0;
            Visit(fromX + 1, fromY + 0, MAX_MAP_SIZE, MAX_MAP_SIZE, ref steps);

            steps = 0;
            Visit(fromX + 0, fromY + 1, MAX_MAP_SIZE, MAX_MAP_SIZE, ref steps);

            sMap[fromX, fromY] = oldCell;
            return true;
        }

        // Find closest cells to attack from and choose the cell in reading order -Y, -X, +X, +Y
        private static bool FindClosestTarget(char enemy, ref int globalClosestX, ref int globalClosestY)
        {
            var closestDistance = int.MaxValue;
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    var cell = sMap[x, y];
                    if (cell == '.')
                    {
                        // Check an enemy is next to it
                        if (sMap[x + 0, y - 1] == enemy || sMap[x - 1, y + 0] == enemy || sMap[x + 1, y + 0] == enemy || sMap[x + 0, y + 1] == enemy)
                        {
                            int distance = sDistances[x, y];
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                globalClosestX = x;
                                globalClosestY = y;
                            }
                        }
                    }
                }
            }
            // Resolve the closest targets in reading order
            if (closestDistance < int.MaxValue)
            {
                return true;
            }
            return false;
        }


        public static (int x, int y) ClosestTarget(int fromX, int fromY)
        {
            // Find the enenmy
            var enemy = sMap[fromX, fromY] switch
            {
                'E' => 'G',
                'G' => 'E',
                _ => throw new InvalidProgramException($"Invalid from location '{sMap[fromX, fromY]}'")
            };

            (int, int) targetCell = (fromX, fromY);

            if (sMap[fromX + 0, fromY - 1] == enemy)
            {
                return targetCell;
            }
            if (sMap[fromX - 1, fromY + 0] == enemy)
            {
                return targetCell;
            }
            if (sMap[fromX + 1, fromY + 0] == enemy)
            {
                return targetCell;
            }
            if (sMap[fromX + 0, fromY + 1] == enemy)
            {
                return targetCell;
            }

            int closestTargetX = int.MaxValue;
            int closestTargetY = int.MaxValue;

            var oldCell = sMap[fromX, fromY];
            sMap[fromX, fromY] = '.';
            if (!GenerateDistanceMap(fromX, fromY))
            {
                throw new InvalidProgramException($"GenerateDistanceMap failed");
            }

            targetCell = (int.MaxValue, int.MaxValue);
            if (FindClosestTarget(enemy, ref closestTargetX, ref closestTargetY))
            {
                targetCell = (closestTargetX, closestTargetY);
            }

            sMap[fromX, fromY] = oldCell;
            return targetCell;
        }

        public static (int x, int y) MoveTowardsTarget(int fromX, int fromY)
        {
            // Find the enenmy
            var enemy = sMap[fromX, fromY] switch
            {
                'E' => 'G',
                'G' => 'E',
                _ => throw new InvalidProgramException($"Invalid from location '{sMap[fromX, fromY]}'")
            };

            (int, int) bestMove = (fromX, fromY);

            // Next to an enemy do not move
            if (sMap[fromX + 0, fromY - 1] == enemy)
            {
                return bestMove;
            }
            if (sMap[fromX - 1, fromY + 0] == enemy)
            {
                return bestMove;
            }
            if (sMap[fromX + 1, fromY + 0] == enemy)
            {
                return bestMove;
            }
            if (sMap[fromX + 0, fromY + 1] == enemy)
            {
                return bestMove;
            }

            var countPossibleMoves = 0;
            // No space to move to
            if (sMap[fromX + 0, fromY - 1] == '.')
            {
                ++countPossibleMoves;
            }
            if (sMap[fromX - 1, fromY + 0] == '.')
            {
                ++countPossibleMoves;
            }
            if (sMap[fromX + 1, fromY + 0] == '.')
            {
                ++countPossibleMoves;
            }
            if (sMap[fromX + 0, fromY + 1] == '.')
            {
                ++countPossibleMoves;
            }
            if (countPossibleMoves == 0)
            {
                return bestMove;
            }

            var (targetCellX, targetCellY) = ClosestTarget(fromX, fromY);

            // No target could be found
            if ((targetCellX == int.MaxValue) && (targetCellY == int.MaxValue))
            {
                return bestMove;
            }
            // Generate distance map from the targetCell
            var oldCell = sMap[targetCellX, targetCellY];
            sMap[targetCellX, targetCellY] = '.';
            if (!GenerateDistanceMap(targetCellX, targetCellY))
            {
                throw new InvalidProgramException($"GenerateDistanceMap failed");
            }

            var shortestSteps = int.MaxValue;
            // Reading order
            if (sDistances[fromX + 0, fromY - 1] < shortestSteps)
            {
                shortestSteps = sDistances[fromX + 0, fromY - 1];
                bestMove = (fromX + 0, fromY - 1);
            }
            if (sDistances[fromX - 1, fromY + 0] < shortestSteps)
            {
                shortestSteps = sDistances[fromX - 1, fromY + 0];
                bestMove = (fromX - 1, fromY + 0);
            }
            if (sDistances[fromX + 1, fromY + 0] < shortestSteps)
            {
                shortestSteps = sDistances[fromX + 1, fromY + 0];
                bestMove = (fromX + 1, fromY + 0);
            }
            if (sDistances[fromX + 0, fromY + 1] < shortestSteps)
            {
                bestMove = (fromX + 0, fromY + 1);
            }
            sMap[targetCellX, targetCellY] = oldCell;

            return bestMove;
        }

        public static (int x, int y) TurnOrder(int turn)
        {
            var turnCount = 0;
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    var cell = sMap[x, y];
                    if ((cell == 'G') || (cell == 'E'))
                    {
                        if (turnCount == turn)
                        {
                            return (x, y);
                        }
                        ++turnCount;
                    }
                }
            }
            throw new InvalidProgramException($"Did not find object to move at {turn}");
        }

        private static bool SimulateRound()
        {
            //var map = GetMap();
            //Console.WriteLine($"==== {sRoundCount} =====");
            //Console.WriteLine($"ElfAttack {sElfAttack}");
            //foreach (var l in map)
            //{
            //    Console.WriteLine(l);
            //}
            //for (var y = 0; y < sHeight; ++y)
            //{
            //    for (var x = 0; x < sWidth; ++x)
            //    {
            //        if ((sHP[x, y] > 0) || (sMap[x, y] == 'E') || (sMap[x, y] == 'G'))
            //        {
            //            Console.WriteLine($"{sMap[x, y]} {x}, {y} HP={sHP[x, y]}");
            //        }
            //    }
            //}
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    sProcessed[x, y] = false;
                }
            }

            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    var cell = sMap[x, y];
                    if ((cell == 'E') || (cell == 'G'))
                    {
                        if (sProcessed[x, y])
                        {
                            continue;
                        }
                        var enemy = (cell == 'E') ? 'G' : 'E';
                        // Combat only ends when a unit finds no targets during its turn.
                        var enemyCount = UnitCount(enemy);
                        if (enemyCount == 0)
                        {
                            return false;
                        }

                        // On each unit's turn, it tries to move into range of an enemy (if it isn't already)
                        var newPosition = MoveTowardsTarget(x, y);
                        var hp = sHP[x, y];
                        sMap[x, y] = '.';
                        sHP[x, y] = 0;
                        sMap[newPosition.x, newPosition.y] = cell;
                        sHP[newPosition.x, newPosition.y] = hp;
                        sProcessed[newPosition.x, newPosition.y] = true;
                        // then attack (if it is in range).
                        ResolveCombat(newPosition.x, newPosition.y);
                    }
                }
            }
            return true;
        }

        private static int UnitCount(char unitType)
        {
            var count = 0;
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    if (sMap[x, y] == unitType)
                    {
                        ++count;
                    }
                }
            }
            return count;
        }

        private static (int elves, int goblins) CountHPs()
        {
            var elvesHP = 0;
            var goblinsHP = 0;
            for (var y = 0; y < sHeight; ++y)
            {
                for (var x = 0; x < sWidth; ++x)
                {
                    if (sMap[x, y] == 'E')
                    {
                        elvesHP += sHP[x, y];
                    }
                    else if (sMap[x, y] == 'G')
                    {
                        goblinsHP += sHP[x, y];
                    }
                }
            }
            return (elvesHP, goblinsHP);
        }

        private static void ResolveCombat(int x, int y)
        {
            //Console.WriteLine($"ElfAttack {sElfAttack}");
            var cell = sMap[x, y];
            var enemy = (cell == 'E') ? 'G' : 'E';
            var minHP = int.MaxValue;
            int minEnemyX = int.MaxValue;
            int minEnemyY = int.MaxValue;
            int enemyX;
            int enemyY;

            enemyX = x + 0;
            enemyY = y - 1;
            if (sMap[enemyX, enemyY] == enemy)
            {
                var enemyHP = sHP[enemyX, enemyY];
                if (enemyHP < minHP)
                {
                    minHP = enemyHP;
                    minEnemyX = enemyX;
                    minEnemyY = enemyY;
                }
            }

            enemyX = x - 1;
            enemyY = y + 0;
            if (sMap[enemyX, enemyY] == enemy)
            {
                var enemyHP = sHP[enemyX, enemyY];
                if (enemyHP < minHP)
                {
                    minHP = enemyHP;
                    minEnemyX = enemyX;
                    minEnemyY = enemyY;
                }
            }

            enemyX = x + 1;
            enemyY = y + 0;
            if (sMap[enemyX, enemyY] == enemy)
            {
                var enemyHP = sHP[enemyX, enemyY];
                if (enemyHP < minHP)
                {
                    minHP = enemyHP;
                    minEnemyX = enemyX;
                    minEnemyY = enemyY;
                }
            }

            enemyX = x + 0;
            enemyY = y + 1;
            if (sMap[enemyX, enemyY] == enemy)
            {
                var enemyHP = sHP[enemyX, enemyY];
                if (enemyHP < minHP)
                {
                    minHP = enemyHP;
                    minEnemyX = enemyX;
                    minEnemyY = enemyY;
                }
            }
            if (minHP != int.MaxValue)
            {
                var attackPower = sElfAttack;
                if (sMap[minEnemyX, minEnemyY] == 'E')
                {
                    attackPower = sGoblinAttack;
                }
                sHP[minEnemyX, minEnemyY] -= attackPower;
                if (sHP[minEnemyX, minEnemyY] <= 0)
                {
                    sHP[minEnemyX, minEnemyY] = 0;
                    sMap[minEnemyX, minEnemyY] = '.';
                }
                //Console.WriteLine($"Attach {x},{y} -> {sMap[minEnemyX, minEnemyY]} @ {minEnemyX},{minEnemyY} Power {attackPower} HP {minHP} -> {sHP[minEnemyX, minEnemyY]}");
            }
        }

        public static void Simulate(int rounds, int elfAttack)
        {
            SetupFromStart();
            sElfAttack = elfAttack;
            for (var r = 0; r < rounds; ++r)
            {
                sRoundCount = r;
                SimulateRound();
            }
        }

        public static int BattleResult(int elfAttack)
        {
            SetupFromStart();
            sElfAttack = elfAttack;
            const int MAX_NUM_ROUNDS = 128;
            for (var r = 0; r < MAX_NUM_ROUNDS; ++r)
            {
                sRoundCount = r;
                if (SimulateRound() == false)
                {
                    return GetBattleResult();
                }
            }
            throw new InvalidProgramException($"Battle not resolved after {MAX_NUM_ROUNDS} rounds");
        }

        public static int GetBattleResult()
        {
            var (elvesHP, goblinsHP) = CountHPs();
            return sRoundCount * (elvesHP + goblinsHP);
        }

        public static int ElfAttackPower()
        {
            var elfCount = UnitCount('E');
            for (var elfAttack = 4; elfAttack < 40; ++elfAttack)
            {
                bool elfDied = false;
                SetupFromStart();
                sElfAttack = elfAttack;
                const int MAX_NUM_ROUNDS = 128;
                for (var r = 0; r < MAX_NUM_ROUNDS; ++r)
                {
                    sRoundCount = r;
                    bool battleResult = SimulateRound();
                    if (UnitCount('E') < elfCount)
                    {
                        elfDied = true;
                        break;
                    }
                    if (!battleResult)
                    {
                        var (elvesHP, goblinsHP) = CountHPs();
                        if (goblinsHP != 0)
                        {
                            throw new InvalidProgramException($"Elves did not win ElfHP:{elvesHP} GoblinHP:{goblinsHP}");
                        }
                        //Console.WriteLine($"Round {sRoundCount} Attack {elfAttack} elfHP {elvesHP}");
                        return elfAttack;
                    }
                }
                if (!elfDied)
                {
                    throw new InvalidProgramException($"Battle not resolved after {MAX_NUM_ROUNDS} rounds");
                }
            }
            throw new InvalidProgramException($"Elf Attack Power could not be found");
        }

        public static int ElfWinBattleResult()
        {
            ElfAttackPower();
            return GetBattleResult();
        }

        public static int GetHP(int x, int y)
        {
            return sHP[x, y];
        }

        public static string[] GetMap()
        {
            var output = new string[sHeight];
            for (var y = 0; y < sHeight; ++y)
            {
                var line = "";
                for (var x = 0; x < sWidth; ++x)
                {
                    line += sMap[x, y];
                }
                output[y] = line;
            }
            return output;
        }

        public static void Run()
        {
            Console.WriteLine("Day15 : Start");
            _ = new Program("Day15/input.txt", true);
            _ = new Program("Day15/input.txt", false);
            Console.WriteLine("Day15 : End");
        }
    }
}
