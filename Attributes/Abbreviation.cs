using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AbbreviationAttribute : Attribute
    {
        public readonly string Abbreviation;

        public AbbreviationAttribute(string abbreviation) : base() => Abbreviation = abbreviation;
    }
}