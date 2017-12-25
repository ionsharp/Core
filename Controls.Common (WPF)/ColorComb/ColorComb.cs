using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorComb : UserControl
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<object>> SelectedColorChanged;

        Canvas PART_Canvas
        {
            get; set;
        }

        HexagonButton PART_RootCell;
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ItemStrokeThicknessProperty = DependencyProperty.Register("ItemStrokeThickness", typeof(double), typeof(ColorComb), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double ItemStrokeThickness
        {
            get
            {
                return (double)GetValue(ItemStrokeThicknessProperty);
            }
            set
            {
                SetValue(ItemStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MaxGenerationsProperty = DependencyProperty.Register("MaxGenerations", typeof(int), typeof(ColorComb), new FrameworkPropertyMetadata(8, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMaxGenerationsChanged));
        /// <summary>
        /// 
        /// </summary>
        public int MaxGenerations
        {
            get
            {
                return (int)GetValue(MaxGenerationsProperty);
            }
            set
            {
                SetValue(MaxGenerationsProperty, value);
            }
        }
        static void OnMaxGenerationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorComb)d).OnMaxGenerationsChanged((int)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorComb), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            private set
            {
                SetValue(SelectedColorProperty, value);
            }
        }
        static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorComb)d).OnSelectedColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TotalChildrenProperty = DependencyProperty.Register("TotalChildren", typeof(int), typeof(ColorComb), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public int TotalChildren
        {
            get
            {
                return (int)GetValue(TotalChildrenProperty);
            }
            private set
            {
                SetValue(TotalChildrenProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty UsesGradientsProperty = DependencyProperty.Register("UsesGradients", typeof(bool), typeof(ColorComb), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool UsesGradients
        {
            get
            {
                return (bool)GetValue(UsesGradientsProperty);
            }
            set
            {
                SetValue(UsesGradientsProperty, value);
            }
        }

        #endregion

        #region ColorComb

        /// <summary>
        /// 
        /// </summary>
        public ColorComb()
        {
            DefaultStyleKey = typeof(ColorComb);
        }

        #endregion

        #region Methods

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            ApplyTemplate();

            PART_Canvas = Template.FindName("PART_Canvas", this) as Canvas;
            InitializeCells();
        }

        #endregion

        #region Private

        /// <summary>
        /// Cascade all cells with color.
        /// </summary>
        void Cascade()
        {
            PART_RootCell.NominalColor = Color.FromScRgb(1f, 1f, 1f, 1f);
            Cascade(PART_RootCell);
            foreach (HexagonButton i in PART_Canvas.Children)
                i.Visited = false;
        }

        /// <summary>
        /// Cascade child cells with color; this method is recursive.
        /// </summary>
        /// <param name="parent"></param>
        void Cascade(HexagonButton parent)
        {
            float delta = 1f / MaxGenerations;
            float ceiling = 0.99f;

            System.Collections.Generic.List<HexagonButton> visitedNodes =
                new System.Collections.Generic.List<HexagonButton>(6);

            for (int i = 0; i < 6; ++i)
            {
                HexagonButton child = parent.Neighbors[i];
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
            foreach (HexagonButton child in visitedNodes)
                Cascade(child);
        }

        HexagonButton GetCell()
        {
            var cell = new HexagonButton(UsesGradients);
            cell.Click += new RoutedEventHandler(OnCellClicked);

            BindingOperations.SetBinding(cell, HexagonButton.UsesGradientsProperty, new Binding()
            {
                Path = new PropertyPath("UsesGradients"),
                Source = this
            });
            BindingOperations.SetBinding(cell, HexagonButton.StrokeThicknessProperty, new Binding()
            {
                Path = new PropertyPath("ItemStrokeThickness"),
                Source = this
            });

            return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        async void InitializeCells()
        {
            await Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                PART_Canvas.Height = PART_Canvas.Width = (HexagonButton.Radius * 2.0) * Convert.ToDouble((MaxGenerations * 2) + 1);
                PART_Canvas.Children.Clear();

                //Define comb of 127 hexagons, starting in center of canvas.
                PART_RootCell = GetCell();

                Canvas.SetLeft(PART_RootCell, PART_Canvas.Width / 2);
                Canvas.SetTop(PART_RootCell, PART_Canvas.Height / 2);

                PART_Canvas.Children.Add(PART_RootCell);

                //Expand outward
                InitializeCells(PART_RootCell, 1);

                Cascade();

                SetCurrentValue(TotalChildrenProperty, PART_Canvas.Children.Count);
            }));
        }

        /// <summary>
        /// Initialize surrounding cells; this method is recursive.
        /// </summary>
        void InitializeCells(HexagonButton Parent, int generation)
        {
            if (generation > MaxGenerations)
                return;

            for (int i = 0; i < 6; ++i)
            {
                if (Parent.Neighbors[i] == null)
                {
                    HexagonButton Cell = this.GetCell();

                    double dx = Canvas.GetLeft(Parent) + HexagonButton.Offset * Math.Cos(i * Math.PI / 3d);
                    double dy = Canvas.GetTop(Parent) + HexagonButton.Offset * Math.Sin(i * Math.PI / 3d);
                    Canvas.SetLeft(Cell, dx);
                    Canvas.SetTop(Cell, dy);

                    PART_Canvas.Children.Add(Cell);

                    Parent.Neighbors[i] = Cell;
                }
            }

            // Set the cross-pointers on the 6 surrounding nodes.
            for (int i = 0; i < 6; ++i)
            {
                HexagonButton child = Parent.Neighbors[i];
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
        {
            SetCurrentValue(SelectedColorProperty, (sender as HexagonButton).NominalColor);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectedColorChanged(Color Value)
        {
            SelectedColorChanged?.Invoke(this, new EventArgs<object>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnMaxGenerationsChanged(int Value)
        {
            InitializeCells();
        }

        #endregion

        #endregion
    }
}