using System;

/*

--- Day 2: Inventory Management System ---

You stop falling through time, catch your breath, and check the screen on the device.
"Destination reached. Current Year: 1518. Current Location: North Pole Utility Closet 83N10." 
You made it! Now, to find those anomalies.

Outside the utility closet, you hear footsteps and a voice.
"...I'm not sure either.
But now that so many people have chimneys, maybe he could sneak in that way?" 
Another voice responds, 
"Actually, we've been working on a new kind of suit that would let him fit through tight spaces like that.
But, I heard that a few days ago, they lost the prototype fabric, the design plans, everything! Nobody on the team can even seem to remember important details of the project!"

"Wouldn't they have had enough fabric to fill several boxes in the warehouse? They'd be stored together, so the box IDs should be similar.
Too bad it would take forever to search the warehouse for two similar box IDs..." They walk too far away to hear any more.

Late at night, you sneak to the warehouse - who knows what kinds of paradoxes you could cause if you were discovered - and use your fancy wrist device to quickly scan every box and produce a list of the likely candidates (your puzzle input).

To make sure you didn't miss any, you scan the likely candidate boxes again, counting the number that have an ID containing exactly two of any letter and then separately counting those with exactly three of any letter.
You can multiply those two counts together to get a rudimentary checksum and compare it to what your device predicts.

For example, if you see the following box IDs:

abcdef contains no letters that appear exactly two or three times.
bababc contains two a and three b, so it counts for both.
abbcde contains two b, but no letter appears exactly three times.
abcccd contains three c, but no letter appears exactly two times.
aabcdd contains two a and two d, but it only counts once.
abcdee contains two e.
ababab contains three a and three b, but it only counts once.

Of these box IDs, four of them contain a letter which appears exactly twice, and three of them contain a letter which appears exactly three times.
Multiplying these together produces a checksum of 4 * 3 = 12.

What is the checksum for your list of box IDs?

Your puzzle answer was 8398.

--- Part Two ---

Confident that your list of box IDs is complete, you're ready to find the boxes full of prototype fabric.

The boxes will have IDs which differ by exactly one character at the same position in both strings.
For example, given the following box IDs:

abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz

The IDs abcde and axcye are close, but they differ by two characters (the second and fourth).
However, the IDs fghij and fguij differ by exactly one character, the third (h and u).
Those must be the correct boxes.

What letters are common between the two correct box IDs? 
(In the example above, this is found by removing the differing character from either ID, producing fgij.

*/

namespace Day02
{
    class Program
    {
        const int MAX_NUM_STRINGS = 1024;
        const int MAX_STRING_LENGTH = 64;

        static int sNumStrings;
        readonly static char[,] sStrings = new char[MAX_NUM_STRINGS, MAX_STRING_LENGTH];
        readonly static int[] sStringSizes = new int[MAX_NUM_STRINGS];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = Checksum();
                Console.WriteLine($"Day02 : Result1 {result1}");
                var expected = 8398;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = CommonChars();
                Console.WriteLine($"Day02 : Result2 {result2}");
                var expected = "hhvsdkatysmiqjxunezgwcdpr";
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            if (lines.Length > MAX_NUM_STRINGS)
            {
                throw new InvalidProgramException($"Too many input lines {lines.Length} MAX:{MAX_NUM_STRINGS}");
            }
            var s = 0;
            foreach (var line in lines)
            {
                var trimLine = line.Trim();
                var numChars = trimLine.Length;
                if (numChars == 0)
                {
                    continue;
                }
                if (numChars > MAX_STRING_LENGTH)
                {
                    throw new InvalidProgramException($"String is too long {numChars} MAX:{MAX_STRING_LENGTH}");
                }
                for (var i = 0; i < numChars; ++i)
                {
                    sStrings[s, i] = trimLine[i];
                }
                sStringSizes[s] = numChars;
                ++s;
            }
            sNumStrings = s;
        }

        public static long Checksum()
        {
            var countTwos = 0L;
            var countThrees = 0L;

            for (var s = 0; s < sNumStrings; ++s)
            {
                var numChars = sStringSizes[s];
                for (var i = 0; i < numChars - 1; ++i)
                {
                    for (var j = i + 1; j < numChars; ++j)
                    {
                        var ci = sStrings[s, i];
                        var cj = sStrings[s, j];
                        if (cj < ci)
                        {
                            sStrings[s, i] = cj;
                            sStrings[s, j] = ci;
                        }
                    }
                }
                var runCount = 0;
                char previousChar = (char)0;
                bool foundTwo = false;
                bool foundThree = false;
                for (var c = 0; c < numChars; ++c)
                {
                    var currentChar = sStrings[s, c];
                    if (currentChar == previousChar)
                    {
                        ++runCount;
                    }
                    else
                    {
                        if (!foundTwo && (runCount == 2))
                        {
                            ++countTwos;
                            foundTwo = true;
                        }
                        else if (!foundThree && (runCount == 3))
                        {
                            ++countThrees;
                            foundThree = true;
                        }
                        runCount = 1;
                        previousChar = currentChar;
                    }
                }
                if (!foundTwo && (runCount == 2))
                {
                    ++countTwos;
                }
                else if (!foundThree && (runCount == 3))
                {
                    ++countThrees;
                }
            }

            var checksum = countTwos * countThrees;
            return checksum;
        }

        public static string CommonChars()
        {
            var match1 = int.MinValue;
            var match2 = int.MinValue;
            var differenceIndex = int.MinValue;
            for (var s1 = 0; s1 < sNumStrings - 1; ++s1)
            {
                var numChars1 = sStringSizes[s1];
                for (var s2 = s1 + 1; s2 < sNumStrings; ++s2)
                {
                    var numChars2 = sStringSizes[s2];
                    if (numChars1 != numChars2)
                    {
                        continue;
                    }
                    var differenceCount = 0;
                    for (var i = 0; i < numChars2; ++i)
                    {
                        if (sStrings[s1, i] != sStrings[s2, i])
                        {
                            ++differenceCount;
                            differenceIndex = i;
                        }
                        if (differenceCount > 1)
                        {
                            break;
                        }
                    }
                    if (differenceCount == 1)
                    {
                        match1 = s1;
                        match2 = s2;
                        break;
                    }
                }
                if ((match1 != int.MinValue) && (match2 != int.MinValue))
                {
                    break;
                }
            }

            var result = "";
            for (var i = 0; i < sStringSizes[match1]; ++i)
            {
                if (i != differenceIndex)
                {
                    result += sStrings[match1, i];
                }
            }

            Console.WriteLine($"Match1 {match1} Match2 {match2}");
            return result;
        }

        public static void Run()
        {
            Console.WriteLine("Day02 : Start");
            _ = new Program("Day02/input.txt", true);
            _ = new Program("Day02/input.txt", false);
            Console.WriteLine("Day02 : End");
        }
    }
}
