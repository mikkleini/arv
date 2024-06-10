using ArvBase.Constants;
using ArvBase.Formulas;
using ArvBase.Functions;
using ArvBase.Functions.Mathematical;
using ArvBase.Numbers;
using ArvBase.Operators;
using ArvBase.Operators.Arithmetic;
using ArvBase.Operators.Bitwise;
using ArvBase.Operators.Conversion;
using ArvBase.Quantities;
using ArvBase.Units;
using System.Numerics;

namespace ArvBase
{
    /// <summary>
    /// Factory of elements
    /// </summary>
    public static class Factory
    {
        // Numbers
        public static readonly Number MinusFour = new(-4);
        public static readonly Number MinusThree = new(-3);
        public static readonly Number MinusTwo = new(-2);
        public static readonly Number MinusOne = new(-1);
        public static readonly Number Zero = new(0);
        public static readonly Number Half = new(0.5M);
        public static readonly Number One = new(1);
        public static readonly Number Two = new(2);
        public static readonly Number Three = new(3);
        public static readonly Number Four = new(4);

        // Standard SI unit prefixes, names and exponents (in factor field)
        public static readonly (string Name, string Symbol, int Exponent, bool UseForDisplay)[] SIStandardUnitMultiples =
        [
            new("Quecto", "q", -30, true ),
            new("Ronto",  "r", -27, true ),
            new("Yocto",  "y", -24, true ),
            new("Zepto",  "z", -21, true ),
            new("Atto",   "a", -18, true ),
            new("Femto",  "f", -15, true ),
            new("Pico",   "p", -12, true ),
            new("Nano",   "n",  -9, true ),
            new("Micro",  "µ",  -6, true ),
            new("Milli",  "m",  -3, true ),
            new("Centi",  "c",  -2, true),
            new("Deci",   "d",  -1, false),
            new("",       "",    0, true ),
            new("Deca",   "da",  1, false),
            new("Hecto",  "ha",  2, false),
            new("Kilo",   "k",   3, true ),
            new("Mega",   "M",   6, true ),
            new("Giga",   "G",   9, true ),
            new("Tera",   "T",  12, true ),
            new("Peta",   "P",  15, true ),
            new("Exa",    "E",  18, true ),
            new("Zetta",  "Z",  21, true ),
            new("Yotta",  "Y",  24, true ),
            new("Ronna",  "R",  27, true ),
            new("Quetta", "Q",  30, true ),
        ];

        // Trigonometric functions
        public static readonly SingleArgumentFunction Sin = new("Sine", "sin", NumberType.Sin);
        public static readonly SingleArgumentFunction Cos = new("Cosine", "cos", NumberType.Cos);
        public static readonly SingleArgumentFunction Tan = new("Tangent", "tan", NumberType.Tan);
        public static readonly SingleArgumentFunction Asin = new("Arcsine", "asin", NumberType.Asin);
        public static readonly SingleArgumentFunction Acos = new("Arccosine", "acos", NumberType.Acos);
        public static readonly SingleArgumentFunction Atan = new("Arctangent", "atan", NumberType.Atan);
        public static readonly DualArgumentFunction Atan2 = new("Two argument arctangent", "atan2", NumberType.Atan2);
        public static readonly SingleArgumentFunction Sinh = new("Hyperbolic sine", "sinh", NumberType.Sinh);
        public static readonly SingleArgumentFunction Cosh = new("Hyperbolic cosine", "cosh", NumberType.Cosh);
        public static readonly SingleArgumentFunction Tanh = new("Hyperbolic tangent", "tanh", NumberType.Tanh);
        public static readonly SingleArgumentFunction Ln = new("Natural (base-E) logarithm", "ln", NumberType.Log);
        public static readonly SingleArgumentFunction Log2 = new("Base-2 logarithm", "log2", NumberType.Log2);
        public static readonly SingleArgumentFunction Log10 = new("Base-10 logarithm", "log10",NumberType.Log10);

        // Number tweaking functions
        public static readonly SingleArgumentFunction Abs = new("Absolute", "abs", NumberType.Abs);
        public static readonly SingleArgumentFunction Sign = new("Sign", "sign", x => NumberType.Sign(x));
        public static readonly SingleArgumentFunction Ceil = new("Ceiling", "ceil", NumberType.Ceiling);
        public static readonly SingleArgumentFunction Floor = new("Floor", "floor", NumberType.Floor);
        public static readonly SingleArgumentFunction Trunc = new("Trunc", "trunc", NumberType.Truncate);
        public static readonly RoundFunction Round = new();
        public static readonly DualArgumentFunction Min = new("Minimum of two", "min", NumberType.Min);
        public static readonly DualArgumentFunction Max = new("Maximum of two", "max", NumberType.Max);
        //public static readonly SingleArgumentFunction Hex = new("Number to hexadecimal", "hex", (x) => x);

        // Other functions
        public static readonly SingleArgumentFunction Sqrt = new("Square root", "sqrt", NumberType.Sqrt);
        public static readonly SingleArgumentFunction Cbrt = new("Cube root", "cbrt", NumberType.Cbrt);

        // Mathematical operators
        public static readonly NegationOperator Negation = new();
        public static readonly AdditionOperator Addition = new();
        public static readonly SubtractionOperator Subtraction = new();
        public static readonly MultiplicationOperator Multiplication = new();
        public static readonly DivisionOperator Division = new();
        public static readonly ExponentOperator Exponent = new();
        public static readonly QuotientOperator Quotient = new();
        public static readonly ReminderOperator Reminder = new();

        // Bitwise operators
        public static readonly InverseOperator Inverse = new();
        public static readonly AndOperator And = new();
        public static readonly OrOperator Or = new();
        public static readonly XorOperator Xor = new();
        public static readonly LeftShiftOperator LeftShift = new();
        public static readonly RightShiftOperator RightShift = new();

        // Special operators
        public static readonly UnitConversionOperator UnitConversion = new();

        // Physics quantities
        public static readonly Quantity Time = new("Time", ["t"]);
        public static readonly Quantity Length = new("Length", ["l"]);
        public static readonly Quantity Speed = new("Speed", ["v"]);
        public static readonly Quantity Mass = new("Mass", ["m"]);
        public static readonly Quantity Density = new("Density", ["ρ"]);
        public static readonly Quantity Acceleration = new("Acceleration", ["a"]);
        public static readonly Quantity Area = new("Area", ["A"]);
        public static readonly Quantity Volume = new("Volume", ["V"]);
        public static readonly Quantity Force = new("Force", ["F"]);
        public static readonly Quantity Frequency = new("Frequency", ["f"]);
        public static readonly Quantity Energy = new("Energy", ["E"]);
        public static readonly Quantity ThermodynamicTemperature = new("Thermodynamic temperature", ["Θ"]);
        public static readonly Quantity AmountOfSubstance = new("Amount of substance", ["N"]);
        public static readonly Quantity LuminousIntensity = new ("Luminous intensity", ["J"]);

        // Electrical quantities
        public static readonly Quantity Voltage = new("Voltage", ["V", "U"]);
        public static readonly Quantity Current = new("Electric current", ["I"]);
        public static readonly Quantity Resistance = new("Electric resistance", ["R"]);
        public static readonly Quantity Conductance = new("Electric conductance", ["G"]);
        public static readonly Quantity Capacitance = new("Capacitance", ["C"]);
        public static readonly Quantity Inductance = new("Inductance", ["L"]);
        public static readonly Quantity Impedance = new("Impedance", ["Z"]);
        public static readonly Quantity Charge = new("Charge", ["C"]);
        public static readonly Quantity Power = new("Power", ["P"]);

        // Digital quantities
        public static readonly Quantity DataSize = new("Datasize", [""]);
        public static readonly Quantity DataRate = new("Datarate", [""]);

        // Dummies
        internal static readonly DummyUnit DummyUnit = new();

        // SI base units
        public static readonly SIBaseUnit Second = new(Time, CreateStandardUnitMultiples("Second", "s"));
        public static readonly SIBaseUnit Metre = new(Length, CreateStandardUnitMultiples("Metre", "m"));
        public static readonly SIBaseUnit Kilogram = new(Mass, CreateStandardUnitMultiples("Gram", "g", -3));
        public static readonly SIBaseUnit Ampere = new(Current, CreateStandardUnitMultiples("Ampere", "A"));
        public static readonly SIBaseUnit Kelvin = new(ThermodynamicTemperature, CreateStandardUnitMultiples("Kelvin", "K"));
        public static readonly SIBaseUnit Mole = new(AmountOfSubstance, CreateStandardUnitMultiples("Mole", "mol"));
        public static readonly SIBaseUnit Candela = new(LuminousIntensity, CreateStandardUnitMultiples("Candela", "cd"));

        // SI derived units
        public static readonly SIDerivedUnit MetrePerSecond = new(Speed,
            [Metre, Second, Division],
            [
                new("Micrometre per second", ["µm/s"], 1e-6M, UnitContext.All),
                new("Millimetre per second", ["mm/s"], 1e-3M, UnitContext.All),
                new("Metre per second",      ["m/s"],  1,     UnitContext.All),
                new("Kilometre per second",  ["km/s"], 1e+3M, UnitContext.All)
            ]);

        public static readonly SIDerivedUnit MetrePerSecondSquared = new(Acceleration,
            [MetrePerSecond, Second, Division],
            [
                new("Micrometre per second squared", ["µm/s²"], 1e-6M, UnitContext.All),
                new("Millimetre per second squared", ["mm/s²"], 1e-3M, UnitContext.All),
                new("Metre per second squared",      ["m/s²"],  1,     UnitContext.All),
                new("Kilometre per second squared",  ["km/s²"], 1e+3M, UnitContext.All)
            ]);

        public static readonly SIDerivedUnit SquareMetre = new(Area,
            [Metre, Two, Exponent],
            [
                new("Square micrometre", ["µm²"],  1e-12M, UnitContext.All),
                new("Square millimetre", ["mm²"],  1e-6M,  UnitContext.All),
                new("Square centimetre", ["cm²"],  1e-4M,  UnitContext.All),
                new("Square decimetre",  ["dm²"],  1e-2M,  UnitContext.All),
                new("Square metre",      ["m²"],   1,      UnitContext.All),
                new("Square kilometre",  ["km²"],  1e+6M,  UnitContext.All)
            ]);

        public static readonly SIDerivedUnit CubicMetre = new(Volume,
            [Metre, Three, Exponent],
            [
                new("Cubic micrometre", ["µm³"],  1e-12M, UnitContext.All),
                new("Cubic millimetre", ["mm³"],  1e-9M,  UnitContext.All),
                new("Cubic centimetre", ["cm³"],  1e-6M,  UnitContext.All),
                new("Cubic decimetre",  ["dm³"],  1e-3M,  UnitContext.All),
                new("Cubic metre",      ["m³"],   1,      UnitContext.All),
                new("Cubic decametre",  ["dam³"], 1e+3M,  UnitContext.All),
                new("Cubic hectometre", ["hm³"],  1e+6M,  UnitContext.All),
                new("Cubic kilometre",  ["km³"],  1e+9M,  UnitContext.All),
            ]);

        public static readonly SIDerivedUnit Newton = new(Force,
            [Kilogram, Metre, Second, MinusTwo, Exponent, Multiplication, Multiplication],
            CreateStandardUnitMultiples("Newton", "N"));

        public static readonly SIDerivedUnit Joule = new(Energy,
            [Kilogram, Metre, Second, MinusTwo, Exponent, Metre, Two, Exponent, Multiplication, Multiplication],
            CreateStandardUnitMultiples("Joule", "J"));

        // TODO Equation in SI base units: kg⋅m2⋅s−3⋅A−1 */
        public static readonly SIDerivedUnit Volt = new(Voltage, [], CreateStandardUnitMultiples("Volt", "V"));

        // TODO Equation in SI base units: kg⋅m2⋅s−3⋅A−2 */
        public static readonly SIDerivedUnit Ohm = new(Resistance, [], CreateStandardUnitMultiples("Ohm", "Ω"));

        // Non-SI units, but widely used 
        public static readonly NonSIUnit Minute           = new(60,                 Second,      [new("Minute", ["min"], 1, UnitContext.All)]);
        public static readonly NonSIUnit Hour             = new(3600,               Second,      [new("Hour", ["h"], 1, UnitContext.All)]);
        public static readonly NonSIUnit Day              = new(24 * 3600,          Second,      [new("Day", ["d"], 1, UnitContext.All)]);
        public static readonly NonSIUnit JulianYear       = new(31557600,           Second,      [new("Julian year", ["a"], 1, UnitContext.Astronomy)]);
        public static readonly NonSIUnit AstronomicalUnit = new(149597870700,       Metre,       [new("Astronomical unit", ["au"], 1, UnitContext.Astronomy)]);
        public static readonly NonSIUnit Are              = new(100,                SquareMetre, [new("Are", ["dam²"], 1, UnitContext.All)]);
        public static readonly NonSIUnit Hectare          = new(10000,              SquareMetre, [new("Hectare", ["ha", "hm²"], 1, UnitContext.All)]);
        public static readonly NonSIUnit Litre            = new(0.001M,             CubicMetre,  CreateStandardUnitMultiples("Litre", "l"));
        public static readonly NonSIUnit Tonne            = new(1000,               Kilogram,    [new("Tonne", ["t"], 1, UnitContext.All)]);
        public static readonly NonSIUnit Dalton           = new(1.66053906660e-27M, Kilogram,    [new("Dalton", ["Da"], 1, UnitContext.All)]);
        public static readonly NonSIUnit ElectronVolt     = new(1.602176634e-19M,   Joule,       [new("Electronvolt", ["eV"], 1, UnitContext.All)]);

        // SI "others"
        public static readonly NonSIUnit KilometrePerHour = new(Fraction(1000, 3600), MetrePerSecond, [new("Kilometre per hour", ["km/h"], 1, UnitContext.All)]);

        // Imperial units
        public static readonly ImperialUnit Foot = new(0.3048M, Metre,
            [
                new("Thou", ["th", "mil"], Fraction(1, 12000), UnitContext.All),
                new("Inch", ["\"", "in"],  Fraction(1, 12),    UnitContext.All),
                new("Hand", ["hh"],        Fraction(1, 3),     UnitContext.All),
                new("Foot", ["ft"],        1,                  UnitContext.All),
                new("Yard", ["yd", "'"],   36,                 UnitContext.All),
                new("Mile", ["mi"],        5280,               UnitContext.All),
            ]);

        public static readonly ImperialUnit NauticalMile = new(1852M, Metre, [new("Nautical mile", ["NM", "nmi", "M"], 1, UnitContext.Nautical)]);
        public static readonly ImperialUnit MilePerHour = new(0.44704M, MetrePerSecond, [new("Mile per hour", ["mph", "mi/h"], 1, UnitContext.All)]);
        public static readonly ImperialUnit Knot = new(0.514444M, MetrePerSecond, [new("Knot", ["kn", "kt"], 1, UnitContext.All)]);
        public static readonly ImperialUnit FootPerSecond = new(0.3048M, MetrePerSecond, [new("Foot per second", ["ft/s", "fps"], 1, UnitContext.All)]);

        public static readonly ImperialUnit SquareFoot = new(0.09290304M, SquareMetre,
            [
                new("Square inch", ["in²", "sq in"], Fraction(1, 144), UnitContext.All),
                new("Square foot", ["ft²", "sq ft"], 1,                UnitContext.All),
                new("Square yard", ["yd²", "sq yd"], 9,                UnitContext.All),
            ]);

        public static readonly ImperialUnit Pound = new(0.45359237M, Kilogram,
            [
                new("Grain",         ["gr"],        Fraction(1, 7000), UnitContext.All),
                new("Drachm",        ["dr"],        Fraction(1, 256),  UnitContext.All),
                new("Ounce",         ["oz"],        Fraction(1, 16),   UnitContext.All),
                new("Pound",         ["lb"],        1,                 UnitContext.All),
                new("Stone",         ["st"],        14,                UnitContext.All),
                new("Slug",          ["slug"],      32.17404856M,      UnitContext.Engineering),
                new("Quarter",       ["qr", "qrt"], 28,                UnitContext.All),
                new("Hundredweight", ["cwt"],       112,               UnitContext.All),
                new("Ton",           ["it"],        2240,              UnitContext.All), // Custom symbol, really is "t"
            ]);

        // Digital units
        public static readonly SIBaseUnit Bit = new(DataSize,
            [
                new("Bit",     ["b", "bit"],    1,                      UnitContext.Programming),
                //new("Kilobit", ["kbit"],        BigInteger.Pow(10, 3),  UnitContext.Programming),
                //new("Kilobit", ["Kbit", "Kb"],  BigInteger.Pow(2,  10), UnitContext.Programming),
                new("Kibibit", ["kibit"],       BigInteger.Pow(2,  10), UnitContext.Programming),
                //new("Megabit", ["Mbit"],        BigInteger.Pow(10, 3),  UnitContext.Programming),
                //new("Megabit", ["Mbit", "Kb"],  BigInteger.Pow(2,  10), UnitContext.Programming),
                new("Mebibit", ["Mibit"],       BigInteger.Pow(2,  20), UnitContext.Programming),
            ]);

        public static readonly NonSIUnit Byte = new(8, Bit,
            [
                new("Byte",     ["B"],   1,                    UnitContext.Programming),
                new("Kibibyte", ["KiB"], BigInteger.One << 10, UnitContext.Programming),
                new("Kilobyte", ["KB"],  BigInteger.One << 10, UnitContext.Programming),
                new("Mebibyte", ["MiB"], BigInteger.One << 20, UnitContext.Programming),
                new("Megabyte", ["MB"],  BigInteger.One << 20, UnitContext.Programming),
                new("Gibibyte", ["GiB"], BigInteger.One << 30, UnitContext.Programming),
                new("Gigabyte", ["GB"],  BigInteger.One << 30, UnitContext.Programming),
                new("Tebibyte", ["TiB"], BigInteger.One << 40, UnitContext.Programming),
                new("Terabyte", ["TB"],  BigInteger.One << 40, UnitContext.Programming),
                new("Pebibyte", ["PiB"], BigInteger.One << 50, UnitContext.Programming),
                new("Petabyte", ["PB"],  BigInteger.One << 50, UnitContext.Programming)
            ]);

        public static readonly SIDerivedUnit BitsPerSecond = new(DataRate,
            [Bit, Second, Division],
            [
                new("Bits per second",     ["b/s"],   1,                    UnitContext.Programming),
                new("Kilobits per second", ["Kib/s"], BigInteger.One << 10, UnitContext.Programming),
                new("Megabits per second", ["Mib/s"], BigInteger.One << 20, UnitContext.Programming),
            ]);

        public static readonly NonSIUnit BytesPerSecond = new(8, BitsPerSecond,
            [
                new("Bytes per second",     ["B/s"],   1,                    UnitContext.Programming),
                new("Kilobytes per second", ["KiB/s"], BigInteger.One << 10, UnitContext.Programming),
                new("Megabytes per second", ["MiB/s"], BigInteger.One << 20, UnitContext.Programming),
            ]);

        // Constants
        public static readonly Constant Pi = new("Pi", ["π", "pi"], 3.14159265358979323846M);
        public static readonly PhysicsConstant SpeedOfLight = new("Speed of light in vacuum", ["c"], 299792458.0M, MetrePerSecond);
        public static readonly Constant GravitationConstant = new("Newtonian constant of gravitation", ["G"], new Number(6.6743e-11M, isScientificNotation: true)); // TODO Make it physics contant with unit: N * m^2 * kg^−2

        // Mathematical formulas
        public static readonly Formula AreaOfSquare = new("Area of square", [PhyVar(Metre, "Side", "a"), Two, Exponent], SquareMetre);
        public static readonly Formula AreaOfSquareMetre = new("Area of square", [PhyVar(Metre, "Side", "a"), Two, Exponent], SquareMetre);
        public static readonly Formula AreaOfRectangle = new("Area of rectangle", [PhyVar(Metre, "Length", "l"), PhyVar(Metre, "Width", "w"), Multiplication], SquareMetre);
        public static readonly Formula AreaOfTriangle = new("Area of triangle", [PhyVar(Metre, "Base", "b"), PhyVar(Metre, "Height", "h"), Multiplication, Two, Division], SquareMetre);
        public static readonly Formula AreaOfCircle = new("Area of circle", [Pi, PhyVar(Metre, "Radius", "r"), Two, Exponent, Multiplication], SquareMetre);
        public static readonly Formula AreaOfSphere = new("Area of sphere", [Four, Pi, PhyVar(Metre, "Radius", "r"), Two, Exponent, Multiplication, Multiplication], SquareMetre);
        public static readonly Formula VolumeOfCube = new("Volume of cube", [PhyVar(Metre, "Side", "a"), Three, Exponent], CubicMetre);
        public static readonly Formula VolumeOfBoxLWH = new("Volume of box by three sides", [PhyVar(Metre, "Length", "l"), PhyVar(Metre, "Width", "w"), PhyVar(Metre, "Height", "h"), Multiplication, Multiplication], CubicMetre);
        public static readonly Formula VolumeOfBoxAH = new("Volume of box by area and height", [PhyVar(SquareMetre, "Area", "a"), PhyVar(Metre, "Height", "h"), Multiplication], CubicMetre);

        // Physics formulas
        public static readonly Formula TravelDistance = new("Travel distance", [PhyVar(MetrePerSecond), PhyVar(Second), Multiplication], Metre);
        public static readonly Formula TravelSpeed = new("Travel speed", [PhyVar(Metre, "Distance", "d"), PhyVar(Second), Division], MetrePerSecond);
        public static readonly Formula TravelTime = new("Travel speed", [PhyVar(Metre, "Distance", "d"), PhyVar(MetrePerSecond), Division], Second);
        public static readonly Formula AccelerationFormula = new("Acceleration", [PhyVar(MetrePerSecond), PhyVar(Second), Division], MetrePerSecondSquared);

        // Electrical formulas
        public static readonly Formula OhmsLaw = new("Ohm's law", [PhyVar(Volt, "Volt", "v"), PhyVar(Ohm, "Resitance", "r"), Division], Ampere);

        // Digital formulas
        public static readonly Formula BitrateByAmountAndTime = new("Datarate by data amount and time", [PhyVar(Bit), PhyVar(Second), Division], BitsPerSecond);

        /// <summary>
        /// All operators
        /// </summary>
        public static readonly IOperator[] Operators =
            [Negation, Addition, Division, Exponent, Multiplication, Quotient, Reminder, Subtraction, Inverse, And, Or, Xor, LeftShift, RightShift, UnitConversion];

        /// <summary>
        /// All functions
        /// </summary>
        public static readonly IFunction[] Functions =
            [Sin, Cos, Tan, Asin, Acos, Atan, Atan2, Sinh, Cosh, Tanh, Ln, Log2, Log10,
            Abs, Sign, Ceil, Floor, Trunc, Round, Min, Max, Sqrt, Cbrt];

        /// <summary>
        /// All constants
        /// </summary>
        public static readonly IConstant[] Constants = [Pi, SpeedOfLight, GravitationConstant];

        /// <summary>
        /// All quantities
        /// </summary>
        public static readonly IQuantity[] Quantities =
            [Time, Length, Speed, Mass, Density, Acceleration, Area, Volume, Force, Frequency, Energy,
            ThermodynamicTemperature, AmountOfSubstance, LuminousIntensity,
            Voltage, Current, Resistance, Conductance, Capacitance, Inductance, Impedance, Charge, Power,
            DataSize, DataRate];

        /// <summary>
        /// All units
        /// </summary>
        public static readonly IUnit[] Units =
            [Second, Metre, Kilogram, Ampere, Kelvin, Mole, Candela,
            MetrePerSecond, SquareMetre, CubicMetre, Newton, Joule,
            Minute, Hour, Day, JulianYear, AstronomicalUnit, Are, Hectare, Litre, Tonne, Dalton, ElectronVolt,
            KilometrePerHour,
            Foot, NauticalMile, MilePerHour, Knot, FootPerSecond, SquareFoot, Pound,
            Ohm, Volt,
            Bit, Byte, BitsPerSecond, BytesPerSecond];

        /// <summary>
        /// All formulas
        /// </summary>
        public static readonly IFormula[] Formulas =
            [AreaOfSquare, AreaOfRectangle, AreaOfTriangle, AreaOfCircle, AreaOfSphere,
            VolumeOfCube, VolumeOfBoxLWH, VolumeOfBoxAH,
            TravelDistance, TravelSpeed, TravelTime, AccelerationFormula,
            OhmsLaw,
            BitrateByAmountAndTime];

        /// <summary>
        /// Calculate fraction
        /// </summary>
        /// <param name="dividend">Dividend</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Fraction</returns>
        public static NumberType Fraction(NumberType dividend, NumberType divisor)
        {
            return dividend / divisor;
        }

        /// <summary>
        /// Utility function to create physics variable
        /// </summary>
        /// <param name="unit">SI unit</param>
        /// <param name="name">Name</param>
        /// <param name="symbol">Symbol</param>
        /// <returns>Physics variable</returns>
        public static PhysicsVariable PhyVar(ISIUnit unit, string name, params string[] symbols)
        {
            return new PhysicsVariable(name, symbols, unit);
        }

        /// <summary>
        /// Utility function to create physics variable
        /// </summary>
        /// <param name="unit">SI unit</param>
        /// <returns>Physics variable</returns>
        public static PhysicsVariable PhyVar(ISIUnit unit)
        {
            return new PhysicsVariable(unit.Quantity.Name, unit.Quantity.Symbols, unit);
        }

        /// <summary>
        /// Create SI standard unit multiples
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="exponentOffset">Exponent offset</param>
        /// <returns></returns>
        public static UnitMultiple[] CreateStandardUnitMultiples(string name, string symbol, int exponentOffset = 0)
        {
            return SIStandardUnitMultiples
                .Select(m => new UnitMultiple(
                    name: m.Name + name.ToLower(),
                    symbols: [m.Symbol + symbol],
                    factor: Exponent.Calculate(10, m.Exponent + exponentOffset),
                    context: UnitContext.All,
                    useForDisplay: m.UseForDisplay))
                .ToArray();
        }
    }
}
