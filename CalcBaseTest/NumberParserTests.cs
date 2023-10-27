using CalcBase;
using CalcBase.Numbers;
using CalcBase.Tokens;
using System.Numerics;

namespace CalcBaseTest
{
    public class NumberParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static void AssertNumberToken(string input, int length, NumberType value, IntegerRadix radix = IntegerRadix.Decimal, bool isScientific = false, DominantCase dominantCase = DominantCase.None)
        {
            IToken t = Parser.ReadNumber(input.AsSpan(), 0);
            Assert.That(t, Is.InstanceOf(typeof(NumberToken)));
            NumberToken it = (NumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(it.Position, Is.EqualTo(0));
                Assert.That(it.Length, Is.EqualTo(length));
                Assert.That(it.Number, Is.TypeOf(typeof(Number)));
                Assert.That(it.Number.Value, Is.EqualTo(value));
                Assert.That(it.Number.Radix, Is.EqualTo(radix));
                Assert.That(it.Number.IsScientificNotation, Is.EqualTo(isScientific));
                Assert.That(it.Number.DominantCase, Is.EqualTo(dominantCase));
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
            AssertNumberToken("0b0", 3, 0, IntegerRadix.Binary);
            AssertNumberToken("0b1", 3, 1, IntegerRadix.Binary);
            AssertNumberToken("0b1 ", 3, 1, IntegerRadix.Binary);
            AssertNumberToken("0b1+", 3, 1, IntegerRadix.Binary);
            AssertNumberToken("0b1x", 3, 1, IntegerRadix.Binary);
            AssertNumberToken("0b000001", 8, 1, IntegerRadix.Binary);
            AssertNumberToken("0b10", 4, 2, IntegerRadix.Binary);
            AssertNumberToken("0b111.", 5, 7, IntegerRadix.Binary);
            AssertNumberToken("0b111,", 5, 7, IntegerRadix.Binary);
            AssertNumberToken("0b10bb", 4, 2, IntegerRadix.Binary);
            AssertNumberToken("0b11111110", 10, 254, IntegerRadix.Binary);
            AssertNumberToken("0b" + zeroes128[0..127] + "1", 130, 1, IntegerRadix.Binary);
            AssertNumberToken("0b1" + zeroes128[1..128], 130, (BigInteger)1 << 127, IntegerRadix.Binary);

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
            AssertNumberToken("0x0", 3, 0, IntegerRadix.Hexadecimal);
            AssertNumberToken("0x1", 3, 1, IntegerRadix.Hexadecimal);
            AssertNumberToken("0xA1", 4, 0xA1, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertNumberToken("0xab", 4, 0xAB, IntegerRadix.Hexadecimal, false, DominantCase.Lower);
            AssertNumberToken("0xaB", 4, 0xAB, IntegerRadix.Hexadecimal, false, DominantCase.None);
            AssertNumberToken("0xA1 ", 4, 0xA1, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertNumberToken("0xF0+", 4, 0xF0, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertNumberToken("0xCx", 3, 0xC, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertNumberToken("0x000008", 8, 8, IntegerRadix.Hexadecimal);
            AssertNumberToken("0xA.", 3, 0xA, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertNumberToken("0x123456789ABCDEF0", 18, 0x123456789ABCDEF0, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertNumberToken("0x" + zeroes128[0..31] + "1", 34, 0x1, IntegerRadix.Hexadecimal);
            AssertNumberToken("0x7" + zeroes128[1..32], 34, (Int128)7 << 124, IntegerRadix.Hexadecimal);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0x", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0xG", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0x" + zeroes128 + "1", 0));
        }

        [Test]
        public void TestIntegerNumberParser()
        {
            // Valid values
            AssertNumberToken("0", 1, 0, IntegerRadix.Decimal);
            AssertNumberToken("1", 1, 1, IntegerRadix.Decimal);
            AssertNumberToken("1 ", 1, 1, IntegerRadix.Decimal);
            AssertNumberToken("1+", 1, 1, IntegerRadix.Decimal);
            AssertNumberToken("1$", 1, 1, IntegerRadix.Decimal);
            AssertNumberToken("2", 1, 2, IntegerRadix.Decimal);
            AssertNumberToken("10", 2, 10, IntegerRadix.Decimal);
            AssertNumberToken("10", 2, 10, IntegerRadix.Decimal);
            AssertNumberToken("1000000000", 10, 1000000000, IntegerRadix.Decimal);
            AssertNumberToken("1000000000b22", 10, 1000000000, IntegerRadix.Decimal);

            // Exponential values
            AssertNumberToken("0E1", 3, 0, IntegerRadix.Decimal, true);
            AssertNumberToken("5E1", 3, 50, IntegerRadix.Decimal, true);
            AssertNumberToken("10E3", 4, 10000, IntegerRadix.Decimal, true);
            AssertNumberToken("10E3+", 4, 10000, IntegerRadix.Decimal, true);
            AssertNumberToken("123E100+", 7, 123 * NumberType.Pow(10, 100), IntegerRadix.Decimal, true);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E 1+", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10EE10", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E1234567890123456", 0));
        }

        [Test]
        public void TestRealNumberParser()
        {
            // Valid values
            AssertNumberToken("0.0", 3, 0.0M);
            AssertNumberToken("0.1", 3, 0.1M);
            AssertNumberToken("1234567890.0987654321", 21, 1234567890.0987654321M);

            // Exponential values
            AssertNumberToken("0.0E1", 5, 0.0M, IntegerRadix.Decimal, true);
            AssertNumberToken("5.52E2", 6, 552.0M, IntegerRadix.Decimal, true);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10.3E 1+", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10EE2.30", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E0.5", 0));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("12.3E4.5", 0));
        }
    }
}