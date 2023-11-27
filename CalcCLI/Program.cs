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

        /// <summary>
        /// Constructor
        /// </summary>
        static Program()
        {
            parser = new();
            solver = new();
        }

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to calculator");

            Debug.WriteLine(Factory.Instance.GetHashCode());

            RunEntry();
            /*
            while (true)
            {
                string? expression = Console.ReadLine();
                if (expression != null)
                {
                    DoTheMath(expression);
                }
            }*/
        }

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
                    DoTheMath(expression.ToString());
                    expression.Clear();
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
                // Delete whole expression ?
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
                // Character entry
                else
                {
                    char c = key.KeyChar;
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

                
                if (result is Measure measureResult)
                {
                    Console.WriteLine($"{expression}={measureResult.Value}{measureResult.Unit.Symbols.First()}");
                }
                else
                {
                    Console.WriteLine($"{expression}={result.Value}");
                }
            }
            catch (ExpressionException ex)
            {
                Console.WriteLine($"{expression}");
                Console.Error.WriteLine(new string(' ', ex.Position) + new string('^', ex.Length));
                Console.Error.WriteLine(ex.Message);
            }
            catch (SolverException ex)
            {
                Console.WriteLine($"{expression}");
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}