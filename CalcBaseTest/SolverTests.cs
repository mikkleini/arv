using CalcBase;
using CalcBase.Numbers;
using CalcBase.Tokens;
using Newtonsoft.Json.Linq;
using System.Numerics;

namespace CalcBaseTest
{
    public class SolverTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test successful equation
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="result">Expected result</param>
        private static void TestEquation(string infix, INumber expectedResult)
        {
            System.Diagnostics.Debug.WriteLine($"Test {infix} expect {expectedResult}");

            var tokens = Parser.ShuntingYard(infix);

            foreach (var token in tokens)
            {
                System.Diagnostics.Debug.WriteLine($"  Token: {token}");
            }

            var actualResult = Solver.Solve(tokens);
            System.Diagnostics.Debug.WriteLine($"Got: {actualResult}");

            Assert.That(actualResult, Is.InstanceOf(expectedResult.GetType()));

            if (actualResult is IntegerNumber actualInt)
            {
                var expectedInt = (IntegerNumber)expectedResult;
                Assert.Multiple(() =>
                {
                    Assert.That(actualInt.Value, Is.EqualTo(expectedInt.Value));
                    Assert.That(actualInt.Radix, Is.EqualTo(expectedInt.Radix));
                    Assert.That(actualInt.DominantCase, Is.EqualTo(expectedInt.DominantCase));
                    Assert.That(actualInt.IsScientificNotation, Is.EqualTo(expectedInt.IsScientificNotation));
                });
            }

            if (actualResult is RealNumber actualReal)
            {
                var expectedReal = (RealNumber)expectedResult;
                Assert.Multiple(() =>
                {
                    Assert.That(actualReal.Value, Is.EqualTo(expectedReal.Value));
                    Assert.That(actualReal.IsScientificNotation, Is.EqualTo(expectedReal.IsScientificNotation));
                });
            }
        }

        [Test]
        public void TestBinaryNumberParser()
        {
            TestEquation("0", IntegerNumber.Create(0));
            TestEquation("1", IntegerNumber.Create(1));
            TestEquation("-1", IntegerNumber.Create(-1));
            TestEquation("1-1", IntegerNumber.Create(0));
            TestEquation("1+2", IntegerNumber.Create(3));
            TestEquation("1 + 2", IntegerNumber.Create(3));
            TestEquation("1-2", IntegerNumber.Create(-1));
            TestEquation("2*3", IntegerNumber.Create(6));
            TestEquation("6/3", IntegerNumber.Create(2));

            TestEquation("10+5-2", IntegerNumber.Create(13));
            TestEquation("10+(5-2)", IntegerNumber.Create(13));
            TestEquation("10/(4-2)", IntegerNumber.Create(5));

            TestEquation("2 * 3 + 4", IntegerNumber.Create(10));
            TestEquation("10 / 2 - 1", IntegerNumber.Create(4));
            TestEquation("10 / 4 - 2", RealNumber.Create(0.5M));
        }
    }
}
