using ArvBase.Operators;
using ArvBase.Tokens;
using ArvBase.Numbers;
using System.Numerics;
using System.Diagnostics;
using ArvBase.Functions;
using ArvBase.Formulas;
using ArvBase.Units;
using ArvBase.Operators.Arithmetic;

namespace ArvBase
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
            return Factory.Formulas.SelectMany(f => GetEquivalentFormulas(f)).FirstOrDefault(f => f.Expression.SequenceEqual(expression, FormulaEqComp));
        }

        /// <summary>
        /// Get all equivalent equations of a single formula
        /// </summary>
        /// <param name="formula">Formula</param>
        /// <returns>Enumeration of equivalent formulas</returns>
        private static IEnumerable<IFormula> GetEquivalentFormulas(IFormula formula)
        {
            // First the original
            yield return formula;

            // Commutative law check:
            // Multiplication or addition operation with two physics variables or numbers
            if ((formula.Expression.Length == 3) &&
                ((formula.Expression[0] is PhysicsVariable) || (formula.Expression[0] is Number)) &&
                ((formula.Expression[1] is PhysicsVariable) || (formula.Expression[1] is Number)) &&
                ((formula.Expression[2] is MultiplicationOperator) || (formula.Expression[2] is AdditionOperator)))
            {
                yield return new Formula(formula.Name,
                    [formula.Expression[1], formula.Expression[0], formula.Expression[2]],
                    formula.ResultUnit);
            }
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
                            UnitMultiple imperialUnitMultiple = GetFittingUnitMultiple(result, matchingImperialUnit);
                            return ValueToMeasure(result, imperialUnitMultiple);
                        }
                    }

                    // Get measure with reasonable unit and multiple
                    UnitMultiple fittingUnitMultiple = GetFittingUnitMultiple(result, formula.ResultUnit);
                    return ValueToMeasure(result, fittingUnitMultiple);
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
                            return ValueToMeasure(result, measureA.Unit);
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

                        Debug.WriteLine($"  Unit conversion from {measureA} to {measureB.Unit}");

                        // Get SI value of measure to be converted
                        NumberType valueA = measureA.GetNominalSIValue();
                        return ValueToMeasure(valueA, measureB.Unit);
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
        /// Convert nominal SI value to measure of specified unit multiple
        /// </summary>
        /// <param name="nominalSIValue">Nominal value</param>
        /// <param name="resultUnitMultiple">Unit multiple</param>
        /// <returns>Measure</returns>
        /// <exception cref="SolverException"></exception>
        private static Measure ValueToMeasure(NumberType nominalSIValue, UnitMultiple resultUnitMultiple)
        {
            if (resultUnitMultiple.Parent is ISIUnit)
            {
                return new Measure(nominalSIValue / resultUnitMultiple.Factor, resultUnitMultiple);
            }
            else if (resultUnitMultiple.Parent is NonSIUnit nonSIUnit)
            {
                return new Measure((nominalSIValue / nonSIUnit.EqualSIValue) / resultUnitMultiple.Factor, resultUnitMultiple);
            }
            else if (resultUnitMultiple.Parent is ImperialUnit imperialUnit)
            {
                return new Measure((nominalSIValue / imperialUnit.EqualSIValue) / resultUnitMultiple.Factor, resultUnitMultiple);
            }

            throw new SolverException($"Unit multiple {resultUnitMultiple} has no base SI unit");
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
        /// Get unit multiple that fits best to represent the value
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="unit">Base unit</param>
        /// <returns>Unit multiple<returns>
        private static UnitMultiple GetFittingUnitMultiple(NumberType value, IUnit unit)
        {
            List<(NumberType factor, UnitMultiple multiple)> multiples = unit.Multiples.Where(m => m.UseForDisplay).Select(m => (m.Factor, m)).ToList();

            if (unit is ISIUnit siUnit)
            {
                foreach (NonSIUnit nonSiUnit in Factory.Units.Where(u => (u is NonSIUnit nonSIUnit) && (nonSIUnit.EqualSIUnit == siUnit)).Cast<NonSIUnit>())
                {
                    multiples.AddRange(nonSiUnit.Multiples.Where(m => m.UseForDisplay).Select(m => (m.Factor * nonSiUnit.EqualSIValue, m)));
                }
            }

            var orderedMultiples = multiples.OrderBy(m => m.factor);

            if (value < orderedMultiples.First().factor)
            {
                return orderedMultiples.First().multiple;
            }
            else if (value >= orderedMultiples.Last().factor)
            {
                return orderedMultiples.Last().multiple;
            }
            else
            {
                return orderedMultiples.Where(m => value >= m.factor).Last().multiple;
            }
        }
    }
}
