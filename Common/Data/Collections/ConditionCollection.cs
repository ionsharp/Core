using Imagin.Common.Web;
using Imagin.Common.Collections.Concurrent;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Data.Collections
{
    [Serializable]
    public class ConditionCollection : ConcurrentObservableCollection<Condition>
    {
        public Condition Create(ServerObjectProperty ConditionProperty, object ConditionOperator, string StringValue, long LongValue, DateTime DateValue, bool IgnoreCase)
        {
            switch (ConditionProperty)
            {
                case ServerObjectProperty.Name:
                case ServerObjectProperty.Path:
                case ServerObjectProperty.Extension:
                    if (!string.IsNullOrEmpty(StringValue))
                        return new StringCondition(ConditionProperty, ConditionOperator, StringValue, IgnoreCase);
                    break;
                case ServerObjectProperty.Accessed:
                case ServerObjectProperty.Created:
                case ServerObjectProperty.Modified:
                    return new DateTimeCondition(ConditionProperty, ConditionOperator, DateValue);
                case ServerObjectProperty.Size:
                    return new LongCondition(ConditionProperty, ConditionOperator, LongValue);
            }
            return null;
        }

        public ConditionCollection() : base()
        {
        }

        public ConditionCollection(IEnumerable<Condition> Conditions) : base(Conditions)
        {
        }
    }
}
