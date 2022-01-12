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
            var (model1, model2) = FindModelNums(param);

            var alu1 = RunProgram(program, model1);
            Debug.Assert(alu1.Z == 0);

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

        private static (int[], int[]) FindModelNums(List<(int, int, int)> prog)
        {
            var maxModel = new int[14];
            var minModel = new int[14];
            var stack = new Stack<(int, int)>();
            for (int i = 0; i < prog.Count; i++)
            {
                if (prog[i].Item1 == 1)
                {
                    stack.Push((i, prog[i].Item3));
                }
                else
                {
                    var (j, x) = stack.Pop();
                    var diff = prog[i].Item2 + x;
                    // model[i] = model[j] + diff;
                    if (diff >= 0)
                    {
                        maxModel[i] = 9;
                        maxModel[j] = 9 - diff;
                        minModel[i] = 1 + diff;
                        minModel[j] = 1;
                    }
                    else
                    {
                        maxModel[i] = 9 + diff;
                        maxModel[j] = 9;
                        minModel[i] = 1;
                        minModel[j] = 1 - diff;
                    }
                }
            }
            Debug.Assert(!stack.Any());
            return (maxModel, minModel);
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
