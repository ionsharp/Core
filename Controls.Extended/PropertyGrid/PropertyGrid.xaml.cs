using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Data;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using Imagin.Controls.Common;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
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

        public event EventHandler<EventArgs<object>> SelectedObjectChanged;

        public static DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(FrameworkElementCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FrameworkElementCollection Buttons
        {
            get
            {
                return (FrameworkElementCollection)GetValue(ButtonsProperty);
            }
            set
            {
                SetValue(ButtonsProperty, value);
            }
        }

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

        /*
        public static DependencyProperty DateTimeFormatProperty = DependencyProperty.Register("DateTimeFormat", typeof(DateTimeFormat), typeof(PropertyGrid), new FrameworkPropertyMetadata(DateTimeFormat.FullDateTime, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DateTimeFormat DateTimeFormat
        {
            get
            {
                return (DateTimeFormat)GetValue(DateTimeFormatProperty);
            }
            set
            {
                SetValue(DateTimeFormatProperty, value);
            }
        }
        */

        public static DependencyProperty FileSizeFormatProperty = DependencyProperty.Register("FileSizeFormat", typeof(FileSizeFormat), typeof(PropertyGrid), new FrameworkPropertyMetadata(FileSizeFormat.BinaryUsingSI, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FileSizeFormat FileSizeFormat
        {
            get
            {
                return (FileSizeFormat)GetValue(FileSizeFormatProperty);
            }
            set
            {
                SetValue(FileSizeFormatProperty, value);
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

        public static DependencyProperty GridStyleProperty = DependencyProperty.Register("GridStyle", typeof(Style), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style GridStyle
        {
            get
            {
                return (Style)GetValue(GridStyleProperty);
            }
            set
            {
                SetValue(GridStyleProperty, value);
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

        public static DependencyProperty LoaderTemplateProperty = DependencyProperty.Register("LoaderTemplate", typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate LoaderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(LoaderTemplateProperty);
            }
            set
            {
                SetValue(LoaderTemplateProperty, value);
            }
        }
        
        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        public bool IsLoading
        {
            get
            {
                return (bool)GetValue(IsLoadingProperty);
            }
            set
            {
                SetValue(IsLoadingProperty, value);
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

        public static DependencyProperty PropertyDescriptionStyleProperty = DependencyProperty.Register("PropertyDescriptionStyle", typeof(Style), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style PropertyDescriptionStyle
        {
            get
            {
                return (Style)GetValue(PropertyDescriptionStyleProperty);
            }
            set
            {
                SetValue(PropertyDescriptionStyleProperty, value);
            }
        }

        public static DependencyProperty PropertyDescriptionTemplateProperty = DependencyProperty.Register("PropertyDescriptionTemplate", typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate PropertyDescriptionTemplate
        {
            get
            {
                return (DataTemplate)GetValue(PropertyDescriptionTemplateProperty);
            }
            set
            {
                SetValue(PropertyDescriptionTemplateProperty, value);
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
            Object.As<PropertyGrid>().OnSelectedObjectChanged((object)e.NewValue);
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

        public static DependencyProperty ShowTypeProperty = DependencyProperty.Register("ShowType", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowType
        {
            get
            {
                return (bool)GetValue(ShowTypeProperty);
            }
            set
            {
                SetValue(ShowTypeProperty, value);
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

        public static DependencyProperty AcceptNullObjectsProperty = DependencyProperty.Register("AcceptNullObjects", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        public bool AcceptNullObjects
        {
            get
            {
                return (bool)GetValue(AcceptNullObjectsProperty);
            }
            set
            {
                SetValue(AcceptNullObjectsProperty, value);
            }
        }

        #endregion

        #region PropertyGrid

        public PropertyGrid()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(EditCollectionCommand, this.EditCollectionCommand_Executed, this.EditCollectionCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(ResetCommand, this.ResetCommand_Executed, this.ResetCommand_CanExecute));

            Buttons = new FrameworkElementCollection();
            Properties = new PropertyModelCollection();
            ListCollectionView = new ListCollectionView(Properties);
            Sort();
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
            foreach (var i in Properties)
                i.Value = null;
        }
        void ResetCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Properties != null && Properties.Count > 0;
        }

        #endregion

        #region Protected

        protected void Group(string PropertyName)
        {
            if (ListCollectionView != null)
            {
                ListCollectionView.GroupDescriptions.Clear();

                if (ShowCategories)
                    ListCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyName));
            }

        }

        protected void Sort()
        {
            if (ListCollectionView != null)
            {
                ListCollectionView.SortDescriptions.Clear();

                var SortDirection = this.IsSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                if (ShowCategories)
                    ListCollectionView.SortDescriptions.Add(new SortDescription("Category", SortDirection));

                ListCollectionView.SortDescriptions.Add(new SortDescription(SortByName ? "Name" : "Type", SortDirection));
            }
        }

        #endregion

        #region Virtual

        protected virtual async Task OnSelectedObjectChanged(object Value)
        {
            if (SelectedObjectChanged != null)
                SelectedObjectChanged(this, new EventArgs<object>(Value));

            if (Value != null)
            {
                Properties.Featured = null;
                Properties.Object = Value;
                Properties.Clear();

                IsLoading = true;
                await SetObject(Value);
                IsLoading = false;
            }
            else if (AcceptNullObjects)
            {
                Properties.Featured = null;
                Properties.Object = null;
                Properties.Clear();
            }
        }

        protected virtual void OnSortChanged()
        {
            Sort();
        }

        /// <summary>
        /// Populate this.Properties with PropertyModels representing each member of the SelectedObject.
        /// </summary>
        protected virtual async Task SetObject(object Value)
        {
            await Properties.BeginFromObject(Value);
        }

        #endregion

        #endregion
    }
}
