using Imagin.Common.Events;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

namespace Imagin.Controls.Extended
{
    public class ColorComb : UserControl
    {
        #region Properties

        public event EventHandler<EventArgs<object>> SelectedColorChanged;

        Canvas PART_Canvas
        {
            get; set;
        }

        HexagonButton PART_RootCell;
        
        public static DependencyProperty ItemStrokeThicknessProperty = DependencyProperty.Register("ItemStrokeThickness", typeof(double), typeof(ColorComb), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty MaxGenerationsProperty = DependencyProperty.Register("MaxGenerations", typeof(int), typeof(ColorComb), new FrameworkPropertyMetadata(8, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMaxGenerationsChanged));
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
        static void OnMaxGenerationsChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ColorComb ColorComb = (ColorComb)Object;
            ColorComb.InitializeChildren();
        }

        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorComb), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }

        public static DependencyProperty TotalChildrenProperty = DependencyProperty.Register("TotalChildren", typeof(int), typeof(ColorComb), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty UsesGradientsProperty = DependencyProperty.Register("UsesGradients", typeof(bool), typeof(ColorComb), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public ColorComb()
        {
            this.DefaultStyleKey = typeof(ColorComb);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize children.
        /// </summary>
        async void InitializeChildren()
        {
            this.SetSize();
            await Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                PART_Canvas.Children.Clear();

                //Define comb of 127 hexagons, starting in center of canvas.
                PART_RootCell = this.GetCell();

                Canvas.SetLeft(PART_RootCell, PART_Canvas.Width / 2);
                Canvas.SetTop(PART_RootCell, PART_Canvas.Height / 2);

                PART_Canvas.Children.Add(PART_RootCell);

                //Expand outward
                InitializeChildren(PART_RootCell, 1);

                CascadeChildColors();

                this.TotalChildren = this.PART_Canvas.Children.Count;
            }));
        }

        /// <summary>
        /// Initialize surrounding nodes; this method is recursive.
        /// </summary>
        void InitializeChildren(HexagonButton Parent, int generation)
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
                InitializeChildren(Parent.Neighbors[i], generation + 1);
        }

        /// <summary>
        /// Cascade children with color.
        /// </summary>
        void CascadeChildColors()
        {
            PART_RootCell.NominalColor = Color.FromScRgb(1f, 1f, 1f, 1f);
            CascadeChildColors(PART_RootCell);
            foreach (HexagonButton h in PART_Canvas.Children)
                h.Visited = false;
        }

        /// <summary>
        /// Cascase children with color; this method is recursive.
        /// </summary>
        /// <param name="parent"></param>
        void CascadeChildColors(HexagonButton parent)
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
                CascadeChildColors(child);
        }

        HexagonButton GetCell()
        {
            var HexagonButton = new HexagonButton(this.UsesGradients);
            HexagonButton.Click += new RoutedEventHandler(this.OnCellClicked);
            this.SetBindings(HexagonButton);
            return HexagonButton;
        }

        void SetBindings (HexagonButton Cell)
        {
            Binding Binding = new Binding()
            {
                Path = new PropertyPath("UsesGradients"),
                Source = this
            };
            BindingOperations.SetBinding(Cell, HexagonButton.UsesGradientsProperty, Binding);
            Binding = new Binding()
            {
                Path = new PropertyPath("ItemStrokeThickness"),
                Source = this
            };
            BindingOperations.SetBinding(Cell, HexagonButton.StrokeThicknessProperty, Binding);
        }

        void SetSize()
        {
            int MaxRowItems = (this.MaxGenerations * 2) + 1;
            double CanvasLength = (HexagonButton.Radius * 2.0) * Convert.ToDouble(MaxRowItems);
            PART_Canvas.Width = CanvasLength;
            PART_Canvas.Height = CanvasLength;
        }

        #region Events

        void OnCellClicked(object sender, RoutedEventArgs e)
        {
            HexagonButton cell = sender as HexagonButton;

            SelectedColor = cell.NominalColor;
            OnColorSelected();
        }

        void OnColorSelected()
        {
            if (SelectedColorChanged != null)
                SelectedColorChanged(this, new EventArgs<object>(SelectedColor));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            this.PART_Canvas = this.Template.FindName("PART_Canvas", this) as Canvas;
            this.InitializeChildren();
        }

        #endregion

        #endregion
    }
}