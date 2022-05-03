using System;

namespace Imagin.Common.Data
{
    #region (enum) Styles

    public enum ArrayStyle
    {
        Comma, Bullet,
    }

    public enum BooleanStyle
    {
        Image, Switch
    }

    public enum CollectionStyle
    {
        Bullet, Button, Comma, Default, ImageToggleButton
    }

    public enum ColorStyle
    {
        Alpha
    }

    public enum DoubleStyle
    {
        Angle, Progress, ProgressRound, Unit
    }

    public enum EnumStyle
    {
        FlagCheck, FlagSelect, FlagSwitch, FlagToggle
    }

    public enum EnumerableStyle
    {
        Comma, Bullet,
    }

    public enum Int32Style
    {
        Index,
    }

    public enum ListStyle
    {
        Comma, Bullet,
    }

    public enum ObjectStyle
    {
        Button,
        Expander
    }

    public enum StringStyle
    {
        FilePath, FolderPath,
        MultiFiles, MultiFolders, MultiLines, MultiPaths,
        Password, Search, Thumbnail, Tokens
    }

    #endregion

    #region StyleAttribute

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StyleAttribute : Attribute
    {
        public readonly Enum Style;

        public StyleAttribute(ArrayStyle style)
            : base() => Style = style;

        public StyleAttribute(BooleanStyle style)
            : base() => Style = style;

        public StyleAttribute(CollectionStyle style)
            : base() => Style = style;

        public StyleAttribute(ColorStyle style)
            : base() => Style = style;

        public StyleAttribute(DoubleStyle style)
            : base() => Style = style;

        public StyleAttribute(EnumStyle style)
            : base() => Style = style;

        public StyleAttribute(EnumerableStyle style)
            : base() => Style = style;

        public StyleAttribute(Int32Style style)
            : base() => Style = style;

        public StyleAttribute(ListStyle style)
            : base() => Style = style;

        public StyleAttribute(ObjectStyle style)
            : base() => Style = style;

        public StyleAttribute(StringStyle style)
            : base() => Style = style;
    }

    #endregion

    #region ItemStyleAttribute

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ItemStyleAttribute : StyleAttribute
    {
        public ItemStyleAttribute(ArrayStyle style)
            : base(style) { }

        public ItemStyleAttribute(BooleanStyle style)
            : base(style) { }

        public ItemStyleAttribute(CollectionStyle style)
            : base(style) { }

        public ItemStyleAttribute(ColorStyle style)
            : base(style) { }

        public ItemStyleAttribute(DoubleStyle style)
            : base(style) { }

        public ItemStyleAttribute(EnumStyle style)
            : base(style) { }

        public ItemStyleAttribute(EnumerableStyle style)
            : base(style) { }

        public ItemStyleAttribute(Int32Style style)
            : base(style) { }

        public ItemStyleAttribute(ListStyle style)
            : base(style) { }

        public ItemStyleAttribute(ObjectStyle style)
            : base(style) { }

        public ItemStyleAttribute(StringStyle style)
            : base(style) { }
    }

    #endregion
}