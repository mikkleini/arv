﻿namespace CalcBase.Functions
{
    public record SinFunction : ISingleArgumentFunction
    {
        public string Name => "Sine";
        public string Symbol => "sin";

        /// <summary>
        /// Calculate sine function value
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Sine value</returns>
        public NumberType Calculate(NumberType x)
        {
            return NumberType.Sin(x);
        }
    }
}
