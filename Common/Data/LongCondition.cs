using Imagin.Common.Web;
using System;

namespace Imagin.Common.Data
{
    [Serializable]
    public class LongCondition : Condition
    {
        public override bool Evaluate(long LongValue)
        {
            LongOperator Operator = (LongOperator)this.Operator;

            long Value = (long)this.Value;
            switch (Operator)
            {
                case LongOperator.DoesNotEqual:
                    return LongValue.CompareTo(Value) != 0;
                case LongOperator.Equals:
                    return LongValue.CompareTo(Value) == 0;
                case LongOperator.GreaterThan:
                    return LongValue.CompareTo(Value) > 0;
                case LongOperator.LessThan:
                    return LongValue.CompareTo(Value) < 0;
            }
            return false;
        }

        public LongCondition() : base()
        {
        }

        public LongCondition(ServerObjectProperty Property, object Operator, object Value) : base(Property, Operator, Value)
        {
        }
    }
}
