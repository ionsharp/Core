using Imagin.Common.Linq;
using Imagin.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TabbedTree : TreeView
    {
        #region Properties

        ContentControl PART_ContentHeader
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnResizeModeProperty = DependencyProperty.Register("ColumnResizeMode", typeof(ColumnResizeMode), typeof(TabbedTree), new FrameworkPropertyMetadata(ColumnResizeMode.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ColumnResizeMode ColumnResizeMode
        {
            get
            {
                return (ColumnResizeMode)GetValue(ColumnResizeModeProperty);
            }
            set
            {
                SetValue(ColumnResizeModeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentBackgroundProperty = DependencyProperty.Register("ContentBackground", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentBorderBrushProperty = DependencyProperty.Register("ContentBorderBrush", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentBorderThicknessProperty = DependencyProperty.Register("ContentBorderThickness", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentHeaderTemplateProperty = DependencyProperty.Register("ContentHeaderTemplate", typeof(DataTemplate), typeof(TabbedTree), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentHeaderVisibilityProperty = DependencyProperty.Register("ContentHeaderVisibility", typeof(Visibility), typeof(TabbedTree), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentPaddingProperty = DependencyProperty.Register("ContentPadding", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TabbedTree), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(TabbedTree), new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplateSelector ContentTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
            }
            set
            {
                SetValue(ContentTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentTransitionProperty = DependencyProperty.Register("ContentTransition", typeof(Transitions), typeof(TabbedTree), new FrameworkPropertyMetadata(Transitions.LeftReplace, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Transitions ContentTransition
        {
            get
            {
                return (Transitions)GetValue(ContentTransitionProperty);
            }
            set
            {
                SetValue(ContentTransitionProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(GridLength), typeof(TabbedTree), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MenuWidthProperty = DependencyProperty.Register("MenuWidth", typeof(GridLength), typeof(TabbedTree), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MenuBackgroundProperty = DependencyProperty.Register("MenuBackground", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MenuBorderBrushProperty = DependencyProperty.Register("MenuBorderBrush", typeof(Brush), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MenuBorderThicknessProperty = DependencyProperty.Register("MenuBorderThickness", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MenuPaddingProperty = DependencyProperty.Register("MenuPadding", typeof(Thickness), typeof(TabbedTree), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SplitterWidthProperty = DependencyProperty.Register("SplitterWidth", typeof(double), typeof(TabbedTree), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double SplitterWidth
        {
            get
            {
                return (double)GetValue(SplitterWidthProperty);
            }
            set
            {
                SetValue(SplitterWidthProperty, value);
            }
        }
        
        #endregion

        #region TabbedTree

        /// <summary>
        /// 
        /// </summary>
        public TabbedTree() : base()
        {
            DefaultStyleKey = typeof(TabbedTree);
            SelectedItemChanged += OnSelectedItemChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (PART_ContentHeader != null)
                PART_ContentHeader.Content = e.NewValue.As<TreeViewItem>()?.Header.ToString() ?? e.NewValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            PART_ContentHeader = Template.FindName("PART_ContentHeader", this) as ContentControl;
        }

        #endregion
    }
}