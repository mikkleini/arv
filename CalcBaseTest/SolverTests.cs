﻿using CalcBase;
using CalcBase.Numbers;
using CalcBase.Tokens;
using CalcBase.Units;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
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
        private void TestEquation(string infix, Number expectedResult)
        {
            Debug.WriteLine("");
            Debug.WriteLine($"Test {infix} expect {expectedResult}");

            var infixTokens = Parser.Tokenize(infix);

            Debug.WriteLine($"  Infix:"); 
            foreach (var token in infixTokens)
            {
                Debug.WriteLine($"    {token}");
            }

            var postfixTokens = Parser.ShuntingYard(infixTokens);

            Debug.WriteLine($"  Postfix:");
            foreach (var token in postfixTokens)
            {
                Debug.WriteLine($"    {token}");
            }

            Number actualResult = Solver.Solve(postfixTokens);

            Debug.WriteLine($"  Result: {actualResult}");

            Assert.Multiple(() =>
            {
                Assert.That(actualResult.Value, Is.EqualTo(expectedResult.Value));
                Assert.That(actualResult.Radix, Is.EqualTo(expectedResult.Radix));
                Assert.That(actualResult.HexadecimalCase, Is.EqualTo(expectedResult.HexadecimalCase));
                Assert.That(actualResult.IsScientificNotation, Is.EqualTo(expectedResult.IsScientificNotation));
            });

            if ((actualResult is Measure actualMeasure) && (expectedResult is Measure expectedMeasure))
            {
                Assert.That(actualMeasure.Unit, Is.EqualTo(expectedMeasure.Unit));
            }
            else if (actualResult is Measure)
            {
                Assert.Fail($"Expected number, got measure");
            }
            else if (expectedResult is Measure)
            {
                Assert.Fail($"Expected measure, got number");
            }
        }

        [Test]
        public void TestSimpleExpressions()
        {
            TestEquation("0", 0);
            TestEquation("1", 1);
            TestEquation("-1", -1);
            TestEquation("1-1", 0);
            TestEquation("1+2", 3);
            TestEquation("1 + 2", 3);
            TestEquation("1-2", -1);
            TestEquation("2*3", 6);
            TestEquation("6/3", 2);

            TestEquation("10+5-2", 13);
            TestEquation("10+(5-2)", 13);
            TestEquation("10/(4-2)", 5);

            TestEquation("2 * 3 + 4", 10);
            TestEquation("10 / 2 - 1", 4);
            TestEquation("5 + 2 * 3", 11);
            TestEquation("4 / 2 * 8", 16);
            TestEquation("5 + (7-4) * 3", 14);
            TestEquation("-2-2", -4);
            TestEquation("101 % 20", 1);

            // These are bit problematic - in C# % is reminder, but % is also symbol for modulus
            TestEquation("-15 % 7", -1);
            TestEquation("101 % -20", 1);

            TestEquation("10 / 4 - 2", 0.5M);
            TestEquation("10.5 + (4.1 - 2) * 3.1", 17.01M);
            TestEquation("12.0+2.0", 14);
            TestEquation("2 ** 3", 8);
            TestEquation("-2 ** 3", -8);
            TestEquation("2 ** -3", 0.125M);
            TestEquation("-2 ** -3", -0.125M);

            // TODO Has accuracy issue:
            TestEquation("625 ** 0.5", 25);
            TestEquation("3 + 3 ** 3", 30);

            Assert.Pass();
        }

        [Test]
        public void TestFunctions()
        {
            TestEquation("cos(0)", 1);
            TestEquation("sin(pi/2)", 1);
            TestEquation("sin(pi*1.5)", -1);
            TestEquation("round(1.23456, 2)", 1.23M);
            TestEquation("round(1.23456 * 2, 2)", 2.47M);
            TestEquation("round(1.23456 * (2 + 1), 2)", 3.70M);
            TestEquation("sin((cos(0)*pi)/2)", 1);
            TestEquation("round(sin(pi/2), 3)", 1);
            TestEquation("floor(min(10-20, max(-40, -50))/2)", -20);

            Assert.Pass();
        }

        [Test]
        public void TestMixedRadix()
        {
            TestEquation("0x400/2", new Number(0x200, IntegerRadix.Hexadecimal | IntegerRadix.Decimal, false, HexadecimalCase.None));
            TestEquation("0xF/6", new Number(2.5M, IntegerRadix.Hexadecimal | IntegerRadix.Decimal, false, HexadecimalCase.Upper));

            Assert.Pass();
        }

        [Test]
        public void TestMeasures()
        {
            var infixTokens = Parser.Tokenize("3.2m");
            var postfixTokens = Parser.ShuntingYard(infixTokens);
            
            Assert.That(postfixTokens.Count, Is.EqualTo(1));
            Assert.That(postfixTokens[0], Is.InstanceOf(typeof(MeasureToken)));
            MeasureToken mt = (MeasureToken)postfixTokens[0];

            Assert.That(mt.Measure.Value, Is.EqualTo((NumberType)3.2M));
            Assert.That(mt.Measure.Unit, Is.EqualTo(Factory.Metre));
        }

        [Test]
        public void TestDerivedUnits()
        {
            TestEquation("3m/2s", new Measure(1.5M, Factory.MetrePerSecond));
        }
    }
}
