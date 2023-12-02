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
        public static readonly Quantity Current = new("Current", ["I"]);
        public static readonly Quantity Resistance = new("Resistance", ["R"]);
        public static readonly Quantity Conductance = new("Conductance", ["G"]);
        public static readonly Quantity Capacitance = new("Capacitance", ["C"]);
        public static readonly Quantity Inductance = new("Inductance", ["L"]);
        public static readonly Quantity Impedance = new("Impedance", ["Z"]);
        public static readonly Quantity Charge = new("Charge", ["C"]);
        public static readonly Quantity Power = new("Power", ["P"]);

        // Digital quantities
        public static readonly Quantity Digital = new("Digital quantity", [""]);

        // SI base units
        public static readonly SIBaseUnit Second = new("Second", ["s"], Time,
            [("as", "attosecond", 1e-18M),
            ("fs", "femtosecond", 1e-15M),
            ("ps", "picosecond", 1e-12M),
            ("ns", "nanosecond", 1e-9M),
            ("µs", "microsecond", 1e-6M),
            ("ms", "millisecond", 1e-3M),
            ("m", "minute", 60M),
            ("h", "hour", 3600M),
            ("d", "day", 3600M * 24M)]);

        public static readonly SIBaseUnit Metre = new("Metre", ["m"], Length,
            [("fm", "femtometre", 1e-15M),
            ("pm", "picometre", 1e-12M),
            ("nm", "nanometre", 1e-9M),
            ("µm", "micrometre", 1e-6M),
            ("mm", "millimetre", 1e-3M),
            ("cm", "centimetre", 1e-2M),
            ("dm", "decimetre", 1e-1M),
            ("km", "kilometre", 1e+3M)]);

        public static readonly SIBaseUnit Kilogram = new("Kilogram", ["kg"], Mass,
            [("ng", "nanogram", 1e-9M),
             ("mg", "milligram", 1e-6M),
             ("g", "gram", 1e-3M),
             ("t", "tonne", 1e+3M)]);

        // SI derived units
        public static readonly SIDerivedUnit MetrePerSecond = new("Metre per second", ["m/s"], Speed,
            [Metre, Second, Division],
            [(["µm/s"], "micrometre per second", 1e-6M),
            (["mm/s"], "millimetre per second", 1e-3M),
            (["km/s"], "kilometre per second", 1e+3M),
            (["km/h"], "kilometre per hour", 3.6M)]);

        public static readonly SIDerivedUnit MetrePerSecondSquared = new("Metre per second squared", ["m/s^2", "m/s²"], Acceleration,
            [Metre, Second, Two, Exponent, Division],
            [(["µm/s²"], "micrometre per second squared", 1e-6M),
            (["mm/s²"], "millimetre per second squared", 1e-3M),
            (["km/s²"], "kilometre per second squared", 1e+3M)]);

        public static readonly SIDerivedUnit SquareMetre = new("Square metre", ["m^2"], Speed,
            [Metre, Two, Exponent],
            [(["µm²"], "square micrometre", 1e-12M),
            (["mm²"], "square millimetre", 1e-6M),
            (["cm²"], "square centimetre", 1e-4M),
            (["dm²"], "square decimetre", 1e-2M),
            (["dam²"], "are", 1e+2M),
            (["hm²"], "hectare", 1e+4M),
            (["km²"], "square kilometre", 1e+6M)]);

        public static readonly SIDerivedUnit Litre = new("Litre", ["l", "L"], Volume,
            [new Number(1e-3M), Metre, Three, Exponent, Multiplication]);

        public static readonly SIDerivedUnit Newton = new("Newton", ["N"], Force,
            [Kilogram, Metre, Time, MinusTwo, Exponent, Multiplication, Multiplication]);

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
            [("KiB", "kibibyte", 1 << 10),
             ("KB",  "kilobyte", 1 << 10),
             ("MiB", "mebibyte", 1 << 20),
             ("MB",  "megabyte", 1 << 20),
             ("GiB", "gibibyte", 1 << 30),
             ("GB",  "gigabyte", 1 << 30),
             ("TiB", "gibibyte", 1 << 40),
             ("TB",  "terabyte", 1 << 40),
             ("PiB", "pebibyte", 1 << 50),
             ("PB",  "petabyte", 1 << 50)]);

        // Constants
        public static readonly Constant Pi = new("Pi", ["π", "pi"], 3.14159265358979323846M);
        public static readonly PhysicsConstant SpeedOfLight = new("Speed of light in vacuum", ["c"], 299792458.0M, MetrePerSecond);
        public static readonly Constant GravitationConstant = new("Newtonian constant of gravitation", ["G"], new Number(6.6743e-11M, isScientificNotation: true)); // TODO Make it physics contant with unit: N * m^2 * kg^−2

        // Mathematical formulas
        public static readonly Formula AreaOfSquare = new("Area of square", [PhyVar("Side", "a", Length), Two, Exponent], Area);
        public static readonly Formula AreaOfRectangle = new("Area of rectangle", [PhyVar("Length", "l", Length), PhyVar("Width", "w", Length), Multiplication], Area);
        public static readonly Formula AreaOfTriangle = new("Area of triangle", [PhyVar("Base", "b", Length), PhyVar("Height", "h", Length), Multiplication, Two, Division], Area);
        public static readonly Formula AreaOfCircle = new("Area of circle", [Pi, PhyVar("Radius", "r", Length), Two, Exponent, Multiplication], Area);
        public static readonly Formula AreaOfSphere = new("Area of sphere", [Four, Pi, PhyVar("Radius", "r", Length), Two, Exponent, Multiplication, Multiplication], Area);
        public static readonly Formula VolumeOfCube = new("Volume of cube", [PhyVar("Side", "a", Length), Three, Exponent], Volume);
        public static readonly Formula VolumeOfBox = new("Volume of box", [PhyVar("Length", "l", Length), PhyVar("Width", "w", Length), PhyVar("Height", "h", Length), Multiplication, Multiplication], Volume);

        // Physics formulas
        public static readonly Formula AccelerationFormula = new("Acceleration", [PhyVar("Speed", "v", Speed), PhyVar("Time", "t", Time), Division], Acceleration);
        public static readonly Formula TravelDistance = new("Travel distance", [PhyVar("Speed", "v", Speed), PhyVar("Time", "t", Time), Multiplication], Length);
        public static readonly Formula TravelSpeed = new("Travel speed", [PhyVar("Distance", "d", Length), PhyVar("Time", "t", Time), Division], Speed);

        /// <summary>
        /// All operators
        /// </summary>
        public static readonly IOperator[] Operators =
            [Negation, Addition, Division, Exponent, Multiplication, Quotient, Reminder, Subtraction, Inverse, And, Or, Xor];

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
            Bit, Byte];

        /// <summary>
        /// All formulas
        /// </summary>
        public static readonly IFormula[] Formulas =
            [AreaOfSquare, AreaOfRectangle, AreaOfTriangle, AreaOfCircle, AreaOfSphere,
            VolumeOfCube, VolumeOfBox,
            AccelerationFormula, TravelDistance, TravelSpeed];
        
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
        /// Utility function to create physics variable
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Physics variable</returns>
        public static PhysicsVariable PhyVar(string name, string symbol, IQuantity quantity)
        {
            return new PhysicsVariable(name, [symbol], quantity);
        }
    }
}
