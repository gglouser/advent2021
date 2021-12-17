using System.IO;
using Advent2021.Solutions.Day16;
using Xunit;

namespace Advent2021.Tests
{
    public class Day16Test
    {
        [Fact]
        public void TestParseLiteral()
        {
            var reader = new Day16.BitsReader("D2FE28");
            var (vers, type) = reader.ReadPacketHeader();
            Assert.Equal(6, vers);
            Assert.Equal(4, type);
            var lit = reader.ReadLiteral();
            Assert.Equal(2021, lit);
        }

        [Fact]
        public void TestParseOperator()
        {
            var reader = new Day16.BitsReader("38006F45291200");
            var (vers, type) = reader.ReadPacketHeader();
            Assert.Equal(1, vers);
            Assert.Equal(6, type);
            var payload = reader.ReadOperands();

            var op0 = Assert.IsType<Day16.LiteralPacket>(payload[0]);
            Assert.Equal(6, op0.Version);
            Assert.Equal(4, op0.Type);
            Assert.Equal(10, op0.Value);

            var op1 = Assert.IsType<Day16.LiteralPacket>(payload[1]);
            Assert.Equal(2, op1.Version);
            Assert.Equal(4, op1.Type);
            Assert.Equal(20, op1.Value);
        }

        [Fact]
        public void TestParseOperator2()
        {
            var reader = new Day16.BitsReader("EE00D40C823060");
            var (vers, type) = reader.ReadPacketHeader();
            Assert.Equal(7, vers);
            Assert.Equal(3, type);
            var payload = reader.ReadOperands();

            var op0 = Assert.IsType<Day16.LiteralPacket>(payload[0]);
            Assert.Equal(2, op0.Version);
            Assert.Equal(4, op0.Type);
            Assert.Equal(1, op0.Value);

            var op1 = Assert.IsType<Day16.LiteralPacket>(payload[1]);
            Assert.Equal(4, op1.Version);
            Assert.Equal(4, op1.Type);
            Assert.Equal(2, op1.Value);

            var op2 = Assert.IsType<Day16.LiteralPacket>(payload[2]);
            Assert.Equal(1, op2.Version);
            Assert.Equal(4, op2.Type);
            Assert.Equal(3, op2.Value);
        }

        [Fact]
        public void TestExample1()
        {
            const string example = "8A004A801A8002F478";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(16, packet.VersionSum());
        }

        [Fact]
        public void TestExample2()
        {
            const string example = "620080001611562C8802118E34";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(12, packet.VersionSum());
        }

        [Fact]
        public void TestExample3()
        {
            const string example = "C0015000016115A2E0802F182340";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(23, packet.VersionSum());
        }

        [Fact]
        public void TestExample4()
        {
            const string example = "A0016C880162017C3686B18A3D4780";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(31, packet.VersionSum());
        }

        [Fact]
        public void TestExample5()
        {
            const string example = "C200B40A82";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(3, packet.Eval());
        }

        [Fact]
        public void TestExample6()
        {
            const string example = "04005AC33890";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(54, packet.Eval());
        }

        [Fact]
        public void TestExample7()
        {
            const string example = "880086C3E88112";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(7, packet.Eval());
        }

        [Fact]
        public void TestExample8()
        {
            const string example = "CE00C43D881120";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(9, packet.Eval());
        }

        [Fact]
        public void TestExample9()
        {
            const string example = "D8005AC2A8F0";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(1, packet.Eval());
        }

        [Fact]
        public void TestExample10()
        {
            const string example = "F600BC2D8F";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(0, packet.Eval());
        }

        [Fact]
        public void TestExample11()
        {
            const string example = "9C005AC2F8F0";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(0, packet.Eval());
        }

        [Fact]
        public void TestExample12()
        {
            const string example = "9C0141080250320F1802104A08";
            var packet = new Day16.BitsReader(example).ReadPacket();
            Assert.Equal(1, packet.Eval());
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day16.txt");
            var result = Day16.Solve(input);
            Assert.Equal(877, result.Part1);
            Assert.Equal(194435634456, result.Part2);
        }
    }
}
