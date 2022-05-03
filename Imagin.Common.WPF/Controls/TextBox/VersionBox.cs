using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class VersionBox : ParseBox<Version>
    {
        public static readonly DependencyProperty DelimiterProperty = DependencyProperty.Register(nameof(Delimiter), typeof(char), typeof(VersionBox), new FrameworkPropertyMetadata('.'));
        public char Delimiter
        {
            get => (char)GetValue(DelimiterProperty);
            set => SetValue(DelimiterProperty, value);
        }

        public VersionBox() : base() { }

        protected override Version GetValue(string value) => value.Version(Delimiter);

        protected override string ToString(Version value) => value?.ToString();
    }
}