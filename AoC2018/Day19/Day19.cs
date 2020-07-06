using System;

/*

--- Day 19: Go With The Flow ---

With the Elves well on their way constructing the North Pole base, you turn your attention back to understanding the inner workings of programming the device.

You can't help but notice that the device's opcodes don't contain any flow control like jump instructions.
The device's manual goes on to explain:

"In programs where flow control is required, the instruction pointer can be bound to a register so that it can be manipulated directly.
This way, setr/seti can function as absolute jumps, addr/addi can function as relative jumps, and other opcodes can cause truly fascinating effects."

This mechanism is achieved through a declaration like #ip 1, which would modify register 1 so that accesses to it let the program indirectly access the instruction pointer itself.
To compensate for this kind of binding, there are now six registers (numbered 0 through 5); the five not bound to the instruction pointer behave as normal.
Otherwise, the same rules apply as the last time you worked with this device.

When the instruction pointer is bound to a register, its value is written to that register just before each instruction is executed, and the value of that register is written back to the instruction pointer immediately after each instruction finishes execution.
Afterward, move to the next instruction by adding one to the instruction pointer, even if the value in the instruction pointer was just updated by an instruction.
(Because of this, instructions must effectively set the instruction pointer to the instruction before the one they want executed next.)

The instruction pointer is 0 during the first instruction, 1 during the second, and so on.
If the instruction pointer ever causes the device to attempt to load an instruction outside the instructions defined in the program, the program instead immediately halts.
The instruction pointer starts at 0.

It turns out that this new information is already proving useful: the CPU in the device is not very powerful, and a background process is occupying most of its time.
You dump the background process' declarations and instructions to a file (your puzzle input), making sure to use the names of the opcodes rather than the numbers.

For example, suppose you have the following program:

#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5
When executed, the following instructions are executed.
Each line contains the value of the instruction pointer at the time the instruction started, the values of the six registers before executing the instructions (in square brackets), the instruction itself, and the values of the six registers after executing the instruction (also in square brackets).

ip=0 [0, 0, 0, 0, 0, 0] seti 5 0 1 [0, 5, 0, 0, 0, 0]
ip=1 [1, 5, 0, 0, 0, 0] seti 6 0 2 [1, 5, 6, 0, 0, 0]
ip=2 [2, 5, 6, 0, 0, 0] addi 0 1 0 [3, 5, 6, 0, 0, 0]
ip=4 [4, 5, 6, 0, 0, 0] setr 1 0 0 [5, 5, 6, 0, 0, 0]
ip=6 [6, 5, 6, 0, 0, 0] seti 9 0 5 [6, 5, 6, 0, 0, 9]
In detail, when running this program, the following events occur:

The first line (#ip 0) indicates that the instruction pointer should be bound to register 0 in this program.
This is not an instruction, and so the value of the instruction pointer does not change during the processing of this line.
The instruction pointer contains 0, and so the first instruction is executed (seti 5 0 1).
It updates register 0 to the current instruction pointer value (0), sets register 1 to 5, sets the instruction pointer to the value of register 0 (which has no effect, as the instruction did not modify register 0), and then adds one to the instruction pointer.
The instruction pointer contains 1, and so the second instruction, seti 6 0 2, is executed.
This is very similar to the instruction before it: 6 is stored in register 2, and the instruction pointer is left with the value 2.
The instruction pointer is 2, which points at the instruction addi 0 1 0.
This is like a relative jump: the value of the instruction pointer, 2, is loaded into register 0.
Then, addi finds the result of adding the value in register 0 and the value 1, storing the result, 3, back in register 0.
Register 0 is then copied back to the instruction pointer, which will cause it to end up 1 larger than it would have otherwise and skip the next instruction (addr 1 2 3) entirely.
Finally, 1 is added to the instruction pointer.
The instruction pointer is 4, so the instruction setr 1 0 0 is run.
This is like an absolute jump: it copies the value contained in register 1, 5, into register 0, which causes it to end up in the instruction pointer.
The instruction pointer is then incremented, leaving it at 6.
The instruction pointer is 6, so the instruction seti 9 0 5 stores 9 into register 5.
The instruction pointer is incremented, causing it to point outside the program, and so the program ends.

What value is left in register 0 when the background process halts?

Your puzzle answer was 1922.

--- Part Two ---

A new background process immediately spins up in its place.
It appears identical, but on closer inspection, you notice that this time, register 0 started with the value 1.

What value is left in register 0 when this new background process halts?

*/

namespace Day19
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
                //RunProgram();
                RunPart2(0, 0);
                var result1 = GetRegister(0);
                Console.WriteLine($"Day19 : Result1 {result1}");
                var expected = 1922;
                if (result1 != expected)
                {
                    throw new InvalidProgramException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                // TOO HIGH: 123276832
                RunPart2(0, 1);
                var result2 = GetRegister(0);
                Console.WriteLine($"Day19 : Result2 {result2}");
                var expected = 123276832;
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

            //#ip 0
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
            Console.WriteLine("Day19 : Start");
            _ = new Program("Day19/input.txt", true);
            _ = new Program("Day19/input.txt", false);
            Console.WriteLine("Day19 : End");
        }

        private static int SumOfFactors(int value)
        {
            var total = 0;
            for (var i = 1; i <= Math.Sqrt(value); ++i)
            {
                var j = value / i;
                if (j * i == value)
                {
                    total += i;
                    if (j != i)
                    {
                        total += j;
                    }
                }
            }
            return total;
        }

        private static void RunPart2(int register, int value)
        {
            for (var r = 0; r < MAX_NUM_REGISTERS; ++r)
            {
                sRegisters[r] = 0;
            }
            sRegisters[register] = value;

            int pc;

            //  0 : addi 4 16 4
            pc = 0 + 16 + 1;
            goto label_17;

        label_1:
            //Console.WriteLine($"0:{sRegisters[0]} 1:{sRegisters[1]} 2:{sRegisters[2]} 3:{sRegisters[3]} 4:{sRegisters[4]} 5:{sRegisters[5]}");
            //  1 : seti 1 8 1
            sRegisters[1] = 1;
            sRegisters[0] = SumOfFactors(sRegisters[2]);
            return;

        label_2:
            //  2 : seti 1 3 5
            sRegisters[5] = 1;

        label_3:
            //  3 : mulr 1 5 3
            sRegisters[3] = sRegisters[1] * sRegisters[5];

            //  4 : eqrr 3 2 3
            sRegisters[3] = sRegisters[3] == sRegisters[2] ? 1 : 0;

            //  5 : addr 3 4 4
            pc = sRegisters[3] + 5 + 1;
            if (pc == 7)
                goto label_7;
            else if (pc == 6)
                goto label_6;
            else
                throw new NotImplementedException($"pc {pc}");

            label_6:
            //  6 : addi 4 1 4
            pc = 6 + 1 + 1;
            goto label_8;

        label_7:
            //  7 : addr 1 0 0
            sRegisters[0] = sRegisters[1] + sRegisters[0];

        label_8:
            //  8 : addi 5 1 5
            sRegisters[5] = sRegisters[5] + 1;

            //  9 : gtrr 5 2 3
            sRegisters[3] = sRegisters[5] > sRegisters[2] ? 1 : 0;

            // 10 : addr 4 3 4
            pc = 10 + sRegisters[3] + 1;
            if (pc == 12)
                goto label_12;
            else if (pc == 11)
                goto label_11;
            else
                throw new NotImplementedException($"pc {pc}");

            label_11:
            // 11 : seti 2 2 4
            pc = 2 + 1;
            goto label_3;

        label_12:
            // 12 : addi 1 1 1
            sRegisters[1] = sRegisters[1] + 1;

            // 13 : gtrr 1 2 3
            sRegisters[3] = sRegisters[1] > sRegisters[2] ? 1 : 0;

            //Console.WriteLine($"0:{sRegisters[0]} 1:{sRegisters[1]} 2:{sRegisters[2]} 3:{sRegisters[3]} 4:{sRegisters[4]} 5:{sRegisters[5]}");
            // 14 : addr 3 4 4
            pc = sRegisters[3] + 14 + 1;
            if (pc == 16)
                goto label_16;
            else if (pc == 15)
                goto label_15;
            else
                throw new NotImplementedException($"pc {pc}");

            label_15:
            // 15 : seti 1 4 4
            pc = 1 + 1;
            goto label_2;

        label_16:
            // 16 : mulr 4 4 4
            pc = 16 * 16 + 1;
            goto label_257;

        label_17:
            // 17 : addi 2 2 2
            sRegisters[2] = sRegisters[2] + 2;
            sRegisters[2] = 2;

            // 18 : mulr 2 2 2
            sRegisters[2] = sRegisters[2] * sRegisters[2];
            sRegisters[2] = 4;

            // 19 : mulr 4 2 2
            sRegisters[2] = 19 * sRegisters[2];
            sRegisters[2] = 19 * 4;

            // 20 : muli 2 11 2
            sRegisters[2] = sRegisters[2] * 11;
            sRegisters[2] = 19 * 4 * 11;

            // 21 : addi 3 6 3
            sRegisters[3] = sRegisters[3] + 6;
            sRegisters[3] = 6;

            // 22 : mulr 3 4 3
            sRegisters[3] = sRegisters[3] * 22;
            sRegisters[3] = 6 * 22;

            // 23 : addi 3 8 3
            sRegisters[3] = sRegisters[3] + 8;
            sRegisters[3] = 6 * 22 + 8;

            // 24 : addr 2 3 2
            sRegisters[2] = sRegisters[2] + sRegisters[3];
            sRegisters[2] = 19 * 4 * 11 + 6 * 22 + 8;

            // 25 : addr 4 0 4
            pc = 25 + sRegisters[0] + 1;
            if (pc == 26)
                goto label_26;
            else if (pc == 27)
                goto label_27;
            else
                throw new NotImplementedException($"pc {pc}");

            label_26:
            // 26 : seti 0 1 4
            pc = sRegisters[0] + 1;
            if (pc == 1)
                goto label_1;
            else if (pc == 2)
                goto label_2;
            else
                throw new NotImplementedException($"pc {pc}");

            label_27:
            // 27 : setr 4 4 3
            sRegisters[3] = 27 + sRegisters[3];

            // 28 : mulr 3 4 3
            sRegisters[3] = sRegisters[3] * 28;

            // 29 : addr 4 3 3
            sRegisters[3] = 29 + sRegisters[3];

            // 30 : mulr 4 3 3
            sRegisters[3] = 30 * sRegisters[3];

            // 31 : muli 3 14 3
            sRegisters[3] = sRegisters[3] * 14;

            // 32 : mulr 3 4 3
            sRegisters[3] = sRegisters[3] * 32;

            // 33 : addr 2 3 2
            sRegisters[2] = sRegisters[2] + sRegisters[3];

            // 34 : seti 0 4 0
            sRegisters[0] = 0;

            // 35 : seti 0 7 4
            pc = 0 + 1;
            goto label_1;

        label_257:
            return;
        }
    }
}
