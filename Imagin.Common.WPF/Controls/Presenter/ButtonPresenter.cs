using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class ButtonPresenter : Presenter<Window>
    {
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(nameof(Spacing), typeof(Thickness), typeof(ButtonPresenter), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness Spacing
        {
            get => (Thickness)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public ButtonPresenter() : base() { }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.Property.Name)
            {
                case nameof(Content):
                    if (Content is IList buttons)
                    {
                        foreach (var i in buttons)
                        {
                            if (i is Button j)
                                j.Margin = Spacing;
                        }
                    }
                    break;
            }
        }
    }
}