using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCLI
{
    public static class ConsoleEx
    {
        public static bool IsColoredConsole = false;
        public static ConsoleColor NormalForegroundColor = ConsoleColor.White;

        /// <summary>
        /// Write colored text to chosen console stream
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="color">Color</param>
        public static void WriteColored(this TextWriter writer, string text, ConsoleColor color)
        {
            if (IsColoredConsole)
            {
                Console.ForegroundColor = color;
            }

            writer.Write(text);

            if (IsColoredConsole)
            {
                Console.ForegroundColor = NormalForegroundColor;
            }
        }

        /// <summary>
        /// Write colored text with line break to chosen console stream
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="color">Color</param>
        public static void WriteColoredLine(this TextWriter writer, string text, ConsoleColor color)
        {
            if (IsColoredConsole)
            {
                Console.ForegroundColor = color;
            }

            writer.WriteLine(text);

            if (IsColoredConsole)
            {
                Console.ForegroundColor = NormalForegroundColor;
            }
        }
    }
}
