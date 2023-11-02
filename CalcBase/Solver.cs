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
using System.Reflection;

namespace CalcBase
{
    public class Solver
    {
        private readonly IFormula[] formulas;

        /// <summary>
        /// Constructor
        /// </summary>
        public Solver()
        {
            formulas = Parser.GetSingletonsOfType<IFormula>().ToArray();
        }

        /// <summary>
        /// Vote the common radix
        /// </summary>
        /// <param name="a">Radix A</param>
        /// <param name="b">Radix B</param>
        /// <returns>Common radix</returns>
        private IntegerRadix VoteCommonRadix(IntegerRadix a, IntegerRadix b)
        {
            // Use radix of first operand
            return a;
        }

        /// <summary>
        /// Vote the common dominant case (in case of hexadecimal numbers)
        /// </summary>
        /// <param name="a">Radix A</param>
        /// <param name="b">Radix B</param>
        /// <returns>Common radix</returns>
        private DominantHexadecimalCase VoteCommonDominantCase(DominantHexadecimalCase a, DominantHexadecimalCase b)
        {
            return (a == DominantHexadecimalCase.None ? b : a);
        }

        /// <summary>
        /// Solve operation
        /// </summary>
        /// <param name="numberStack">Numbers stack</param>
        /// <param name="opToken">Operator token</param>
        /// <returns>Result</returns>
        /// <exception cref="SolverException"></exception>
        private Number SolveOperation(Stack<Number> numberStack, OperatorToken opToken)
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
                    return new Measure()
                    {
                        Value = result,
                        Radix = a.Radix,
                        DominantCase = a.DominantCase,
                        IsScientificNotation = a.IsScientificNotation,
                        Unit = measure.Unit
                    };
                }
                else
                {
                    return new Number()
                    {
                        Value = result,
                        Radix = a.Radix,
                        DominantCase = a.DominantCase,
                        IsScientificNotation = a.IsScientificNotation
                    };
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

                // If both operands are measure and have different units, then there should be a formula to get the resulting unit
                if ((a is Measure measureA) && (b is Measure measureB) && (measureA.Unit != measureB.Unit))
                {
                    IFormula? formula = formulas.FirstOrDefault(f => (f.Expression.Length == 3) &&
                            (f.Expression[0] is UnitToken formAToken) && (formAToken.Unit == measureA.Unit) &&
                            (f.Expression[1] is UnitToken formBToken) && (formBToken.Unit == measureB.Unit) &&
                            (f.Expression[2] is OperatorToken formOpToken) && (formOpToken.Operator == op));

                    if (formula == null)
                    {
                        throw new SolverException($"No formula to calculate {measureA} {op.Symbol} {measureB}");
                    }

                    return new Measure()
                    {
                        Value = result,
                        Radix = VoteCommonRadix(a.Radix, b.Radix),
                        DominantCase = VoteCommonDominantCase(a.DominantCase, b.DominantCase),
                        IsScientificNotation = a.IsScientificNotation || b.IsScientificNotation,
                        Unit = formula.Result
                    };
                }
                else if (a is Measure mA)
                {
                    return new Measure()
                    {
                        Value = result,
                        Radix = VoteCommonRadix(a.Radix, b.Radix),
                        DominantCase = VoteCommonDominantCase(a.DominantCase, b.DominantCase),
                        IsScientificNotation = a.IsScientificNotation || b.IsScientificNotation,
                        Unit = mA.Unit
                    };
                }
                else if (b is Measure mB)
                {
                    return new Measure()
                    {
                        Value = result,
                        Radix = VoteCommonRadix(a.Radix, b.Radix),
                        DominantCase = VoteCommonDominantCase(a.DominantCase, b.DominantCase),
                        IsScientificNotation = a.IsScientificNotation || b.IsScientificNotation,
                        Unit = mB.Unit
                    };
                }
                else
                {
                    return new Number()
                    {
                        Value = result,
                        Radix = VoteCommonRadix(a.Radix, b.Radix),
                        DominantCase = VoteCommonDominantCase(a.DominantCase, b.DominantCase),
                        IsScientificNotation = a.IsScientificNotation || b.IsScientificNotation
                    };
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
        private Number SolveFunction(Stack<Number> numberStack, FunctionToken funcToken)
        {
            IFunction func = funcToken.Function;

            // Is it no argument function ?
            if (func is INoArgumentFunction noArgFunc)
            {
                Debug.WriteLine($"  Function {func.Name} ()");
                NumberType result = noArgFunc.Calculate();

                return new Number()
                {
                    Value = result,
                    Radix = noArgFunc.OutputRadix,
                    DominantCase = DominantHexadecimalCase.None,
                    IsScientificNotation = false
                };
            }
            // Is it single argument function ?
            else if (func is ISingleArgumentFunction singleFunc)
            {
                if (!numberStack.TryPop(out Number? a))
                {
                    throw new ExpressionException("Missing argument", funcToken.Position, funcToken.Length);
                }

                Debug.WriteLine($"  Function {func.Name} ({a.Value})");
                NumberType result = singleFunc.Calculate(a.Value);

                return new Number()
                {
                    Value = result,
                    Radix = a.Radix,
                    DominantCase = a.DominantCase,
                    IsScientificNotation = a.IsScientificNotation
                };
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

                Debug.WriteLine($"  Function {func.Name} ({a.Value}, {b.Value})");
                NumberType result = dualFunc.Calculate(a.Value, b.Value);

                return new Number()
                {
                    Value = result,
                    Radix = a.Radix,
                    DominantCase = a.DominantCase,
                    IsScientificNotation = a.IsScientificNotation
                };
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
        public Number Solve(IEnumerable<IToken> tokens)
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
