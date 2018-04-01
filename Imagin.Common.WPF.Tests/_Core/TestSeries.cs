using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Tests
{
    public abstract class TestSeries : NamedObject
    {
        protected const string Separator = "==============================================================================";

        /// <summary>
        /// Initializes an instance of the <see cref="TestSeries"/> class.
        /// </summary>
        /// <param name="name">A name that describes the nature of the tests.</param>
        public TestSeries(string name) : base(name) { }

        protected abstract IEnumerable<Test> GetTests();

        public void Perform()
        {
            Console.WriteLine("Tests > [start] > {0}", Name);
            Console.WriteLine(Separator);

            Console.WriteLine(string.Empty);
            GetTests().ForEach<Test>(i => i.Perform());

            Console.WriteLine(Separator);
            Console.WriteLine("Tests > [break] > {0}", Name);

            Console.WriteLine(string.Empty);
        }
    }
}
