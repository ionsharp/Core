using Imagin.Common.Text;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class BulletElement : TextBlock
    {
        public static readonly DependencyProperty BulletProperty = DependencyProperty.Register(nameof(Bullet), typeof(Bullets), typeof(BulletElement), new FrameworkPropertyMetadata(Bullets.Square));
        public Bullets Bullet
        {
            get => (Bullets)GetValue(BulletProperty);
            set => SetValue(BulletProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(BulletElement), new FrameworkPropertyMetadata(default(double)));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public BulletElement() => InitializeComponent();
    }
}