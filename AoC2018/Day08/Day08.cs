using System;

/*

--- Day 8: Memory Maneuver ---

The sleigh is much easier to pull than you'd expect for something its weight.
Unfortunately, neither you nor the Elves know which way the North Pole is from here.

You check your wrist device for anything that might help.
It seems to have some kind of navigation system! Activating the navigation system produces more bad news: "Failed to start navigation system.
Could not read software license file."

The navigation system's license file consists of a list of numbers (your puzzle input).
The numbers define a data structure which, when processed, produces some kind of tree that can be used to calculate the license number.

The tree is made up of nodes; a single, outermost node forms the tree's root, and it contains all other nodes in the tree (or contains nodes that contain nodes, and so on).

Specifically, a node consists of:

A header, which is always exactly two numbers:
The quantity of child nodes.
The quantity of metadata entries.
Zero or more child nodes (as specified in the header).
One or more metadata entries (as specified in the header).
Each child node is itself a node that has its own header, child nodes, and metadata.
For example:

2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
A----------------------------------
    B----------- C-----------
                     D-----
In this example, each node of the tree is also marked with an underline starting with a letter for easier identification.
In it, there are four nodes:

A, which has 2 child nodes (B, C) and 3 metadata entries (1, 1, 2).
B, which has 0 child nodes and 3 metadata entries (10, 11, 12).
C, which has 1 child node (D) and 1 metadata entry (2).
D, which has 0 child nodes and 1 metadata entry (99).
The first check done on the license file is to simply add up all of the metadata entries.
In this example, that sum is 1+1+2+10+11+12+2+99=138.

What is the sum of all metadata entries?

Your puzzle answer was 48155.

--- Part Two ---

The second check is slightly more complicated: you need to find the value of the root node (A in the example above).

The value of a node depends on whether it has child nodes.

If a node has no child nodes, its value is the sum of its metadata entries.
So, the value of node B is 10+11+12=33, and the value of node D is 99.

However, if a node does have child nodes, the metadata entries become indexes which refer to those child nodes.
A metadata entry of 1 refers to the first child node, 2 to the second, 3 to the third, and so on.
The value of this node is the sum of the values of the child nodes referenced by the metadata entries.
If a referenced child node does not exist, that reference is skipped.
A child node can be referenced multiple time and counts each time it is referenced.
A metadata entry of 0 does not refer to any child node.

For example, again using the above nodes:

Node C has one metadata entry, 2.
Because node C has only one child node, 2 references a child node which does not exist, and so the value of node C is 0.
Node A has three metadata entries: 1, 1, and 2.
The 1 references node A's first child node, B, and the 2 references node A's second child node, C.
Because node B has a value of 33 and node C has a value of 0, the value of node A is 33+33+0=66.
So, in this example, the value of the root node is 66.

What is the value of the root node?

*/

namespace Day08
{
    class Program
    {
        const int MAX_NUM_METADATA_PER_NODE = 128;
        const int MAX_NUM_CHILD_NODES = 128;
        const int MAX_NUM_NODES = 2048;
        const int MAX_NUM_VALUES = MAX_NUM_NODES * MAX_NUM_METADATA_PER_NODE;
        readonly static int[,] sMetadatas = new int[MAX_NUM_NODES, MAX_NUM_METADATA_PER_NODE];
        readonly static int[,] sChilds = new int[MAX_NUM_NODES, MAX_NUM_CHILD_NODES];
        readonly static int[] sChildsCount = new int[MAX_NUM_NODES];
        readonly static int[] sMetadatasCount = new int[MAX_NUM_NODES];
        readonly static int[] sValues = new int[MAX_NUM_VALUES];
        static int sValuesCount;
        static int sNodesCount;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = Sum();
                Console.WriteLine($"Day08 : Result1 {result1}");
                var expected = 44893;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = RootNodeValue();
                Console.WriteLine($"Day08 : Result2 {result2}");
                var expected = 27433;
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
                throw new InvalidProgramException($"Bad input expected a single line got {lines.Length}");
            }
            var tokens = lines[0].Trim().Split();
            sValuesCount = 0;
            foreach (var token in tokens)
            {
                sValues[sValuesCount] = int.Parse(token);
                ++sValuesCount;
            }

            sNodesCount = 0;
            var end = ReadNode(0);
            if (end != sValuesCount)
            {
                throw new InvalidProgramException($"Unexpected end after reading all nodes expected {sValuesCount} got {end}");
            }
        }

        static int ReadNode(int start)
        {
            // A header, which is always exactly two numbers:
            // The quantity of child nodes.
            // The quantity of metadata entries.
            // Zero or more child nodes (as specified in the header).
            // One or more metadata entries (as specified in the header).
            // Each child node is itself a node that has its own header, child nodes, and metadata.
            var thisNode = sNodesCount;
            ++sNodesCount;
            var childNodesCount = sValues[start + 0];
            var metadatasCount = sValues[start + 1];
            var index = start + 2;

            sChildsCount[thisNode] = childNodesCount;
            for (var c = 0; c < childNodesCount; ++c)
            {
                sChilds[thisNode, c] = sNodesCount;
                index = ReadNode(index);
            }

            sMetadatasCount[thisNode] = metadatasCount;
            for (var m = 0; m < metadatasCount; ++m)
            {
                sMetadatas[thisNode, m] = sValues[index];
                ++index;
            }

            return index;
        }

        public static long Sum()
        {
            var sum = 0L;
            for (var n = 0; n < sNodesCount; ++n)
            {
                for (var m = 0; m < sMetadatasCount[n]; ++m)
                {
                    sum += sMetadatas[n, m];
                }
            }
            return sum;
        }

        public static long RootNodeValue()
        {
            return NodeValue(0);
        }

        static long NodeValue(int node)
        {
            var sum = 0L;
            //If a node has no child nodes, its value is the sum of its metadata entries.
            if (sChildsCount[node] == 0)
            {
                for (var m = 0; m < sMetadatasCount[node]; ++m)
                {
                    sum += sMetadatas[node, m];
                }
                return sum;
            }
            //However, if a node does have child nodes, the metadata entries become indexes which refer to those child nodes.
            //A metadata entry of 1 refers to the first child node, 2 to the second, 3 to the third, and so on.
            //The value of this node is the sum of the values of the child nodes referenced by the metadata entries.
            //If a referenced child node does not exist, that reference is skipped.
            //A child node can be referenced multiple time and counts each time it is referenced.
            //A metadata entry of 0 does not refer to any child node.
            for (var m = 0; m < sMetadatasCount[node]; ++m)
            {
                var childIndex = sMetadatas[node, m];
                if (childIndex == 0)
                {
                    continue;
                }
                childIndex -= 1;
                if (childIndex >= sChildsCount[node])
                {
                    continue;
                }
                var childNodeIndex = sChilds[node, childIndex];
                sum += NodeValue(childNodeIndex);
            }
            return sum;
        }

        public static void Run()
        {
            Console.WriteLine("Day08 : Start");
            _ = new Program("Day08/input.txt", true);
            _ = new Program("Day08/input.txt", false);
            Console.WriteLine("Day08 : End");
        }
    }
}
