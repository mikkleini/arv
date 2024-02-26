﻿using CalcBase.Operators;
using CalcBase.Tokens;
using CalcBase.Numbers;
using System.Numerics;
using System.Diagnostics;
using CalcBase.Functions;
using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Quantities;
using CalcBase.Units;

namespace CalcBase
{
    /// <summary>
    /// Postfix (RPN) expressions solver
    /// </summary>
    public class Solver
    {
        private static readonly FormulaEqualityComparer FormulaEqComp = new();

        /// <summary>
        /// Try finding SI derived unit from expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>SI derived unit</returns>
        private static ISIDerivedUnit? TryFindDerivedUnit(IElement[] expression)
        {
            return (SIDerivedUnit?) Factory.Units.FirstOrDefault(u => (u is ISIDerivedUnit devU) && (devU.Expression.SequenceEqual(expression)));
        }

        /// <summary>
        /// Try finding formula from expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>SI derived unit</returns>
        private static IFormula? TryFindFormula(IElement[] expression)
        {
            return Factory.Formulas.FirstOrDefault(f => f.Expression.SequenceEqual(expression, FormulaEqComp));
        }

        /// <summary>
        /// Solve operation
        /// </summary>
        /// <param name="numberStack">Numbers stack</param>
        /// <param name="opToken">Operator token</param>
        /// <returns>Result</returns>
        /// <exception cref="SolverException"></exception>
        private static Number SolveOperation(Stack<Number> numberStack, OperatorToken opToken)
        {
            IOperator op = opToken.Operator;

            // Is it unary operation ?
            if ((op.OpCount == OperatorOpCountType.Unary) && (op is IUnaryOperator unaryOp))
            {
                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing operand", opToken.Position, opToken.Length);
                }

                Debug.WriteLine($"  Unary operation {op.Name} with {a.Value}");
                NumberType result = unaryOp.Calculate(a.Value);

                if (a is Measure measure)
                {
                    return new Measure(result, measure.Unit, a.Radix, a.IsScientificNotation, a.HexadecimalCase);
                }
                else
                {
                    return new Number(result, a.Radix, a.IsScientificNotation, a.HexadecimalCase);
                }
            }
            else if ((op.OpCount == OperatorOpCountType.Binary) && (op is IBinaryOperator binOp))
            {
                if (!numberStack.TryPop(out Number? b))
                {
                    throw new ExpressionException("Missing operand", opToken.Position, opToken.Length);
                }

                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing operand", opToken.Position, opToken.Length);
                }

                Debug.WriteLine($"  Binary operation {op.Name} with {a.Value} and {b.Value}");
                NumberType result = binOp.Calculate(a.Value, b.Value);

                // Check if should use scientific notation
                bool resultUseScientificNotation = false;
                if (a.IsScientificNotation && b.IsScientificNotation)
                {
                    resultUseScientificNotation = true;
                }
                else if (a.IsScientificNotation || b.IsScientificNotation)
                {
                    resultUseScientificNotation = false;// NumberType.Abs(NumberType (result)) > 5;
                }

                // Vote radix and potential hexadecimal casing
                IntegerRadix resultRadix = a.Radix | b.Radix;
                HexadecimalCase resultHexCase = a.HexadecimalCase | b.HexadecimalCase;

                // Check for formula
                IFormula? formula = TryFindFormula([a, b, binOp]);
                if (formula != null)
                {
                    Debug.WriteLine($"  Formula: {formula.Name}");
                    IUnit? resultUnit = Factory.Units.FirstOrDefault(u => u.Quantity == formula.Result);
                    if (resultUnit != null)
                    {
                        return new Measure(result, resultUnit, resultRadix, resultUseScientificNotation, resultHexCase);
                    }
                    else
                    {
                        return new Number(result, resultRadix, resultUseScientificNotation, resultHexCase);
                    }
                }

                // If both operands are measures and have different units, then there should be a formula to get the resulting unit
                if ((a is Measure measureA) && (b is Measure measureB))
                {
                    // Check for derived SI unit
                    ISIDerivedUnit? derivedUnit = TryFindDerivedUnit([measureA.Unit, measureB.Unit, binOp]);
                    if (derivedUnit != null)
                    {
                        Debug.WriteLine($"  Derived unit: {derivedUnit.Name}");
                        return new Measure(result, derivedUnit, resultRadix, resultUseScientificNotation, resultHexCase);
                    }

                    // TODO Check for reverse formulas.
                }

                if (a is Measure mA)
                {
                    return new Measure(result, mA.Unit, resultRadix, resultUseScientificNotation, resultHexCase);
                }
                else if (b is Measure mB)
                {
                    return new Measure(result, mB.Unit, resultRadix, resultUseScientificNotation, resultHexCase);
                }
                else
                {
                    return new Number(result, resultRadix, resultUseScientificNotation, resultHexCase);
                }
            }
            else
            {
                // Something's wrong
                throw new ExpressionException($"Unsupported operation: {op}", opToken.Position, opToken.Length);
            }
        }

        /// <summary>
        /// Solve function
        /// </summary>
        /// <param name="numberStack">Numbers stack</param>
        /// <param name="funcToken">Function token</param>
        /// <returns>Result</returns>
        /// <exception cref="SolverException"></exception>
        private static Number SolveFunction(Stack<Number> numberStack, FunctionToken funcToken)
        {
            IFunction func = funcToken.Function;
            NumberType result;

            // Is it no argument function ?
            if (func is INoArgumentFunction noArgFunc)
            {
                try
                {
                    Debug.WriteLine($"  Function {func.Name} ()");
                    result = noArgFunc.Calculate();
                }
                catch (Exception ex)
                {
                    throw new ExpressionException(ex.Message, funcToken);
                }

                return new Number(result, noArgFunc.OutputRadix, false, HexadecimalCase.None);
            }
            // Is it single argument function ?
            else if (func is ISingleArgumentFunction singleFunc)
            {
                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing argument", funcToken.Position, funcToken.Length);
                }

                try
                {
                    Debug.WriteLine($"  Function {func.Name} ({a.Value})");
                    result = singleFunc.Calculate(a.Value);
                }
                catch (Exception ex)
                {
                    throw new ExpressionException(ex.Message, funcToken);
                }

                return new Number(result, a.Radix, a.IsScientificNotation, a.HexadecimalCase);
            }
            // Is it dual argument function ?
            else if (func is IDualArgumentFunction dualFunc)
            {
                if (!numberStack.TryPop(out Number? b))
                {
                    throw new ExpressionException("Missing argument", funcToken.Position, funcToken.Length);
                }

                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing argument", funcToken.Position, funcToken.Length);
                }

                try
                {
                    Debug.WriteLine($"  Function {func.Name} ({a.Value}, {b.Value})");
                    result = dualFunc.Calculate(a.Value, b.Value);
                }
                catch (Exception ex)
                {
                    throw new ExpressionException(ex.Message, funcToken);
                }

                return new Number(result, a.Radix, a.IsScientificNotation, a.HexadecimalCase);
            }
            else
            {
                // Something's wrong
                throw new ExpressionException($"Unsupported function: {func}", funcToken.Position, funcToken.Length);
            }
        }

        /// <summary>
        /// Solve postfix expression
        /// </summary>
        /// <param name="tokens">Tokens in postfix order</param>
        /// <returns>Result</returns>
        public static Number Solve(IEnumerable<IToken> tokens)
        {
            Stack<Number> numberStack = new();
            
            foreach (var token in tokens)
            {
                if (token is NumberToken numToken)
                {
                    numberStack.Push(numToken.Number);
                }
                else if (token is MeasureToken measureToken)
                {
                    numberStack.Push(measureToken.Measure);
                }
                else if (token is ConstantToken constToken)
                {
                    numberStack.Push(constToken.Constant.Number);
                }
                else if (token is OperatorToken opToken)
                {
                    numberStack.Push(SolveOperation(numberStack, opToken));
                }
                else if (token is FunctionToken funcToken)
                {
                    numberStack.Push(SolveFunction(numberStack, funcToken));
                }
            }

            if (numberStack.Count > 1)
            {
                throw new ExpressionException("Excessive numbers", 0, 0);
            }

            return numberStack.Pop();
        }

        /// <summary>
        /// Get different result strings from result
        /// </summary>
        /// <param name="result">Result</param>
        /// <returns>Enumerable of tuples of value and unit</returns>
        public static IEnumerable<(string, string)> GetResultStrings(Number result)
        {
            Debug.WriteLine($"Result {result.Value}");

            // If radix is not (only) decimal and value is integer, then output in other radixes.
            if ((result.Radix != IntegerRadix.Decimal) && (result.Value.IsInteger()))
            {
                BigInteger resultInt = (BigInteger)result.Value;

                // Output hexadecimal if it was used
                if ((result.Radix & IntegerRadix.Hexadecimal) != 0)
                {
                    if (result.HexadecimalCase == HexadecimalCase.Lower)
                    {
                        yield return ($"{Parser.HexadecimalNumberPrefix}{resultInt:x}", string.Empty);
                    }
                    else
                    {
                        yield return ($"{Parser.HexadecimalNumberPrefix}{resultInt:X}", string.Empty);
                    }
                }

                // Output binary if it was used
                if ((result.Radix & IntegerRadix.Binary) != 0)
                {
                    yield return ($"{Parser.BinaryNumberPrefix}{resultInt:b}", string.Empty);
                }
            }

            // Result is a measure ?
            if (result is Measure measureResult)
            {
                UnitMultiple multiple = GetFittingUnitMultiple(measureResult);                
                NumberType factoredValue = result.Value / multiple.Factor;
                yield return ($"{factoredValue}", multiple.Symbols.First());

                // If factor is not 10-based number then return the value with primary unit
                // Example: 1KiB means 1024B and 1024 is not 10-based, hence both 1KiB and 1024B are returned.
                if (!NumberType.Log10(multiple.Factor).IsInteger())
                {
                    yield return ($"{result.Value}", measureResult.Unit.Symbols.First());
                }
            }
            else
            {
                yield return ($"{result.Value}", string.Empty);
            }
        }

        /// <summary>
        /// Get unit multiple that fits best to represent measure value
        /// </summary>
        /// <param name="measure">Measure</param>
        /// <returns>Unit multiple<returns>
        private static UnitMultiple GetFittingUnitMultiple(Measure measure)
        {
            if (measure.Value < measure.Unit.Multiples.First().Factor)
            {
                return measure.Unit.Multiples.First();
            }
            else if (measure.Value >= measure.Unit.Multiples.Last().Factor)
            {
                return measure.Unit.Multiples.Last();
            }
            else
            {
                return measure.Unit.Multiples.Where(m => measure.Value >= m.Factor).LastOrDefault();
            }
        }
    }
}
