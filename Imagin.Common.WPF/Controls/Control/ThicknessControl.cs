using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class ThicknessControl : Control
    {
        public static readonly ReferenceKey<Grid> GridKey = new();

        public static readonly ResourceKey<DoubleUpDown> DoubleUpDownStyleKey = new();

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(ThicknessControl), new FrameworkPropertyMetadata(double.MaxValue, OnMaximumChanged));
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        static void OnMaximumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ThicknessControl).OnMaximumChanged(new Value<double>(e));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(ThicknessControl), new FrameworkPropertyMetadata(double.MinValue, OnMinimumChanged));
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        static void OnMinimumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ThicknessControl).OnMinimumChanged(new Value<double>(e));

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(nameof(Spacing), typeof(Thickness), typeof(ThicknessControl), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness Spacing
        {
            get => (Thickness)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(Thickness), typeof(ThicknessControl), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness Thickness
        {
            get => (Thickness)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        readonly List<BindingExpressionBase> bindings = new();

        public ThicknessControl() : base() => this.RegisterHandler(OnLoaded, OnUnloaded);

        //...

        void OnLoaded()
        {
            var index = 0;
            foreach (DoubleUpDown i in this.GetChild<Grid>(GridKey).Children)
            {
                IValueConverter converter = null;
                switch (index)
                {
                    case 0:
                        converter = new SimpleConverter<Thickness, double>
                        (
                            j => j.Left,
                            j => new Thickness(j, Thickness.Top, Thickness.Right, Thickness.Bottom)
                        );
                        break;
                    case 1:
                        converter = new SimpleConverter<Thickness, double>
                        (
                            j => j.Top,
                            j => new Thickness(Thickness.Left, j, Thickness.Right, Thickness.Bottom)
                        );
                        break;
                    case 2:
                        converter = new SimpleConverter<Thickness, double>
                        (
                            j => j.Right,
                            j => new Thickness(Thickness.Left, Thickness.Top, j, Thickness.Bottom)
                        );
                        break;
                    case 3:
                        converter = new SimpleConverter<Thickness, double>
                        (
                            j => j.Bottom,
                            j => new Thickness(Thickness.Left, Thickness.Top, Thickness.Right, j)
                        );
                        break;
                }

                var binding = i.Bind(DoubleUpDown.ValueProperty.Property, nameof(Thickness), this, BindingMode.TwoWay, converter);
                bindings.Add(binding);
                index++;
            }
        }

        void OnUnloaded()
        {
            bindings.Clear();
            foreach (DoubleUpDown i in this.GetChild<Grid>(GridKey).Children)
                i.Unbind(DoubleUpDown.ValueProperty.Property);
        }

        void Update()
        {
            foreach (var i in bindings)
                i.UpdateTarget();
        }

        //...

        protected virtual void OnMaximumChanged(Value<double> input) => Update();

        protected virtual void OnMinimumChanged(Value<double> input) => Update();
    }
}