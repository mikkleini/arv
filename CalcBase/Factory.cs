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
        public static readonly (string Symbol, string Name, int Exponent, bool UseForDisplay)[] SIStandardUnitMultiples =
        [
            new("q", "quecto", -30, true),
            new("r", "ronto", -27, true),
            new("y", "yocto", -24, true),
            new("z", "zepto", -21, true),
            new("a", "atto", -18, true),
            new("f", "femto", -15, true),
            new("p", "pico", -12, true),
            new("n", "nano", -9, true),
            new("µ", "micro", -6, true),
            new("m", "milli", -3, true),
            new("c", "centi", -2, false),
            new("", "", 0, true),
            new("d", "deci", -1, false),
            new("da", "deca", 1, false),
            new("ha", "hecto", 2, false),
            new("k", "kilo", 3, true),
            new("M", "mega", 6, true),
            new("G", "giga", 9, true),
            new("T", "tera", 12, true),
            new("P", "peta", 15, true),
            new("E", "exa", 18, true),
            new("Z", "zetta", 21, true),
            new("Y", "yotta", 24, true),
            new("R", "ronna", 27, true),
            new("Q", "quetta", 30, true),
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
        public static readonly SingleArgumentFunction Log10 = new("Base-10 logarithm", "log10", x=> NumberType.Log10(x));

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
        public static readonly Quantity Digital = new("Digital quantity", [""]);

        // SI base units
        public static readonly SIBaseUnit Second = new("Second", ["s"], Time,
            [
                ..CreateStandardUnitMultiples("s", "second"),
                new("m", "minute", 60, true),
                new("h", "hour", 3600, true),
                new("d", "day", 24 * 3600, true)
            ]);

        public static readonly SIBaseUnit Metre = new("Metre", ["m"], Length, CreateStandardUnitMultiples("m", "metre"));

        public static readonly SIBaseUnit Kilogram = new("Kilogram", ["kg"], Mass,
            [
                ..CreateStandardUnitMultiples("g", "gram", -3),
                new("t", "tonne", 1e+3M, true)
            ]);

        public static readonly SIBaseUnit Ampere = new("Ampere", ["A"], Current, CreateStandardUnitMultiples("A", "ampere"));

        // SI derived units
        public static readonly SIDerivedUnit MetrePerSecond = new("Metre per second", ["m/s"], Speed,
            [Metre, Second, Division],
            [
                new("µm/s", "micrometre per second", 1e-6M, true),
                new("mm/s", "millimetre per second", 1e-3M, true),
                new("km/h", "kilometre per hour", Division.Calculate(1, 3.6M), true),
                new("km/s", "kilometre per second", 1e+3M, true)
            ]);

        public static readonly SIDerivedUnit MetrePerSecondSquared = new("Metre per second squared", ["m/s^2", "m/s²"], Acceleration,
            [Metre, Second, Two, Exponent, Division],
            [
                new("µm/s²", "micrometre per second squared", 1e-6M, true),
                new("mm/s²", "millimetre per second squared", 1e-3M, true),
                new("km/s²", "kilometre per second squared", 1e+3M, true)
            ]);

        public static readonly SIDerivedUnit SquareMetre = new("Square metre", ["m^2"], Area,
            [Metre, Two, Exponent],
            [
                new("µm²", "square micrometre", 1e-12M, true),
                new("mm²", "square millimetre", 1e-6M, true),
                new("cm²", "square centimetre", 1e-4M, false),
                new("dm²", "square decimetre", 1e-2M, false),
                new("dam²", "are", 1e+2M, false),
                new("hm²", "hectare", 1e+4M, true),
                new("km²", "square kilometre", 1e+6M, true)
            ]);

        public static readonly SIDerivedUnit Litre = new("Litre", ["l", "L"], Volume,
            [new Number(1e-3M), Metre, Three, Exponent, Multiplication]);

        public static readonly SIDerivedUnit Newton = new("Newton", ["N"], Force,
            [Kilogram, Metre, Time, MinusTwo, Exponent, Multiplication, Multiplication]);

        // TODO Equation in SI base units: kg⋅m2⋅s−3⋅A−1 */
        public static readonly SIDerivedUnit Volt = new("Volt", ["V"], Voltage, [], CreateStandardUnitMultiples("V", "volt"));

        // TODO Equation in SI base units: kg⋅m2⋅s−3⋅A−2 */
        public static readonly SIDerivedUnit Ohm = new("Electric resistance", ["Ω", "ohm"], Resistance, [], CreateStandardUnitMultiples("Ω", "ohm"));

        // Imperial units
        public static readonly ImperialUnit Inch = new("Inch", ["\"", "in"], 0.0254M, Metre);
        public static readonly ImperialUnit Foot = new("Foot", ["'", "ft"], 0.3048M, Metre);
        public static readonly ImperialUnit Yard = new("Yard", ["yd"], 0.9144M, Metre);
        public static readonly ImperialUnit Mile = new("Mile", ["mi"], 1609.344M, Metre);
        public static readonly ImperialUnit NauticalMile = new("Nautical mile", ["NM", "nmi", "M"], 1852M, Metre);
        public static readonly ImperialUnit MilePerHour = new("Mile per hour", ["mph"], 0.44704M, MetrePerSecond);

        // Digital units
        public static readonly SIBaseUnit Bit = new("Bit", ["b"], Digital);
        public static readonly SIBaseUnit Byte = new("Byte", ["B"], Digital,
            [
                new("KiB", "kibibyte", 1 << 10, true),
                new("KB",  "kilobyte", 1 << 10, true),
                new("MiB", "mebibyte", 1 << 20, true),
                new("MB",  "megabyte", 1 << 20, true),
                new("GiB", "gibibyte", 1 << 30, true),
                new("GB",  "gigabyte", 1 << 30, true),
                new("TiB", "gibibyte", 1 << 40, true),
                new("TB",  "terabyte", 1 << 40, true),
                new("PiB", "pebibyte", 1 << 50, true),
                new("PB",  "petabyte", 1 << 50, true)
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
            Digital];

        /// <summary>
        /// All units
        /// </summary>
        public static readonly IUnit[] Units =
            [Second, Metre, Kilogram, MetrePerSecond, SquareMetre, Newton,
            Inch, Foot, Yard, Mile, NauticalMile, MilePerHour,
            Ampere, Ohm, Volt,
            Bit, Byte];

        /// <summary>
        /// All formulas
        /// </summary>
        public static readonly IFormula[] Formulas =
            [AreaOfSquare, AreaOfRectangle, AreaOfTriangle, AreaOfCircle, AreaOfSphere,
            VolumeOfCube, VolumeOfBox,
            AccelerationFormula, TravelDistance, TravelSpeed,
            OhmsLaw];
        
        /// <summary>
        /// Enumerate constants by their symbols starting from longest, ending with shorters
        /// </summary>
        public static IEnumerable<(string symbol, IConstant constant)> ConstantsBySymbols => Constants
            .SelectMany(c => c.Symbols.Select(s => (symbol: s, constant: c)))
            .OrderByDescending(x => x.symbol.Length);

        /// <summary>
        /// Enumerate units by their symbols starting from longest, ending with shorters
        /// </summary>
        public static IEnumerable<(string symbol, IUnit unit)> UnitsBySymbols => Units
            .SelectMany(u => u.Symbols.Select(s => (symbol: s, unit: u)))
            .OrderByDescending(x => x.symbol.Length);

        /// <summary>
        /// Enumerate SI units by their multiples symboles from longest, ending with shorters
        /// </summary>
        public static IEnumerable<(string symbol, ISIUnit unit, UnitMultiple multiple)> SIUnitMultiplesBySymbols => Units
            .Where(u => u is ISIUnit).Cast<ISIUnit>()
            .SelectMany(u => u.Multiples.Select(m => (symbol: m.Symbol, unit: u, multiple: m)))
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
        /// <param name="symbol">Symbol</param>
        /// <param name="name">Name</param>
        /// <param name="exponentOffset">Exponent offset</param>
        /// <returns></returns>
        public static UnitMultiple[] CreateStandardUnitMultiples(string symbol, string name, int exponentOffset = 0)
        {
            return SIStandardUnitMultiples
                .Select(m => new UnitMultiple(m.Symbol + symbol, m.Name + name, Exponent.Calculate(10, m.Exponent + exponentOffset), m.UseForDisplay))
                .ToArray();
        }
    }
}
