using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day16
{
    public record Result(int Part1, long Part2) { }

    public static class Day16
    {
        public const int PACKET_TYPE_SUM = 0;
        public const int PACKET_TYPE_PRODUCT = 1;
        public const int PACKET_TYPE_MIN = 2;
        public const int PACKET_TYPE_MAX = 3;
        public const int PACKET_TYPE_LITERAL = 4;
        public const int PACKET_TYPE_GREATER = 5;
        public const int PACKET_TYPE_LESS = 6;
        public const int PACKET_TYPE_EQUAL = 7;

        public static Result Solve(IEnumerable<string> input)
        {
            var bitstream = new BitsReader(input.First());

            var packet = bitstream.ReadPacket();
            var part1 = packet.VersionSum();
            var part2 = packet.Eval();

            return new Result(part1, part2);
        }

        public static IEnumerable<byte> ToBitstream(string hexstr)
        {
            foreach (var c in hexstr)
            {
                var nibble = c < 'A' ? c - '0' : c - 'A' + 10;
                yield return (byte)((nibble & 8) >> 3);
                yield return (byte)((nibble & 4) >> 2);
                yield return (byte)((nibble & 2) >> 1);
                yield return (byte)(nibble & 1);
            }
        }

        public class BitsReader
        {
            private IEnumerable<byte> Bitstream;

            public BitsReader(IEnumerable<byte> bits) => Bitstream = bits;
            public BitsReader(string hexstr) => Bitstream = ToBitstream(hexstr);

            public bool Any() => Bitstream.Any();

            public int ReadInt(int nbits)
            {
                var n = Bitstream.Take(nbits).Aggregate(0, (acc, bit) => 2 * acc + bit);
                Bitstream = Bitstream.Skip(nbits);
                return n;
            }

            public (int, int) ReadPacketHeader() => (ReadInt(3), ReadInt(3));

            public long ReadLiteral()
            {
                long n = 0;
                var moreBits = 0;
                do
                {
                    moreBits = ReadInt(1);
                    n = (n << 4) + ReadInt(4);
                } while (moreBits != 0);
                return n;
            }

            public List<Packet> ReadOperands()
            {
                var operands = new List<Packet>();
                var payloadType = ReadInt(1);
                if (payloadType == 0)
                {
                    var payloadLen = ReadInt(15);
                    var payload = new BitsReader(Bitstream.Take(payloadLen));
                    Bitstream = Bitstream.Skip(payloadLen);
                    while (payload.Any())
                    {
                        operands.Add(payload.ReadPacket());
                    }
                }
                else
                {
                    var numPackets = ReadInt(11);
                    while (numPackets-- > 0)
                    {
                        operands.Add(ReadPacket());
                    }
                }
                return operands;
            }

            public Packet ReadPacket()
            {
                var (vers, type) = ReadPacketHeader();
                if (type == PACKET_TYPE_LITERAL)
                {
                    var lit = ReadLiteral();
                    return new LiteralPacket(vers, type, lit);
                }
                else
                {
                    var operands = ReadOperands();
                    return new OperatorPacket(vers, type, operands);
                }
            }
        }

        public abstract class Packet
        {
            public int Version;
            public int Type;
            public Packet(int version, int type)
            {
                Version = version;
                Type = type;
            }
            public virtual int VersionSum()
            {
                return Version;
            }
            public abstract long Eval();
        }

        public class LiteralPacket : Packet
        {
            public long Value;
            public LiteralPacket(int version, int type, long value) : base(version, type)
            {
                Value = value;
            }
            public override long Eval() => Value;
        }

        public class OperatorPacket : Packet
        {
            public List<Packet> Operands;
            public OperatorPacket(int version, int type, List<Packet> operands) : base(version, type)
            {
                Operands = operands;
            }
            public override int VersionSum()
            {
                return Version + Operands.Select(p => p.VersionSum()).Sum();
            }

            public override long Eval()
            {
                var operands = Operands.Select(p => p.Eval());
                switch (Type)
                {
                    case PACKET_TYPE_SUM:
                        return operands.Sum();
                    case PACKET_TYPE_PRODUCT:
                        return operands.Aggregate(1L, (a, b) => a * b);
                    case PACKET_TYPE_MIN:
                        return operands.Min();
                    case PACKET_TYPE_MAX:
                        return operands.Max();
                    case PACKET_TYPE_GREATER:
                        return operands.ElementAt(0) > operands.ElementAt(1) ? 1 : 0;
                    case PACKET_TYPE_LESS:
                        return operands.ElementAt(0) < operands.ElementAt(1) ? 1 : 0;
                    case PACKET_TYPE_EQUAL:
                        return operands.ElementAt(0) == operands.ElementAt(1) ? 1 : 0;
                    default:
                        throw new Exception("invalid operator type");
                }
            }
        }
    }
}
