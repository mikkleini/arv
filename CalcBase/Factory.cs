using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Functions;
using CalcBase.Operators;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Units;
using System.Reflection;

namespace CalcBase
{
    /// <summary>
    /// Factory of elements
    /// </summary>
    public class Factory
    {
        // Operators
        public static IOperator Multiplication => MultiplicationOperator.Instance;
        public static IOperator Division => DivisionOperator.Instance;

        // Quantities
        public static Quantity Time => new("Time", ["t"]);
        public static Quantity Length => new("Length", ["l"]);
        public static Quantity Speed => new("Speed", ["v"]);
        public static Quantity Mass => new("Mass", ["m"]);
        public static Quantity Acceleration => new("Acceleration", ["a"]);
        public static Quantity Area => new("Area", ["A"]);
        public static Quantity Volume => new("Volume", ["V"]);

        // Electrical quantities
        public static Quantity Voltage => new("Voltage", ["V", "U"]);
        public static Quantity Current => new("Current", ["I"]);
        public static Quantity Resistance => new("Resistance", ["R"]);

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
            [(["µm/s", "um/s"], "micrometre per second", 1e-6M),
            (["mm/s"], "millimetre per second", 1e-3M),
            (["km/s"], "kilometre per second", 1e+3M),
            (["km/h"], "kilometre per hour", 3.6M)]);

        public static SIDerivedUnit SquareMetre => new("Square metre", ["m^2"], Speed,
            [Metre, Metre, Multiplication],
            [(["µm^2", "um^2", "µm²"], "square micrometre", 1e-12M),
            (["mm^2", "mm²"], "square millimetre", 1e-6M),
            (["cm^2", "cm²"], "square centimetre", 1e-4M),
            (["dm^2", "dm²"], "square decimetre", 1e-2M),
            (["dam^2", "dam²"], "are", 1e+2M),
            (["hm^2", "hm²"], "hectare", 1e+4M),
            (["km^2", "km²"], "square kilometre", 1e+6M)]);

        // Imperial units
        public static ImperialUnit Inch => new("Inch", ["\"", "in"], 0.0254M, Metre);
        public static ImperialUnit Foot => new("Foot", ["'", "ft"], 0.3048M, Metre);
        public static ImperialUnit Yard => new("Yard", ["yd"], 0.9144M, Metre);
        public static ImperialUnit Mile => new("Mile", ["mi"], 1609.344M, Metre);
        public static ImperialUnit NauticalMile => new("Nautical mile", ["NM", "nmi", "M"], 1852M, Metre);
        public static ImperialUnit MilePerHour => new("Mile per hour", ["mph"], 0.44704M, MetrePerSecond);

        /// <summary>
        /// All pperators
        /// </summary>
        private IOperator[] Operators { get; init; }

        /// <summary>
        /// All functions
        /// </summary>
        private IFunction[] Functions { get; init; }

        /// <summary>
        /// All constants
        /// </summary>
        private IConstant[] Constants { get; init; }

        /// <summary>
        /// All quantities
        /// </summary>
        private IQuantity[] Quantities { get; init; }

        /// <summary>
        /// All units
        /// </summary>
        private IUnit[] Units { get; init; }
        
        /// <summary>
        /// All formulas
        /// </summary>
        private IFormula[] Formulas { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Factory()
        {
            Quantities = [Time, Length, Speed, Mass];
            Units = [Second, Metre, Kilogram, MetrePerSecond, SquareMetre,
                Inch, Foot, Yard, Mile, NauticalMile, MilePerHour];
        }

        /// <summary>
        /// Get singletons of specific type of interface
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <returns>Enumable of type instances</returns>
        public static IEnumerable<T> GetSingletonsOfType<T>()
        {
            IEnumerable<Type> allSingletonTypes = Assembly.GetExecutingAssembly().GetTypes()
               .Where(t => IsSubclassOfRawGeneric(typeof(Singleton<>), t));
            IEnumerable<Type> rightTypes = allSingletonTypes.Where(t => t.GetInterfaces().Contains(typeof(T)));

            foreach (Type t in rightTypes)
            {
                PropertyInfo? prop = t.BaseType?.GetProperty("Instance");
                object? value = prop?.GetValue(null);
                if (value != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Found {value.GetType().Name}");
                    yield return (T)value;
                }
            }
        }

        /// <summary>
        /// Code from https://stackoverflow.com/a/457708
        /// </summary>
        /// <param name="generic">Generic type</param>
        /// <param name="toCheck">Type of check</param>
        /// <returns>true if type inherits generic type</returns>
        static bool IsSubclassOfRawGeneric(Type generic, Type? toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}
