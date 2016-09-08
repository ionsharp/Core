using Imagin.Common.Events;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class PropertyGrid : UserControl
    {
        #region Properties

        #region Events

        public event EventHandler<ObjectEventArgs> SelectedObjectChanged;

        #endregion

        #region Dependency

        public static DependencyProperty GridBackgroundProperty = DependencyProperty.Register("GridBackground", typeof(Brush), typeof(PropertyGrid), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush GridBackground
        {
            get
            {
                return (Brush)GetValue(GridBackgroundProperty);
            }
            set
            {
                SetValue(GridBackgroundProperty, value);
            }
        }

        public static DependencyProperty GridBorderBrushProperty = DependencyProperty.Register("GridBorderBrush", typeof(Brush), typeof(PropertyGrid), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush GridBorderBrush
        {
            get
            {
                return (Brush)GetValue(GridBorderBrushProperty);
            }
            set
            {
                SetValue(GridBorderBrushProperty, value);
            }
        }

        public static DependencyProperty GridBorderThicknessProperty = DependencyProperty.Register("GridBorderThickness", typeof(Thickness), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness GridBorderThickness
        {
            get
            {
                return (Thickness)GetValue(GridBorderThicknessProperty);
            }
            set
            {
                SetValue(GridBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty AlternationCountProperty = DependencyProperty.Register("AlternationCount", typeof(int), typeof(PropertyGrid), new FrameworkPropertyMetadata(2, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int AlternationCount
        {
            get
            {
                return (int)GetValue(AlternationCountProperty);
            }
            set
            {
                SetValue(AlternationCountProperty, value);
            }
        }

        public static DependencyProperty HeadersVisibilityProperty = DependencyProperty.Register("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(DataGridHeadersVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataGridHeadersVisibility HeadersVisibility
        {
            get
            {
                return (DataGridHeadersVisibility)GetValue(HeadersVisibilityProperty);
            }
            set
            {
                SetValue(HeadersVisibilityProperty, value);
            }
        }

        public static DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(DataGridGridLinesVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataGridGridLinesVisibility GridLinesVisibility
        {
            get
            {
                return (DataGridGridLinesVisibility)GetValue(GridLinesVisibilityProperty);
            }
            set
            {
                SetValue(GridLinesVisibilityProperty, value);
            }
        }

        public static DependencyProperty ShowFeaturedProperty = DependencyProperty.Register("ShowFeatured", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowFeatured
        {
            get
            {
                return (bool)GetValue(ShowFeaturedProperty);
            }
            set
            {
                SetValue(ShowFeaturedProperty, value);
            }
        }

        public static DependencyProperty SearchQueryProperty = DependencyProperty.Register("SearchQuery", typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string SearchQuery
        {
            get
            {
                return (string)GetValue(SearchQueryProperty);
            }
            set
            {
                SetValue(SearchQueryProperty, value);
            }
        }

        public static DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(PropertyItemCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PropertyItemCollection Properties
        {
            get
            {
                return (PropertyItemCollection)GetValue(PropertiesProperty);
            }
            set
            {
                SetValue(PropertiesProperty, value);
            }
        }

        public static DependencyProperty ListCollectionViewProperty = DependencyProperty.Register("ListCollectionView", typeof(ListCollectionView), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ListCollectionView ListCollectionView
        {
            get
            {
                return (ListCollectionView)GetValue(ListCollectionViewProperty);
            }
            set
            {
                SetValue(ListCollectionViewProperty, value);
            }
        }

        public static DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));
        public object SelectedObject
        {
            get
            {
                return (object)GetValue(SelectedObjectProperty);
            }
            set
            {
                SetValue(SelectedObjectProperty, value);
            }
        }
        private static void OnSelectedObjectChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            PropertyGrid PropertyGrid = (PropertyGrid)Object;
            PropertyGrid.OnSelectedObjectChanged();
        }

        public static DependencyProperty ShowHeaderProperty = DependencyProperty.Register("ShowHeader", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowHeader
        {
            get
            {
                return (bool)GetValue(ShowHeaderProperty);
            }
            set
            {
                SetValue(ShowHeaderProperty, value);
            }
        }

        public static DependencyProperty IsSortAscendingProperty = DependencyProperty.Register("IsSortAscending", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsSortAscendingChanged));
        public bool IsSortAscending
        {
            get
            {
                return (bool)GetValue(IsSortAscendingProperty);
            }
            set
            {
                SetValue(IsSortAscendingProperty, value);
            }
        }
        private static void OnIsSortAscendingChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            PropertyGrid PropertyGrid = (PropertyGrid)Object;
            if (PropertyGrid.IsSortAscending) PropertyGrid.Sort(ListSortDirection.Ascending);
        }

        public static DependencyProperty IsSortDescendingProperty = DependencyProperty.Register("IsSortDescending", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsSortDescendingChanged));
        public bool IsSortDescending
        {
            get
            {
                return (bool)GetValue(IsSortDescendingProperty);
            }
            set
            {
                SetValue(IsSortDescendingProperty, value);
            }
        }
        private static void OnIsSortDescendingChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            PropertyGrid PropertyGrid = (PropertyGrid)Object;
            if (PropertyGrid.IsSortDescending)
                PropertyGrid.Sort(ListSortDirection.Descending);
        }

        public static DependencyProperty ShowCategoriesProperty = DependencyProperty.Register("ShowCategories", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowCategoriesChanged));
        public bool ShowCategories
        {
            get
            {
                return (bool)GetValue(ShowCategoriesProperty);
            }
            set
            {
                SetValue(ShowCategoriesProperty, value);
            }
        }
        private static void OnShowCategoriesChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            PropertyGrid PropertyGrid = (PropertyGrid)Object;
            PropertyGrid.Group("Category");
        }

        #endregion

        #endregion

        #region PropertyGrid

        public PropertyGrid()
        {
            InitializeComponent();
            this.Properties = new PropertyItemCollection();
            this.ListCollectionView = new ListCollectionView(this.Properties);
        }

        #endregion

        #region Methods

        #region Commands

        void Clear_Executed(object sender, RoutedEventArgs e)
        {
            this.SearchQuery = string.Empty;
        }

        void Reset_Executed(object sender, RoutedEventArgs e)
        {
            foreach (PropertyItem Item in this.Properties)
                Item.Value = null;
        }

        #endregion

        #region Protected

        protected void Group(string PropertyName)
        {
            if (this.ListCollectionView == null)
                return;
            this.ListCollectionView.GroupDescriptions.Clear();
            if (this.ShowCategories)
                this.ListCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyName));
        }

        protected void Sort(ListSortDirection Direction)
        {
            if (this.ListCollectionView == null)
                return;
            this.ListCollectionView.SortDescriptions.Clear();
            if (this.ShowCategories)
                this.ListCollectionView.SortDescriptions.Add(new SortDescription("Category", Direction));
            this.ListCollectionView.SortDescriptions.Add(new SortDescription("Name", Direction));
        }

        #endregion

        #region Virtual

        protected virtual void OnSelectedObjectChanged()
        {
            if (this.SelectedObjectChanged != null)
                this.SelectedObjectChanged(this, new ObjectEventArgs(this.SelectedObject));
            if (this.SelectedObject == null)
                return;
            this.Properties.Object = this.SelectedObject;
            this.Properties.Clear();
            this.SetObject();
        }

        /// <summary>
        /// Populate this.Properties with PropertyItems representing each member of the SelectedObject.
        /// </summary>
        protected virtual void SetObject()
        {
            this.Properties.BeginFromObject();
        }

        #endregion

        #endregion
    }
}
