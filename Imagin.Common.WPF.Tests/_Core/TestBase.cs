namespace Imagin.Common.Tests
{
    public abstract class Test : NamedObject
    {
        protected const string Separator = "------------------------------------------------";

        public Test(string name) : base(name) { }

        public abstract object Perform();
    }
}
