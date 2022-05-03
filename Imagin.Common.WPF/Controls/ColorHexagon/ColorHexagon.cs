using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Colors;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ColorHexagon : Control, IUnsubscribe
    {
        public static readonly ReferenceKey<Canvas> CanvasKey = new();

        #region Fields

        public static readonly double DefaultButtonLength
            = DefaultButtonRadius * 2;

        public const double DefaultButtonRadius
            = 12.0;

        public static readonly double ButtonOffset
            = DefaultButtonLength * Math.Cos(30d * Math.PI / 180d);

        //...

        readonly Dictionary<RadioButton, RadioButton[]> Neighbors = new();
        readonly Dictionary<RadioButton, bool> Visited = new();

        //...

        Canvas canvas => this.GetChild<Canvas>(CanvasKey);

        RadioButton root;

        #endregion

        #region Properties

        public event EventHandler<EventArgs<object>> SelectedColorChanged;

        //...

        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(ColorHexagon), new FrameworkPropertyMetadata(0.5));
        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register(nameof(ButtonHeight), typeof(double), typeof(ColorHexagon), new FrameworkPropertyMetadata(25d));
        public double ButtonHeight
        {
            get => (double)GetValue(ButtonHeightProperty);
            set => SetValue(ButtonHeightProperty, value);
        }

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(nameof(ButtonWidth), typeof(double), typeof(ColorHexagon), new FrameworkPropertyMetadata(22d));
        public double ButtonWidth
        {
            get => (double)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof(ButtonStyle), typeof(Style), typeof(ColorHexagon), new FrameworkPropertyMetadata(null));
        public Style ButtonStyle
        {
            get => (Style)GetValue(ButtonStyleProperty);
            set => SetValue(ButtonStyleProperty, value);
        }

        static readonly DependencyPropertyKey CountKey = DependencyProperty.RegisterReadOnly(nameof(Count), typeof(int), typeof(ColorHexagon), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty CountProperty = CountKey.DependencyProperty;
        public int Count
        {
            get => (int)GetValue(CountProperty);
            private set => SetValue(CountKey, value);
        }

        public static readonly DependencyProperty GenerationsProperty = DependencyProperty.Register(nameof(Generations), typeof(int), typeof(ColorHexagon), new FrameworkPropertyMetadata(8, OnGenerationsChanged));
        public int Generations
        {
            get => (int)GetValue(GenerationsProperty);
            set => SetValue(GenerationsProperty, value);
        }
        static void OnGenerationsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ColorHexagon).OnGenerationsChanged(new Value<int>(e));

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorHexagon), new FrameworkPropertyMetadata(1.0));
        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorHexagon), new FrameworkPropertyMetadata(default(Color), OnSelectedColorChanged));
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            private set => SetValue(SelectedColorProperty, value);
        }
        static void OnSelectedColorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ColorHexagon).OnSelectedColorChanged(new Value<Color>(e));

        public static readonly DependencyProperty SelectedColorChangedCommandProperty = DependencyProperty.Register(nameof(SelectedColorChangedCommand), typeof(ICommand), typeof(ColorHexagon), new FrameworkPropertyMetadata(null));
        public ICommand SelectedColorChangedCommand
        {
            get => (ICommand)GetValue(SelectedColorChangedCommandProperty);
            set => SetValue(SelectedColorChangedCommandProperty, value);
        }

        #endregion

        #region ColorHexagon

        public ColorHexagon() : base() => this.RegisterHandler(i => RefreshAsync(), i => Clear());

        #endregion

        #region Methods

        void OnButtonClicked(object sender, RoutedEventArgs e) 
            => SetCurrentValue(SelectedColorProperty, GetColor(sender as RadioButton));

        //...

        void Cascade()
        {
            root.Foreground 
                = Brushes.White;
            root.Tag 
                = Brushes.White;

            Cascade(root);
            foreach (RadioButton i in canvas.Children)
                Visited[i] = false;
        }

        void Cascade(RadioButton parent)
        {
            List<RadioButton> nodes = new(6);
            for (int i = 0; i < 6; ++i)
            {
                if (Neighbors[parent][i] is RadioButton child)
                {
                    if (!Visited[child])
                    {
                        var c = Cascade(i, GetColor(parent));
                        child.Tag = c;
                        child.MultiBind(ForegroundProperty, new MultiConverter<SolidColorBrush>(j =>
                        {
                            if (j.Values?.Length == 3)
                            {
                                if (j.Values[0] is Color oldColor)
                                {
                                    if (j.Values[1] is double s)
                                    {
                                        if (j.Values[2] is double b)
                                        {
                                            var hsb = new HSL(oldColor.Int32().GetHue(), s, b);
                                            return new(HSL.From(hsb).Convert());
                                        }
                                    }
                                    return new(oldColor);
                                }
                            }
                            return default;
                        }), null,
                        new Binding() { Source = c }, new Binding(nameof(Saturation)) { Source = this }, new Binding(nameof(Brightness)) { Source = this });

                        Visited[child] = true;
                        nodes.Add(child);
                    }
                }
            }

            //Ensures root node not over-visited
            Visited[parent] = true;
            foreach (RadioButton child in nodes)
                Cascade(child);
        }

        Color Cascade(int i, Color c)
        {
            float delta = 1f / Generations;
            float ceiling = 0.99f;

            switch (i)
            {
                case 0: // increase cyan; else reduce red
                    if (c.ScG < ceiling && c.ScB < ceiling)
                    {
                        c.ScG = Math.Min(Math.Max(0f, c.ScG + delta), 1f);
                        c.ScB = Math.Min(Math.Max(0f, c.ScB + delta), 1f);
                    }
                    else
                    {
                        c.ScR = Math.Min(Math.Max(0f, c.ScR - delta), 1f);
                    }
                    break;
                case 1: // increase blue; else reduce yellow
                    if (c.ScB < ceiling)
                    {
                        c.ScB = Math.Min(Math.Max(0f, c.ScB + delta), 1f);
                    }
                    else
                    {
                        c.ScR = Math.Min(Math.Max(0f, c.ScR - delta), 1f);
                        c.ScG = Math.Min(Math.Max(0f, c.ScG - delta), 1f);
                    }
                    break;
                case 2: // increase magenta; else reduce green
                    if (c.ScR < ceiling && c.ScB < ceiling)
                    {
                        c.ScR = Math.Min(Math.Max(0f, c.ScR + delta), 1f);
                        c.ScB = Math.Min(Math.Max(0f, c.ScB + delta), 1f);
                    }
                    else
                    {
                        c.ScG = Math.Min(Math.Max(0f, c.ScG - delta), 1f);
                    }
                    break;
                case 3: // increase red; else reduce cyan
                    if (c.ScR < ceiling)
                    {
                        c.ScR = Math.Min(Math.Max(0f, c.ScR + delta), 1f);
                    }
                    else
                    {
                        c.ScG = Math.Min(Math.Max(0f, c.ScG - delta), 1f);
                        c.ScB = Math.Min(Math.Max(0f, c.ScB - delta), 1f);
                    }
                    break;
                case 4: // increase yellow; else reduce blue
                    if (c.ScR < ceiling && c.ScG < ceiling)
                    {
                        c.ScR = Math.Min(Math.Max(0f, c.ScR + delta), 1f);
                        c.ScG = Math.Min(Math.Max(0f, c.ScG + delta), 1f);
                    }
                    else
                    {
                        c.ScB = Math.Min(Math.Max(0f, c.ScB - delta), 1f);
                    }
                    break;
                case 5: // increase green; else reduce magenta
                    if (c.ScG < ceiling)
                    {
                        c.ScG = Math.Min(Math.Max(0f, c.ScG + delta), 1f);
                    }
                    else
                    {
                        c.ScR = Math.Min(Math.Max(0f, c.ScR - delta), 1f);
                        c.ScB = Math.Min(Math.Max(0f, c.ScB - delta), 1f);
                    }
                    break;
            }
            return c;
        }

        //...

        void Clear()
        {
            Unsubscribe();
            canvas.Children.Clear();

            root = null;
        }

        //...

        RadioButton CreateButton()
        {
            var result = new RadioButton();
            result.Bind
                (RadioButton.HeightProperty, 
                nameof(ButtonHeight), this);
            result.Bind
                (RadioButton.StyleProperty, 
                nameof(ButtonStyle), this);
            result.Bind
                (RadioButton.WidthProperty,
                nameof(ButtonWidth), this);

            result.Click += OnButtonClicked;

            Neighbors
                .Add(result, new RadioButton[6]);
            Visited
                .Add(result, false);
            return result;
        }

        //...

        Color GetColor(RadioButton input) 
            => input.Tag.As<Color>();

        //...

        async void RefreshAsync()
        {
            await Dispatch.InvokeAsync(() =>
            {
                Clear();

                canvas.Height 
                    = canvas.Width = (DefaultButtonRadius * 2.0) * Convert.ToDouble((Generations * 2) + 1);

                root = CreateButton();

                Canvas.SetLeft
                    (root, canvas.Width / 2);
                Canvas.SetTop
                    (root, canvas.Height / 2);

                canvas.Children.Add(root);

                Refresh(root, 1);
                Cascade();

                Count = canvas.Children.Count;
            });
        }

        //...

        /// <summary>
        /// Refreshes neighboring buttons (recursive).
        /// </summary>
        void Refresh(RadioButton parent, int generation)
        {
            if (generation > Generations)
                return;

            for (int i = 0; i < 6; ++i)
            {
                if (Neighbors[parent][i] == null)
                {
                    var button = CreateButton();

                    double dx = Canvas.GetLeft(parent) 
                        + ButtonOffset * Math.Cos(i * Math.PI / 3d);
                    double dy = Canvas.GetTop(parent) 
                        + ButtonOffset * Math.Sin(i * Math.PI / 3d);

                    Canvas.SetLeft
                        (button, dx);
                    Canvas.SetTop
                        (button, dy);

                    canvas.Children.Add(button);
                    Neighbors[parent][i] = button;
                }
            }

            // Set the cross-pointers on the 6 surrounding nodes.
            for (int i = 0; i < 6; ++i)
            {
                if (Neighbors[parent][i] is RadioButton button)
                {
                    int ip3 = (i + 3) % 6;
                    Neighbors[button][ip3] = parent;

                    int ip1 = (i + 1) % 6;
                    int ip2 = (i + 2) % 6;
                    int im1 = (i + 5) % 6;
                    int im2 = (i + 4) % 6;
                    Neighbors[button][ip2] = Neighbors[parent][ip1];
                    Neighbors[button][im2] = Neighbors[parent][im1];
                }
            }

            // Recurse into each of the 6 surrounding nodes.
            for (int i = 0; i < 6; ++i)
                Refresh(Neighbors[parent][i], generation + 1);
        }

        //...

        protected virtual void OnGenerationsChanged(Value<int> input) => RefreshAsync();

        protected virtual void OnSelectedColorChanged(Value<Color> input)
        {
            SelectedColorChangedCommand
                ?.Execute(input.New);
            SelectedColorChanged
                ?.Invoke(this, new EventArgs<object>(input.New));
        }

        //...

        public void Unsubscribe()
        {
            foreach (RadioButton i in canvas.Children)
            {
                i.Unbind(RadioButton
                    .HeightProperty);
                i.Unbind(RadioButton
                    .StyleProperty);
                i.Unbind(RadioButton
                    .WidthProperty);

                i.Click -= OnButtonClicked;

                Neighbors
                    .Remove(i);
                Visited
                    .Remove(i);
            }
        }

        #endregion
    }
}