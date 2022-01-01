using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advent2021.Solutions.Day24
{
    public record Result(string Part1, string Part2) { }

    public static class Day24
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var program = input.ToArray();
            var param = ExtractParams(program);
            /*
                w = inp()
                x = z % 26 + S
                z /= ZScale
                if (x != w) {
                    z = z * 26 + w + T
                }

                z = 0
            (1, 11, 7)
                x = 11 -- no valid w
                z = a0 + 7

                z = a0 + 7
            (1, 14, 8)
                x = a0 + 7 + 14  -- no valid w
                z = 26*(a0 + 7) + a1 + 8

                z = (a0 + 7, a1 + 8)
            (1, 10, 16)
                x = a1 + 8 + 10  -- no valid w
                z = 26*(a0 + 7, a1 + 8) + a2 + 16
                
                z = (a0 + 7, a1 + 8, a2 + 16)
            (1, 14, 8)
                x = a2 + 16 + 14 -- no valid w
                z = 26*(a0 + 7, a1 + 8, a2 + 16) + a3 + 8

                z = (a0 + 7, a1 + 8, a2 + 16, a3 + 8)
            (26, -8, 3)
                z = (a0 + 7, a1 + 8, a2 + 16)
                x = a3 + 8 - 8 = a3
                -- a4 == a3

                z = (a0 + 7, a1 + 8, a2 + 16)
            (1, 14, 12)
                x = a2 + 16 + 14 -- no valid w
                z = 26*(a0 + 7, a1 + 8, a2 + 16) + a5 + 12

                z = (a0 + 7, a1 + 8, a2 + 16, a5 + 12)
            (26, -11, 1)
                z = (a0 + 7, a1 + 8, a2 + 16)
                x = a5 + 12 - 11 = a5 + 1
                -- a6 == a5 + 1

                z = (a0 + 7, a1 + 8, a2 + 16)
            (1, 10, 8)
                x = a2 + 16 + 10 -- no valid w
                z = (a0 + 7, a1 + 8, a2 + 16, a7 + 8)
                
                z = (a0 + 7, a1 + 8, a2 + 16, a7 + 8)
            (26, -6, 8)
                z = (a0 + 7, a1 + 8, a2 + 16)
                x = a7 + 8 - 6 = a7 + 2
                -- a8 == a7 + 2

                z = (a0 + 7, a1 + 8, a2 + 16)
            (26, -9, 14)
                z = (a0 + 7, a1 + 8)
                x = a2 + 16 - 9 = a2 + 7
                -- a9 == a2 + 7

                z = (a0 + 7, a1 + 8)
            (1, 12, 4)
                z = (a0 + 7, a1 + 8, a10 + 4)
            (26, -5, 14)
                -- a11 = a10 - 1
                z = (a0 + 7, a1 + 8)
            (26, -4, 15)
                -- a12 = a1 + 4
                z = (a0 + 7)
                (26, -9, 6)
                -- a13 = a0 - 2

            -- a4 == a3
            -- a6 == a5 + 1
            -- a8 == a7 + 2
            -- a9 == a2 + 7
            -- a11 = a10 - 1
            -- a12 = a1 + 4
            -- a13 = a0 - 2
            */

            var model1 = new int[] {9,5,2,9,9,8,9,7,9,9,9,8,9,7};
            var alu1 = RunProgram(program, model1);
            Debug.Assert(alu1.Z == 0);

            var model2 = new int[] {3,1,1,1,1,1,2,1,3,8,2,1,5,1};
            var alu2 = RunProgram(program, model2);
            Debug.Assert(alu2.Z == 0);

            var part1 = string.Join("", model1);
            var part2 = string.Join("", model2);

            return new Result(part1, part2);
        }

        public static (string op, string a, string b) ParseInstruction(string instr)
        {
            var parts = instr.Split(null);
            if (parts.Length == 2)
            {
                return (parts[0], parts[1], "");
            }
            else
            {
                return (parts[0], parts[1], parts[2]);
            }
        }

        public static ALU RunProgram(IEnumerable<string> program, int[] inputs)
        {
            var alu = new ALU();
            var inputQueue = new Queue<int>(inputs);
            foreach (var instr in program.Select(ParseInstruction))
            {
                alu.Exec(instr.op, instr.a, instr.b, inputQueue);
            }
            return alu;
        }

        public static List<(int, int, int)> ExtractParams(IEnumerable<string> input)
        {
            var program = input.Select(ParseInstruction).ToArray();
            var chunk = new (string op, string a, string b)[] {
                ("inp", "w", ""),
                ("mul", "x", "0"),
                ("add", "x", "z"),
                ("mod", "x", "26"),
                ("div", "z", "*"),
                ("add", "x", "*"),
                ("eql", "x", "w"),
                ("eql", "x", "0"),
                ("mul", "y", "0"),
                ("add", "y", "25"),
                ("mul", "y", "x"),
                ("add", "y", "1"),
                ("mul", "z", "y"),
                ("mul", "y", "0"),
                ("add", "y", "w"),
                ("add", "y", "*"),
                ("mul", "y", "x"),
                ("add", "z", "y")
            };

            var param = new List<(int, int, int)>();

            for (int i = 0; i < program.Length; i += chunk.Length)
            {
                var p = (0, 0, 0);
                for (int j = 0; j < chunk.Length; j++)
                {
                    Debug.Assert(program[i + j].op == chunk[j].op);
                    Debug.Assert(program[i + j].a == chunk[j].a);
                    if (chunk[j].b != "*")
                    {
                        Debug.Assert(program[i + j].b == chunk[j].b);
                    }
                    else
                    {
                        if (j == 4) p.Item1 = int.Parse(program[i + j].b);
                        else if (j == 5) p.Item2 = int.Parse(program[i + j].b);
                        else if (j == 15) p.Item3 = int.Parse(program[i + j].b);
                    }
                }
                Console.WriteLine(p);
                param.Add(p);
            }

            return param;
        }
    }

    public class ALU
    {
        public int W;
        public int X;
        public int Y;
        public int Z;

        public void Exec(string op, string a, string b, Queue<int> inputs)
        {
            switch (op)
            {
                case "inp":
                    Assign(a, inputs.Dequeue());
                    break;
                case "add":
                    Assign(a, ValueOf(a) + ValueOf(b));
                    break;
                case "mul":
                    Assign(a, ValueOf(a) * ValueOf(b));
                    break;
                case "div":
                    Assign(a, ValueOf(a) / ValueOf(b));
                    break;
                case "mod":
                    Assign(a, ValueOf(a) % ValueOf(b));
                    break;
                case "eql":
                    Assign(a, ValueOf(a) == ValueOf(b) ? 1 : 0);
                    break;
                default:
                    throw new Exception("invalid ALU operation");
            }
        }

        private void Assign(string reg, int value)
        {
            switch (reg)
            {
                case "w": W = value; break;
                case "x": X = value; break;
                case "y": Y = value; break;
                case "z": Z = value; break;
                default: throw new Exception("invalid register");
            }
        }

        private int ValueOf(string arg)
        {
            switch (arg)
            {
                case "w": return W;
                case "x": return X;
                case "y": return Y;
                case "z": return Z;
                default: return int.Parse(arg);
            }
        }
    }
}
