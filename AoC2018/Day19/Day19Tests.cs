using NUnit.Framework;

namespace Day19
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] {
"#ip 0",
"seti 5 0 1",
"seti 6 0 2",
"addi 0 1 0",
"addr 1 2 3",
"setr 1 0 0",
"seti 8 0 4",
"seti 9 0 5"
        }, 5, 9, TestName = "RunProgram A Reg 5 = 9")]
        public void RunProgram(string[] program, int register, int expected)
        {
            Program.Parse(program);
            Program.RunProgram();
            Assert.That(Program.GetRegister(5), Is.EqualTo(expected));
        }
    }
}
