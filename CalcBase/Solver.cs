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
using System.Reflection;

namespace CalcBase
{
    /// <summary>
    /// Postfix (RPN) expressions solver
    /// </summary>
    public class Solver
    {
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

                // If both operands are measure and have different units, then there should be a formula to get the resulting unit
                if ((a is Measure measureA) && (b is Measure measureB) && (measureA.Unit != measureB.Unit))
                {
                    IElement[] expression = [measureA.Unit, measureB.Unit, binOp];
                    IUnit? matchingUnit = Factory.Units.FirstOrDefault(u => (u is ISIDerivedUnit devU) && (devU.Expression.SequenceEqual(expression)));
                    if (matchingUnit != null)
                    {
                        return new Measure(result, matchingUnit, resultRadix, resultUseScientificNotation, resultHexCase);
                    }

                    // TODO Find formula...
                    throw new ExpressionException("No suitable derived unit or formula found", opToken.Position, opToken.Length);                    
                }
                else if (a is Measure mA)
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
    }
}
