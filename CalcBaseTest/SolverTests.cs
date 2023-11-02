using CalcBase;
using CalcBase.Numbers;
using CalcBase.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Numerics;

namespace CalcBaseTest
{
    public class SolverTests
    {
        private Parser parser;
        private Solver solver;

        [SetUp]
        public void Setup()
        {
            parser = new Parser();
            solver = new Solver();
        }

        /// <summary>
        /// Test successful equation
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="result">Expected result</param>
        private void TestEquation(string infix, Number expectedResult)
        {
            Debug.WriteLine("");
            Debug.WriteLine($"Test {infix} expect {expectedResult}");

            var infixTokens = parser.Tokenize(infix);
            foreach (var token in infixTokens)
            {
                Debug.WriteLine($"  Infix: {token}");
            }

            var postfixTokens = parser.ShuntingYard(infixTokens);

            foreach (var token in postfixTokens)
            {
                Debug.WriteLine($"  Postfix: {token}");
            }

            Number actualResult = solver.Solve(postfixTokens);

            Debug.WriteLine($"  Got: {actualResult}");

            Assert.Multiple(() =>
            {
                Assert.That(actualResult.Value, Is.EqualTo(expectedResult.Value));
                Assert.That(actualResult.Radix, Is.EqualTo(expectedResult.Radix));
                Assert.That(actualResult.DominantCase, Is.EqualTo(expectedResult.DominantCase));
                Assert.That(actualResult.IsScientificNotation, Is.EqualTo(expectedResult.IsScientificNotation));
            });
        }

        [Test]
        public void TestSimpleExpressions()
        {
            TestEquation("0", Number.Create(0));
            TestEquation("1", Number.Create(1));
            TestEquation("-1", Number.Create(-1));
            TestEquation("1-1", Number.Create(0));
            TestEquation("1+2", Number.Create(3));
            TestEquation("1 + 2", Number.Create(3));
            TestEquation("1-2", Number.Create(-1));
            TestEquation("2*3", Number.Create(6));
            TestEquation("6/3", Number.Create(2));

            TestEquation("10+5-2", Number.Create(13));
            TestEquation("10+(5-2)", Number.Create(13));
            TestEquation("10/(4-2)", Number.Create(5));

            TestEquation("2 * 3 + 4", Number.Create(10));
            TestEquation("10 / 2 - 1", Number.Create(4));
            TestEquation("5 + 2 * 3", Number.Create(11));
            TestEquation("5 + (7-4) * 3", Number.Create(14));
            TestEquation("3 + 3 ** 3", Number.Create(30));
            TestEquation("-2-2", Number.Create(-4));
            TestEquation("101 % 20", Number.Create(1));

            // These are bit problematic - in C# % is reminder, but % is also symbol for modulus
            TestEquation("-15 % 7", Number.Create(-1));
            TestEquation("101 % -20", Number.Create(1));

            TestEquation("10 / 4 - 2", Number.Create(0.5M));
            TestEquation("10.5 + (4.1 - 2) * 3.1", Number.Create(17.01M));
            TestEquation("12.0+2.0", Number.Create(14));
            TestEquation("2 ** -3", Number.Create(0.125M));

            // TODO Has accuracy issue:
            TestEquation("625 ** 0.5", Number.Create(25));

            TestEquation("0xF0 + 15", Number.Create(0xFF, IntegerRadix.Hexadecimal, false, DominantHexadecimalCase.Upper));
            TestEquation("240 + 0x0F", Number.Create(0xFF, IntegerRadix.Decimal, false, DominantHexadecimalCase.Upper));

            Assert.Pass();
        }

        [Test]
        public void TestFunctions()
        {
            TestEquation("cos(0)", Number.Create(1));
            TestEquation("sin(Pi/2)", Number.Create(1));
            TestEquation("sin(Pi*1.5)", Number.Create(-1));
            TestEquation("round(1.23456, 2)", Number.Create(1.23));
            TestEquation("round(1.23456 * 2, 2)", Number.Create(2.47));
            TestEquation("round(1.23456 * (2 + 1), 2)", Number.Create(3.70));
            TestEquation("sin((cos(0)*pi)/2)", Number.Create(1));
            TestEquation("round(sin(Pi/2), 3)", Number.Create(1.000));

            Assert.Pass();
        }

        [Test]
        public void TestRationals()
        {
            BigRational x;
            
            x = BigRational.Pow((int)3, (int)3);
            Debug.WriteLine(x.ToString());

            x = BigRational.Pow(new BigRational(3), new BigRational(3));
            Debug.WriteLine(x.ToString());

            x = 10.125M;
            NumberType y = NumberType.NumDen(x, out NumberType.Integer z);
            Debug.WriteLine($"Den: {x} -> {y}/{z}");

            Assert.Pass();
        }

        [Test]
        public void TestMeasures()
        {
            var infixTokens = parser.Tokenize("3.2m");
            var postfixTokens = parser.ShuntingYard(infixTokens);
            
            Assert.That(postfixTokens.Count, Is.EqualTo(1));
            Assert.That(postfixTokens[0], Is.InstanceOf(typeof(MeasureToken)));
            MeasureToken mt = (MeasureToken)postfixTokens[0];

            Assert.That(mt.Measure.Value, Is.EqualTo(new NumberType(3.2M)));
            Assert.That(mt.Measure.Unit.Symbol, Is.EqualTo("m"));
        }
    }
}
