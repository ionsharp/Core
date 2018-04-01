using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Tests
{
    public class ArithmeticTest<TInputA, TInputB> : ArithmeticTest
    {
        readonly TInputA _a;
        public TInputA A => _a;

        readonly TInputB _b;
        public TInputB B => _b;

        readonly Func<TInputA, TInputB, object> _action;

        public ArithmeticTest(string name, TInputA a, TInputB b, Func<TInputA, TInputB, object> action) : base(name)
        {
            _a = a;
            _b = b;
            _action = action;
        }

        public override object Perform()
        {
            var result = _action?.Invoke(_a, _b);
            Console.WriteLine(Name);

            WriteIdentifier("a [{0}]".F(_a.GetType().FullName));
            Console.WriteLine(_a);

            WriteIdentifier("b [{0}]".F(_b.GetType().FullName));
            Console.WriteLine(_b);

            WriteIdentifier("= [{0}]".F(result.GetType().FullName));
            Console.WriteLine(result);

            Console.WriteLine(string.Empty);
            Console.WriteLine(Separator);
            Console.WriteLine(string.Empty);
            return result;
        }
    }
}