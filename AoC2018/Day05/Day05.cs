using System;

/*

--- Day 5: Alchemical Reduction ---

You've managed to sneak in to the prototype suit manufacturing lab.
The Elves are making decent progress, but are still struggling with the suit's size reduction capabilities.

While the very latest in 1518 alchemical technology might have solved their problem eventually, you can do better.
You scan the chemical composition of the suit's material and discover that it is formed by extremely long polymers (one of which is available as your puzzle input).

The polymer is formed by smaller units which, when triggered, react with each other such that two adjacent units of the same type and opposite polarity are destroyed.
Units' types are represented by letters; units' polarity is represented by capitalization.
For instance, r and R are units with the same type but opposite polarity, whereas r and s are entirely different types and do not react.

For example:

In aA, a and A react, leaving nothing behind.
In abBA, bB destroys itself, leaving aA. As above, this then destroys itself, leaving nothing.
In abAB, no two adjacent units are of the same type, and so nothing happens.
In aabAAB, even though aa and AA are of the same type, their polarities match, and so nothing happens.
Now, consider a larger example, dabAcCaCBAcCcaDA:

dabAcCaCBAcCcaDA  The first 'cC' is removed.
dabAaCBAcCcaDA    This creates 'Aa', which is removed.
dabCBAcCcaDA      Either 'cC' or 'Cc' are removed (the result is the same).
dabCBAcaDA        No further actions can be taken.
After all possible reactions, the resulting polymer contains 10 units.

How many units remain after fully reacting the polymer you scanned? (Note: in this puzzle and others, the input is large; if you copy/paste your input, make sure you get the whole thing.)

Your puzzle answer was 10886.

--- Part Two ---

Time to improve the polymer.

One of the unit types is causing problems; it's preventing the polymer from collapsing as much as it should.
Your goal is to figure out which unit type is causing the most problems, remove all instances of it (regardless of polarity), fully react the remaining polymer, and measure its length.

For example, again using the polymer dabAcCaCBAcCcaDA from above:

Removing all A/a units produces dbcCCBcCcD. Fully reacting this polymer produces dbCBcD, which has length 6.
Removing all B/b units produces daAcCaCAcCcaDA. Fully reacting this polymer produces daCAcaDA, which has length 8.
Removing all C/c units produces dabAaBAaDA. Fully reacting this polymer produces daDA, which has length 4.
Removing all D/d units produces abAcCaCBAcCcaA. Fully reacting this polymer produces abCBAc, which has length 6.
In this example, removing all C/c units was best, producing the answer 4.

What is the length of the shortest polymer you can produce by removing all units of exactly one type and fully reacting the result?

*/

namespace Day05
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            if (lines.Length != 1)
            {
                throw new InvalidProgramException($"Invalid input must have one line only {lines.Length}");
            }

            if (part1)
            {
                var result1 = Reduce(lines[0]);
                Console.WriteLine($"Day05 : Result1 {result1}");
                var expected = 10886;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = Shortest(lines[0]);
                Console.WriteLine($"Day05 : Result2 {result2}");
                var expected = 4684;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static int Reduce(string input)
        {
            var chars = input.ToCharArray();
            return ReduceImpl(ref chars);
        }

        static int ReduceImpl(ref char[] chars)
        {
            var caseDifference = Math.Abs('a' - 'A');
            bool madeReplacement;
            do
            {
                madeReplacement = false;
                var lastCharIndex = 0;
                for (var i = 1; i < chars.Length; ++i)
                {
                    var lastChar = chars[lastCharIndex];
                    var thisChar = chars[i];
                    if (lastChar == 0)
                    {
                        lastChar = thisChar;
                    }
                    if (thisChar != 0)
                    {
                        var diff = Math.Abs(thisChar - lastChar);
                        if (diff == caseDifference)
                        {
                            chars[lastCharIndex] = (char)0;
                            chars[i] = (char)0;
                            madeReplacement = true;
                        }
                        lastCharIndex = i;
                    }
                }
            } while (madeReplacement);

            var compoundCount = 0;
            for (var i = 0; i < chars.Length; ++i)
            {
                if (chars[i] != 0)
                {
                    ++compoundCount;
                }
            }
            return compoundCount;
        }

        public static int Shortest(string input)
        {
            var chars = input.ToCharArray();
            var totalCount = chars.Length;
            var minLength = ReduceImpl(ref chars);

            var oldChars = new char[totalCount];
            for (var r = 0; r < 26; ++r)
            {
                char lowerC = (char)('a' + r);
                char upperC = (char)('A' + r);
                bool replacedChar = false;
                for (var i = 0; i < totalCount; ++i)
                {
                    oldChars[i] = chars[i];
                    var c = chars[i];
                    if (c == 0)
                    {
                        continue;
                    }
                    if ((c == lowerC) || (c == upperC))
                    {
                        chars[i] = (char)0;
                        replacedChar = true;
                    }
                }
                if (replacedChar)
                {
                    var length = ReduceImpl(ref chars);
                    minLength = Math.Min(length, minLength);
                    for (var i = 0; i < totalCount; ++i)
                    {
                        chars[i] = oldChars[i];
                    }
                }
            }

            return minLength;
        }

        public static void Run()
        {
            Console.WriteLine("Day05 : Start");
            _ = new Program("Day05/input.txt", true);
            _ = new Program("Day05/input.txt", false);
            Console.WriteLine("Day05 : End");
        }
    }
}
