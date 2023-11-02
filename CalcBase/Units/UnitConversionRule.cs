namespace CalcBase.Units
{
    /// <summary>
    /// Unit conversion rule
    /// </summary>
    public record UnitConversionRule
    {
        /// <summary>
        /// Base unit to convert from
        /// </summary>
        public required IUnit BaseUnit { get; init; }

        /// <summary>
        /// Multiplication factor of base unit to get the 1 convertible unit
        /// </summary>
        public required NumberType MultiplyFactor { get; init; }

        /// <summary>
        /// Creator
        /// </summary>
        /// <param name="baseUnit">Base unit</param>
        /// <param name="multiplyFactor">Multiplication factor</param>
        public static UnitConversionRule Create(IUnit baseUnit, NumberType multiplyFactor)
        {
            return new UnitConversionRule()
            {
                BaseUnit = baseUnit,
                MultiplyFactor = multiplyFactor
            };
        }
    }
}
