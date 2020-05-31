using System;

/*

--- Day 11: Chronal Charge ---

You watch the Elves and their sleigh fade into the distance as they head toward the North Pole.

Actually, you're the one fading.
The falling sensation returns.

The low fuel warning light is illuminated on your wrist-mounted device.
Tapping it once causes it to project a hologram of the situation: a 300x300 grid of fuel cells and their current power levels, some negative.
You're not sure what negative power means in the context of time travel, but it can't be good.

Each fuel cell has a coordinate ranging from 1 to 300 in both the X (horizontal) and Y (vertical) direction.
In X,Y notation, the top-left cell is 1,1, and the top-right cell is 300,1.

The interface lets you select any 3x3 square of fuel cells.
To increase your chances of getting to your destination, you decide to choose the 3x3 square with the largest total power.

The power level in a given fuel cell can be found through the following process:

Find the fuel cell's rack ID, which is its X coordinate plus 10.
Begin with a power level of the rack ID times the Y coordinate.
Increase the power level by the value of the grid serial number (your puzzle input).
Set the power level to itself multiplied by the rack ID.
Keep only the hundreds digit of the power level (so 12345 becomes 3; numbers with no hundreds digit become 0).
Subtract 5 from the power level.
For example, to find the power level of the fuel cell at 3,5 in a grid with serial number 8:

The rack ID is 3 + 10 = 13.
The power level starts at 13 * 5 = 65.
Adding the serial number produces 65 + 8 = 73.
Multiplying by the rack ID produces 73 * 13 = 949.
The hundreds digit of 949 is 9.
Subtracting 5 produces 9 - 5 = 4.
So, the power level of this fuel cell is 4.

Here are some more example power levels:

Fuel cell at  122,79, grid serial number 57: power level -5.
Fuel cell at 217,196, grid serial number 39: power level  0.
Fuel cell at 101,153, grid serial number 71: power level  4.
Your goal is to find the 3x3 square which has the largest total power.
The square must be entirely within the 300x300 grid.
Identify this square using the X,Y coordinate of its top-left fuel cell.
For example:

For grid serial number 18, the largest total 3x3 square has a top-left corner of 33,45 (with a total power of 29); these fuel cells appear in the middle of this 5x5 region:

-2  -4   4   4   4
-4   4   4   4  -5
 4   3   3   4  -4
 1   1   2   4  -3
-1   0   2  -5  -2

For grid serial number 42, the largest 3x3 square's top-left is 21,61 (with a total power of 30); they are in the middle of this region:

-3   4   2   2   2
-4   4   3   3   4
-5   3   3   4  -4
 4   3   3   4  -3
 3   3   3  -5  -1

What is the X,Y coordinate of the top-left fuel cell of the 3x3 square with the largest total power?

*/

namespace Day11
{
    class Program
    {
        const int MAX_GRID_SIZE = 300;
        readonly static int[,] sPowerLevels = new int[MAX_GRID_SIZE, MAX_GRID_SIZE];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            if (lines.Length != 1)
            {
                throw new InvalidProgramException($"Invalid input expected a single line got {lines.Length}");
            }
            var serialNumber = int.Parse(lines[0]);
            ComputePowerLevels(serialNumber);

            if (part1)
            {
                var result1 = FindLargest3x3();
                Console.WriteLine($"Day11 : Result1 {result1}");
                var expected = (-1, -1);
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day11 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static int PowerLevel(int serialNumber, int x, int y)
        {
            //Find the fuel cell's rack ID, which is its X coordinate plus 10.
            //Begin with a power level of the rack ID times the Y coordinate.
            //Increase the power level by the value of the grid serial number (your puzzle input).
            //Set the power level to itself multiplied by the rack ID.
            //Keep only the hundreds digit of the power level (so 12345 becomes 3; numbers with no hundreds digit become 0).
            //Subtract 5 from the power level.
            var rackID = x + 10;
            var powerLevel = rackID * y;
            powerLevel += serialNumber;
            powerLevel *= rackID;
            powerLevel /= 100;
            powerLevel %= 10;
            powerLevel -= 5;
            return powerLevel;
        }

        public static void ComputePowerLevels(int serialNumber)
        {
            for (var y = 0; y < MAX_GRID_SIZE; ++y)
            {
                for (var x = 0; x < MAX_GRID_SIZE; ++x)
                {
                    sPowerLevels[x, y] = PowerLevel(serialNumber, x + 1, y + 1);
                }
            }
        }

        public static (int x0, int y0) FindLargest3x3()
        {
            return (int.MinValue, int.MinValue);
        }

        public static void Run()
        {
            Console.WriteLine("Day11 : Start");
            _ = new Program("Day11/input.txt", true);
            _ = new Program("Day11/input.txt", false);
            Console.WriteLine("Day11 : End");
        }
    }
}
