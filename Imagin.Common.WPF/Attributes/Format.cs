using System;

namespace Imagin.Common
{
    #region (enum) Formats

    public enum ColorFormat
    {
        ColorBox,
        TextBox,
        Both
    }

    public enum RangeFormat
    {
        None,
        Both,
        Slider,
        UpDown,
    }

    #endregion

    #region FormatAttribute

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FormatAttribute : Attribute
    {
        public readonly object Format;

        public FormatAttribute(ColorFormat format)
            : base() => Format = format;

        public FormatAttribute(RangeFormat format)
            : base() => Format = format;
    }

    #endregion

    #region ItemFormatAttribute

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ItemFormatAttribute : FormatAttribute
    {
        public ItemFormatAttribute(ColorFormat format)
            : base(format) { }

        public ItemFormatAttribute(RangeFormat format)
            : base(format) { }
    }

    #endregion
}