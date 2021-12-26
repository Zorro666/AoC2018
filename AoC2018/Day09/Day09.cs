using System;

/*

--- Day 9: Marble Mania ---

You talk to the Elves while you wait for your navigation system to initialize.
To pass the time, they introduce you to their favorite marble game.

The Elves play this game by taking turns arranging the marbles in a circle according to very particular rules.
The marbles are numbered starting with 0 and increasing by 1 until every marble has a number.

First, the marble numbered 0 is placed in the circle.
At this point, while it contains only a single marble, it is still a circle: the marble is both clockwise from itself and counter-clockwise from itself.
This marble is designated the current marble.

Then, each Elf takes a turn placing the lowest-numbered remaining marble into the circle between the marbles that are 1 and 2 marbles clockwise of the current marble.
(When the circle is large enough, this means that there is one marble between the marble that was just placed and the current marble.) The marble that was just placed then becomes the current marble.

However, if the marble that is about to be placed has a number which is a multiple of 23, something entirely different happens.
First, the current player keeps the marble they would have placed, adding it to their score.
In addition, the marble 7 marbles counter-clockwise from the current marble is removed from the circle and also added to the current player's score.
The marble located immediately clockwise of the marble that was removed becomes the new current marble.

For example, suppose there are 9 players.
After the marble with value 0 is placed in the middle, each player (shown in square brackets) takes a turn.
The result of each of those turns would produce circles of marbles like this, where clockwise is to the right and the resulting current marble is in parentheses:

[-] (0)
[1]  0 (1)
[2]  0 (2) 1 
[3]  0  2  1 (3)
[4]  0 (4) 2  1  3 
[5]  0  4  2 (5) 1  3 
[6]  0  4  2  5  1 (6) 3 
[7]  0  4  2  5  1  6  3 (7)
[8]  0 (8) 4  2  5  1  6  3  7 
[9]  0  8  4 (9) 2  5  1  6  3  7 
[1]  0  8  4  9  2(10) 5  1  6  3  7 
[2]  0  8  4  9  2 10  5(11) 1  6  3  7 
[3]  0  8  4  9  2 10  5 11  1(12) 6  3  7 
[4]  0  8  4  9  2 10  5 11  1 12  6(13) 3  7 
[5]  0  8  4  9  2 10  5 11  1 12  6 13  3(14) 7 
[6]  0  8  4  9  2 10  5 11  1 12  6 13  3 14  7(15)
[7]  0(16) 8  4  9  2 10  5 11  1 12  6 13  3 14  7 15 
[8]  0 16  8(17) 4  9  2 10  5 11  1 12  6 13  3 14  7 15 
[9]  0 16  8 17  4(18) 9  2 10  5 11  1 12  6 13  3 14  7 15 
[1]  0 16  8 17  4 18  9(19) 2 10  5 11  1 12  6 13  3 14  7 15 
[2]  0 16  8 17  4 18  9 19  2(20)10  5 11  1 12  6 13  3 14  7 15 
[3]  0 16  8 17  4 18  9 19  2 20 10(21) 5 11  1 12  6 13  3 14  7 15 
[4]  0 16  8 17  4 18  9 19  2 20 10 21  5(22)11  1 12  6 13  3 14  7 15 
[5]  0 16  8 17  4 18(19) 2 20 10 21  5 22 11  1 12  6 13  3 14  7 15 
[6]  0 16  8 17  4 18 19  2(24)20 10 21  5 22 11  1 12  6 13  3 14  7 15 
[7]  0 16  8 17  4 18 19  2 24 20(25)10 21  5 22 11  1 12  6 13  3 14  7 15

The goal is to be the player with the highest score after the last marble is used up.
Assuming the example above ends after the marble numbered 25, the winning score is 23+9=32 (because player 5 kept marble 23 and removed marble 9, while no other player got any points in this very short example game).

Here are a few more examples:

10 players; last marble is worth 1618 points: high score is 8317
13 players; last marble is worth 7999 points: high score is 146373
17 players; last marble is worth 1104 points: high score is 2764
21 players; last marble is worth 6111 points: high score is 54718
30 players; last marble is worth 5807 points: high score is 37305

What is the winning Elf's score?

--- Part Two ---

Amused by the speed of your answer, the Elves are curious:

What would the new winning Elf's score be if the number of the last marble were 100 times larger?

*/

namespace Day09
{
    class Program
    {
        const int MAX_NUM_MARBLES = 1024 * 128 * 128;
        readonly static int[] sMarblesNext = new int[MAX_NUM_MARBLES];
        readonly static int[] sMarblesPrev = new int[MAX_NUM_MARBLES];
        readonly static int[] sMarblesValues = new int[MAX_NUM_MARBLES];
        static int sPlayersCount;
        static int sLastMarbleValue;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = HighScore();
                Console.WriteLine($"Day09 : Result1 {result1}");
                var expected = 413188;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                sLastMarbleValue *= 100;
                var result2 = HighScore();
                Console.WriteLine($"Day09 : Result2 {result2}");
                var expected = 3377272893;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            if (lines.Length != 1)
            {
                throw new InvalidProgramException($"Invalid input expected a single line got {lines.Length}");
            }
            // '9 players; last marble is worth 25 points'
            var tokens = lines[0].Trim().Split();
            if ((tokens[1] != "players;") || (tokens[2] != "last") || (tokens[3] != "marble") ||
                (tokens[4] != "is") || (tokens[5] != "worth") || (tokens[7] != "points"))
            {
                throw new InvalidProgramException($"Invalid input '{lines[0]}' expected 'X players; last marble is worth Y points'");
            }
            sPlayersCount = int.Parse(tokens[0]);
            sLastMarbleValue = int.Parse(tokens[6]);
        }

        static int Next(int index)
        {
            return sMarblesNext[index];
        }

        static int Prev(int index)
        {
            return sMarblesPrev[index];
        }

        static int Append(int index, int newNode)
        {
            var oldNext = Next(index);
            // BEFORE
            // node[index] -> next
            // node[index] <- next

            // AFTER
            // node[index] -> NEW NODE
            sMarblesNext[index] = newNode;
            // node[index] <- NEW NODE
            sMarblesPrev[newNode] = index;

            // NEW NODE -> next
            sMarblesNext[newNode] = oldNext;
            // NEW NODE <- next
            sMarblesPrev[oldNext] = newNode;

            return newNode;
        }

        static int Remove(int index)
        {
            var oldPrev = Prev(index);
            var oldNext = Next(index);
            // BEFORE
            // node[index] -> next
            // node[index] <- next
            // prev -> node[index]
            // prev <- node[index] <- next

            // AFTER
            // prev -> next
            sMarblesNext[oldPrev] = oldNext;
            // prev <- next
            sMarblesPrev[oldNext] = oldPrev;

            sMarblesNext[index] = -1;
            sMarblesPrev[index] = -1;
            sMarblesValues[index] = -1;

            return oldNext;
        }

        public static long HighScore()
        {
            var highScore = long.MinValue;
            var currentMarbleIndex = 0;
            sMarblesValues[currentMarbleIndex] = 0;
            sMarblesNext[currentMarbleIndex] = 0;
            sMarblesPrev[currentMarbleIndex] = 0;
            var marblesCount = 1;
            var playerScores = new long[sPlayersCount];
            for (var m = 1; m <= sLastMarbleValue; ++m)
            {
                if (m % 23 != 0)
                {
                    var appendIndex = Next(currentMarbleIndex);
                    currentMarbleIndex = Append(appendIndex, marblesCount);
                    sMarblesValues[currentMarbleIndex] = m;
                    currentMarbleIndex = marblesCount;
                    ++marblesCount;
                }
                else
                {
                    var currentPlayer = m % sPlayersCount;
                    var removeIndex = currentMarbleIndex;
                    for (var i = 0; i < 7; ++i)
                    {
                        removeIndex = Prev(removeIndex);
                    }
                    var removedMarble = sMarblesValues[removeIndex];
                    playerScores[currentPlayer] += removedMarble;
                    playerScores[currentPlayer] += m;
                    highScore = Math.Max(highScore, playerScores[currentPlayer]);
                    currentMarbleIndex = Remove(removeIndex);
                }
                /*
                int index = 0;
                for (var i = 0; i < marblesCount; ++i)
                {
                    Console.Write($"{sMarblesValues[index]} ");
                    index = Next(index);
                }
                Console.WriteLine($"");
                */
            }

            return highScore;
        }

        public static void Run()
        {
            Console.WriteLine("Day09 : Start");
            _ = new Program("Day09/input.txt", true);
            _ = new Program("Day09/input.txt", false);
            Console.WriteLine("Day09 : End");
        }
    }
}
