using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities.Physical;
using CalcBase.Tokens;
using CalcBase.Units;
using CalcBase.Units.Physics.SI;

namespace CalcBase.Formulas.Physics
{
    public record TravelDistanceFormula : Singleton<VelocityFormula>, IFormula
    {
        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IToken[] Expression => new IToken[]
        {
            new UnitToken()
            {
                Position = 0,
                Length = 1,
                Unit = MetrePerSecondUnit.Instance
            },
            new UnitToken()
            {
                Position = 2,
                Length = 1,
                Unit = SecondUnit.Instance
            },
            new OperatorToken()
            {
                Position = 1,
                Length = 1,
                Operator = MultiplicationOperator.Instance
            }
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IUnit Result => MeterUnit.Instance;
    }
}
