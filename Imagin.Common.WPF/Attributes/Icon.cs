using System;

namespace Imagin.Common
{
    public class IconAttribute : Attribute
    {
        public readonly string Color;

        public readonly string Icon;

        public IconAttribute(string icon)
        {
            Icon = icon;
        }

        public IconAttribute(Images icon) : this($"{InternalAssembly.AbsoluteImagePath}{icon}.png") { }

        public IconAttribute(Images icon, ThemeKeys color) : this(icon)
        {
            Color = $"{color}";
        }
    }
}