using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class UnitControl : Control
    {
        public static readonly ResourceKey<DoubleUpDown> ComboBoxStyleKey = new();

        public static readonly ResourceKey<DoubleUpDown> DoubleUpDownStyleKey = new();

        //...

        public static readonly DependencyProperty ActualValueProperty = DependencyProperty.Register(nameof(ActualValue), typeof(double), typeof(UnitControl), new FrameworkPropertyMetadata(0.0));
        public double ActualValue
        {
            get => (double)GetValue(ActualValueProperty);
            set => SetValue(ActualValueProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(UnitControl), new FrameworkPropertyMetadata(false));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register(nameof(Resolution), typeof(float), typeof(UnitControl), new FrameworkPropertyMetadata(72f, OnResolutionChanged));
        public float Resolution
        {
            get => (float)GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
        }
        static void OnResolutionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UnitControl>().OnResolutionChanged(new Value<float>(e));

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(nameof(Spacing), typeof(Thickness), typeof(UnitControl), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness Spacing
        {
            get => (Thickness)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(UnitControl), new FrameworkPropertyMetadata(null));
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(GraphicUnit), typeof(UnitControl), new FrameworkPropertyMetadata(GraphicUnit.Pixel, OnUnitChanged));
        public GraphicUnit Unit
        {
            get => (GraphicUnit)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }
        static void OnUnitChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UnitControl>().OnUnitChanged(new Value<GraphicUnit>(e));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(UnitControl), new FrameworkPropertyMetadata(0.0));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        readonly BindingExpressionBase j = null;

        //...

        public UnitControl() : base()
        {
            j = this.Bind(ActualValueProperty, nameof(Value), this, BindingMode.TwoWay, new SimpleConverter<double, double>
            (
                i => i.Convert(GraphicUnit.Pixel, Unit, Resolution),
                i => i.Convert(Unit, GraphicUnit.Pixel, Resolution)
            ));
        }

        //...

        protected virtual void OnResolutionChanged(Value<float> i) => j?.UpdateTarget();

        protected virtual void OnUnitChanged(Value<GraphicUnit> i) => j?.UpdateTarget();
    }
}