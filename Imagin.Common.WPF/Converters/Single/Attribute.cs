using Imagin.Common.Linq;
using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public abstract class AttributeConverter<T> : Converter<object, string> where T : Attribute
    {
        protected virtual Attribute GetAttribute(Enum input) => input.GetAttribute<T>();

        protected virtual Attribute GetAttribute(Type input) => input.GetAttribute<T>();

        protected virtual string GetFallback(Enum input) => null;

        protected virtual string GetFallback(Type input) => null;

        protected abstract string GetResult(Attribute input);

        protected override ConverterValue<string> ConvertTo(ConverterData<object> input)
        {
            if (input.Value is Enum a)
                return GetResult(GetAttribute(a)) ?? GetFallback(a);

            if (input.Value is Type b)
                return GetResult(GetAttribute(b)) ?? GetFallback(b);

            if (input.Value is object c)
                return GetResult(GetAttribute(c.GetType())) ?? GetFallback(c.GetType());

            return Nothing.Do;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class AbbreviationConverter : AttributeConverter<AbbreviationAttribute>
    {
        public static AbbreviationConverter Default { get; private set; } = new();
        AbbreviationConverter() { }

        protected override string GetResult(Attribute input)
            => input.As<AbbreviationAttribute>().Abbreviation;
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class CategoryConverter : AttributeConverter<CategoryAttribute>
    {
        public static CategoryConverter Default { get; private set; } = new();
        public CategoryConverter() : base() { }

        protected override Attribute GetAttribute(Enum input)
            => input.GetAttribute<CategoryAttribute>()
            ?? input.GetAttribute<System.ComponentModel.CategoryAttribute>() as Attribute;

        protected override Attribute GetAttribute(Type input)
            => input.GetAttribute<CategoryAttribute>()
            ?? input.GetAttribute<System.ComponentModel.CategoryAttribute>() as Attribute;

        protected override string GetResult(Attribute input)
            => input.As<CategoryAttribute>()?.Category ?? input.As<System.ComponentModel.CategoryAttribute>()?.Category;
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class DescriptionConverter : AttributeConverter<DescriptionAttribute>
    {
        public static DescriptionConverter Default { get; private set; } = new();
        DescriptionConverter() { }

        protected override Attribute GetAttribute(Enum input)
            => input.GetAttribute<DescriptionAttribute>()
            ?? input.GetAttribute<System.ComponentModel.DescriptionAttribute>() as Attribute;

        protected override Attribute GetAttribute(Type input)
            => input.GetAttribute<DescriptionAttribute>()
            ?? input.GetAttribute<System.ComponentModel.DescriptionAttribute>() as Attribute;

        protected override string GetResult(Attribute input)
            => input.As<DescriptionAttribute>()?.Description ?? input.As<System.ComponentModel.DescriptionAttribute>()?.Description;
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class DisplayNameConverter : AttributeConverter<DisplayNameAttribute>
    {
        public static DisplayNameConverter Default { get; private set; } = new();
        DisplayNameConverter() { }

        protected override Attribute GetAttribute(Enum input)
            => input.GetAttribute<DisplayNameAttribute>() 
            ?? input.GetAttribute<System.ComponentModel.DisplayNameAttribute>() as Attribute;

        protected override Attribute GetAttribute(Type input)
            => input.GetAttribute<DisplayNameAttribute>()
            ?? input.GetAttribute<System.ComponentModel.DisplayNameAttribute>() as Attribute;

        protected override string GetFallback(Enum input) => input.ToString().SplitCamel();

        protected override string GetFallback(Type input) => input.Name.SplitCamel();

        protected override string GetResult(Attribute input)
            => input.As<DisplayNameAttribute>()?.DisplayName 
            ?? input.As<System.ComponentModel.DisplayNameAttribute>()?.DisplayName;
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class IconConverter : AttributeConverter<IconAttribute>
    {
        public static IconConverter Default { get; private set; } = new();
        IconConverter() { }

        protected override string GetResult(Attribute input)
            => input.As<IconAttribute>()?.Icon;
    }
}