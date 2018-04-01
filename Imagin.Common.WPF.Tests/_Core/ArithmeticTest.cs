using System;

namespace Imagin.Common.Tests
{
    public abstract class ArithmeticTest : Test
    {
        protected const string IdentifierFormat = "-> {0}";

        public ArithmeticTest(string name) : base(name) { }

        protected static void WriteIdentifier(string identifer)
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine(IdentifierFormat, identifer);
            Console.WriteLine(string.Empty);
        }
    }
}