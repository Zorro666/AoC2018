using System;

/*

--- Day 16: Chronal Classification ---

As you see the Elves defend their hot chocolate successfully, you go back to falling through time.
This is going to become a problem.

If you're ever going to return to your own time, you need to understand how this device on your wrist works.
You have a little while before you reach your next destination, and with a bit of trial and error, you manage to pull up a programming manual on the device's tiny screen.

According to the manual, the device has four registers (numbered 0 through 3) that can be manipulated by instructions containing one of 16 opcodes.
The registers start with the value 0.

Every instruction consists of four values: an opcode, two inputs (named A and B), and an output (named C), in that order.
The opcode specifies the behavior of the instruction and how the inputs are interpreted.
The output, C, is always treated as a register.

In the opcode descriptions below, if something says "value A", it means to take the number given as A literally.
(This is also called an "immediate" value.) If something says "register A", it means to use the number given as A to read from (or write to) the register with that number.
So, if the opcode addi adds register A and value B, storing the result in register C, and the instruction addi 0 7 3 is encountered, it would add 7 to the value contained by register 0 and store the sum in register 3, never modifying registers 0, 1, or 2 in the process.

Many opcodes are similar except for how they interpret their arguments.
The opcodes fall into seven general categories:

Addition:

addr (add register) stores into register C the result of adding register A and register B.
addi (add immediate) stores into register C the result of adding register A and value B.

Multiplication:

mulr (multiply register) stores into register C the result of multiplying register A and register B.
muli (multiply immediate) stores into register C the result of multiplying register A and value B.

Bitwise AND:

banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.

Bitwise OR:

borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.

Assignment:

setr (set register) copies the contents of register A into register C. (Input B is ignored.)
seti (set immediate) stores value A into register C. (Input B is ignored.)

Greater-than testing:

gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.

Equality testing:

eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.

Unfortunately, while the manual gives the name of each opcode, it doesn't seem to indicate the number.

However, you can monitor the CPU to see the contents of the registers before and after instructions are executed to try to work them out.

Each opcode has a number from 0 through 15, but the manual doesn't say which is which.

For example, suppose you capture the following sample:

Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]
This sample shows the effect of the instruction 9 2 1 2 on the registers.
Before the instruction is executed, register 0 has value 3, register 1 has value 2, and registers 2 and 3 have value 1.
After the instruction is executed, register 2's value becomes 2.

The instruction itself, 9 2 1 2, means that opcode 9 was executed with A=2, B=1, and C=2.
Opcode 9 could be any of the 16 opcodes listed above, but only three of them behave in a way that would cause the result shown in the sample:

Opcode 9 could be mulr: register 2 (which has a value of 1) times register 1 (which has a value of 2) produces 2, which matches the value stored in the output register, register 2.
Opcode 9 could be addi: register 2 (which has a value of 1) plus value 1 produces 2, which matches the value stored in the output register, register 2.
Opcode 9 could be seti: value 2 matches the value stored in the output register, register 2; the number given for B is irrelevant.
None of the other opcodes produce the result captured in the sample.
Because of this, the sample above behaves like three opcodes.

You collect many of these samples (the first section of your puzzle input).
The manual also includes a small test program (the second section of your puzzle input) - you can ignore it for now.

Ignoring the opcode numbers, how many samples in your puzzle input behave like three or more opcodes?

Your puzzle answer was 663.

--- Part Two ---

Using the samples you collected, work out the number of each opcode and execute the test program (the second section of your puzzle input).

What value is contained in register 0 after executing the test program?

*/

namespace Day16
{
    class Program
    {
        const int MAX_NUM_SAMPLES = 1024;
        const int MAX_NUM_REGISTERS = 4;
        enum Instruction
        {
            addr = 0,
            addi = 1,
            mulr = 2,
            muli = 3,
            banr = 4,
            bani = 5,
            borr = 6,
            bori = 7,
            setr = 8,
            seti = 9,
            gtir = 10,
            gtri = 11,
            gtrr = 12,
            eqir = 13,
            eqri = 14,
            eqrr = 15
        };

        readonly private static int[,] sBeforeRegisters = new int[MAX_NUM_SAMPLES, MAX_NUM_REGISTERS];
        readonly private static int[,] sAfterRegisters = new int[MAX_NUM_SAMPLES, MAX_NUM_REGISTERS];
        readonly private static int[] sOpcodes = new int[MAX_NUM_SAMPLES];
        readonly private static int[] sInputAs = new int[MAX_NUM_SAMPLES];
        readonly private static int[] sInputBs = new int[MAX_NUM_SAMPLES];
        readonly private static int[] sOutputRegs = new int[MAX_NUM_SAMPLES];
        readonly private static int[] sRegisters = new int[MAX_NUM_REGISTERS];
        static int sSamplesCount;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = CountThreeOrMoreOpcodes();
                Console.WriteLine($"Day16 : Result1 {result1}");
                var expected = 663;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -123;
                Console.WriteLine($"Day16 : Result2 {result2}");
                var expected = 1797;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        public static void Run()
        {
            Console.WriteLine("Day16 : Start");
            _ = new Program("Day16/input.txt", true);
            _ = new Program("Day16/input.txt", false);
            Console.WriteLine("Day16 : End");
        }

        public static void Parse(string[] lines)
        {
            sSamplesCount = 0;
            bool lookingForBefore = true;
            bool lookingForOpcode = false;
            bool lookingForAfter = false;
            int programStart = int.MinValue;
            for (var y = 0; y < lines.Length; ++y)
            {
                var l = lines[y].Trim();
                if (l.Length == 0)
                {
                    continue;
                }
                // 'Before: [1, 1, 3, 2]'
                // '13 1 2 3'
                // 'After: [1, 1, 3, 0]'
                // ''
                // ''
                // ''
                // '14 3 3 2'
                if (lookingForBefore == true)
                {
                    if (l.StartsWith("Before: [") == false)
                    {
                        var opcodes = l.Split();
                        if (opcodes.Length == 4)
                        {
                            programStart = y;
                            break;
                        }
                        else
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting 'Before: [x, y, z, w]'");
                        }
                    }
                    else
                    {
                        // 'Before: [1, 1, 3, 2]'
                        var tokens = l.Split('[');
                        if (tokens.Length != 2)
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting 2 tokens split by '[' got {tokens.Length}");
                        }
                        if (tokens[1][^1] != ']')
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting ']' as final character got {tokens[1][^1]}");
                        }
                        var instructionText = tokens[1].TrimEnd(']');
                        tokens = instructionText.Split(',');
                        if (tokens.Length != 4)
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting 4 tokens got {tokens.Length}");
                        }

                        for (var i = 0; i < 4; ++i)
                        {
                            sBeforeRegisters[sSamplesCount, i] = int.Parse(tokens[i]);
                        }
                        lookingForBefore = false;
                        lookingForOpcode = true;
                        lookingForAfter = false;
                    }
                }
                else if (lookingForOpcode == true)
                {
                    // '13 1 2 3'
                    var tokens = l.Split();
                    if (tokens.Length != 4)
                    {
                        throw new InvalidProgramException($"Bad line '{l}' expecting 4 tokens got {tokens.Length}");
                    }

                    sOpcodes[sSamplesCount] = int.Parse(tokens[0]);
                    sInputAs[sSamplesCount] = int.Parse(tokens[1]);
                    sInputBs[sSamplesCount] = int.Parse(tokens[2]);
                    sOutputRegs[sSamplesCount] = int.Parse(tokens[3]);
                    lookingForBefore = false;
                    lookingForOpcode = false;
                    lookingForAfter = true;
                }
                else if (lookingForAfter == true)
                {
                    // 'After:  [1, 1, 3, 0]'
                    if (l.StartsWith("After:  [") == false)
                    {
                        throw new InvalidProgramException($"Bad line '{l}' expecting 'After:  [x, y, z, w]'");
                    }
                    else
                    {
                        // 'After:  [1, 1, 3, 2]'
                        var tokens = l.Split('[');
                        if (tokens.Length != 2)
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting 2 tokens split by '[' got {tokens.Length}");
                        }
                        if (tokens[1][^1] != ']')
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting ']' as final character got {tokens[1][^1]}");
                        }
                        var instructionText = tokens[1].TrimEnd(']');
                        tokens = instructionText.Split(',');
                        if (tokens.Length != 4)
                        {
                            throw new InvalidProgramException($"Bad line '{l}' expecting 4 tokens got {tokens.Length}");
                        }

                        for (var i = 0; i < 4; ++i)
                        {
                            sAfterRegisters[sSamplesCount, i] = int.Parse(tokens[i]);
                        }
                        ++sSamplesCount;
                        lookingForBefore = true;
                        lookingForOpcode = false;
                        lookingForAfter = false;
                    }
                }
            }
        }

        private static int Instruction_addr(int A, int B, int C)
        {
            // addr(add register) stores into register C the result of adding register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA + valueB;
            return output;
        }

        private static int Instruction_addi(int A, int B, int C)
        {
            // addi(add immediate) stores into register C the result of adding register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA + valueB;
            return output;
        }

        private static int Instruction_mulr(int A, int B, int C)
        {
            // mulr(multiply register) stores into register C the result of multiplying register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA * valueB;
            return output;
        }

        private static int Instruction_muli(int A, int B, int C)
        {
            // muli(multiply immediate) stores into register C the result of multiplying register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA * valueB;
            return output;
        }

        private static int Instruction_banr(int A, int B, int C)
        {
            // banr(bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA & valueB;
            return output;
        }

        private static int Instruction_bani(int A, int B, int C)
        {
            // bani(bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA & valueB;
            return output;
        }

        private static int Instruction_borr(int A, int B, int C)
        {
            // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA | valueB;
            return output;
        }

        private static int Instruction_bori(int A, int B, int C)
        {
            // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA | valueB;
            return output;
        }

        private static int Instruction_setr(int A, int _, int C)
        {
            // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
            var valueA = sRegisters[A];
            var output = valueA;
            return output;
        }

        private static int Instruction_seti(int A, int _, int C)
        {
            // seti(set immediate) stores value A into register C. (Input B is ignored.)
            var valueA = A;
            var output = valueA;
            return output;
        }

        private static int Instruction_gtir(int A, int B, int C)
        {
            // gtir(greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            var valueA = A;
            var valueB = sRegisters[B];
            var output = valueA > valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_gtri(int A, int B, int C)
        {
            // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA > valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_gtrr(int A, int B, int C)
        {
            // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA > valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_eqir(int A, int B, int C)
        {
            // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            var valueA = A;
            var valueB = sRegisters[B];
            var output = valueA == valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_eqri(int A, int B, int C)
        {
            // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA == valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_eqrr(int A, int B, int C)
        {
            // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA == valueB ? 1 : 0;
            return output;
        }

        private static void PrepareRegisters(int s)
        {
            for (var i = 0; i < MAX_NUM_REGISTERS; ++i)
            {
                sRegisters[i] = sBeforeRegisters[s, i];
            }
        }

        private static bool TestRegisters(int s)
        {
            for (var i = 0; i < MAX_NUM_REGISTERS; ++i)
            {
                if (sRegisters[i] != sAfterRegisters[s, i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool TestInstruction(int s, Instruction instruction)
        {
            var A = sInputAs[s];
            var B = sInputBs[s];
            var C = sOutputRegs[s];

            PrepareRegisters(s);
            sRegisters[C] = instruction switch
            {
                Instruction.addr => Instruction_addr(A, B, C),
                Instruction.addi => Instruction_addi(A, B, C),
                Instruction.mulr => Instruction_mulr(A, B, C),
                Instruction.muli => Instruction_muli(A, B, C),
                Instruction.banr => Instruction_banr(A, B, C),
                Instruction.bani => Instruction_bani(A, B, C),
                Instruction.borr => Instruction_borr(A, B, C),
                Instruction.bori => Instruction_bori(A, B, C),
                Instruction.setr => Instruction_setr(A, B, C),
                Instruction.seti => Instruction_seti(A, B, C),
                Instruction.gtir => Instruction_gtir(A, B, C),
                Instruction.gtri => Instruction_gtri(A, B, C),
                Instruction.gtrr => Instruction_gtrr(A, B, C),
                Instruction.eqir => Instruction_eqir(A, B, C),
                Instruction.eqri => Instruction_eqri(A, B, C),
                Instruction.eqrr => Instruction_eqrr(A, B, C),
                _ => throw new NotImplementedException()
            };

            return TestRegisters(s);
        }

        private static int TestSample(int s)
        {
            var count = 0;

            count += TestInstruction(s, Instruction.addr) ? 1 : 0;
            count += TestInstruction(s, Instruction.addi) ? 1 : 0;
            count += TestInstruction(s, Instruction.mulr) ? 1 : 0;
            count += TestInstruction(s, Instruction.muli) ? 1 : 0;
            count += TestInstruction(s, Instruction.banr) ? 1 : 0;
            count += TestInstruction(s, Instruction.bani) ? 1 : 0;
            count += TestInstruction(s, Instruction.borr) ? 1 : 0;
            count += TestInstruction(s, Instruction.bori) ? 1 : 0;
            count += TestInstruction(s, Instruction.setr) ? 1 : 0;
            count += TestInstruction(s, Instruction.seti) ? 1 : 0;
            count += TestInstruction(s, Instruction.gtir) ? 1 : 0;
            count += TestInstruction(s, Instruction.gtri) ? 1 : 0;
            count += TestInstruction(s, Instruction.gtrr) ? 1 : 0;
            count += TestInstruction(s, Instruction.eqir) ? 1 : 0;
            count += TestInstruction(s, Instruction.eqri) ? 1 : 0;
            count += TestInstruction(s, Instruction.eqrr) ? 1 : 0;
            return count;
        }

        public static int CountThreeOrMoreOpcodes()
        {
            var count = 0;
            for (var i = 0; i < sSamplesCount; ++i)
            {
                count += TestSample(i) >= 3 ? 1 : 0;
            }
            return count;
        }
    }
}
