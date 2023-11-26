using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Functions;
using CalcBase.Functions.Mathematical;
using CalcBase.Functions.Trigonometric;
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
    public class Factory
    {
        // Numbers
        public static Number MinusFour => new(-4);
        public static Number MinusThree => new(-3);
        public static Number MinusTwo => new(-2);
        public static Number MinusOne => new(-1);
        public static Number Zero => new(0);
        public static Number Half => new(0.5M);
        public static Number One => new(1);
        public static Number Two => new(2);
        public static Number Three => new(3);
        public static Number Four => new(4);

        // Constants
        public static Constant Pi => new("Pi", ["π", "pi"], 3.14159265358979323846M);
        public static PhysicsConstant SpeedOfLight => new("Speed of light in vacuum", ["c"], 299792458.0M, MetrePerSecond);        
        public static Constant GravitationConstant => new("Newtonian constant of gravitation", ["G"], new Number(6.6743e-11M, isScientificNotation: true)); // TODO Make it physics contant with unit: N * m^2 * kg^−2

        // Functions
        public static RoundFunction Round => new();
        public static CosFunction Cos => new();
        public static SinFunction Sin => new();

        // Mathematical operators
        public static NegationOperator Negation => new();
        public static AdditionOperator Addition => new();
        public static SubtractionOperator Subtraction => new();
        public static MultiplicationOperator Multiplication => new();
        public static DivisionOperator Division => new();
        public static ExponentOperator Exponent => new();
        public static QuotientOperator Quotient => new();
        public static ReminderOperator Reminder => new();

        // Bitwise operators
        public static InverseOperator Inverse => new();
        public static AndOperator And => new();
        public static OrOperator Or => new();
        public static XorOperator Xor => new();        

        // Quantities
        public static Quantity Time => new("Time", ["t"]);
        public static Quantity Length => new("Length", ["l"]);
        public static Quantity Speed => new("Speed", ["v"]);
        public static Quantity Mass => new("Mass", ["m"]);
        public static Quantity Density => new("Density", ["ρ"]);
        public static Quantity Acceleration => new("Acceleration", ["a"]);
        public static Quantity Area => new("Area", ["A"]);
        public static Quantity Volume => new("Volume", ["V"]);
        public static Quantity Force => new("Force", ["F"]);
        public static Quantity Frequency => new("Frequency", ["f"]);

        // Electrical quantities
        public static Quantity Voltage => new("Voltage", ["V", "U"]);
        public static Quantity Current => new("Current", ["I"]);
        public static Quantity Resistance => new("Resistance", ["R"]);
        public static Quantity Conductance => new("Conductance", ["G"]);
        public static Quantity Capacitance => new("Capacitance", ["C"]);
        public static Quantity Inductance => new("Inductance", ["L"]);
        public static Quantity Impedance => new("Impedance", ["Z"]);
        public static Quantity Charge => new("Charge", ["C"]);
        public static Quantity Power => new("Power", ["P"]);

        // SI base units
        public static SIBaseUnit Second => new("Second", ["s"], Time,
            [("as", "attosecond", 1e-18M),
            ("fs", "femtosecond", 1e-15M),
            ("ps", "picosecond", 1e-12M),
            ("ns", "nanosecond", 1e-9M),
            ("µs", "microsecond", 1e-6M),
            ("ms", "millisecond", 1e-3M),
            ("m", "minute", 60M),
            ("h", "hour", 3600M),
            ("d", "day", 3600M * 24M)]);

        public static SIBaseUnit Metre => new("Metre", ["m"], Length,
            [("fm", "femtometre", 1e-15M),
            ("pm", "picometre", 1e-12M),
            ("nm", "nanometre", 1e-9M),
            ("µm", "micrometre", 1e-6M),
            ("mm", "millimetre", 1e-3M),
            ("cm", "centimetre", 1e-2M),
            ("dm", "decimetre", 1e-1M),
            ("km", "kilometre", 1e+3M)]);

        public static SIBaseUnit Kilogram => new("Kilogram", ["kg"], Mass,
            [("ng", "nanogram", 1e-9M),
             ("mg", "milligram", 1e-6M),
             ("g", "gram", 1e-3M),
             ("t", "tonne", 1e+3M)]);

        // SI derived units
        public static SIDerivedUnit MetrePerSecond => new("Metre per second", ["m/s"], Speed,
            [Metre, Second, Division],
            [(["µm/s"], "micrometre per second", 1e-6M),
            (["mm/s"], "millimetre per second", 1e-3M),
            (["km/s"], "kilometre per second", 1e+3M),
            (["km/h"], "kilometre per hour", 3.6M)]);

        public static SIDerivedUnit MetrePerSecondSquared => new("Metre per second squared", ["m/s^2", "m/s²"], Acceleration,
            [Metre, Second, Two, Exponent, Division],
            [(["µm/s²"], "micrometre per second squared", 1e-6M),
            (["mm/s²"], "millimetre per second squared", 1e-3M),
            (["km/s²"], "kilometre per second squared", 1e+3M)]);

        public static SIDerivedUnit SquareMetre => new("Square metre", ["m^2"], Speed,
            [Metre, Two, Exponent],
            [(["µm²"], "square micrometre", 1e-12M),
            (["mm²"], "square millimetre", 1e-6M),
            (["cm²"], "square centimetre", 1e-4M),
            (["dm²"], "square decimetre", 1e-2M),
            (["dam²"], "are", 1e+2M),
            (["hm²"], "hectare", 1e+4M),
            (["km²"], "square kilometre", 1e+6M)]);

        public static SIDerivedUnit Litre => new("Litre", ["l", "L"], Volume,
            [new Number(1e-3M), Metre, Three, Exponent, Multiplication]);

        public static SIDerivedUnit Newton => new("Newton", ["N"], Force,
            [Kilogram, Metre, Time, MinusTwo, Exponent, Multiplication, Multiplication]);

        // Imperial units
        public static ImperialUnit Inch => new("Inch", ["\"", "in"], 0.0254M, Metre);
        public static ImperialUnit Foot => new("Foot", ["'", "ft"], 0.3048M, Metre);
        public static ImperialUnit Yard => new("Yard", ["yd"], 0.9144M, Metre);
        public static ImperialUnit Mile => new("Mile", ["mi"], 1609.344M, Metre);
        public static ImperialUnit NauticalMile => new("Nautical mile", ["NM", "nmi", "M"], 1852M, Metre);
        public static ImperialUnit MilePerHour => new("Mile per hour", ["mph"], 0.44704M, MetrePerSecond);

        // Mathematical formulas
        public static Formula AreaOfSquare => new("Area of square", [PhyVar("Side", "a", Length), Two, Exponent], Area);
        public static Formula AreaOfRectangle => new("Area of rectangle", [PhyVar("Length", "l", Length), PhyVar("Width", "w", Length), Multiplication], Area);
        public static Formula AreaOfTriangle => new("Area of triangle", [PhyVar("Base", "b", Length), PhyVar("Height", "h", Length), Multiplication, Two, Division], Area);
        public static Formula AreaOfCircle => new("Area of circle", [Pi, PhyVar("Radius", "r", Length), Two, Exponent, Multiplication], Area);
        public static Formula AreaOfSphere => new("Area of sphere", [Four, Pi, PhyVar("Radius", "r", Length), Two, Exponent, Multiplication, Multiplication], Area);
        public static Formula VolumeOfCube => new("Volume of cube", [PhyVar("Side", "a", Length), Three, Exponent], Volume);
        public static Formula VolumeOfBox => new("Volume of box", [PhyVar("Length", "l", Length), PhyVar("Width", "w", Length), PhyVar("Height", "h", Length), Multiplication, Multiplication], Volume);

        // Physics formulas
        public static Formula AccelerationFormula => new("Acceleration", [PhyVar("Speed", "v", Speed), PhyVar("Time", "t", Time), Division], Acceleration);
        public static Formula TravelDistance => new("Travel distance", [PhyVar("Speed", "v", Speed), PhyVar("Time", "t", Time), Multiplication], Length);
        public static Formula TravelSpeed => new("Travel speed", [PhyVar("Distance", "d", Length), PhyVar("Time", "t", Time), Division], Speed);

        /// <summary>
        /// All operators
        /// </summary>
        public IOperator[] Operators { get; init; }

        /// <summary>
        /// All functions
        /// </summary>
        public IFunction[] Functions { get; init; }

        /// <summary>
        /// All constants
        /// </summary>
        public IConstant[] Constants { get; init; }

        /// <summary>
        /// All quantities
        /// </summary>
        public IQuantity[] Quantities { get; init; }

        /// <summary>
        /// All units
        /// </summary>
        public IUnit[] Units { get; init; }

        /// <summary>
        /// All formulas
        /// </summary>
        public IFormula[] Formulas { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Factory()
        {
            Constants = [Pi, SpeedOfLight, GravitationConstant];

            Operators = [Negation, Addition, Division, Exponent, Multiplication, Quotient, Reminder, Subtraction, Inverse, And, Or, Xor];

            Functions = [Round, Cos, Sin];
            
            Quantities = [Time, Length, Speed, Mass, Density, Acceleration, Area, Volume, Force, Frequency,
                Voltage, Current, Resistance, Conductance, Capacitance, Inductance, Impedance, Charge, Power];

            Units = [Second, Metre, Kilogram, MetrePerSecond, SquareMetre, Newton,
                Inch, Foot, Yard, Mile, NauticalMile, MilePerHour];

            Formulas = [AreaOfSquare, AreaOfRectangle, AreaOfTriangle, AreaOfCircle, AreaOfSphere, VolumeOfCube, VolumeOfBox,
                AccelerationFormula, TravelDistance, TravelSpeed];
        }

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
