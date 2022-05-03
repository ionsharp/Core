using System;

namespace Imagin.Common.Colors
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ComponentAttribute : Attribute
    {
        public Component Info { get; private set; }

        public ComponentAttribute(double minimum, double maximum, char unit, string symbol, string name = "") : base()
            => Info = new(minimum, maximum, unit, symbol, name == "" ? symbol : name);

        public ComponentAttribute(double minimum, double maximum, string symbol, string name = "") : this(minimum, maximum, ' ', symbol, name) { }
    }

    public class Component : Base
    {
        public double Maximum { get; private set; }

        public double Minimum { get; private set; }

        public string Name { get; private set; }

        public string Symbol { get; private set; }

        public char Unit { get; private set; }

        public Component(double minimum, double maximum, char unit, string symbol, string name) : base()
        {
            Minimum = minimum; Maximum = maximum; Unit = unit; Symbol = symbol; Name = name;
        }
    }
}