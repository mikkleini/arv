using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Functions;
using CalcBase.Numbers;
using CalcBase.Operators;
using CalcBase.Operators.Arithmetic;
using CalcBase.Operators.Conversion;
using CalcBase.Quantities;
using CalcBase.Tokens;
using CalcBase.Units;
using static System.Net.Mime.MediaTypeNames;

[assembly: InternalsVisibleTo("CalcBaseTest")]
namespace CalcBase
{
    /// <summary>
    /// Expression parser
    /// </summary>
    public partial class Parser
    {
        public static readonly string BinaryNumberPrefix = "0b";
        public static readonly string HexadecimalNumberPrefix = "0x";
        public static readonly char RadixPointSymbol = '.';
        public static readonly char ArgumentSeparatorSymbol = ',';
        public static readonly char ExponentPointSymbolUpper = 'E';
        public static readonly char ExponentPointSymbolLower = 'e';
        public static readonly (char unicode, char simple)[] SpecialCharacterReplacements =
        [
            ('µ', 'u'),
            ('²', '2'),
            ('³', '3')
        ];

        private static readonly List<(string text, IElement element)> TextMatches = [];

        /// <summary>
        /// Prepare text matcher (lazy method)
        /// </summary>
        internal static void PrepareTextMatcher()
        {
            // Already prepared ?
            if (TextMatches.Count > 0)
            {
                return;
            }

            // Add operators to the matching list
            TextMatches.AddRange(Factory.Operators.Select(o => (o.Symbol, (IElement)o)));

            // Add functions to matching list
            TextMatches.AddRange(Factory.Functions.Select(f => (f.Symbol, (IElement)f)));

            // Add constants to matching list
            TextMatches.AddRange(Factory.Constants
                .SelectMany(c => c.Symbols
                .SelectMany(s => GetSymbolWithSimplification(s)
                .Select(ss => (ss, (IElement)c)))));

            // Add unit multiples with simplified symbols to the list
            TextMatches.AddRange(Factory.Units
                .SelectMany(u => u.Multiples
                .SelectMany(m => m.Symbols
                .SelectMany(s => GetSymbolWithSimplification(s)
                .Select(ss => (ss, (IElement)m))))));

            // Order list
            //TextMatches.OrderBy(e => e.text.Length).ToList();

            // Debug print conflicting symbols
            #if DEBUG
            foreach (var g in TextMatches.GroupBy(m => m.text).Where(g => g.Count() > 1))
            {
                System.Diagnostics.Debug.WriteLine($"Symbol conflict: \"{g.First().text}\" has {g.Count()} candidates");
            }
            #endif
        }

        /// <summary>
        /// Get symbol with potential simplification (special characters replaced with simpler ones)
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>Symbols</returns>
        internal static IEnumerable<string> GetSymbolWithSimplification(string symbol)
        {
            // First return the original symbol
            yield return symbol;

            // Next check if symbol contains any special characters - if does, then replace them
            bool hasBeenSimplified = false;
            char[] s = symbol.ToCharArray();

            foreach ((char unicode, char simple) in SpecialCharacterReplacements)
            {
                if (s.AsSpan().Contains(unicode))
                {
                    s.AsSpan().Replace(unicode, simple);
                    hasBeenSimplified = true;
                }
            }

            // If was simplified, then return it
            if (hasBeenSimplified)
            {
                yield return new string(s);
            }
        }

        /// <summary>
        /// Check if matching text element fits the context
        /// </summary>
        /// <param name="element">Element</param>
        /// <param name="prevToken">Previous token (if any)</param>
        /// <returns>true if match fits the context</returns>
        internal static bool CheckTextElementFitToContext(IElement element, IToken? prevToken)
        {
            if (element is NegationOperator)
            {
                // Negation can only be used in certain places
                return (prevToken == null) || (prevToken is LeftParenthesisToken) || (prevToken is ArgumentSeparatorToken) || (prevToken is OperatorToken);
            }
            else if (element is SubtractionOperator)
            {
                // Can only subtract from other value
                return ((prevToken != null) &&
                    ((prevToken is NumberToken) || (prevToken is MeasureToken) || (prevToken is UnitToken) || (prevToken is RightParenthesisToken)));
            }
            else if (element is UnitMultiple)
            {
                // Units can only follow numbers or be after unit compersion operator
                return (prevToken != null) && ((prevToken is NumberToken) || ((prevToken is OperatorToken opToken) && (opToken.Operator is UnitConversionOperator)));
            }
            else if (element is IFunction)
            {
                // Functions can only be at the beginning, after operator or after opening bracket
                return (prevToken == null) || (prevToken is OperatorToken) || (prevToken is LeftParenthesisToken);
            }

            return true;
        }

        /// <summary>
        /// Read text (don't yet know if it's constant, unit, variable or function)
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="start">Number start position</param>
        /// <param name="prevToken">Previous token (if any)</param>
        /// <returns>Text token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static IToken ReadText(ReadOnlySpan<char> infix, int start, IToken? prevToken)
        {
            PrepareTextMatcher();

            // Create new string because spans cannot be used inside lambdas
            string text = infix.Slice(start).ToString();
            var matches = TextMatches.Where(m => text.StartsWith(m.text));

            // Any match ?
            if (matches.Any())
            {
                // Get the match(es) with longest symbol (to avoid function 'cos' being matched as constant 'c')
                int longestSymbol = matches.OrderByDescending(m => m.text.Length).First().text.Length;
                var longestMatches = matches.Where(m => m.text.Length == longestSymbol);

                // Are there more than one long matches ? Rare case, but possible for "min" as minute and "min" as function.
                if (longestMatches.Count() > 1)
                {
                    // Use context fit check to rule out one or another
                    longestMatches = longestMatches.Where(m => CheckTextElementFitToContext(m.element, prevToken));

                    // Are we down to one candidate ?
                    if (longestMatches.Count() > 1)
                    {
                        throw new ExpressionException($"Ambigious function/constant/unit: {text}", start, longestSymbol);
                    }
                }

                // Any match left ?
                if (longestMatches.Any())
                {
                    (string symbol, IElement element) = longestMatches.First();

                    if (element is IOperator op)
                    {
                        return new OperatorToken()
                        {
                            Operator = op,
                            Position = start,
                            Length = symbol.Length,
                        };
                    }
                    else if (element is IFunction func)
                    {
                        return new FunctionToken()
                        {
                            Function = func,
                            Position = start,
                            Length = symbol.Length,
                        };
                    }
                    else if (element is IConstant constant)
                    {
                        return new ConstantToken()
                        {
                            Constant = constant,
                            Position = start,
                            Length = symbol.Length,
                        };
                    }
                    else if (element is UnitMultiple unitMultiple)
                    {
                        return new UnitToken()
                        {
                            Unit = unitMultiple,
                            Position = start,
                            Length = symbol.Length
                        };
                    }
                    else
                    {
                        throw new NotImplementedException("Unimplemented element");
                    }
                }
            }

            throw new ExpressionException($"Unknown symbol: {text}", start, text.Length);
        }

        /// <summary>
        /// Minus operator requires special handling
        /// TODO Maybe there's more universal way to resolve conflicting symbols?
        /// </summary>
        /// <param name="prevToken">Previous token or null if none</param>
        /// <returns>Operator</returns>
        internal static IOperator DecideMinusOperator(IToken? prevToken)
        {
            if (prevToken == null)
            {
                return Factory.Negation;
            }
            else if (prevToken is LeftParenthesisToken)
            {
                return Factory.Negation;
            }
            else if (prevToken is ArgumentSeparatorToken)
            {
                return Factory.Negation;
            }
            else if (prevToken is OperatorToken)
            {
                return Factory.Negation;
            }

            return Factory.Subtraction;
        }

        /// <summary>
        /// Tokenize infix expression
        /// </summary>
        /// <returns></returns>
        public static List<IToken> Tokenize(string infix)
        {
            IToken? prevToken = null;
            List<IToken> tokens = [];
            int i = 0;

            while (i < infix.Length)
            {
                char c = infix[i];
                IToken? token = null;

                if (char.IsWhiteSpace(c))
                {
                    // Ignore
                    i++;
                    continue;
                }
                else if (char.IsDigit(c))
                {
                    token = ReadNumber(infix, i);
                }
                else if (c == '(')
                {
                    token = new LeftParenthesisToken()
                    {
                        Position = i,
                        Length = 1
                    };
                }
                else if (c == ')')
                {
                    token = new RightParenthesisToken()
                    {
                        Position = i,
                        Length = 1
                    };
                }
                else if (c == ArgumentSeparatorSymbol)
                {
                    token = new ArgumentSeparatorToken()
                    {
                        Position = i,
                        Length = 1
                    };
                }
                else
                {
                    token = ReadText(infix, i, prevToken);
                }

                // Found token ?
                if (token != null)
                {
                    tokens.Add(token);
                    i += token.Length;
                    prevToken = token;
                }
                else
                {
                    throw new ExpressionException($"Syntax error ({c} at {i})", i, 1);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Count and check function arguments
        /// </summary>
        /// <param name="infix">Infix tokens</param>
        /// <param name="funcIndex">Function index</param>
        /// <returns></returns>
        private static void CheckFunctionArguments(List<IToken> infix, int funcIndex)
        {
            FunctionToken funcToken = (FunctionToken)infix[funcIndex];
            int numExpectedArgs = funcToken.ArgumentCount;
            int numActualArgs = 0;
            int funcParenthesisDepth = 0;

            // Count arguments
            for (int i = funcIndex + 2; i < infix.Count; i++)
            {
                IToken? previous = (i > 0 ? infix[i - 1] : null);
                IToken? current = infix[i];

                if (current is LeftParenthesisToken)
                {
                    funcParenthesisDepth++;
                }
                else if (current is RightParenthesisToken)
                {
                    funcParenthesisDepth--;
                    if (funcParenthesisDepth < 0)
                    {
                        // It's the end of the function
                        numActualArgs++;
                        break;
                    }
                }
                else if (current is ArgumentSeparatorToken)
                {
                    if (previous is ArgumentSeparatorToken)
                    {
                        throw new ExpressionException("Missing argument", current);
                    }

                    numActualArgs++;
                }
            }

            if (numActualArgs < numExpectedArgs)
            {
                throw new ExpressionException($"Missing function argument(s)", funcToken);
            }
            else if (numActualArgs > numExpectedArgs)
            {
                throw new ExpressionException($"Excessive function argument(s)", funcToken);
            }
        }

        /// <summary>
        /// Check for infix errors
        /// </summary>
        /// <param name="infix">Infix tokens</param>
        /// <exception cref="ExpressionException"></exception>
        public static void InfixErrorCheck(List<IToken> infix, int startIndex = 0)
        {
            int parenthesisDepth = 0;

            if (infix.Count == 0)
            {
                throw new ExpressionException("Empty expression", 0, 0);
            }

            // Check token by token
            for (int i = 0; i < infix.Count; i++)
            {
                IToken? previous = (i > 0 ? infix[i - 1] : null);
                IToken? current = infix[i];
                IToken? next = (i < infix.Count - 1 ? infix[i + 1] : null);

                if (current is LeftParenthesisToken)
                {
                    parenthesisDepth++;
                }
                else if (current is RightParenthesisToken)
                {
                    parenthesisDepth--;
                    if (parenthesisDepth < 0)
                    {
                        throw new ExpressionException("Excessive closing parenthesis", current);
                    }
                }

                // Empty parenthesis not used with function
                if ((previous is not FunctionToken) && (current is LeftParenthesisToken) && (next is RightParenthesisToken))
                {
                    throw new ExpressionException("Empty parenthesis", current.Position, next.Position - current.Position + 1);
                }

                // Make sure function is followed by opening parenthesis
                if ((current is FunctionToken) && (next is not LeftParenthesisToken))
                {
                    throw new ExpressionException("Missing parenthesis for argument(s)", current);
                }

                // Check function argument count (must be after parenthesis check)
                if (current is FunctionToken)
                {
                    CheckFunctionArguments(infix, i);
                }
            }

            // Any missing closing parenthesis ?
            if (parenthesisDepth > 0)
            {
                throw new ExpressionException($"Missing closing parenthesis", infix.Last().Position + 1, 1);
            }
        }

        /// <summary>
        /// Shunting yard algoritm to get postfix (RPN) expression tokens from infix tokens
        /// </summary>
        /// <param name="infix">Token in infix order</param>
        /// <returns>List of tokens in postfix (RPN) order</returns>
        public static List<IToken> ShuntingYard(List<IToken> infix)
        {
            List<IToken> postfix = [];
            Stack<IToken> opStack = new();

            for (int i = 0; i < infix.Count; i++)
            {
                IToken? current = infix[i];
                IToken? next = (i < infix.Count - 1 ? infix[i + 1] : null);
                
                if (current is NumberToken numberToken)
                {
                    if (next is UnitToken unitToken)
                    {
                        // Combine number and unit token into measure token
                        postfix.Add(new MeasureToken()
                        {
                            Position = numberToken.Position,
                            Length = unitToken.Position + numberToken.Length - numberToken.Position,
                            Measure = new Measure(numberToken.Number, unitToken.Unit)
                        });

                        // Leap over unit token
                        i++;
                    }
                    else
                    {
                        postfix.Add(numberToken);
                    }
                }
                else if (current is UnitToken unitToken1)
                {
                    throw new ExpressionException($"Excessive unit: {unitToken1.Unit.Name}", current.Position, current.Length);
                }
                else if (current is ConstantToken constToken)
                {
                    postfix.Add(constToken);
                }
                else if (current is LeftParenthesisToken leftParenthesisToken)
                {
                    opStack.Push(leftParenthesisToken);
                }
                else if (current is RightParenthesisToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) && ((stackedToken is OperatorToken) || (stackedToken is FunctionToken)))
                    {
                        postfix.Add(opStack.Pop());
                    }
                    opStack.Pop();
                }
                else if (current is ArgumentSeparatorToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) && ((stackedToken is OperatorToken) || (stackedToken is FunctionToken)))
                    {
                        postfix.Add(opStack.Pop());
                    }
                }
                else if (current is OperatorToken opToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) &&
                        (((stackedToken is OperatorToken stackedOpToken) && (opToken.Operator.Precedence >= stackedOpToken.Operator.Precedence)) ||
                        (stackedToken is FunctionToken)))
                    {
                        postfix.Add(opStack.Pop());
                    }

                    opStack.Push(opToken);

                    // Special case with unit conversion operator
                    if (next is UnitToken unitToken2)
                    {
                        // Combine unit token to nominal measure and add it as an operand
                        postfix.Add(new MeasureToken()
                        {
                            Position = unitToken2.Position,
                            Length = unitToken2.Length,
                            Measure = new Measure(Factory.One, unitToken2.Unit)
                        });

                        // Leap over unit token
                        i++;
                    }
                }
                else if (current is FunctionToken funcToken)
                {
                    opStack.Push(funcToken);
                }
                else
                {
                    // Something's wrong
                    throw new ExpressionException($"Unhandled token: {current}", current.Position, current.Length);
                }
            }

            while (opStack.TryPop(out IToken? opToken))
            {
                postfix.Add(opToken);
            }

            return postfix;
        }
    }
}
