using Imagin.Controls.Common;
using Imagin.Controls.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public class TabbedTree : AdvancedTreeView
    {
        #region Properties

        ContentControl PART_ContentHeader
        {
            get; set;
        }

        public static DependencyProperty CanResizeContentProperty = DependencyProperty.Register("CanResizeContent", typeof(bool), typeof(TabbedTree), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanResizeContent
        {
            get
            {
                return (bool)GetValue(CanResizeContentProperty);
            }
            set
            {
                SetValue(CanResizeContentProperty, value);
            }
        }

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

        public static DependencyProperty ContentHeaderTemplateProperty = DependencyProperty.Register("ContentHeaderTemplate", typeof(DataTemplate), typeof(TabbedTree), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ContentHeaderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentHeaderTemplateProperty);
            }
            set
            {
                SetValue(ContentHeaderTemplateProperty, value);
            }
        }

        public static DependencyProperty ContentHeaderVisibilityProperty = DependencyProperty.Register("ContentHeaderVisibility", typeof(Visibility), typeof(TabbedTree), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility ContentHeaderVisibility
        {
            get
            {
                return (Visibility)GetValue(ContentHeaderVisibilityProperty);
            }
            set
            {
                SetValue(ContentHeaderVisibilityProperty, value);
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

        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TabbedTree), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
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

        public static DependencyProperty MenuPaddingProperty = DependencyProperty.Register("MenuPadding", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness MenuPadding
        {
            get
            {
                return (Thickness)GetValue(MenuPaddingProperty);
            }
            set
            {
                SetValue(MenuPaddingProperty, value);
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
        static void OnSelectedIndexChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((TabbedTree)Object).OnSelectedIndexChanged((string)e.NewValue);
        }

        #endregion

        #region TabbedTree

        public TabbedTree() : base()
        {
            this.DefaultStyleKey = typeof(TabbedTree);

            this.SelectedItemChanged += OnSelectedItemChanged;
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            this.PART_ContentHeader = this.Template.FindName("PART_ContentHeader", this) as ContentControl;
        }

        #endregion

        #region Methods

        IEnumerable<int> GetIndices(string Index)
        {
            var Indices = Index.Split(',');
            foreach (var i in Indices)
            {
                var j = 0;
                if (!int.TryParse(i, out j))
                    continue;
                yield return j;
            }
        }

        void SelectTarget(IEnumerable<int> Indices, ItemsControl Control = null)
        {
            if (Indices != null)
            {
                var Target = this as ItemsControl;
                foreach (var i in Indices)
                {
                    if (Target.Items.Count > i)
                        Target = Target.Items[i] as ItemsControl;
                    else break;
                }
                if (Target != null && Target is TreeViewItem)
                    TreeViewItemExtensions.SetIsSelected(Target as TreeViewItem, true);
            }
        }

        protected virtual void OnSelectedIndexChanged(string Value)
        {
            this.SelectTarget(this.GetIndices(Value));
        }

        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.PART_ContentHeader != null)
            {
                if (e.NewValue is TreeViewItem)
                    this.PART_ContentHeader.Content = (e.NewValue as TreeViewItem).Header.ToString();
                else this.PART_ContentHeader.Content = e.NewValue;
            }
        }

        /// <summary>
        /// Set selected index by specifying numeric depth.
        /// </summary>
        /// <param name="Values"></param>
        public void SetSelectedIndex(params int[] Values)
        {
            var Result = string.Empty;
            foreach (var i in Values)
                Result += i.ToString() + ",";
            this.SelectedIndex = Result.TrimEnd(',');
        }

        #endregion
    }
}