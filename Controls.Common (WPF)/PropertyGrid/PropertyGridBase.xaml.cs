using Imagin.Common.Collections;
using Imagin.Common.Config;
using Imagin.Common.Data;
using Imagin.Common.Globalization;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class PropertyGridBase : UserControl
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected bool isNesting = false;

        /// <summary>
        /// Stores a reference to every nested property relative to the original host; properties are stored in order of depth.
        /// </summary>
        protected Stack<object> nest = new Stack<object>();

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AcceptsNullObjectsProperty = DependencyProperty.Register("AcceptsNullObjects", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool AcceptsNullObjects
        {
            get
            {
                return (bool)GetValue(AcceptsNullObjectsProperty);
            }
            set
            {
                SetValue(AcceptsNullObjectsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(FrameworkElementCollection), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanResizeDescriptionProperty = DependencyProperty.Register("CanResizeDescription", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCanResizeDescriptionChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool CanResizeDescription
        {
            get
            {
                return (bool)GetValue(CanResizeDescriptionProperty);
            }
            set
            {
                SetValue(CanResizeDescriptionProperty, value);
            }
        }
        static void OnCanResizeDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnCanResizeDescriptionChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CollapseGroupsProperty = DependencyProperty.Register("CollapseGroups", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DataGridStyleProperty = DependencyProperty.Register("DataGridStyle", typeof(Style), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style DataGridStyle
        {
            get
            {
                return (Style)GetValue(DataGridStyleProperty);
            }
            set
            {
                SetValue(DataGridStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DateTimeFormatProperty = DependencyProperty.Register("DateTimeFormat", typeof(string), typeof(PropertyGridBase), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string DateTimeFormat
        {
            get
            {
                return (string)GetValue(DateTimeFormatProperty);
            }
            set
            {
                SetValue(DateTimeFormatProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DescriptionStringFormatProperty = DependencyProperty.Register("DescriptionStringFormat", typeof(string), typeof(PropertyGridBase), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string DescriptionStringFormat
        {
            get
            {
                return (string)GetValue(DescriptionStringFormatProperty);
            }
            set
            {
                SetValue(DescriptionStringFormatProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DescriptionTemplateProperty = DependencyProperty.Register("DescriptionTemplate", typeof(DataTemplate), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate DescriptionTemplate
        {
            get
            {
                return (DataTemplate)GetValue(DescriptionTemplateProperty);
            }
            set
            {
                SetValue(DescriptionTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DescriptionTemplateSelectorProperty = DependencyProperty.Register("DescriptionTemplateSelector", typeof(DataTemplateSelector), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplateSelector DescriptionTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(DescriptionTemplateSelectorProperty);
            }
            set
            {
                SetValue(DescriptionTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DescriptionVisibilityProperty = DependencyProperty.Register("DescriptionVisibility", typeof(Visibility), typeof(PropertyGridBase), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDescriptionVisibilityChanged));
        /// <summary>
        /// 
        /// </summary>
        public Visibility DescriptionVisibility
        {
            get
            {
                return (Visibility)GetValue(DescriptionVisibilityProperty);
            }
            set
            {
                SetValue(DescriptionVisibilityProperty, value);
            }
        }
        static void OnDescriptionVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnDescriptionVisibilityChanged((Visibility)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FileSizeFormatProperty = DependencyProperty.Register("FileSizeFormat", typeof(FileSizeFormat), typeof(PropertyGridBase), new FrameworkPropertyMetadata(FileSizeFormat.BinaryUsingSI, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(PropertyGridBase), new FrameworkPropertyMetadata(DataGridGridLinesVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HeadersVisibilityProperty = DependencyProperty.Register("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(PropertyGridBase), new FrameworkPropertyMetadata(DataGridHeadersVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ListCollectionViewProperty = DependencyProperty.Register("ListCollectionView", typeof(ListCollectionView), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty LoaderTemplateProperty = DependencyProperty.Register("LoaderTemplate", typeof(DataTemplate), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsSortAscendingProperty = DependencyProperty.Register("IsSortAscending", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsSortDescendingProperty = DependencyProperty.Register("IsSortDescending", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        /// <summary>
        /// 
        /// </summary>
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

        static void OnSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnSortChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsSourceEnabledProperty = DependencyProperty.Register("IsSourceEnabled", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool IsSourceEnabled
        {
            get
            {
                return (bool)GetValue(IsSourceEnabledProperty);
            }
            set
            {
                SetValue(IsSourceEnabledProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NestedPropertyStringFormatProperty = DependencyProperty.Register("NestedPropertyStringFormat", typeof(string), typeof(PropertyGridBase), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string NestedPropertyStringFormat
        {
            get
            {
                return (string)GetValue(NestedPropertyStringFormatProperty);
            }
            set
            {
                SetValue(NestedPropertyStringFormatProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NestedPropertyTemplateProperty = DependencyProperty.Register("NestedPropertyTemplate", typeof(DataTemplate), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate NestedPropertyTemplate
        {
            get
            {
                return (DataTemplate)GetValue(NestedPropertyTemplateProperty);
            }
            set
            {
                SetValue(NestedPropertyTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NestedPropertyTemplateSelectorProperty = DependencyProperty.Register("NestedPropertyTemplateSelector", typeof(DataTemplateSelector), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplateSelector NestedPropertyTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(NestedPropertyTemplateSelectorProperty);
            }
            set
            {
                SetValue(NestedPropertyTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(PropertyModelCollection), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PropertyColumnWidthProperty = DependencyProperty.Register("PropertyColumnWidth", typeof(DataGridLength), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataGridLength PropertyColumnWidth
        {
            get
            {
                return (DataGridLength)GetValue(PropertyColumnWidthProperty);
            }
            set
            {
                SetValue(PropertyColumnWidthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SearchQueryProperty = DependencyProperty.Register("SearchQuery", typeof(string), typeof(PropertyGridBase), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowCategoriesProperty = DependencyProperty.Register("ShowCategories", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowCategoriesChanged));
        /// <summary>
        /// 
        /// </summary>
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
            Object.As<PropertyGridBase>().OnShowCategoriesChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowFeaturedProperty = DependencyProperty.Register("ShowFeatured", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowHeaderProperty = DependencyProperty.Register("ShowHeader", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowTypeProperty = DependencyProperty.Register("ShowType", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SortByNameProperty = DependencyProperty.Register("SortByName", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SortByTypeProperty = DependencyProperty.Register("SortByType", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SplitterStyleProperty = DependencyProperty.Register("SplitterStyle", typeof(Style), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style SplitterStyle
        {
            get
            {
                return (Style)GetValue(SplitterStyleProperty);
            }
            set
            {
                SetValue(SplitterStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TypeStringFormatProperty = DependencyProperty.Register("TypeStringFormat", typeof(string), typeof(PropertyGridBase), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string TypeStringFormat
        {
            get
            {
                return (string)GetValue(TypeStringFormatProperty);
            }
            set
            {
                SetValue(TypeStringFormatProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TypeTemplateProperty = DependencyProperty.Register("TypeTemplate", typeof(DataTemplate), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate TypeTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TypeTemplateProperty);
            }
            set
            {
                SetValue(TypeTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TypeTemplateSelectorProperty = DependencyProperty.Register("TypeTemplateSelector", typeof(DataTemplateSelector), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplateSelector TypeTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(TypeTemplateSelectorProperty);
            }
            set
            {
                SetValue(TypeTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueColumnWidthProperty = DependencyProperty.Register("ValueColumnWidth", typeof(DataGridLength), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataGridLength ValueColumnWidth
        {
            get
            {
                return (DataGridLength)GetValue(ValueColumnWidthProperty);
            }
            set
            {
                SetValue(ValueColumnWidthProperty, value);
            }
        }

        #endregion

        #region PropertyGridBase

        /// <summary>
        /// 
        /// </summary>
        public PropertyGridBase()
        {
            Buttons = new FrameworkElementCollection();

            Properties = new PropertyModelCollection();
            ListCollectionView = new ListCollectionView(Properties);

            PropertyColumnWidth = new DataGridLength(40d, DataGridLengthUnitType.Star);
            ValueColumnWidth = new DataGridLength(60d, DataGridLengthUnitType.Star);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            InitializeComponent();
            EvaluateGridSplitter();
            Sort();
        }

        #endregion

        #region Methods

        #region Abstract

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract object GetSource();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected abstract void Nest(object source);

        /// <summary>
        /// 
        /// </summary>
        protected abstract void RewindNest();

        #endregion

        #region Commands

        /// <summary>
        /// 
        /// </summary>
        public abstract ICommand DisconnectSourceCommand
        {
            get;
        }

        ICommand nestCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand NestCommand
        {
            get
            {
                nestCommand = nestCommand ?? new RelayCommand<object>(p => Nest(p), p => !IsLoading && p is object);
                return nestCommand;
            }
        }

        ICommand resetSourceCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand ResetSourceCommand
        {
            get
            {
                resetSourceCommand = resetSourceCommand ?? new RelayCommand(() => Properties.ForEach(i => i.Value = null), () => GetSource() != null);
                return resetSourceCommand;
            }
        }

        ICommand rewindNestCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand RewindNestCommand
        {
            get
            {
                rewindNestCommand = rewindNestCommand ?? new RelayCommand(() => RewindNest(), () => !IsLoading && nest.Any());
                return rewindNestCommand;
            }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLoaded(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            OnUnloaded(e);
        }

        #endregion

        #region Private

        void EvaluateGridSplitter()
        {
            var result = Visibility.Visible;

            if (!CanResizeDescription || DescriptionVisibility == Visibility.Collapsed)
            {
                result = Visibility.Collapsed;
            }
            else if (DescriptionVisibility == Visibility.Hidden)
                result = Visibility.Hidden;

            PART_GridSplitter.Visibility = result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PropertyName"></param>
        void Group(string PropertyName)
        {
            if (ListCollectionView != null)
            {
                ListCollectionView.GroupDescriptions.Clear();

                if (ShowCategories)
                    ListCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Sort()
        {
            if (ListCollectionView != null)
            {
                ListCollectionView.SortDescriptions.Clear();

                var d = IsSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                if (ShowCategories)
                    ListCollectionView.SortDescriptions.Add(new SortDescription("Category", d));

                var n = "Name";

                if (SortByType)
                    n = "Type";

                ListCollectionView.SortDescriptions.Add(new SortDescription(n, d));
            }
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnCanResizeDescriptionChanged(bool Value)
        {
            EvaluateGridSplitter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDescriptionVisibilityChanged(Visibility Value)
        {
            EvaluateGridSplitter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLanguageChanged(object sender, EventArgs<CultureInfo> e)
        {
            foreach (var i in Properties)
            {
                var localizedCategory = LocalizationProvider.GetValue<string>(i.Category);
                localizedCategory = localizedCategory.IsNullOrEmpty() ? i.Category : localizedCategory;
                i.LocalizedCategory = localizedCategory;
            }

            if (ShowCategories)
                Group("LocalizedCategory");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            if (Application.Current is IApp)
            {
                var languages = Application.Current.As<IApp>().Languages;
                if (languages != null)
                    languages.Set += OnLanguageChanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUnloaded(RoutedEventArgs e)
        {
            if (Application.Current is IApp)
            {
                var languages = Application.Current.As<IApp>().Languages;
                if (languages != null)
                    languages.Set -= OnLanguageChanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnShowCategoriesChanged(bool Value)
        {
            Group("LocalizedCategory");
            Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSortChanged()
        {
            Sort();
        }

        #endregion

        #endregion
    }
}
