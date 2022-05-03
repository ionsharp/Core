using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class UriBox : ParseBox<Uri>
    {
        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(UriKind), typeof(UpDown), new FrameworkPropertyMetadata(default(UriKind)));
        public UriKind Kind
        {
            get => (UriKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public UriBox() : base() { }

        protected override Uri GetValue(string value) => value.Uri(Kind);

        protected override string ToString(Uri value) => value?.ToString();
    }
}