using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Tests
{
    public class ArithmeticTest<TInput> : ArithmeticTest
    {
        readonly TInput _a;
        public TInput A => _a;

        readonly Func<TInput, object> _action;

        public ArithmeticTest(string name, TInput a, Func<TInput, object> action) : base(name)
        {
            _a = a;
            _action = action;
        }

        public override object Perform()
        {
            var result = _action?.Invoke(_a);
            Console.WriteLine(Name);

            WriteIdentifier("a [{0}]".F(_a.GetType().FullName));
            Console.WriteLine(_a);

            WriteIdentifier("= [{0}]".F(result.GetType().FullName));
            Console.WriteLine(result);

            Console.WriteLine(string.Empty);
            Console.WriteLine(Separator);
            Console.WriteLine(string.Empty);
            return result;
        }
    }
}
