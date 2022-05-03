using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Demo
{
    #region (abstract) Test

    public abstract class Test : Base
    {
        public Type Type { get; private set; }

        public Test(Type type)
        {
            Type = type;
        }
    }

    #endregion

    #region (abstract) Test<T>

    public abstract class Test<T> : Test
    {
        public Test() : base(typeof(T)) { }
    }

    #endregion

    //...

    #region StringCondition

    public class StringConditionTest : Test<StringCondition>
    {
        string condition = "0,1,(0|1|(1,0|1))";
        public string Condition
        {
            get => condition;
            set => this.Change(ref condition, value);
        }

        Result result = null;
        public Result Result
        {
            get => result;
            set => this.Change(ref result, value);
        }

        public BooleanTokenizer Tokenizer { get; private set; } = new();

        string values = "0;1;0;1;1;0;1";
        public string Values
        {
            get => values;
            set => this.Change(ref values, value);
        }

        public StringConditionTest() : base() { }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Condition):
                case nameof(Values):
                    var result = new StringCondition(condition);
                    Try.Invoke(() => Result = result.Evaluate(Tokenizer.Tokenize(values, ';').ToArray()) ? new Success() { Text = "True!" } : new Error("False!"), e => Result = e);
                    break;
            }
        }
    }

    #endregion
}