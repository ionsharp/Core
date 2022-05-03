using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A WrapPanel with alignable content.
    /// </summary>
    /// <remarks>
    /// Borrowed from http://stackoverflow.com/questions/806777/wpf-how-can-i-center-all-items-in-a-wrappanel.
    /// </remarks>
    public class AlignableWrapPanel : Panel
    {
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(AlignableWrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), IsWidthHeightValid);
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(AlignableWrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), IsWidthHeightValid);
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(AlignableWrapPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, OnOrientationChanged));
        Orientation orientation;
        public Orientation Orientation
        {
            get => orientation;
            set => SetValue(OrientationProperty, value);
        }
        static void OnOrientationChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((AlignableWrapPanel)i).orientation = new Value<Orientation>(e).New;

        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register(nameof(HorizontalContentAlignment), typeof(HorizontalAlignment), typeof(AlignableWrapPanel), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsArrange));
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register(nameof(VerticalContentAlignment), typeof(VerticalAlignment), typeof(AlignableWrapPanel), new FrameworkPropertyMetadata(VerticalAlignment.Top, FrameworkPropertyMetadataOptions.AffectsArrange));
        public VerticalAlignment VerticalContentAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }

        struct UVSize
        {
            readonly Orientation orientation;

            internal double U;

            internal double V;

            internal double Width
            {
                get { return (orientation == Orientation.Horizontal ? U : V); }
                private set { if (orientation == Orientation.Horizontal) U = value; else V = value; }
            }

            internal double Height
            {
                get { return (orientation == Orientation.Horizontal ? V : U); }
                private set { if (orientation == Orientation.Horizontal) V = value; else U = value; }
            }

            internal UVSize(Orientation orientation, double width, double height)
            {
                U = V = 0d;
                this.orientation = orientation;
                Width = width;
                Height = height;
            }

            internal UVSize(Orientation orientation)
            {
                U = V = 0d;
                this.orientation = orientation;
            }
        }

        public AlignableWrapPanel() : base() { }

        protected override Size MeasureOverride(Size constraint)
        {
            var curLineSize = new UVSize(Orientation);
            var panelSize = new UVSize(Orientation);

            var uvConstraint = new UVSize(Orientation, constraint.Width, constraint.Height);

            var itemWidth = ItemWidth;
            var itemHeight = ItemHeight;

            var itemWidthSet = !double.IsNaN(itemWidth);
            var itemHeightSet = !double.IsNaN(itemHeight);

            var childConstraint = new Size((itemWidthSet ? itemWidth : constraint.Width), (itemHeightSet ? itemHeight : constraint.Height));

            var children = InternalChildren;
            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                if (child == null) continue;

                //Flow passes its own constrint to children 
                child.Measure(childConstraint);

                //this is the size of the child in UV space 
                var sz = new UVSize(
                        Orientation,
                        (itemWidthSet ? itemWidth : child.DesiredSize.Width),
                        (itemHeightSet ? itemHeight : child.DesiredSize.Height));

                if (curLineSize.U + sz.U > uvConstraint.U)
                {
                    //need to switch to another line 
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    if (!(sz.U > uvConstraint.U)) continue;
                    //the element is wider then the constrint - give it a separate line
                    panelSize.U = Math.Max(sz.U, panelSize.U);
                    panelSize.V += sz.V;
                    curLineSize = new UVSize(Orientation);
                }
                else
                {
                    //continue to accumulate a line
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }
            }

            //the last line size, if any should be added 
            panelSize.U = Math.Max(curLineSize.U, panelSize.U);
            panelSize.V += curLineSize.V;

            //go from UV space to W/H space
            return new Size(panelSize.Width, panelSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var firstInLine = 0;
            var itemWidth = ItemWidth;
            var itemHeight = ItemHeight;
            double accumulatedV = 0;
            var itemU = (Orientation == Orientation.Horizontal ? itemWidth : itemHeight);
            var curLineSize = new UVSize(Orientation);
            var uvFinalSize = new UVSize(Orientation, finalSize.Width, finalSize.Height);
            var itemWidthSet = !double.IsNaN(itemWidth);
            var itemHeightSet = !double.IsNaN(itemHeight);
            var useItemU = (Orientation == Orientation.Horizontal ? itemWidthSet : itemHeightSet);

            var children = InternalChildren;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                if (child == null) continue;

                var sz = new UVSize(
                        Orientation,
                        (itemWidthSet ? itemWidth : child.DesiredSize.Width),
                        (itemHeightSet ? itemHeight : child.DesiredSize.Height));

                if (curLineSize.U + sz.U > uvFinalSize.U)
                {
                    //need to switch to another line 
                    ArrangeLine(finalSize, accumulatedV, curLineSize, firstInLine, i, useItemU, itemU);

                    accumulatedV += curLineSize.V;
                    curLineSize = sz;

                    if (sz.U > uvFinalSize.U)
                    {
                        //the element is wider then the constraint - give it a separate line 
                        //switch to next line which only contain one element 
                        ArrangeLine(finalSize, accumulatedV, sz, i, ++i, useItemU, itemU);

                        accumulatedV += sz.V;
                        curLineSize = new UVSize(Orientation);
                    }

                    firstInLine = i;
                }
                else
                {
                    //continue to accumulate a line
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }
            }
            //arrange the last line, if any
            if (firstInLine < children.Count)
                ArrangeLine(finalSize, accumulatedV, curLineSize, firstInLine, children.Count, useItemU, itemU);
            return finalSize;
        }

        void ArrangeLine(Size finalSize, double v, UVSize line, int start, int end, bool useItemU, double itemU)
        {
            double u;
            var isHorizontal = Orientation == Orientation.Horizontal;

            if (orientation == Orientation.Vertical)
            {
                u = VerticalContentAlignment switch
                {
                    VerticalAlignment.Center => (finalSize.Height - line.U) / 2,
                    VerticalAlignment.Bottom => finalSize.Height - line.U,
                    _ => 0,
                };
            }
            else
            {
                u = HorizontalContentAlignment switch
                {
                    HorizontalAlignment.Center => (finalSize.Width - line.U) / 2,
                    HorizontalAlignment.Right => finalSize.Width - line.U,
                    _ => 0,
                };
            }

            var children = InternalChildren;
            for (var i = start; i < end; i++)
            {
                var child = children[i];
                if (child == null) continue;
                var childSize = new UVSize(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                var layoutSlotU = (useItemU ? itemU : childSize.U);
                child.Arrange(new Rect(
                        isHorizontal ? u : v,
                        isHorizontal ? v : u,
                        isHorizontal ? layoutSlotU : line.V,
                        isHorizontal ? line.V : layoutSlotU));
                u += layoutSlotU;
            }
        }

        static bool IsWidthHeightValid(object value)
        {
            var v = (double)value;
            return (double.IsNaN(v)) || (v >= 0.0d && !double.IsPositiveInfinity(v));
        }
    }
}