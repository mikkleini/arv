using CalcBase;
using System.Diagnostics;
using System;
using System.Text;
using CalcBase.Numbers;
using System.Data;
using System.Linq.Expressions;
using CalcBase.Tokens;
using System.Numerics;

namespace CalcCLI
{
    public class Program
    {
        private static readonly Parser parser;
        private static readonly Solver solver;
        private static List<string> expressionHistory = new();
        private static int selectedExpression = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        static Program()
        {
            parser = new();
            solver = new();

            // TODO For testing only. To be removed.
            expressionHistory.Add("1+2");
            expressionHistory.Add("3*4");
            expressionHistory.Add("0x400/2");
        }

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to calculator");

            RunEntry();
        }

        /// <summary>
        /// Run expression command line entry
        /// </summary>
        private static void RunEntry()
        {
            StringBuilder expression = new();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                // Exit ?
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if ((key.Modifiers == ConsoleModifiers.Control) && (key.Key == ConsoleKey.X))
                {
                    break;
                }

                // Expression entered ?
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (expression.Length > 0)
                    {
                        DoTheMath(expression.ToString());
                        expression.Clear();
                    }
                }
                // Delete last character ?
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (expression.Length > 0)
                    {
                        expression.Remove(expression.Length - 1, 1);
                        Console.Write(' ');
                        Console.CursorLeft--;
                    }
                }
                // Delete current character ?
                else if (key.Key == ConsoleKey.Delete)
                {
                    if (Console.CursorLeft < expression.Length)
                    {
                        int pos = Console.CursorLeft;
                        expression.Remove(pos, 1);
                        Console.Write(expression.ToString().Substring(pos) + " ");
                        Console.CursorLeft = pos;
                    }
                }
                // Navigate left ?
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (Console.CursorLeft > 0)
                    {
                        Console.CursorLeft--;
                    }
                }
                // Navigate right ?
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (Console.CursorLeft < expression.Length)
                    {
                        Console.CursorLeft++;
                    }
                }
                // Navigate back in history ?
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    ExpressionHistoryWalk(expression, -1);
                }
                // Navigate forward in history ?
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    ExpressionHistoryWalk(expression, +1);
                }
                // Character entry
                else
                {
                    char c = key.KeyChar;

                    // Accept only visible characters
                    if ((c != '\0') && (char.IsAscii(c)))
                    {
                        // Insert or append ?
                        if (Console.CursorLeft < expression.Length)
                        {
                            int pos = Console.CursorLeft - 1;
                            expression.Insert(pos, c);
                            Console.Write(expression.ToString().Substring(pos + 1));
                            Console.CursorLeft = pos + 1;
                        }
                        else
                        {
                            expression.Append(c);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Do the math
        /// </summary>
        /// <param name="expression">Expression</param>
        private static void DoTheMath(string expression)
        {
            try
            {
                expression = expression.TrimEnd('=');
                var infix = parser.Tokenize(expression);
                parser.InfixErrorCheck(infix);
                var postfix = parser.ShuntingYard(infix);
                Number result = solver.Solve(postfix);

                // Create result string
                StringBuilder resultStr = new();
                resultStr.Append(expression);
                resultStr.Append('=');

                // What radix ?
                if ((result.Radix != IntegerRadix.Decimal) && (NumberType.IsInteger(result.Value)))
                {
                    if (result.Radix == IntegerRadix.Hexadecimal)
                    {
                        resultStr.Append(Parser.HexadecimalNumberPrefix);
                        if (result.DominantCase == DominantHexadecimalCase.Lower)
                        {
                            // TODO Figure out why the hexadecimal output has leading zeroes
                            resultStr.Append(result.Value.ToString("x").TrimStart('0'));
                        }
                        else
                        {
                            resultStr.Append(result.Value.ToString("X").TrimStart('0'));
                        }
                    }
                    else if (result.Radix == IntegerRadix.Binary)
                    {
                        resultStr.Append(Parser.BinaryNumberPrefix);
                        resultStr.Append($"{result.Value:b}");
                    }
                }
                else
                {
                    resultStr.Append(result.Value.ToString());
                }

                // If result is measure, then append unit
                if (result is Measure measureResult)
                {
                    resultStr.Append(measureResult.Unit.Symbols.First());
                }

                Console.WriteLine(resultStr);

                // Add expression to history
                if ((expressionHistory.Count > 0) && (expressionHistory.Last() != expression))
                {
                    expressionHistory.Add(expression);
                    selectedExpression = expressionHistory.Count - 1;
                }
            }
            catch (ExpressionException ex)
            {
                Console.WriteLine($"{expression}");
                Console.Error.WriteLine(ChRep(' ', ex.Position) + ChRep('^', ex.Length));
                Console.Error.WriteLine(ex.Message);
            }
            catch (SolverException ex)
            {
                Console.WriteLine($"{expression}");
                Console.Error.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Repeat char to create a string
        /// </summary>
        /// <param name="c">Character</param>
        /// <param name="count">Count</param>
        /// <returns>Constructed string</returns>
        private static string ChRep(char c, int count)
        {
            return new string(c, count);
        }

        /// <summary>
        /// Walk expression history backward/forward
        /// </summary>
        /// <param name="dir">Direction</param>
        private static void ExpressionHistoryWalk(StringBuilder expression, int dir)
        {
            // "Walk" but stick to boundary
            int newPos = Math.Max(0, Math.Min(expressionHistory.Count - 1, selectedExpression + dir));
            if (newPos == selectedExpression)
            {
                return;
            }

            selectedExpression = newPos;

            // Remember current expression character count (it may require overwriting on console)
            int prevLength = expression.Length;

            // Set new expression
            expression.Clear();
            expression.Append(expressionHistory[selectedExpression]);

            // Rewrite line in the console
            Console.CursorLeft = 0;
            Console.Write(expressionHistory[selectedExpression]);

            // If old expression was longer, then overwrite that with spaces
            if (prevLength > expression.Length)
            {
                Console.Write(ChRep(' ', prevLength - expression.Length));
                Console.CursorLeft = expression.Length;
            }
        }
    }
}