using CalcBase;
using System.Diagnostics;
using System;
using System.Text;
using CalcBase.Numbers;
using System.Data;
using System.Linq.Expressions;
using CalcBase.Tokens;
using System.Numerics;
using System.Collections.Generic;

namespace CalcCLI
{
    public class Program
    {
        private static readonly bool isColoredCLI = true;
        private static readonly ConsoleColor normalColor = ConsoleColor.White;
        private static readonly List<string> expressionHistory = [];
        private static int selectedExpression = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        static Program()
        {
            if (Console.IsOutputRedirected)
            {
                isColoredCLI = false;
            }

            if (isColoredCLI)
            {
                normalColor = Console.ForegroundColor;
            }
        }

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            WriteColor("Welcome to calculator", ConsoleColor.DarkGray, true);
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
                // Delete previous character ?
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (Console.CursorLeft < expression.Length)
                    {
                        int pos = Console.CursorLeft;
                        expression.Remove(pos, 1);
                        Console.Write(expression.ToString().Substring(pos));
                        Console.Write(" ");
                        Console.CursorLeft = pos;
                    }
                }
                // Delete current character ?
                else if (key.Key == ConsoleKey.Delete)
                {
                    if (Console.CursorLeft < expression.Length)
                    {
                        int pos = Console.CursorLeft;
                        expression.Remove(pos, 1);
                        Console.Write(expression.ToString().Substring(pos));
                        Console.Write(" ");
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
                        if (Console.CursorLeft <= expression.Length)
                        {
                            int pos = Console.CursorLeft;
                            expression.Insert(pos - 1, c);
                            Console.Write(expression.ToString().Substring(pos));
                            Console.CursorLeft = pos;
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
            Number result;

            try
            {
                expression = expression.TrimEnd('=');
                var infix = Parser.Tokenize(expression);
                Parser.InfixErrorCheck(infix);
                var postfix = Parser.ShuntingYard(infix);
                result = Solver.Solve(postfix);
            }
            catch (ExpressionException ex)
            {
                Console.WriteLine(expression);
                WriteColor(ChRep(' ', ex.Position) + ChRep('^', ex.Length), ConsoleColor.Red, true);
                WriteErrorColor(ex.Message, ConsoleColor.Red, true);
                return;
            }
            catch (SolverException ex)
            {
                Console.WriteLine(expression);
                WriteErrorColor(ex.Message, ConsoleColor.Red, true);
                return;
            }

            Console.Write(expression);

            // Write all available values
            foreach ((string value, string unit) in Solver.GetResultStrings(result))
            {
                WriteColor("=", ConsoleColor.Green);
                WriteColor(value, normalColor);
                if (!string.IsNullOrWhiteSpace(unit))
                {
                    WriteColor(unit, ConsoleColor.Yellow);
                }
            }

            // End the line
            Console.WriteLine();

            // Add expression to history unless it's already there at the end
            if (expressionHistory.LastOrDefault() != expression)
            {
                expressionHistory.Add(expression);
            }

            // Make selected expression deliberately out-of-range so the next "up" button will lead to selection of this expression
            selectedExpression = expressionHistory.Count;
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
            if (expressionHistory.Count == 0)
            {
                return;
            }

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

        /// <summary>
        /// Write colored text to console
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="color">Color</param>
        /// <param name="lineEnd">Line end?</param>
        private static void WriteColor(string text, ConsoleColor color, bool lineEnd = false)
        {
            if (isColoredCLI)
            {
                Console.ForegroundColor = color;
            }

            if (lineEnd)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }

            if (isColoredCLI)
            {
                Console.ForegroundColor = normalColor;
            }
        }

        /// <summary>
        /// Write colored error text to console
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="color">Color</param>
        /// <param name="lineEnd">Line end?</param>
        private static void WriteErrorColor(string text, ConsoleColor color, bool lineEnd = false)
        {
            if (isColoredCLI)
            {
                Console.ForegroundColor = color;
            }

            if (lineEnd)
            {
                Console.Error.WriteLine(text);
            }
            else
            {
                Console.Error.Write(text);
            }

            if (isColoredCLI)
            {
                Console.ForegroundColor = normalColor;
            }
        }
    }
}