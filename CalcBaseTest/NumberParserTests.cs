using CalcBase;
using CalcBase.Tokens;

namespace CalcBaseTest
{
    public class NumberParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static void AssertIntegerNumberToken(string input, int length, IntType value, IntegerRadix radix, bool isScientific = false, DominantCase dominantCase = DominantCase.None)
        {
            IToken t = Parser.ReadNumber(input.AsSpan(), 0, out int end);
            Assert.That(t, Is.InstanceOf(typeof(IntegerNumberToken)));
            IntegerNumberToken it = (IntegerNumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(end, Is.EqualTo(length));
                Assert.That(it.Position, Is.EqualTo(0));
                Assert.That(it.Length, Is.EqualTo(length));
                Assert.That(it.Value, Is.EqualTo(value));
                Assert.That(it.Radix, Is.EqualTo(radix));
                Assert.That(it.IsScientificNotation, Is.EqualTo(isScientific));
                Assert.That(it.DominantCase, Is.EqualTo(dominantCase));
            });
        }

        private static void AssertRealNumberToken(string input, int length, RealType value, bool isScientific = false)
        {
            IToken t = Parser.ReadNumber(input.AsSpan(), 0, out int end);
            Assert.That(t, Is.InstanceOf(typeof(RealNumberToken)));
            RealNumberToken it = (RealNumberToken)t;
            Assert.Multiple(() =>
            {
                Assert.That(end, Is.EqualTo(length));
                Assert.That(it.Position, Is.EqualTo(0));
                Assert.That(it.Value, Is.EqualTo(value));
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
            AssertIntegerNumberToken("0b0", 3, 0, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b1", 3, 1, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b1 ", 3, 1, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b1+", 3, 1, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b1x", 3, 1, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b000001", 8, 1, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b10", 4, 2, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b111.", 5, 7, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b111,", 5, 7, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b10bb", 4, 2, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b11111110", 10, 254, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b" + zeroes128[0..127] + "1", 130, 1, IntegerRadix.Binary);
            AssertIntegerNumberToken("0b1" + zeroes128[1..128], 130, (IntType)1 << 127, IntegerRadix.Binary);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0b", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0b.3", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0b" + zeroes128 + "1", 0, out _));
        }

        [Test]
        public void TestHexadecimalNumberParser()
        {
            string zeroes128 = "00000000000000000000000000000000";

            // Valid values
            AssertIntegerNumberToken("0x0", 3, 0, IntegerRadix.Hexadecimal);
            AssertIntegerNumberToken("0x1", 3, 1, IntegerRadix.Hexadecimal);
            AssertIntegerNumberToken("0xA1", 4, 0xA1, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertIntegerNumberToken("0xab", 4, 0xAB, IntegerRadix.Hexadecimal, false, DominantCase.Lower);
            AssertIntegerNumberToken("0xaB", 4, 0xAB, IntegerRadix.Hexadecimal, false, DominantCase.None);
            AssertIntegerNumberToken("0xA1 ", 4, 0xA1, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertIntegerNumberToken("0xF0+", 4, 0xF0, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertIntegerNumberToken("0xCx", 3, 0xC, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertIntegerNumberToken("0x000008", 8, 8, IntegerRadix.Hexadecimal);
            AssertIntegerNumberToken("0xA.", 3, 0xA, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertIntegerNumberToken("0x123456789ABCDEF0", 18, 0x123456789ABCDEF0, IntegerRadix.Hexadecimal, false, DominantCase.Upper);
            AssertIntegerNumberToken("0x" + zeroes128[0..31] + "1", 34, 0x1, IntegerRadix.Hexadecimal);
            AssertIntegerNumberToken("0x7" + zeroes128[1..32], 34, (Int128)7 << 124, IntegerRadix.Hexadecimal);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0x", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0xG", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("0x" + zeroes128 + "1", 0, out _));
        }

        [Test]
        public void TestIntegerNumberParser()
        {
            // Valid values
            AssertIntegerNumberToken("0", 1, 0, IntegerRadix.Decimal);
            AssertIntegerNumberToken("1", 1, 1, IntegerRadix.Decimal);
            AssertIntegerNumberToken("1 ", 1, 1, IntegerRadix.Decimal);
            AssertIntegerNumberToken("1+", 1, 1, IntegerRadix.Decimal);
            AssertIntegerNumberToken("1$", 1, 1, IntegerRadix.Decimal);
            AssertIntegerNumberToken("2", 1, 2, IntegerRadix.Decimal);
            AssertIntegerNumberToken("10", 2, 10, IntegerRadix.Decimal);
            AssertIntegerNumberToken("10", 2, 10, IntegerRadix.Decimal);
            AssertIntegerNumberToken("1000000000", 10, 1000000000, IntegerRadix.Decimal);
            AssertIntegerNumberToken("1000000000b22", 10, 1000000000, IntegerRadix.Decimal);

            // Exponential values
            AssertIntegerNumberToken("0E1", 3, 0, IntegerRadix.Decimal, true);
            AssertIntegerNumberToken("5E1", 3, 50, IntegerRadix.Decimal, true);
            AssertIntegerNumberToken("10E3", 4, 10000, IntegerRadix.Decimal, true);
            AssertIntegerNumberToken("10E3+", 4, 10000, IntegerRadix.Decimal, true);
            AssertIntegerNumberToken("123E100+", 7, 123 * IntType.Pow(10, 100), IntegerRadix.Decimal, true);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E 1+", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10EE10", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E1234567890123456", 0, out _));
        }

        [Test]
        public void TestRealNumberParser()
        {
            // Valid values
            AssertRealNumberToken("0.0", 3, 0.0M);
            AssertRealNumberToken("0.1", 3, 0.1M);
            AssertRealNumberToken("1234567890.0987654321", 21, 1234567890.0987654321M);

            // Exponential values
            AssertRealNumberToken("0.0E1", 5, 0.0M, true);
            AssertRealNumberToken("5.52E2", 6, 552.0M, true);

            // Invalid values
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10.3E 1+", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10EE2.30", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10E0.5", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("12.3E4.5", 0, out _));
            Assert.Throws<ExpressionException>(() => Parser.ReadNumber("10000000000000000000000000000000000000000000000000000.0", 0, out _));
        }
    }
}