using System;

/*

--- Day 21: Chronal Conversion ---

You should have been watching where you were going, because as you wander the new North Pole base, you trip and fall into a very deep hole!

Just kidding. You're falling through time again.

If you keep up your current pace, you should have resolved all of the temporal anomalies by the next time the device activates.
Since you have very little interest in browsing history in 500-year increments for the rest of your life, you need to find a way to get back to your present time.

After a little research, you discover two important facts about the behavior of the device:

First, you discover that the device is hard-wired to always send you back in time in 500-year increments.
Changing this is probably not feasible.

Second, you discover the activation system (your puzzle input) for the time travel module.
Currently, it appears to run forever without halting.

If you can cause the activation system to halt at a specific moment, maybe you can make the device send you so far back in time that you cause an integer underflow in time itself and wrap around back to your current time!

The device executes the program as specified in manual section one and manual section two.

Your goal is to figure out how the program works and cause it to halt.
You can only control register 0; every other register begins at 0 as usual.

Because time travel is a dangerous activity, the activation system begins with a few instructions which verify that bitwise AND (via bani) does a numeric operation and not an operation as if the inputs were interpreted as strings.
If the test fails, it enters an infinite loop re-running the test instead of allowing the program to execute normally.
If the test passes, the program continues, and assumes that all other bitwise operations (banr, bori, and borr) also interpret their inputs as numbers.
(Clearly, the Elves who wrote this system were worried that someone might introduce a bug while trying to emulate this system with a scripting language.)

What is the lowest non-negative integer value for register 0 that causes the program to halt after executing the fewest instructions? (Executing the same instruction multiple times counts as multiple instructions executed.)

Your puzzle answer was 15690445.

--- Part Two ---

In order to determine the timing window for your underflow exploit, you also need an upper bound:

What is the lowest non-negative integer value for register 0 that causes the program to halt after executing the most instructions? (The program must actually halt; running forever does not count as halting.)

*/

namespace Day21
{
    class Program
    {
        const int MAX_NUM_INSTRUCTIONS = 1024;
        const int MAX_NUM_REGISTERS = 6;
        const int NUM_INTS_PER_INSTRUCTION = 4;
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
            eqrr = 15,
            UNKNOWN = 16
        };

        readonly private static int[] sRegisters = new int[MAX_NUM_REGISTERS];
        readonly private static int[,] sProgram = new int[MAX_NUM_INSTRUCTIONS, NUM_INTS_PER_INSTRUCTION];
        static int sInstructionsCount;
        static int sPCregister;

        private Program(string inputFile, bool part1)
        {
            var lines = AoC.Program.ReadLines(inputFile);
            Parse(lines);

            if (part1)
            {
                var result1 = ManualRunProgram(0, 0);
                Console.WriteLine($"Day21 : Result1 {result1}");
                var expected = 15690445;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = ManualRunProgram(1, 11000);
                Console.WriteLine($"Day21 : Result2 {result2}");
                var expected = 936387;
                if (result2 != expected)
                {
                    throw new InvalidProgramException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        private static int Instruction_addr(int A, int B)
        {
            // addr(add register) stores into register C the result of adding register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA + valueB;
            return output;
        }

        private static int Instruction_addi(int A, int B)
        {
            // addi(add immediate) stores into register C the result of adding register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA + valueB;
            return output;
        }

        private static int Instruction_mulr(int A, int B)
        {
            // mulr(multiply register) stores into register C the result of multiplying register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA * valueB;
            return output;
        }

        private static int Instruction_muli(int A, int B)
        {
            // muli(multiply immediate) stores into register C the result of multiplying register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA * valueB;
            return output;
        }

        private static int Instruction_banr(int A, int B)
        {
            // banr(bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA & valueB;
            return output;
        }

        private static int Instruction_bani(int A, int B)
        {
            // bani(bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA & valueB;
            return output;
        }

        private static int Instruction_borr(int A, int B)
        {
            // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA | valueB;
            return output;
        }

        private static int Instruction_bori(int A, int B)
        {
            // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA | valueB;
            return output;
        }

        private static int Instruction_setr(int A, int _)
        {
            // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
            var valueA = sRegisters[A];
            var output = valueA;
            return output;
        }

        private static int Instruction_seti(int A, int _)
        {
            // seti(set immediate) stores value A into register C. (Input B is ignored.)
            var valueA = A;
            var output = valueA;
            return output;
        }

        private static int Instruction_gtir(int A, int B)
        {
            // gtir(greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            var valueA = A;
            var valueB = sRegisters[B];
            var output = valueA > valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_gtri(int A, int B)
        {
            // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA > valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_gtrr(int A, int B)
        {
            // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA > valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_eqir(int A, int B)
        {
            // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            var valueA = A;
            var valueB = sRegisters[B];
            var output = valueA == valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_eqri(int A, int B)
        {
            // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = B;
            var output = valueA == valueB ? 1 : 0;
            return output;
        }

        private static int Instruction_eqrr(int A, int B)
        {
            // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            var valueA = sRegisters[A];
            var valueB = sRegisters[B];
            var output = valueA == valueB ? 1 : 0;
            return output;
        }

        private static void ExecuteInstruction(int i)
        {
            var opcode = sProgram[i, 0];
            var A = sProgram[i, 1];
            var B = sProgram[i, 2];
            var C = sProgram[i, 3];
            var instruction = (Instruction)opcode;
            if (instruction == Instruction.UNKNOWN)
            {
                throw new InvalidProgramException($"Executing an unknown instruction {i} opcode {opcode}");
            }
            sRegisters[C] = instruction switch
            {
                Instruction.addr => Instruction_addr(A, B),
                Instruction.addi => Instruction_addi(A, B),
                Instruction.mulr => Instruction_mulr(A, B),
                Instruction.muli => Instruction_muli(A, B),
                Instruction.banr => Instruction_banr(A, B),
                Instruction.bani => Instruction_bani(A, B),
                Instruction.borr => Instruction_borr(A, B),
                Instruction.bori => Instruction_bori(A, B),
                Instruction.setr => Instruction_setr(A, B),
                Instruction.seti => Instruction_seti(A, B),
                Instruction.gtir => Instruction_gtir(A, B),
                Instruction.gtri => Instruction_gtri(A, B),
                Instruction.gtrr => Instruction_gtrr(A, B),
                Instruction.eqir => Instruction_eqir(A, B),
                Instruction.eqri => Instruction_eqri(A, B),
                Instruction.eqrr => Instruction_eqrr(A, B),
                _ => throw new NotImplementedException()
            };
        }

        public static void Parse(string[] lines)
        {
            sInstructionsCount = 0;
            sPCregister = int.MinValue;
            if (lines.Length == 0)
            {
                throw new InvalidProgramException($"Empty input");
            }

            //#ip 1
            var line = lines[0].Trim();
            var tokens = line.Split();
            if (tokens.Length != 2)
            {
                throw new InvalidProgramException($"Invalid line '{line}' got {tokens.Length} tokens expected 2");
            }
            if (tokens[0].Trim() != "#ip")
            {
                throw new InvalidProgramException($"Invalid line '{line}' got '{tokens[0]}' expected '#ip'");
            }
            sPCregister = int.Parse(tokens[1]);
            if ((sPCregister < 0) || (sPCregister >= MAX_NUM_REGISTERS))
            {
                throw new InvalidProgramException($"Invalid line '{line}' PCregister {sPCregister} out of range 0-{MAX_NUM_REGISTERS}");
            }

            for (var y = 1; y < lines.Length; ++y)
            {
                //seti 5 0 1
                line = lines[y].Trim();
                tokens = line.Split();
                if (tokens.Length != 4)
                {
                    throw new InvalidProgramException($"Invalid line '{line}' got {tokens.Length} expected 4");
                }
                var instruction = tokens[0] switch
                {
                    "addr" => Instruction.addr,
                    "addi" => Instruction.addi,
                    "mulr" => Instruction.mulr,
                    "muli" => Instruction.muli,
                    "banr" => Instruction.banr,
                    "bani" => Instruction.bani,
                    "borr" => Instruction.borr,
                    "bori" => Instruction.bori,
                    "setr" => Instruction.setr,
                    "seti" => Instruction.seti,
                    "gtir" => Instruction.gtir,
                    "gtri" => Instruction.gtri,
                    "gtrr" => Instruction.gtrr,
                    "eqir" => Instruction.eqir,
                    "eqri" => Instruction.eqri,
                    "eqrr" => Instruction.eqrr,
                    _ => throw new InvalidProgramException($"Unknown register 'tokens[0]'")
                };
                sProgram[sInstructionsCount, 0] = (int)instruction;
                for (var i = 1; i < NUM_INTS_PER_INSTRUCTION; ++i)
                {
                    sProgram[sInstructionsCount, i] = int.Parse(tokens[i]);
                }
                ++sInstructionsCount;
            }
        }

        public static void RunProgram()
        {
            for (var r = 0; r < MAX_NUM_REGISTERS; ++r)
            {
                sRegisters[r] = 0;
            }
            var pc = 0;
            while ((pc >= 0) && (pc < sInstructionsCount))
            {
                sRegisters[sPCregister] = pc;
                ExecuteInstruction(pc);
                pc = sRegisters[sPCregister];
                ++pc;
            }
            //Console.WriteLine($"0:{sRegisters[0]} 1:{sRegisters[1]} 2:{sRegisters[2]} 3:{sRegisters[3]} 4:{sRegisters[4]} 5:{sRegisters[5]}");
        }

        public static int GetRegister(int register)
        {
            return sRegisters[register];
        }

        public static void Run()
        {
            Console.WriteLine("Day21 : Start");
            _ = new Program("Day21/input.txt", true);
            _ = new Program("Day21/input.txt", false);
            Console.WriteLine("Day21 : End");
        }

        private static int ManualRunProgram(int reg0, int maxLoops)
        {
            var values = new int[maxLoops + 1];
            var countValues = 0;
            var lastNewValue = int.MaxValue;
            var countLoops = 0;
            sRegisters[0] = reg0;
            goto Instruction_0;
        //goto Instruction_5;
        //#ip 1
        //  0: seti 123 0 4
        Instruction_0: sRegisters[4] = 123;
            goto Instruction_1;
        //  1: bani 4 456 4
        Instruction_1: sRegisters[4] = sRegisters[4] & 456;
            goto Instruction_2;
        //  1: bani 4 456 4
        //  2: eqri 4 72 4
        Instruction_2: //sRegisters[4] = (sRegisters[4] == 72) ? 1 : 0;
            goto Instruction_3;
        //  3: addr 4 1 1
        Instruction_3: //sRegisters[1] = sRegisters[4] + 3; // sRegisters[1];
            if (sRegisters[4] == 72)
            {
                sRegisters[4] = 1;
                goto Instruction_5;
            }
            else
            {
                sRegisters[4] = 0;
                goto Instruction_4;
            }
        //  4: seti 0 0 1
        Instruction_4: sRegisters[1] = 0;
            goto Instruction_1;
        //  5: seti 0 4 4
        Instruction_5: sRegisters[4] = 0;
        //  6: bori 4 65536 3
        Instruction_6: sRegisters[3] = sRegisters[4] | 65536;
            //  7: seti 12670166 8 4
            goto Instruction_7;
        Instruction_7: sRegisters[4] = 12670166;
            //  8: bani 3 255 2
            goto Instruction_8;
        Instruction_8: sRegisters[2] = sRegisters[3] & 255;
            //  9: addr 4 2 4
            goto Instruction_9;
        Instruction_9: sRegisters[4] = sRegisters[4] + sRegisters[2];
            // 10: bani 4 16777215 4
            goto Instruction_10;
        Instruction_10: sRegisters[4] = 16777215 & sRegisters[4];
            // 11: muli 4 65899 4
            goto Instruction_11;
        Instruction_11: sRegisters[4] = 65899 * sRegisters[4];
            // 12: bani 4 16777215 4
            goto Instruction_12;
        Instruction_12: sRegisters[4] = 16777215 & sRegisters[4];
            // 13: gtir 256 3 2
            goto Instruction_13;
        Instruction_13: sRegisters[2] = (256 > sRegisters[3]) ? 1 : 0;
            goto Instruction_14;
        // 14: addr 2 1 1
        Instruction_14: sRegisters[1] = sRegisters[2] + 14; // sRegisters[1];
            if (256 > sRegisters[3])
            {
                goto Instruction_16;
            }
            else
            {
                goto Instruction_15;
            }
        // 15: addi 1 1 1
        Instruction_15: sRegisters[1] = 16; // sRegisters[1] + 1;
            goto Instruction_17;
        // 16: seti 27 6 1
        Instruction_16: sRegisters[1] = 27;
            goto Instruction_28;
        // 17: seti 0 0 2
        Instruction_17: sRegisters[2] = 0;
        // 18: addi 2 1 5
        Instruction_18: sRegisters[5] = sRegisters[2] + 1;
            goto Instruction_19;
        // 19: muli 5 256 5
        Instruction_19: sRegisters[5] = 256 * sRegisters[5];
            goto Instruction_20;
        // 20: gtrr 5 3 5
        Instruction_20: //sRegisters[5] = (sRegisters[5] > sRegisters[3]) ? 1 : 0;
            goto Instruction_21;
        // 21: addr 5 1 1
        Instruction_21: //sRegisters[1] = sRegisters[5] + sRegisters[1];
            if (sRegisters[5] > sRegisters[3])
            {
                sRegisters[5] = 1;
                goto Instruction_23;
            }
            else
            {
                goto Instruction_22;
            }
        // 22: addi 1 1 1
        Instruction_22: sRegisters[1] = 23; // sRegisters[1] + 1;
            goto Instruction_24;
        // 23: seti 25 6 1
        Instruction_23: sRegisters[1] = 25;
            goto Instruction_26;
        // 24: addi 2 1 2
        Instruction_24: sRegisters[2] = sRegisters[2] + 1;
            goto Instruction_25;
        // 25: seti 17 8 1
        Instruction_25: sRegisters[1] = 17;
            goto Instruction_18;
        // 26: setr 2 5 3
        Instruction_26: sRegisters[3] = sRegisters[2];
            goto Instruction_27;
        // 27: seti 7 2 1
        Instruction_27: sRegisters[1] = 7;
            goto Instruction_8;
        // 28: eqrr 4 0 2
        Instruction_28: sRegisters[2] = (sRegisters[4] == sRegisters[0]) ? 1 : 0;
            goto Instruction_29;
        // 29: addr 2 1 1
        Instruction_29: sRegisters[1] = sRegisters[2] + 29; // sRegisters[1];
            if (sRegisters[4] == sRegisters[0])
            {
                goto Instruction_31;
            }
            else
            {
                bool found = false;
                for (var i = 0; i < countValues; ++i)
                {
                    if (values[i] == sRegisters[4])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    lastNewValue = sRegisters[4];
                    values[countValues] = sRegisters[4];
                    ++countValues;
                }
                //Console.WriteLine($"Loop: {sRegisters[4]:X} lastNew:{lastNewValue:X} found:{found}");
                ++countLoops;
                if (countLoops > maxLoops)
                {
                    Console.WriteLine($"END:{lastNewValue}");
                    return lastNewValue;
                }
                goto Instruction_30;
            }
        // 30: seti 5 8 1
        Instruction_30: sRegisters[1] = 5;
            goto Instruction_6;
        Instruction_31:
            Console.WriteLine($"SUCCESS {sRegisters[0]}");
            return lastNewValue;
        }
    }
}
