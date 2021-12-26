using System;

/*

--- Day 13: Mine Cart Madness ---

A crop of this size requires significant logistics to transport produce, soil, fertilizer, and so on.
The Elves are very busy pushing things around in carts on some kind of rudimentary system of tracks they've come up with.

Seeing as how cart-and-track systems don't appear in recorded history for another 1000 years, the Elves seem to be making this up as they go along.
They haven't even figured out how to avoid collisions yet.

You map out the tracks (your puzzle input) and see where you can help.

Tracks consist of straight paths (| and -), curves (/ and \), and intersections (+).
Curves connect exactly two perpendicular pieces of track; for example, this is a closed loop:

/----\
|    |
|    |
\----/
Intersections occur when two perpendicular paths cross.
At an intersection, a cart is capable of turning left, turning right, or continuing straight.
Here are two loops connected by two intersections:

/-----\
|     |
|  /--+--\
|  |  |  |
\--+--/  |
   |     |
   \-----/
Several carts are also on the tracks.
Carts always face either up (^), down (v), left (<), or right (>).
(On your initial map, the track under each cart is a straight path matching the direction the cart is facing.)

Each time a cart has the option to turn (by arriving at any intersection), it turns left the first time, goes straight the second time, turns right the third time, and then repeats those directions starting again with left the fourth time, straight the fifth time, and so on.
This process is independent of the particular intersection at which the cart has arrived - that is, the cart has no per-intersection memory.

Carts all move at the same speed; they take turns moving a single step at a time.
They do this based on their current location: carts on the top row move first (acting from left to right), then carts on the second row move (again from left to right), then carts on the third row, and so on.
Once each cart has moved one step, the process repeats; each of these loops is called a tick.

For example, suppose there are two carts on a straight track:

|  |  |  |  |
v  |  |  |  |
|  v  v  |  |
|  |  |  v  X
|  |  ^  ^  |
^  ^  |  |  |
|  |  |  |  |
First, the top cart moves.
It is facing down (v), so it moves down one square.
Second, the bottom cart moves.
It is facing up (^), so it moves up one square.
Because all carts have moved, the first tick ends.
Then, the process repeats, starting with the first cart.
The first cart moves down, then the second cart moves up - right into the first cart, colliding with it! (The location of the crash is marked with an X.) This ends the second and last tick.

Here is a longer example:

/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   

/-->\        
|   |  /----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \->--/
  \------/   

/---v        
|   |  /----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \-+>-/
  \------/   

/---\        
|   v  /----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \-+->/
  \------/   

/---\        
|   |  /----\
| /->--+-\  |
| | |  | |  |
\-+-/  \-+--^
  \------/   

/---\        
|   |  /----\
| /-+>-+-\  |
| | |  | |  ^
\-+-/  \-+--/
  \------/   

/---\        
|   |  /----\
| /-+->+-\  ^
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /----<
| /-+-->-\  |
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /---<\
| /-+--+>\  |
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /--<-\
| /-+--+-v  |
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /-<--\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /<---\
| /-+--+-\  |
| | |  | |  |
\-+-/  \-<--/
  \------/   

/---\        
|   |  v----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \<+--/
  \------/   

/---\        
|   |  /----\
| /-+--v-\  |
| | |  | |  |
\-+-/  ^-+--/
  \------/   

/---\        
|   |  /----\
| /-+--+-\  |
| | |  X |  |
\-+-/  \-+--/
  \------/   

After following their respective paths for a while, the carts eventually crash.
To help prevent crashes, you'd like to know the location of the first crash.
Locations are given in X,Y coordinates, where the furthest left column is X=0 and the furthest top row is Y=0:

           111
 0123456789012
0/---\        
1|   |  /----\
2| /-+--+-\  |
3| | |  X |  |
4\-+-/  \-+--/
5  \------/   

In this example, the location of the first crash is 7,3.

Your puzzle answer was 41,22.

--- Part Two ---

There isn't much you can do to prevent crashes in this ridiculous system.
However, by predicting the crashes, the Elves know where to be in advance and instantly remove the two crashing carts the moment any crash occurs.

They can proceed like this for a while, but eventually, they're going to run out of carts.
It could be useful to figure out where the last cart that hasn't crashed will end up.

For example:

/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/

/---\  
|   |  
| v-+-\
| | | |
\-+-/ |
  |   |
  ^---^

/---\  
|   |  
| /-+-\
| v | |
\-+-/ |
  ^   ^
  \---/

/---\  
|   |  
| /-+-\
| | | |
\-+-/ ^
  |   |
  \---/

After four very expensive crashes, a tick ends with only one cart remaining; its final location is 6,4.

What is the location of the last cart at the end of the first tick where it is the only cart left?

*/

namespace Day13
{
    class Program
    {
        const int MAX_NUM_STEPS = 1024 * 32;
        const int MAX_MAP_SIZE = 1024;
        const int MAX_NUM_CARTS = 64;
        readonly static char[,] sMap = new char[MAX_MAP_SIZE, MAX_MAP_SIZE];
        readonly static int[] sCartsX = new int[MAX_NUM_CARTS];
        readonly static int[] sCartsY = new int[MAX_NUM_CARTS];
        readonly static int[] sCartsDX = new int[MAX_NUM_CARTS];
        readonly static int[] sCartsDY = new int[MAX_NUM_CARTS];
        readonly static int[] sCartsTurnCounter = new int[MAX_NUM_CARTS];
        static int sWidth;
        static int sHeight;
        static int sNumCarts;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = FirstCrash();
                Console.WriteLine($"Day13 : Result1 {result1}");
                var expected = (26, 92);
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = LastCart();
                Console.WriteLine($"Day13 : Result2 {result2}");
                var expected = (86, 18);
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            // '/->-\        '
            // '|   |  /----\'
            // '| /-+--+-\  |'
            // '| | |  | v  |'
            // '\-+-/  \-+--/'
            // '  \------/   '
            sWidth = lines[0].Length;
            sHeight = lines.Length;
            sNumCarts = 0;
            for (var y = 0; y < sHeight; ++y)
            {
                var line = lines[y];
                if (line.Length != sWidth)
                {
                    throw new InvalidProgramException($"Inconsistent width {sHeight} != {line.Length}");
                }
                for (var x = 0; x < sWidth; ++x)
                {
                    var c = line[x];
                    var mapC = c;
                    if ((c == '^') || (c == 'v') || (c == '<') || (c == '>'))
                    {
                        if (sNumCarts > MAX_NUM_CARTS)
                        {
                            throw new InvalidProgramException($"Too many carts MAX:{MAX_NUM_CARTS}");
                        }
                        sCartsX[sNumCarts] = x;
                        sCartsY[sNumCarts] = y;
                        (int dx, int dy) = c switch
                        {
                            '<' => (-1, 0),
                            '>' => (+1, 0),
                            '^' => (0, -1),
                            'v' => (0, +1),
                            _ => throw new InvalidProgramException($"Invalid cart dx,dy {c}"),
                        };
                        sCartsDX[sNumCarts] = dx;
                        sCartsDY[sNumCarts] = dy;
                        sCartsTurnCounter[sNumCarts] = 0;
                        ++sNumCarts;

                        mapC = '|';
                    }
                    if ((mapC != '/') && (mapC != '\\') && (mapC != '|') && (mapC != '+') && (mapC != '-') && (mapC != ' '))
                    {
                        throw new InvalidProgramException($"Unknown map character {mapC}");
                    }
                    sMap[x, y] = mapC;
                }
            }
        }

        public static (int x, int y) FirstCrash()
        {
            return Simulate(true);
        }

        public static (int x, int y) LastCart()
        {
            return Simulate(false);
        }

        public static (int x, int y) Simulate(bool firstCrash)
        {
            var cartOrder = new int[sNumCarts];
            int activeCarts = sNumCarts;
            for (var i = 0; i < MAX_NUM_STEPS; ++i)
            {
                for (var c = 0; c < sNumCarts; ++c)
                {
                    cartOrder[c] = c;
                }
                for (var c1 = 0; c1 < sNumCarts - 1; ++c1)
                {
                    for (var c2 = c1 + 1; c2 < sNumCarts; ++c2)
                    {
                        // carts on the top row move first (acting from left to right), then carts on the second row move (again from left to right), then carts on the third row, and so on.
                        var cartIndex1 = cartOrder[c1];
                        var cartIndex2 = cartOrder[c2];
                        if (sCartsY[cartIndex2] < sCartsY[cartIndex1])
                        {
                            cartOrder[c1] = cartIndex2;
                            cartOrder[c2] = cartIndex1;
                        }
                        else if (sCartsY[cartIndex2] == sCartsY[cartIndex1])
                        {
                            if (sCartsX[cartIndex2] < sCartsX[cartIndex1])
                            {
                                cartOrder[c1] = cartIndex2;
                                cartOrder[c2] = cartIndex1;
                            }
                        }
                    }
                }

                for (var c = 0; c < sNumCarts; ++c)
                {
                    var cartIndex = cartOrder[c];
                    var x = sCartsX[cartIndex];
                    var y = sCartsY[cartIndex];

                    if ((x == MAX_MAP_SIZE) && (y == MAX_MAP_SIZE))
                    {
                        continue;
                    }

                    var dx = sCartsDX[cartIndex];
                    var dy = sCartsDY[cartIndex];

                    x += dx;
                    y += dy;

                    // Check for a crash
                    for (var c2 = 0; c2 < sNumCarts; ++c2)
                    {
                        if (c2 == cartIndex)
                        {
                            continue;
                        }
                        if ((sCartsX[c2] == x) && (sCartsY[c2] == y))
                        {
                            if (firstCrash)
                            {
                                return (x, y);
                            }

                            activeCarts -= 2;

                            sCartsX[cartIndex] = MAX_MAP_SIZE;
                            sCartsY[cartIndex] = MAX_MAP_SIZE;
                            sCartsDX[cartIndex] = 0;
                            sCartsDY[cartIndex] = 0;

                            sCartsX[c2] = MAX_MAP_SIZE;
                            sCartsY[c2] = MAX_MAP_SIZE;
                            sCartsDX[c2] = 0;
                            sCartsDY[c2] = 0;

                            x = MAX_MAP_SIZE;
                            y = MAX_MAP_SIZE;

                            break;
                        }
                    }

                    sCartsX[cartIndex] = x;
                    sCartsY[cartIndex] = y;

                    if ((x == MAX_MAP_SIZE) && (y == MAX_MAP_SIZE))
                    {
                        continue;
                    }

                    var mapC = sMap[x, y];
                    var newDX = dx;
                    var newDY = dy;
                    if (mapC == '/')
                    {
                        // left => down : -1,0 -> 0,+1
                        // right => up : +1,0 -> 0,-1
                        // down => left : 0,+1 -> -1,0
                        // up => right : 0,-1 -> +1,0
                        newDX = -dy;
                        newDY = -dx;
                    }
                    else if (mapC == '\\')
                    {
                        // left => up : -1,0 -> 0,-1
                        // right => down : +1,0 -> 0,+1
                        // down => right : 0,+1 -> +1,0
                        // up => left : 0,-1 -> -1,0
                        newDX = +dy;
                        newDY = +dx;
                    }
                    else if (mapC == '+')
                    {
                        // 0 = turn left
                        if (sCartsTurnCounter[cartIndex] == 0)
                        {
                            // left => down : -1,0 -> 0,+1
                            // right => up : +1,0 -> 0,-1
                            // down => right : 0,+1 -> +1,0
                            // up => left : 0,-1 -> -1,0
                            newDX = +dy;
                            newDY = -dx;
                            sCartsTurnCounter[cartIndex] = 1;
                        }
                        // 1 = straight
                        else if (sCartsTurnCounter[cartIndex] == 1)
                        {
                            sCartsTurnCounter[cartIndex] = 2;
                        }
                        // 2 = turn right
                        else if (sCartsTurnCounter[cartIndex] == 2)
                        {
                            // left => up : -1,0 -> 0,-1
                            // right => down : +1,0 -> 0,+1
                            // down => left : 0,+1 -> -1,0
                            // up => right : 0,-1 -> +1,0
                            newDX = -dy;
                            newDY = +dx;
                            sCartsTurnCounter[cartIndex] = 0;
                        }
                    }
                    else if ((mapC != '|') && (mapC != '-'))
                    {
                        throw new InvalidProgramException($"Unknown mapC '{mapC}' at {x},{y}");
                    }
                    if (sMap[x + newDX, y + newDY] == ' ')
                    {
                        throw new InvalidProgramException($"Fallen off map");
                    }

                    sCartsDX[cartIndex] = newDX;
                    sCartsDY[cartIndex] = newDY;
                }
                if (activeCarts == 1)
                {
                    for (var c = 0; c < sNumCarts; ++c)
                    {
                        var x = sCartsX[c];
                        var y = sCartsY[c];

                        if ((x == MAX_MAP_SIZE) && (y == MAX_MAP_SIZE))
                        {
                            continue;
                        }
                        return (x, y);
                    }
                }
            }
            throw new InvalidProgramException($"No crash found after {MAX_NUM_STEPS} steps");
        }

        public static void Run()
        {
            Console.WriteLine("Day13 : Start");
            _ = new Program("Day13/input.txt", true);
            _ = new Program("Day13/input.txt", false);
            Console.WriteLine("Day13 : End");
        }
    }
}
