using Imagin.Common.Web;
using System;

namespace Imagin.Common.Data
{
    [Serializable]
    public class StringCondition : Condition
    {
        bool ignoreCase = true;
        public bool IgnoreCase
        {
            get
            {
                return ignoreCase;
            }
            set
            {
                ignoreCase = value;
                OnPropertyChanged("ignoreCase");
            }
        }

        public override bool Evaluate(string StringValue)
        {
            StringOperator Operator = (StringOperator)this.Operator;

            string Value = (string)this.Value;
            switch (Operator)
            {
                case StringOperator.BeginsWith:
                    return this.IgnoreCase ? StringValue.ToLower().StartsWith(Value.ToLower()) : StringValue.StartsWith(Value);
                case StringOperator.Contains:
                    return this.IgnoreCase ? StringValue.ToLower().Contains(Value.ToLower()) : StringValue.Contains(Value);
                case StringOperator.DoesNotContain:
                    return this.IgnoreCase ? !StringValue.ToLower().Contains(Value.ToLower()) : !StringValue.Contains(Value);
                case StringOperator.EndsWith:
                    return this.IgnoreCase ? StringValue.ToLower().EndsWith(Value.ToLower()) : StringValue.EndsWith(Value);
                case StringOperator.IsEqualTo:
                    return this.IgnoreCase ? StringValue.ToLower().Equals(Value.ToLower()) : StringValue.Equals(Value);
                case StringOperator.MatchesRegex:
                    return System.Text.RegularExpressions.Regex.IsMatch(StringValue, Value);
            }
            return false;
        }

        public StringCondition() : base()
        {
        }

        public StringCondition(ServerObjectProperty Property, object Operator, object Value, bool IgnoreCase) : base(Property, Operator, Value)
        {
            this.IgnoreCase = IgnoreCase;
        }
    }
}
