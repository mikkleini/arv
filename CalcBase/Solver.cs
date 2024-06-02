using CalcBase.Operators;
using CalcBase.Tokens;
using CalcBase.Numbers;
using System.Numerics;
using System.Diagnostics;
using CalcBase.Functions;
using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Quantities;
using CalcBase.Units;
using System.Numerics.Generic;
using System.ComponentModel;

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
        private static SIDerivedUnit? TryFindDerivedUnit(IElement[] expression)
        {
            return (SIDerivedUnit?)Factory.Units.FirstOrDefault(u => (u is SIDerivedUnit devU) && (devU.Expression.SequenceEqual(expression, FormulaEqComp)));
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
            NumberType result;
            IOperator op = opToken.Operator;

            // Is it unary operation ?
            if ((op.OpCount == OperatorOpCountType.Unary) && (op is IUnaryOperator unaryOp))
            {
                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing operand", opToken.Position, opToken.Length);
                }

                Debug.WriteLine($"  Unary operation {op.Name} with {a.Value}");
                result = unaryOp.Calculate(a.Value);

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
                // Make sure operands are given
                if (!numberStack.TryPop(out Number? b))
                {
                    throw new ExpressionException("Missing operand", opToken.Position, opToken.Length);
                }

                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing operand", opToken.Position, opToken.Length);
                }

                // Just a numeric operation ?
                if ((a is not Measure) && (b is not Measure))
                {
                    result = binOp.Calculate(a.Value, b.Value);
                    return new Number(result,
                        a.Radix | b.Radix,
                        a.IsScientificNotation || b.IsScientificNotation,
                        a.HexadecimalCase | b.HexadecimalCase);
                }

                // Check for derived SI unit (NOT DONE RIGHT NOW. TBD IF NEEDED)
                /*
                SIDerivedUnit? derivedUnit = TryFindDerivedUnit([a, b, binOp]);
                if (derivedUnit != null)
                {
                    NumberType valueA = NominalSIValue(a);
                    NumberType valueB = NominalSIValue(b);
                    
                    Debug.WriteLine($"  Derived unit {derivedUnit.Name} with {valueA} and {valueB}");
                    result = binOp.Calculate(valueA, valueB);

                    UnitMultiple unit = GetFittingUnitMultiple(result, derivedUnit);
                    return new Measure(result / unit.Factor, unit);
                }*/

                // Check for formula
                IFormula? formula = TryFindFormula([a, b, binOp]);
                if (formula != null)
                {
                    NumberType valueA = a.GetNominalSIValue();
                    NumberType valueB = b.GetNominalSIValue();

                    Debug.WriteLine($"  Formula {formula.Name} with {valueA} and {valueB}");
                    result = binOp.Calculate(valueA, valueB);

                    // Need to convert to imperial ?
                    if ((a is Measure measureImpA) && (measureImpA.Unit.Parent is ImperialUnit))
                    {
                        ImperialUnit? matchingImperialUnit = (ImperialUnit?)Factory.Units
                            .FirstOrDefault(u => (u is ImperialUnit impUnit) && (impUnit.EqualSIUnit == formula.ResultUnit));
                        if (matchingImperialUnit != null)
                        {
                            result /= matchingImperialUnit.EqualSIValue;
                            UnitMultiple imperialUnitMultiple = GetFittingUnitMultiple(result, matchingImperialUnit);
                            return new Measure(result / imperialUnitMultiple.Factor, imperialUnitMultiple);
                        }
                    }

                    // Use formula SI unit
                    UnitMultiple unit = GetFittingUnitMultiple(result, formula.ResultUnit);
                    return new Measure(result / unit.Factor, unit);
                }

                // Are both operands measures ?
                if ((a is Measure measureA) && (b is Measure measureB))
                {
                    // Only "linear" operations are allowed
                    if (ReferenceEquals(binOp, Factory.Addition) || ReferenceEquals(binOp, Factory.Subtraction))
                    {
                        // Measures of the same base unit ?
                        if (measureA.Unit.Parent == measureB.Unit.Parent)
                        {
                            Debug.WriteLine($"  Binary operation {op.Name} with {measureA} and {measureB}");
                            result = binOp.Calculate(measureA.Value * measureA.Unit.Factor, measureB.Value * measureB.Unit.Factor);
                            return new Measure(result / measureA.Unit.Factor, measureA.Unit, a.Radix, a.IsScientificNotation, a.HexadecimalCase);
                        }
                        // Measures of the same quantity ?
                        else if (measureA.Unit.Parent.Quantity == measureB.Unit.Parent.Quantity)
                        {
                            NumberType valueA = measureA.GetNominalSIValue();
                            NumberType valueB = measureB.GetNominalSIValue();

                            Debug.WriteLine($"  Operation {binOp.Name} with {valueA} and {valueB}");
                            result = binOp.Calculate(valueA, valueB);

                            // Deal with non-SI and imperial units
                            if (measureA.Unit.Parent is NonSIUnit nonSIUnit)
                            {
                                return new Measure((result / nonSIUnit.EqualSIValue) / measureA.Unit.Factor, measureA.Unit);
                            }
                            else if (measureA.Unit.Parent is ImperialUnit imperialUnit)
                            {
                                return new Measure((result / imperialUnit.EqualSIValue) / measureA.Unit.Factor, measureA.Unit);
                            }
                            else
                            {
                                return new Measure(result / measureA.Unit.Factor, measureA.Unit);
                            }
                        }
                        else
                        {
                            throw new ExpressionException("Incompatible operands", opToken.Position, opToken.Length);
                        }
                    }

                    // Unit conversion ?
                    if (ReferenceEquals(binOp, Factory.UnitConversion))
                    {
                        // Same unit ?
                        if (measureA.GetBaseSIUnit() != measureB.GetBaseSIUnit())
                        {
                            throw new ExpressionException("Incompatible conversion", opToken.Position, opToken.Length);
                        }

                        Debug.WriteLine($"  Binary operation {op.Name} with {measureA} and {measureB}");

                        // Get SI value of measure to be converted
                        NumberType valueA = measureA.GetNominalSIValue();

                        // Deal with conversion to non-SI and imperial units
                        if (measureB.Unit.Parent is ISIUnit siUnit)
                        {
                            return new Measure(valueA / measureB.Unit.Factor, measureB.Unit);
                        }
                        else if (measureB.Unit.Parent is NonSIUnit nonSIUnit)
                        {
                            return new Measure((valueA / nonSIUnit.EqualSIValue) / measureB.Unit.Factor, measureB.Unit);
                        }
                        else if (measureB.Unit.Parent is ImperialUnit imperialUnit)
                        {
                            return new Measure((valueA / imperialUnit.EqualSIValue) / measureB.Unit.Factor, measureB.Unit);
                        }
                        else
                        {
                            throw new ExpressionException("Unresolvable conversion", opToken.Position, opToken.Length);
                        }
                    }
                }
                else
                {
                    // Only "scaling" operations are allowed
                    if (ReferenceEquals(binOp, Factory.Multiplication))
                    {
                        if ((a is Measure mulMeasureA) && (b is not Measure))
                        {
                            result = binOp.Calculate(mulMeasureA.Value, b.Value);
                            return new Measure(result, mulMeasureA.Unit);
                        }
                        else if ((a is not Measure) && (b is Measure mulMeasureB))
                        {
                            result = binOp.Calculate(a.Value, mulMeasureB.Value);
                            return new Measure(result, mulMeasureB.Unit);
                        }
                    }
                    else if (ReferenceEquals(binOp, Factory.Division) && (a is Measure divMeasureA) && (b is not Measure))
                    {
                        result = binOp.Calculate(divMeasureA.Value, b.Value);
                        return new Measure(result, divMeasureA.Unit);
                    }
                }

                throw new ExpressionException("Invalid operation", opToken.Position, opToken.Length);
            }
            else
            {
                // Something's wrong
                throw new ExpressionException($"Unsupported operation: {op}", opToken.Position, opToken.Length);
            }
        }

        /// <summary>
        /// Get number nominal value
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>Value</returns>
        private static NumberType NominalValue(Number number)
        {
            if (number is Measure measure)
            {
                return measure.Value * measure.Unit.Factor;
            }

            return number.Value;
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
                yield return ($"{measureResult.Value}", measureResult.Unit.Symbols.First());

                // TODO If factor is not 10-based number then return the value with primary unit
                // Example: 1KiB means 1024B and 1024 is not 10-based, hence both 1KiB and 1024B are returned.
                /*if (!NumberType.Log10(measureResult.Unit.Factor).IsInteger())
                {
                    yield return ($"{measureResult.Value}", measureResult.Unit.Symbols.First());
                }*/
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
            if (measure.Value < measure.Unit.Parent.Multiples.First().Factor)
            {
                return measure.Unit.Parent.Multiples.First();
            }
            else if (measure.Value >= measure.Unit.Parent.Multiples.Last().Factor)
            {
                return measure.Unit.Parent.Multiples.Last();
            }
            else
            {
                return measure.Unit.Parent.Multiples.Where(m => measure.Value >= m.Factor).Last();
            }
        }

        /// <summary>
        /// Get unit multiple that fits best to represent value of unit
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="unit">Base unit</param>
        /// <returns>Unit multiple<returns>
        private static UnitMultiple GetFittingUnitMultiple(NumberType value, IUnit unit)
        {
            IEnumerable<UnitMultiple> filteredMultiples = unit.Multiples.Where(m => m.UseForDisplay);

            if (value < filteredMultiples.First().Factor)
            {
                return filteredMultiples.First();
            }
            else if (value >= filteredMultiples.Last().Factor)
            {
                return filteredMultiples.Last();
            }
            else
            {
                return filteredMultiples.Where(m => value >= m.Factor).Last();
            }
        }
    }
}
