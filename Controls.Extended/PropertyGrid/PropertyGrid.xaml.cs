using Imagin.Common.Events;
using Imagin.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class PropertyGrid : UserControl
    {
        #region Properties

        #region Events

        public event EventHandler<EventArgs<object>> SelectedObjectChanged;

        #endregion

        #region Dependency
        
        public static DependencyProperty CollapseGroupsProperty = DependencyProperty.Register("CollapseGroups", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CollapseGroups
        {
            get
            {
                return (bool)GetValue(CollapseGroupsProperty);
            }
            set
            {
                SetValue(CollapseGroupsProperty, value);
            }
        }

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

        public static DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(PropertyModelCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PropertyModelCollection Properties
        {
            get
            {
                return (PropertyModelCollection)GetValue(PropertiesProperty);
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
        static void OnSelectedObjectChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<PropertyGrid>().OnSelectedObjectChanged();
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
        
        public static DependencyProperty SortByNameProperty = DependencyProperty.Register("SortByName", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        public bool SortByName
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

        public static DependencyProperty SortByTypeProperty = DependencyProperty.Register("SortByType", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        public bool SortByType
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

        public static DependencyProperty IsSortAscendingProperty = DependencyProperty.Register("IsSortAscending", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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

        public static DependencyProperty IsSortDescendingProperty = DependencyProperty.Register("IsSortDescending", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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

        static void OnSortChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyGrid)Object).OnSortChanged();
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
        static void OnShowCategoriesChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<PropertyGrid>().Group("Category");
        }

        public static DependencyProperty ShowPropertyDescriptionProperty = DependencyProperty.Register("ShowPropertyDescription", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPropertyDescription
        {
            get
            {
                return (bool)GetValue(ShowPropertyDescriptionProperty);
            }
            set
            {
                SetValue(ShowPropertyDescriptionProperty, value);
            }
        }
        
        #endregion

        #endregion

        #region PropertyGrid

        public PropertyGrid()
        {
            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(EditCollectionCommand, this.EditCollectionCommand_Executed, this.EditCollectionCommand_CanExecute));
            this.CommandBindings.Add(new CommandBinding(ResetCommand, this.ResetCommand_Executed, this.ResetCommand_CanExecute));

            this.Properties = new PropertyModelCollection();
            this.ListCollectionView = new ListCollectionView(this.Properties);
            this.Sort();
        }

        #endregion

        #region Methods

        #region Commands

        public static readonly RoutedUICommand EditCollectionCommand = new RoutedUICommand("EditCollectionCommand", "EditCollectionCommand", typeof(PropertyGrid));
        void EditCollectionCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var Window = new Window()
            {
                Content = new CollectionEditor()
                {
                    Collection = e.Parameter
                },
                Height = 425,
                Title = "Edit Collection",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 720
            };
            Window.ShowDialog();
        }
        void EditCollectionCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }

        public static readonly RoutedUICommand ResetCommand = new RoutedUICommand("ResetCommand", "ResetCommand", typeof(PropertyGrid));
        void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var i in this.Properties)
                i.Value = null;
        }
        void ResetCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Properties != null && this.Properties.Count > 0;
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

        protected void Sort()
        {
            if (this.ListCollectionView != null)
            {
                this.ListCollectionView.SortDescriptions.Clear();

                var SortDirection = this.IsSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                if (this.ShowCategories)
                    this.ListCollectionView.SortDescriptions.Add(new SortDescription("Category", SortDirection));
                this.ListCollectionView.SortDescriptions.Add(new SortDescription(this.SortByName ? "Name" : "Type", SortDirection));
            }
        }

        #endregion

        #region Virtual

        protected virtual void OnSelectedObjectChanged()
        {
            if (this.SelectedObjectChanged != null)
                this.SelectedObjectChanged(this, new EventArgs<object>(this.SelectedObject));
            if (this.SelectedObject == null) return;
            this.Properties.Object = this.SelectedObject;
            this.Properties.Clear();
            this.SetObject();
        }

        protected virtual void OnSortChanged()
        {
            this.Sort();
        }

        /// <summary>
        /// Populate this.Properties with PropertyModels representing each member of the SelectedObject.
        /// </summary>
        protected virtual void SetObject()
        {
            this.Properties.BeginFromObject();
        }

        #endregion

        #endregion
    }
}
