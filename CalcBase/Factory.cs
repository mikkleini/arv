using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Functions;
using CalcBase.Functions.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators;
using CalcBase.Operators.Arithmetic;
using CalcBase.Operators.Bitwise;
using CalcBase.Quantities;
using CalcBase.Units;
using System.Numerics;

#pragma warning disable CS8604 // Possible null reference argument. Suppress it in unit parent initialization.

namespace CalcBase
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
            new("Centi",  "c",  -2, false),
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
        public static readonly DualArgumentFunction Atan2 = new("Two argument arctangent", "atan2", (y, x) => NumberType.Atan2(y, x));
        public static readonly SingleArgumentFunction Sinh = new("Hyperbolic sine", "sinh", NumberType.Sinh);
        public static readonly SingleArgumentFunction Cosh = new("Hyperbolic cosine", "cosh", NumberType.Cosh);
        public static readonly SingleArgumentFunction Tanh = new("Hyperbolic tangent", "tanh", NumberType.Tanh);

        public static readonly SingleArgumentFunction Ln = new("Natural (base-E) logarithm", "ln", x => NumberType.Log(x));
        public static readonly SingleArgumentFunction Log2 = new("Base-2 logarithm", "log2", x => NumberType.Log2(x));
        public static readonly SingleArgumentFunction Log10 = new("Base-10 logarithm", "log10", x => NumberType.Log10(x));

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
        public static readonly Quantity DataSize = new("Data size", [""]);
        public static readonly Quantity DataRate = new("Datarate", [""]);

        // SI base units
        public static readonly SIBaseUnit Second = new(Time,
            [
                ..CreateStandardUnitMultiples(Second, "Second", "s"),
                new(Second, "Minute", ["min"], 60, UnitContext.All),
                new(Second, "Hour", ["h"], 3600, UnitContext.All),
                new(Second, "Day", ["d"], 24 * 3600, UnitContext.All)
            ]);

        public static readonly SIBaseUnit Metre = new(Length, CreateStandardUnitMultiples(Metre, "Metre", "m"));

        public static readonly SIBaseUnit Kilogram = new(Mass,
            [
                ..CreateStandardUnitMultiples(Kilogram, "Gram", "g", -3),
                new(Kilogram, "Tonne", ["t"], 1e+3M, UnitContext.All)
            ]);

        public static readonly SIBaseUnit Ampere = new(Current, CreateStandardUnitMultiples(Ampere, "Ampere", "A"));

        // SI derived units
        public static readonly SIDerivedUnit MetrePerSecond = new(Speed,
            [Metre, Second, Division],
            [
                new(MetrePerSecond, "Micrometre per second", ["µm/s"], 1e-6M, UnitContext.All),
                new(MetrePerSecond, "Millimetre per second", ["mm/s"], 1e-3M, UnitContext.All),
                new(MetrePerSecond, "Metre per second",      ["m/s"],  1,     UnitContext.All),
                new(MetrePerSecond, "Kilometre per hour",    ["km/h"], Division.Calculate(1, 3.6M), UnitContext.All),
                new(MetrePerSecond, "Kilometre per second",  ["km/s"], 1e+3M, UnitContext.All)
            ]);

        public static readonly SIDerivedUnit MetrePerSecondSquared = new(Acceleration,
            [Metre, Second, Two, Exponent, Division],
            [
                new(MetrePerSecondSquared, "Micrometre per second squared", ["µm/s²"], 1e-6M, UnitContext.All),
                new(MetrePerSecondSquared, "Millimetre per second squared", ["mm/s²"], 1e-3M, UnitContext.All),
                new(MetrePerSecondSquared, "Metre per second squared",      ["m/s²"],  1,     UnitContext.All),
                new(MetrePerSecondSquared, "Kilometre per second squared",  ["km/s²"], 1e+3M, UnitContext.All)
            ]);

        public static readonly SIDerivedUnit SquareMetre = new(Area,
            [Metre, Two, Exponent],
            [
                new(SquareMetre, "Square micrometre", ["µm²"],  1e-12M, UnitContext.All),
                new(SquareMetre, "Square millimetre", ["mm²"],  1e-6M,  UnitContext.All),
                new(SquareMetre, "Square centimetre", ["cm²"],  1e-4M,  UnitContext.All),
                new(SquareMetre, "Square decimetre",  ["dm²"],  1e-2M,  UnitContext.All),
                new(SquareMetre, "Square metre",      ["m²"],   1,      UnitContext.All),
                new(SquareMetre, "Are",               ["dam²"], 1e+2M,  UnitContext.All),
                new(SquareMetre, "Hectare",           ["hm²"],  1e+4M,  UnitContext.All),
                new(SquareMetre, "Square kilometre",  ["km²"],  1e+6M,  UnitContext.All)
            ]);

        public static readonly SIDerivedUnit Litre = new(Volume,
            [new Number(1e-3M), Metre, Three, Exponent, Multiplication],
            CreateStandardUnitMultiples(Litre, "Litre", "l"));

        public static readonly SIDerivedUnit Newton = new(Force,
            [Kilogram, Metre, Time, MinusTwo, Exponent, Multiplication, Multiplication],
            CreateStandardUnitMultiples(Newton, "Newton", "N"));

        // TODO Equation in SI base units: kg⋅m2⋅s−3⋅A−1 */
        public static readonly SIDerivedUnit Volt = new(Voltage, [], CreateStandardUnitMultiples(Volt, "Volt", "V"));

        // TODO Equation in SI base units: kg⋅m2⋅s−3⋅A−2 */
        public static readonly SIDerivedUnit Ohm = new(Resistance, [], CreateStandardUnitMultiples(Ohm, "Ohm", "Ω"));

        // Imperial units
        public static readonly ImperialUnit Foot = new(0.3048M, Metre,
            [
                new(Foot, "Thou", ["th", "mil"], Division.Calculate(1, 12000), UnitContext.All),
                new(Foot, "Inch", ["\"", "in"],  Division.Calculate(1, 12),    UnitContext.All),
                new(Foot, "Hand", ["hh"],        Division.Calculate(1, 3),     UnitContext.All),
                new(Foot, "Foot", ["ft"],        1,                            UnitContext.All),
                new(Foot, "Yard", ["yd", "'"],   36,                           UnitContext.All),
                new(Foot, "Mile", ["mi"],        5280,                         UnitContext.All),
            ]);

        public static readonly ImperialUnit NauticalMile = new(1852M, Metre,
            [
                new(NauticalMile, "Nautical mile", ["NM", "nmi", "M"], 1, UnitContext.Nautical)
            ]);

        public static readonly ImperialUnit MilePerHour = new(0.44704M, MetrePerSecond,
            [
                new(MilePerHour, "Mile per hour", ["mph"], 1, UnitContext.All)
            ]);

        public static readonly ImperialUnit SquareInch = new(0.00064516M, SquareMetre,
            [
                new(SquareInch, "Square inch", ["inch²", "in²", "sq in"], 1, UnitContext.All)
            ]);

        // Digital units
        public static readonly SIBaseUnit Bit = new(DataSize,
            [
                new(Bit, "Bit",     ["b", "bit"],    1,                      UnitContext.Programming),
                //new("Kilobit", ["kbit"],        BigInteger.Pow(10, 3),  UnitContext.Programming),
                //new("Kilobit", ["Kbit", "Kb"],  BigInteger.Pow(2,  10), UnitContext.Programming),
                new(Bit, "Kibibit", ["kibit"],       BigInteger.Pow(2,  10), UnitContext.Programming),
                //new("Megabit", ["Mbit"],        BigInteger.Pow(10, 3),  UnitContext.Programming),
                //new("Megabit", ["Mbit", "Kb"],  BigInteger.Pow(2,  10), UnitContext.Programming),
                new(Bit, "Mebibit", ["Mibit"],       BigInteger.Pow(2,  20), UnitContext.Programming),
            ]);

        public static readonly SIBaseUnit Byte = new(DataSize,
            [
                new(Byte, "Byte",     ["B"],   1,                    UnitContext.Programming),
                new(Byte, "Kibibyte", ["KiB"], BigInteger.One << 10, UnitContext.Programming),
                new(Byte, "Kilobyte", ["KB"],  BigInteger.One << 10, UnitContext.Programming),
                new(Byte, "Mebibyte", ["MiB"], BigInteger.One << 20, UnitContext.Programming),
                new(Byte, "Megabyte", ["MB"],  BigInteger.One << 20, UnitContext.Programming),
                new(Byte, "Gibibyte", ["GiB"], BigInteger.One << 30, UnitContext.Programming),
                new(Byte, "Gigabyte", ["GB"],  BigInteger.One << 30, UnitContext.Programming),
                new(Byte, "Tebibyte", ["TiB"], BigInteger.One << 40, UnitContext.Programming),
                new(Byte, "Terabyte", ["TB"],  BigInteger.One << 40, UnitContext.Programming),
                new(Byte, "Pebibyte", ["PiB"], BigInteger.One << 50, UnitContext.Programming),
                new(Byte, "Petabyte", ["PB"],  BigInteger.One << 50, UnitContext.Programming)
            ]);

        public static readonly SIDerivedUnit BytesPerSecond = new(DataRate,
            [Byte, Second, Division],
            [
                new(BytesPerSecond, "Bytes per second",     ["B/s"],   1,                    UnitContext.Programming),
                new(BytesPerSecond, "Kilobytes per second", ["KiB/s"], BigInteger.One << 10, UnitContext.Programming),
                new(BytesPerSecond, "Megabytes per second", ["MiB/s"], BigInteger.One << 20, UnitContext.Programming),
            ]);

        // Constants
        public static readonly Constant Pi = new("Pi", ["π", "pi"], 3.14159265358979323846M);
        public static readonly PhysicsConstant SpeedOfLight = new("Speed of light in vacuum", ["c"], 299792458.0M, MetrePerSecond);
        public static readonly Constant GravitationConstant = new("Newtonian constant of gravitation", ["G"], new Number(6.6743e-11M, isScientificNotation: true)); // TODO Make it physics contant with unit: N * m^2 * kg^−2

        // Mathematical formulas
        public static readonly Formula AreaOfSquare = new("Area of square", [PhyVar(Length, "Side", "a"), Two, Exponent], Area);
        public static readonly Formula AreaOfRectangle = new("Area of rectangle", [PhyVar(Length, "Length", "l"), PhyVar(Length, "Width", "w"), Multiplication], Area);
        public static readonly Formula AreaOfTriangle = new("Area of triangle", [PhyVar(Length, "Base", "b"), PhyVar(Length, "Height", "h"), Multiplication, Two, Division], Area);
        public static readonly Formula AreaOfCircle = new("Area of circle", [Pi, PhyVar(Length, "Radius", "r"), Two, Exponent, Multiplication], Area);
        public static readonly Formula AreaOfSphere = new("Area of sphere", [Four, Pi, PhyVar(Length, "Radius", "r"), Two, Exponent, Multiplication, Multiplication], Area);
        public static readonly Formula VolumeOfCube = new("Volume of cube", [PhyVar(Length, "Side", "a"), Three, Exponent], Volume);
        public static readonly Formula VolumeOfBox = new("Volume of box", [PhyVar(Length, "Length", "l"), PhyVar(Length, "Width", "w"), PhyVar(Length, "Height", "h"), Multiplication, Multiplication], Volume);

        // Physics formulas
        public static readonly Formula AccelerationFormula = new("Acceleration", [PhyVar(Speed), PhyVar(Time), Division], Acceleration);
        public static readonly Formula TravelDistance = new("Travel distance", [PhyVar(Speed), PhyVar(Time), Multiplication], Length);
        public static readonly Formula TravelSpeed = new("Travel speed", [PhyVar(Length, "Distance", "d"), PhyVar(Time), Division], Speed);

        // Electrical formulas
        public static readonly Formula OhmsLaw = new("Ohm's law", [PhyVar(Voltage), PhyVar(Resistance), Division], Current);

        // Digital formulas
        public static readonly Formula DataAmountByDataRate = new("Data amount by datarate and time", [PhyVar(DataRate), PhyVar(Time), Multiplication], DataSize);

        /// <summary>
        /// All operators
        /// </summary>
        public static readonly IOperator[] Operators =
            [Negation, Addition, Division, Exponent, Multiplication, Quotient, Reminder, Subtraction, Inverse, And, Or, Xor, LeftShift, RightShift];

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
            [Time, Length, Speed, Mass, Density, Acceleration, Area, Volume, Force, Frequency,
            Voltage, Current, Resistance, Conductance, Capacitance, Inductance, Impedance, Charge, Power,
            DataSize, DataRate];

        /// <summary>
        /// All units
        /// </summary>
        public static readonly IUnit[] Units =
            [Second, Metre, Kilogram, MetrePerSecond, SquareMetre, Newton,
            Foot, NauticalMile, MilePerHour,
            Ampere, Ohm, Volt,
            Bit, Byte, BytesPerSecond];

        /// <summary>
        /// All formulas
        /// </summary>
        public static readonly IFormula[] Formulas =
            [AreaOfSquare, AreaOfRectangle, AreaOfTriangle, AreaOfCircle, AreaOfSphere,
            VolumeOfCube, VolumeOfBox,
            AccelerationFormula, TravelDistance, TravelSpeed,
            OhmsLaw,
            DataAmountByDataRate];
        
        /// <summary>
        /// Enumerate constants by their symbols starting from longest, ending with shorters
        /// </summary>
        public static IEnumerable<(string symbol, IConstant constant)> ConstantsBySymbols => Constants
            .SelectMany(c => c.Symbols.Select(s => (symbol: s, constant: c)))
            .OrderByDescending(x => x.symbol.Length);

        /// <summary>
        /// Enumerate units by their multiples symbols from longest, ending with shorters
        /// </summary>
        public static IEnumerable<(string symbol, UnitMultiple unit)> UnitsBySymbols => Units
            .SelectMany(u => u.Multiples.Select(m => (m.Symbols, unit: m)))
            .SelectMany(m => m.Symbols.Select(s => (symbol: s, m.unit)))
            .OrderByDescending(x => x.symbol.Length);

        /// <summary>
        /// Utility function to create physics variable
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="name">Name</param>
        /// <param name="symbol">Symbol</param>
        /// <returns>Physics variable</returns>
        public static PhysicsVariable PhyVar(IQuantity quantity, string name, string symbol)
        {
            return new PhysicsVariable(name, [symbol], quantity);
        }

        /// <summary>
        /// Utility function to create physics variable
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <returns>Physics variable</returns>
        public static PhysicsVariable PhyVar(IQuantity quantity)
        {
            return new PhysicsVariable(quantity.Name, quantity.Symbols, quantity);
        }

        /// <summary>
        /// Create SI standard unit multiples
        /// </summary>
        /// <param name="parent">Parent unit</param>
        /// <param name="name">Name</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="exponentOffset">Exponent offset</param>
        /// <returns></returns>
        public static UnitMultiple[] CreateStandardUnitMultiples(IUnit? parent, string name, string symbol, int exponentOffset = 0)
        {
            return SIStandardUnitMultiples
                .Select(m => new UnitMultiple(parent, (m.Name + name).ToLower(), [m.Symbol + symbol], Exponent.Calculate(10, m.Exponent + exponentOffset), UnitContext.All))
                .ToArray();
        }
    }
}
