using System;

/*
--- Day 7: The Sum of Its Parts ---

You find yourself standing on a snow-covered coastline; apparently, you landed a little off course.
The region is too hilly to see the North Pole from here, but you do spot some Elves that seem to be trying to unpack something that washed ashore.
It's quite cold out, so you decide to risk creating a paradox by asking them for directions.

"Oh, are you the search party?" Somehow, you can understand whatever Elves from the year 1018 speak; you assume it's Ancient Nordic Elvish.
Could the device on your wrist also be a translator? "Those clothes don't look very warm; take this." They hand you a heavy coat.

"We do need to find our way back to the North Pole, but we have higher priorities at the moment.
You see, believe it or not, this box contains something that will solve all of Santa's transportation problems - at least, that's what it looks like from the pictures in the instructions." It doesn't seem like they can read whatever language it's in, but you can: "Sleigh kit.
Some assembly required."

"'Sleigh'? What a wonderful name! You must help us assemble this 'sleigh' at once!" They start excitedly pulling more parts out of the box.

The instructions specify a series of steps and requirements about which steps must be finished before others can begin (your puzzle input).
Each step is designated by a single letter.
For example, suppose you have the following instructions:

Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.
Visually, these requirements look like this:

  -->A--->B--
 /    \      \
C      -->D----->E
 \           /
  ---->F-----

Your first goal is to determine the order in which the steps should be completed.
If more than one step is ready, choose the step which is first alphabetically.
In this example, the steps would be completed as follows:

Only C is available, and so it is done first.
Next, both A and F are available. A is first alphabetically, so it is done next.
Then, even though F was available earlier, steps B and D are now also available, and B is the first alphabetically of the three.
After that, only D and F are available. E is not available because only some of its prerequisites are complete. Therefore, D is completed next.
F is the only choice, so it is done next.
Finally, E is completed.
So, in this example, the correct order is CABDFE.

In what order should the steps in your instructions be completed?

Your puzzle answer was BFLNGIRUSJXEHKQPVTYOCZDWMA.

--- Part Two ---

As you're about to begin construction, four of the Elves offer to help.
"The sun will set soon; it'll go faster if we work together." 
Now, you need to account for multiple people working on steps simultaneously.
If multiple steps are available, workers should still begin them in alphabetical order.

Each step takes 60 seconds plus an amount corresponding to its letter: A=1, B=2, C=3, and so on.
So, step A takes 60+1=61 seconds, while step Z takes 60+26=86 seconds.
No time is required between steps.

To simplify things for the example, however, suppose you only have help from one Elf (a total of two workers) and that each step takes 60 fewer seconds (so that step A takes 1 second and step Z takes 26 seconds).
Then, using the same instructions as above, this is how each second would be spent:

Second   Worker 1   Worker 2   Done
   0        C          .        
   1        C          .        
   2        C          .        
   3        A          F       C
   4        B          F       CA
   5        B          F       CA
   6        D          F       CAB
   7        D          F       CAB
   8        D          F       CAB
   9        D          .       CABF
  10        E          .       CABFD
  11        E          .       CABFD
  12        E          .       CABFD
  13        E          .       CABFD
  14        E          .       CABFD
  15        .          .       CABFDE
Each row represents one second of time.
The Second column identifies how many seconds have passed as of the beginning of that second.
Each worker column shows the step that worker is currently doing (or . if they are idle).
The Done column shows completed steps.

Note that the order of the steps has changed; this is because steps now take time to finish and multiple workers can begin multiple steps simultaneously.

In this example, it would take 15 seconds for two workers to complete these steps.

With 5 workers and the 60+ second step durations described above, how long will it take to complete all of the steps?
*/

namespace Day07
{
    class Program
    {
        const int MAX_NUM_NODES = 128;
        readonly static bool[,] sParents = new bool[MAX_NUM_NODES, MAX_NUM_NODES];
        readonly static bool[,] sChildren = new bool[MAX_NUM_NODES, MAX_NUM_NODES];
        readonly static bool[] sActiveNodes = new bool[MAX_NUM_NODES];
        readonly static bool[] sCompletedNodes = new bool[MAX_NUM_NODES];

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = ConstructionOrder();
                Console.WriteLine($"Day07 : Result1 {result1}");
                var expected = "BFLNGIRUSJXEHKQPVTYOCZDWMA";
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day07 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Parse(string[] lines)
        {
            for (var i = 0; i < MAX_NUM_NODES; ++i)
            {
                for (var j = 0; j < MAX_NUM_NODES; ++j)
                {
                    sParents[i, j] = false;
                    sChildren[i, j] = false;
                }
                sActiveNodes[i] = false;
                sCompletedNodes[i] = false;
            }
            foreach (var line in lines)
            {
                var tokens = line.Trim().Split();
                bool validLine = false;
                // "Step C must be finished before step A can begin.",
                if (tokens.Length == 10)
                {
                    if ((tokens[0] == "Step") && (tokens[2] == "must") && (tokens[3] == "be") &&
                        (tokens[4] == "finished") && (tokens[5] == "before") && (tokens[6] == "step") &&
                        (tokens[8] == "can") && (tokens[9] == "begin."))
                    {
                        var parent = tokens[1].Trim();
                        var child = tokens[7].Trim();
                        if ((child.Length == 1) && (parent.Length == 1))
                        {
                            int childIndex = child[0];
                            int parentIndex = parent[0];
                            if ((childIndex >= 'A') && (childIndex <= 'Z') &&
                                (parentIndex >= 'A') && (parentIndex <= 'Z'))
                            {
                                validLine = true;
                                if (sParents[childIndex, parentIndex])
                                {
                                    throw new InvalidProgramException($"Invalid line '{line}' Node {childIndex} '{child[0]}' already has this parent {parentIndex} '{parent[0]}'");
                                }
                                if (sChildren[parentIndex, childIndex])
                                {
                                    throw new InvalidProgramException($"Invalid line '{line}' Parent {parentIndex} '{parent[0]}' already has this child {childIndex} '{child[0]}'");
                                }
                                sActiveNodes[childIndex] = true;
                                sActiveNodes[parentIndex] = true;
                                sParents[childIndex, parentIndex] = true;
                                sChildren[parentIndex, childIndex] = true;
                            }
                        }
                    }
                }
                if (!validLine)
                {
                    throw new InvalidProgramException($"Bad line '{line}' Expected 'Step [A-Z] must be finished before step [A-Z] can begin.");
                }
            }
        }

        public static string ConstructionOrder()
        {
            for (var i = 0; i < MAX_NUM_NODES; ++i)
            {
                sCompletedNodes[i] = false;
            }

            string order = "";
            bool doMore;
            do
            {
                doMore = false;
                for (var i = 0; i < MAX_NUM_NODES; ++i)
                {
                    if (!sActiveNodes[i])
                    {
                        continue;
                    }
                    if (sCompletedNodes[i])
                    {
                        continue;
                    }
                    bool parentsAllCompleted = true;
                    for (var j = 0; j < MAX_NUM_NODES; ++j)
                    {
                        if (sParents[i, j] && !sCompletedNodes[j])
                        {
                            parentsAllCompleted = false;
                            break;
                        }
                    }
                    if (parentsAllCompleted)
                    {
                        doMore = true;
                        order += (char)i;
                        sCompletedNodes[i] = true;
                        break;
                    }
                }
            }
            while (doMore);
            return order;
        }

        public static void Run()
        {
            Console.WriteLine("Day07 : Start");
            _ = new Program("Day07/input.txt", true);
            _ = new Program("Day07/input.txt", false);
            Console.WriteLine("Day07 : End");
        }
    }
}
