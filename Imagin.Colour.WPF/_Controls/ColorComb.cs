using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorComb : Control
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<object>> SelectedColorChanged;

        Canvas _canvas;

        Hexagon _root;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CellDispositionProperty = DependencyProperty.Register(nameof(CellDisposition), typeof(ColorDisposition), typeof(ColorComb), new FrameworkPropertyMetadata(ColorDisposition.Solid));
        /// <summary>
        /// 
        /// </summary>
        public ColorDisposition CellDisposition
        {
            get => (ColorDisposition)GetValue(CellDispositionProperty);
            set => SetValue(CellDispositionProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CellStrokeThicknessProperty = DependencyProperty.Register(nameof(CellStrokeThickness), typeof(double), typeof(ColorComb), new FrameworkPropertyMetadata(0.15));
        /// <summary>
        /// 
        /// </summary>
        public double CellStrokeThickness
        {
            get => (double)GetValue(CellStrokeThicknessProperty);
            set => SetValue(CellStrokeThicknessProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), typeof(ColorComb), new FrameworkPropertyMetadata(0));
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get => (int)GetValue(CountProperty);
            private set => SetValue(CountProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GenerationsProperty = DependencyProperty.Register(nameof(Generations), typeof(int), typeof(ColorComb), new FrameworkPropertyMetadata(8, OnGenerationsChanged));
        /// <summary>
        /// 
        /// </summary>
        public int Generations
        {
            get => (int)GetValue(GenerationsProperty);
            set => SetValue(GenerationsProperty, value);
        }
        static void OnGenerationsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => ((ColorComb)element).OnGenerationsChanged((int)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorComb), new FrameworkPropertyMetadata(default(Color), OnSelectedColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            private set => SetValue(SelectedColorProperty, value);
        }
        static void OnSelectedColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => ((ColorComb)element).OnSelectedColorChanged((Color)e.NewValue);

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ColorComb() => DefaultStyleKey = typeof(ColorComb);

        #endregion

        #region Methods

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            ApplyTemplate();

            _canvas = Template.FindName("PART_Canvas", this) as Canvas;
            InitializeCells();
        }

        #endregion

        #region Private

        /// <summary>
        /// Cascade all cells with color.
        /// </summary>
        void Cascade()
        {
            _root.NominalColor = Color.FromScRgb(1f, 1f, 1f, 1f);
            Cascade(_root);
            foreach (Hexagon i in _canvas.Children)
                i.Visited = false;
        }

        /// <summary>
        /// Cascade child cells with color; this method is recursive.
        /// </summary>
        /// <param name="parent"></param>
        void Cascade(Hexagon parent)
        {
            float delta = 1f / Generations;
            float ceiling = 0.99f;

            System.Collections.Generic.List<Hexagon> visitedNodes =
                new System.Collections.Generic.List<Hexagon>(6);

            for (int i = 0; i < 6; ++i)
            {
                Hexagon child = parent.Neighbors[i];
                if (child != null && !child.Visited)
                {
                    Color c = parent.NominalColor;
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
                    child.NominalColor = c;
                    child.Visited = true;
                    visitedNodes.Add(child);
                }
            }

            parent.Visited = true; // ensures root node not over-visited
            foreach (Hexagon child in visitedNodes)
                Cascade(child);
        }

        Hexagon GetCell()
        {
            var cell = new Hexagon(CellDisposition);
            cell.Click += new RoutedEventHandler(OnCellClicked);

            BindingOperations.SetBinding(cell, Hexagon.DispositionProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CellDisposition)),
                Source = this
            });
            BindingOperations.SetBinding(cell, Hexagon.StrokeThicknessProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CellStrokeThickness)),
                Source = this
            });

            return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        async void InitializeCells()
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                _canvas.Height = _canvas.Width = (Hexagon.Radius * 2.0) * Convert.ToDouble((Generations * 2) + 1);
                _canvas.Children.Clear();

                //Define comb of 127 hexagons, starting in center of canvas.
                _root = GetCell();

                Canvas.SetLeft(_root, _canvas.Width / 2);
                Canvas.SetTop(_root, _canvas.Height / 2);

                _canvas.Children.Add(_root);

                //Expand outward
                InitializeCells(_root, 1);

                Cascade();

                SetCurrentValue(CountProperty, _canvas.Children.Count);
            }));
        }

        /// <summary>
        /// Initialize surrounding cells; this method is recursive.
        /// </summary>
        void InitializeCells(Hexagon Parent, int generation)
        {
            if (generation > Generations)
                return;

            for (int i = 0; i < 6; ++i)
            {
                if (Parent.Neighbors[i] == null)
                {
                    var Cell = GetCell();

                    double dx = Canvas.GetLeft(Parent) + Hexagon.Offset * Math.Cos(i * Math.PI / 3d);
                    double dy = Canvas.GetTop(Parent) + Hexagon.Offset * Math.Sin(i * Math.PI / 3d);
                    Canvas.SetLeft(Cell, dx);
                    Canvas.SetTop(Cell, dy);

                    _canvas.Children.Add(Cell);

                    Parent.Neighbors[i] = Cell;
                }
            }

            // Set the cross-pointers on the 6 surrounding nodes.
            for (int i = 0; i < 6; ++i)
            {
                Hexagon child = Parent.Neighbors[i];
                if (child != null)
                {
                    int ip3 = (i + 3) % 6;
                    child.Neighbors[ip3] = Parent;

                    int ip1 = (i + 1) % 6;
                    int ip2 = (i + 2) % 6;
                    int im1 = (i + 5) % 6;
                    int im2 = (i + 4) % 6;
                    child.Neighbors[ip2] = Parent.Neighbors[ip1];
                    child.Neighbors[im2] = Parent.Neighbors[im1];
                }
            }

            // Recurse into each of the 6 surrounding nodes.
            for (int i = 0; i < 6; ++i)
                InitializeCells(Parent.Neighbors[i], generation + 1);
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCellClicked(object sender, RoutedEventArgs e)
            => SetCurrentValue(SelectedColorProperty, (sender as Hexagon).NominalColor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnGenerationsChanged(int Value) => InitializeCells();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectedColorChanged(Color Value)
            => SelectedColorChanged?.Invoke(this, new EventArgs<object>(Value));

        #endregion

        #endregion
    }
}