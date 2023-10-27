using CalcBase;
using System.Diagnostics;
using System;
using System.Text;
using CalcBase.Numbers;
using System.Data;
using System.Linq.Expressions;

namespace CalcCLI
{
    public class Program
    {
        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to calculator");

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
                    expression.Remove(expression.Length - 1, 1);
                    Console.Write(' ');
                    Console.CursorLeft--;
                }
                // Delete whole expression ?
                else if (key.Key == ConsoleKey.Delete)
                {
                    expression.Clear();
                }
                // Character entry
                else
                {
                    char c = key.KeyChar;
                    if ((c != '\0') && (char.IsAscii(c)))
                    {
                        expression.Append(c);
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
                var infix = Parser.Tokenize(expression);
                Parser.InfixErrorCheck(infix);
                var postfix = Parser.ShuntingYard(infix);
                Number result = Solver.Solve(postfix);
                Console.WriteLine($"{expression}={result.Value}");
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