using Imagin.Common.Web;
using System;

namespace Imagin.Common.Data
{
    [Serializable]
    public class DateTimeCondition : Condition
    {
        public override bool Evaluate(DateTime DateTimeValue)
        {
            DateTimeOperator Operator = (DateTimeOperator)this.Operator;

            DateTime Value = (DateTime)this.Value;
            switch (Operator)
            {
                case DateTimeOperator.After:
                    return DateTimeValue.CompareTo(Value) > 0;
                case DateTimeOperator.Before:
                    return DateTimeValue.CompareTo(Value) < 0;
                case DateTimeOperator.DoesNotEqual:
                    return DateTimeValue.CompareTo(Value) != 0;
                case DateTimeOperator.Equals:
                    return DateTimeValue.CompareTo(Value) == 0;
            }
            return false;
        }

        public DateTimeCondition() : base()
        {
        }

        public DateTimeCondition(ServerObjectProperty Property, object Operator, object Value) : base(Property, Operator, Value)
        {
        }
    }
}
