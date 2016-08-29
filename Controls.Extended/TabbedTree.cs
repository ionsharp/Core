using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public class TabbedTree : TreeView
    {
        #region DependencyProperties

        public static DependencyProperty ContentBackgroundProperty = DependencyProperty.Register("ContentBackground", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush ContentBackground
        {
            get
            {
                return (Brush)GetValue(ContentBackgroundProperty);
            }
            set
            {
                SetValue(ContentBackgroundProperty, value);
            }
        }

        public static DependencyProperty ContentBorderBrushProperty = DependencyProperty.Register("ContentBorderBrush", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush ContentBorderBrush
        {
            get
            {
                return (Brush)GetValue(ContentBorderBrushProperty);
            }
            set
            {
                SetValue(ContentBorderBrushProperty, value);
            }
        }

        public static DependencyProperty ContentBorderThicknessProperty = DependencyProperty.Register("ContentBorderThickness", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ContentBorderThickness
        {
            get
            {
                return (Thickness)GetValue(ContentBorderThicknessProperty);
            }
            set
            {
                SetValue(ContentBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty ContentPaddingProperty = DependencyProperty.Register("ContentPadding", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ContentPadding
        {
            get
            {
                return (Thickness)GetValue(ContentPaddingProperty);
            }
            set
            {
                SetValue(ContentPaddingProperty, value);
            }
        }

        public static DependencyProperty MenuWidthProperty = DependencyProperty.Register("MenuWidth", typeof(GridLength), typeof(TabbedTree), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength MenuWidth
        {
            get
            {
                return (GridLength)GetValue(MenuWidthProperty);
            }
            set
            {
                SetValue(MenuWidthProperty, value);
            }
        }

        public static DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(GridLength), typeof(TabbedTree), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength ContentWidth
        {
            get
            {
                return (GridLength)GetValue(ContentWidthProperty);
            }
            set
            {
                SetValue(ContentWidthProperty, value);
            }
        }

        public static DependencyProperty MenuBorderThicknessProperty = DependencyProperty.Register("MenuBorderThickness", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness MenuBorderThickness
        {
            get
            {
                return (Thickness)GetValue(MenuBorderThicknessProperty);
            }
            set
            {
                SetValue(MenuBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty MenuBackgroundProperty = DependencyProperty.Register("MenuBackground", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MenuBackground
        {
            get
            {
                return (Brush)GetValue(MenuBackgroundProperty);
            }
            set
            {
                SetValue(MenuBackgroundProperty, value);
            }
        }

        public static DependencyProperty MenuBorderBrushProperty = DependencyProperty.Register("MenuBorderBrush", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MenuBorderBrush
        {
            get
            {
                return (Brush)GetValue(MenuBorderBrushProperty);
            }
            set
            {
                SetValue(MenuBorderBrushProperty, value);
            }
        }

        public static DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(string), typeof(TabbedTree), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged));
        public string SelectedIndex
        {
            get
            {
                return (string)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        private static void OnSelectedIndexChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            TabbedTree TabbedTree = (TabbedTree)Object;
            TabbedTree.SelectIndex(TabbedTree.SelectedIndex);
        }

        #endregion

        #region TabbedTree

        public TabbedTree() : base()
        {
            this.DefaultStyleKey = typeof(TabbedTree);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            //If a selected index is not specified, select first by default.
            if (!string.IsNullOrEmpty(this.SelectedIndex))
                this.SelectIndex(this.SelectedIndex);
            else this.SetSelectedIndex(0);
        }

        #endregion

        #region Methods

        public void SetSelectedIndex(params int[] Values)
        {
            string Temp = string.Empty;
            foreach (int i in Values) Temp += i.ToString() + ",";
            Temp = Temp.TrimEnd(',');
            this.SelectedIndex = Temp;
        }

        List<int> GetIndices(string Index)
        {
            if (!string.IsNullOrEmpty(Index))
            {
                List<int> Values = new List<int>();
                string[] OldValues = Index.Split(',');
                try
                {
                    for (int i = 0, Count = OldValues.Count(); i < Count; i++)
                        Values.Add(Convert.ToInt32(OldValues[i]));
                    return Values;
                }
                catch
                {

                }
            }
            return default(List<int>);
        }

        /// <summary>
        /// An array that represents the index depth.
        /// </summary>
        /// <param name="Index">0-based index.</param>
        void SelectIndex(string Index)
        {
            List<int> Values = this.GetIndices(Index);
            if (Values == null)
                return;
            TreeViewItem Target = null;
            foreach (int i in Values)
            {
                if (Target == null)
                {
                    if (this.Items.Count > i)
                        Target = this.Items[i] as TreeViewItem; //Can never be null; guarentees Target != null after first pass or breaks.
                    else break;
                }
                else
                {
                    if (Target.Items.Count > i)
                        Target = Target.Items[i] as TreeViewItem;
                    else break;
                }
            }
            if (Target != null)
                Target.IsSelected = true;
        }

        #endregion
    }
}