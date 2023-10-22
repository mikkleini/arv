using CalcBase;
using System;

namespace CalcBaseTest
{
    public class NumberParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private void AssertBinaryNumberToken(string input, int length, IntType value)
        {
            Token t = Parser.ReadNumber(input.AsSpan(), 0);
            Assert.Multiple(() =>
            {
                Assert.That(t.Type, Is.EqualTo(TokenType.BinaryNumber));
                Assert.That(t, Is.InstanceOf(typeof(BinaryNumberToken)));
            });
            BinaryNumberToken bt = (BinaryNumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(bt.Value, Is.EqualTo(value));
                Assert.That(bt.Length, Is.EqualTo(length));
            });
        }

        private void AssertHexadecimalNumberToken(string input, int length, IntType value)
        {
            Token t = Parser.ReadNumber(input.AsSpan(), 0);
            Assert.Multiple(() =>
            {
                Assert.That(t.Type, Is.EqualTo(TokenType.HexadecimalNumber));
                Assert.That(t, Is.InstanceOf(typeof(HexadecimalNumberToken)));
            });
            HexadecimalNumberToken ht = (HexadecimalNumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(ht.Value, Is.EqualTo(value));
                Assert.That(ht.Length, Is.EqualTo(length));
            });
        }

        private void AssertIntegerNumberToken(string input, int length, IntType value, IntType exp, bool isScientific = false)
        {
            Token t = Parser.ReadNumber(input.AsSpan(), 0);
            Assert.Multiple(() =>
            {
                Assert.That(t.Type, Is.EqualTo(TokenType.IntegerNumber));
                Assert.That(t, Is.InstanceOf(typeof(IntegerNumberToken)));
            });
            IntegerNumberToken it = (IntegerNumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(it.Value, Is.EqualTo(value));
                Assert.That(it.Exponent, Is.EqualTo(exp));
                Assert.That(it.Length, Is.EqualTo(length));
                Assert.That(it.IsScientificNotation, Is.EqualTo(isScientific));
            });
        }

        private void AssertRealNumberToken(string input, int length, RealType value, IntType exp, bool isScientific = false)
        {
            Token t = Parser.ReadNumber(input.AsSpan(), 0);
            Assert.Multiple(() =>
            {
                Assert.That(t.Type, Is.EqualTo(TokenType.RealNumber));
                Assert.That(t, Is.InstanceOf(typeof(RealNumberToken)));
            });
            RealNumberToken it = (RealNumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(it.Value, Is.EqualTo(value));
                Assert.That(it.Exponent, Is.EqualTo(exp));
                Assert.That(it.Length, Is.EqualTo(length));
                Assert.That(it.IsScientificNotation, Is.EqualTo(isScientific));
            });
        }

        [Test]
        public void TestBinaryNumberParser()
        {
            string zeroes128 =
                "00000000000000000000000000000000" +
                "00000000000000000000000000000000" +
                "00000000000000000000000000000000" +
                "00000000000000000000000000000000";

            // Valid values
            AssertBinaryNumberToken("0b0", 3, 0);
            AssertBinaryNumberToken("0b1", 3, 1);
            AssertBinaryNumberToken("0b1 ", 3, 1);
            AssertBinaryNumberToken("0b1+", 3, 1);
            AssertBinaryNumberToken("0b1x", 3, 1);
            AssertBinaryNumberToken("0b000001", 8, 1);
            AssertBinaryNumberToken("0b10", 4, 2);
            AssertBinaryNumberToken("0b111.", 5, 7);
            AssertBinaryNumberToken("0b111,", 5, 7);
            AssertBinaryNumberToken("0b10bb", 4, 2);
            AssertBinaryNumberToken("0b11111110", 10, 254);
            AssertBinaryNumberToken("0b" + zeroes128[0..127] + "1", 130, 1);
            AssertBinaryNumberToken("0b1" + zeroes128[1..128], 130, (IntType)1 << 127);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0b", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0b.3", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0b" + zeroes128 + "1", 0));
        }

        [Test]
        public void TestHexadecimalNumberParser()
        {
            string zeroes128 = "00000000000000000000000000000000";

            // Valid values
            AssertHexadecimalNumberToken("0x0", 3, 0);
            AssertHexadecimalNumberToken("0x1", 3, 1);
            AssertHexadecimalNumberToken("0xA1", 4, 0xA1);
            AssertHexadecimalNumberToken("0xab", 4, 0xAB);
            AssertHexadecimalNumberToken("0xA1 ", 4, 0xA1);
            AssertHexadecimalNumberToken("0xF0+", 4, 0xF0);
            AssertHexadecimalNumberToken("0xCx", 3, 0xC);
            AssertHexadecimalNumberToken("0x000008", 8, 8);
            AssertHexadecimalNumberToken("0xA.", 3, 0xA);
            AssertHexadecimalNumberToken("0x123456789ABCDEF0", 18, 0x123456789ABCDEF0);
            AssertHexadecimalNumberToken("0x" + zeroes128[0..31] + "1", 34, 0x1);
            AssertHexadecimalNumberToken("0x7" + zeroes128[1..32], 34, (Int128)7 << 124);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0x", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0xG", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0x" + zeroes128 + "1", 0));
        }

        [Test]
        public void TestIntegerNumberParser()
        {
            // Valid values
            AssertIntegerNumberToken("0", 1, 0, 1);
            AssertIntegerNumberToken("1", 1, 1, 1);
            AssertIntegerNumberToken("1 ", 1, 1, 1);
            AssertIntegerNumberToken("1+", 1, 1, 1);
            AssertIntegerNumberToken("1$", 1, 1, 1);
            AssertIntegerNumberToken("2", 1, 2, 1);
            AssertIntegerNumberToken("10", 2, 10, 1);
            AssertIntegerNumberToken("10", 2, 10, 1);
            AssertIntegerNumberToken("1000000000", 10, 1000000000, 1);
            AssertIntegerNumberToken("1000000000b22", 10, 1000000000, 1);

            // Exponential values
            AssertIntegerNumberToken("0E1", 3, 0, 1, true);
            AssertIntegerNumberToken("5E1", 3, 5, 1, true);
            AssertIntegerNumberToken("10E3", 4, 10, 3, true);
            AssertIntegerNumberToken("10E3+", 4, 10, 3, true);
            AssertIntegerNumberToken("521E123+", 7, 521, 123, true);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E 1+", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10EE10", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10000000000000000000000000000000000000000000000000000", 0));
        }

        [Test]
        public void TestRealNumberParser()
        {
            // Valid values
            AssertRealNumberToken("0.0", 3, 0.0M, 1);
            AssertRealNumberToken("0.1", 3, 0.1M, 1);

            // Exponential values
            AssertRealNumberToken("0.0E1", 5, 0.0M, 1, true);
            AssertRealNumberToken("5.52E2", 6, 5.52M, 2, true);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10.3E 1+", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10EE2.30", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E0.5", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("12.3E4.5", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10000000000000000000000000000000000000000000000000000", 0));
        }
    }
}