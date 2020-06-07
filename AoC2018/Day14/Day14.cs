using System;

/*

--- Day 14: Chocolate Charts ---

You finally have a chance to look at all of the produce moving around.
Chocolate, cinnamon, mint, chili peppers, nutmeg, vanilla...
the Elves must be growing these plants to make hot chocolate! 
As you realize this, you hear a conversation in the distance.
When you go to investigate, you discover two Elves in what appears to be a makeshift underground kitchen/laboratory.

The Elves are trying to come up with the ultimate hot chocolate recipe; they're even maintaining a scoreboard which tracks the quality score (0-9) of each recipe.

Only two recipes are on the board: the first recipe got a score of 3, the second, 7.
Each of the two Elves has a current recipe: the first Elf starts with the first recipe, and the second Elf starts with the second recipe.

To create new recipes, the two Elves combine their current recipes.
This creates new recipes from the digits of the sum of the current recipes' scores.
With the current recipes' scores of 3 and 7, their sum is 10, and so two new recipes would be created: the first with score 1 and the second with score 0.
If the current recipes' scores were 2 and 3, the sum, 5, would only create one recipe (with a score of 5) with its single digit.

The new recipes are added to the end of the scoreboard in the order they are created.
So, after the first round, the scoreboard is 3, 7, 1, 0.

After all new recipes are added to the scoreboard, each Elf picks a new current recipe.
To do this, the Elf steps forward through the scoreboard a number of recipes equal to 1 plus the score of their current recipe.
So, after the first round, the first Elf moves forward 1 + 3 = 4 times, while the second Elf moves forward 1 + 7 = 8 times.
If they run out of recipes, they loop back around to the beginning.
After the first round, both Elves happen to loop around until they land on the same recipe that they had in the beginning; in general, they will move to different recipes.

Drawing the first Elf as parentheses and the second Elf as square brackets, they continue this process:

(3)[7]
(3)[7] 1  0 
 3  7  1 [0](1) 0 
 3  7  1  0 [1] 0 (1)
(3) 7  1  0  1  0 [1] 2 
 3  7  1  0 (1) 0  1  2 [4]
 3  7  1 [0] 1  0 (1) 2  4  5 
 3  7  1  0 [1] 0  1  2 (4) 5  1 
 3 (7) 1  0  1  0 [1] 2  4  5  1  5 
 3  7  1  0  1  0  1  2 [4](5) 1  5  8 
 3 (7) 1  0  1  0  1  2  4  5  1  5  8 [9]
 3  7  1  0  1  0  1 [2] 4 (5) 1  5  8  9  1  6 
 3  7  1  0  1  0  1  2  4  5 [1] 5  8  9  1 (6) 7 
 3  7  1  0 (1) 0  1  2  4  5  1  5 [8] 9  1  6  7  7 
 3  7 [1] 0  1  0 (1) 2  4  5  1  5  8  9  1  6  7  7  9 
 3  7  1  0 [1] 0  1  2 (4) 5  1  5  8  9  1  6  7  7  9  2 

The Elves think their skill will improve after making a few recipes (your puzzle input).

However, that could take ages; you can speed this up considerably by identifying the scores of the ten recipes after that.

For example:

If the Elves think their skill will improve after making 9 recipes, the scores of the ten recipes after the first nine on the scoreboard would be 5158916779 (highlighted in the last line of the diagram).
After 5 recipes, the scores of the next ten would be 0124515891.
After 18 recipes, the scores of the next ten would be 9251071085.
After 2018 recipes, the scores of the next ten would be 5941429882.
What are the scores of the ten recipes immediately after the number of recipes in your puzzle input?

Your puzzle answer was 4910101614.

--- Part Two ---

As it turns out, you got the Elves' plan backwards.
They actually want to know how many recipes appear on the scoreboard to the left of the first recipes whose scores are the digits from your puzzle input.

51589 first appears after 9 recipes.
01245 first appears after 5 recipes.
92510 first appears after 18 recipes.
59414 first appears after 2018 recipes.

How many recipes appear on the scoreboard to the left of the score sequence in your puzzle input?

*/

namespace Day14
{
    class Program
    {
        const int MAX_NUM_RECIPES = 1024 * 1024 * 32;
        const int MAX_NUM_ITERATIONS = 1024 * 1024 * 16;
        readonly static byte[] sRecipes = new byte[MAX_NUM_RECIPES];
        static int sRecipeCount;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            var numRecipes = int.Parse(lines[0]);

            if (part1)
            {
                var result1 = NextTenRecipes(numRecipes);
                Console.WriteLine($"Day14 : Result1 {result1}");
                var expected = "4910101614";
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = HowManyRecipes(lines[0].Trim());
                Console.WriteLine($"Day14 : Result2 {result2}");
                var expected = 20253137;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static string NextTenRecipes(int numRecipes)
        {
            sRecipeCount = 2;
            sRecipes[0] = 3;
            sRecipes[1] = 7;
            var chef1 = 0;
            var chef2 = 1;

            for (var r = 0; r < numRecipes + 10; ++r)
            {
                var recipe1 = sRecipes[chef1];
                var recipe2 = sRecipes[chef2];
                var total = recipe1 + recipe2;
                var tens = total / 10;
                if (tens != 0)
                {
                    sRecipes[sRecipeCount] = (byte)tens;
                    ++sRecipeCount;
                }
                var units = total % 10;
                sRecipes[sRecipeCount] = (byte)units;
                ++sRecipeCount;
                chef1 += 1 + recipe1;
                chef2 += 1 + recipe2;
                chef1 %= sRecipeCount;
                chef2 %= sRecipeCount;
                if (sRecipeCount > numRecipes + 10)
                {
                    break;
                }
            }
            if (sRecipeCount < numRecipes + 10)
            {
                throw new InvalidProgramException($"Not enough recipes made max:{sRecipeCount} num:{numRecipes}");
            }

            var recipes = new char[10];
            for (var r = 0; r < 10; ++r)
            {
                recipes[r] = (char)('0' + sRecipes[r + numRecipes]);
            }

            return new string(recipes);
        }

        public static int HowManyRecipes(string pattern)
        {
            sRecipeCount = 2;
            sRecipes[0] = 3;
            sRecipes[1] = 7;
            var chef1 = 0;
            var chef2 = 1;
            var patternLength = pattern.Length;

            for (var i = 0; i < MAX_NUM_ITERATIONS; ++i)
            {
                var recipe1 = sRecipes[chef1];
                var recipe2 = sRecipes[chef2];
                var total = recipe1 + recipe2;
                var tens = total / 10;
                bool foundIt;
                int end;
                if (tens != 0)
                {
                    sRecipes[sRecipeCount] = (byte)tens;
                    ++sRecipeCount;
                    foundIt = true;
                    end = sRecipeCount;
                    for (var r = 0; r < patternLength; ++r)
                    {
                        var charFound = (char)('0' + sRecipes[end - r - 1]);
                        var charToMatch = pattern[patternLength - r - 1];
                        if (charFound != charToMatch)
                        {
                            foundIt = false;
                            break;
                        }
                    }
                    if (foundIt)
                    {
                        return sRecipeCount - patternLength;
                    }
                }
                var units = total % 10;
                sRecipes[sRecipeCount] = (byte)units;
                ++sRecipeCount;
                chef1 += 1 + recipe1;
                chef2 += 1 + recipe2;
                chef1 %= sRecipeCount;
                chef2 %= sRecipeCount;

                foundIt = true;
                end = sRecipeCount;
                for (var r = 0; r < patternLength; ++r)
                {
                    var charFound = (char)('0' + sRecipes[end - r - 1]);
                    var charToMatch = pattern[patternLength - r - 1];
                    if (charFound != charToMatch)
                    {
                        foundIt = false;
                        break;
                    }
                }
                if (foundIt)
                {
                    return sRecipeCount - patternLength;
                }
            }
            throw new InvalidProgramException($"Recipe pattern {pattern} not found after {MAX_NUM_ITERATIONS} iterations");
        }

        public static void Run()
        {
            Console.WriteLine("Day14 : Start");
            _ = new Program("Day14/input.txt", true);
            _ = new Program("Day14/input.txt", false);
            Console.WriteLine("Day14 : End");
        }
    }
}
