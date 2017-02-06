using Imagin.Common.Collections;
using Imagin.Common.Data;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Stores a reference to every nested property relative to the original host; properties are stored in order of depth.
        /// </summary>
        Stack<object> nest = new Stack<object>();

        public event EventHandler<EventArgs<object>> SelectedObjectChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AcceptsNullObjectsProperty = DependencyProperty.Register("AcceptsNullObjects", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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
        public static DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(FrameworkElementCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty CanResizeDescriptionProperty = DependencyProperty.Register("CanResizeDescription", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CollapseGroupsProperty = DependencyProperty.Register("CollapseGroups", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DateTimeFormatProperty = DependencyProperty.Register("DateTimeFormat", typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DescriptionStringFormatProperty = DependencyProperty.Register("DescriptionStringFormat", typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DescriptionTemplateProperty = DependencyProperty.Register("DescriptionTemplate", typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DescriptionTemplateSelectorProperty = DependencyProperty.Register("DescriptionTemplateSelector", typeof(DataTemplateSelector), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DescriptionVisibilityProperty = DependencyProperty.Register("DescriptionVisibility", typeof(Visibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FileSizeFormatProperty = DependencyProperty.Register("FileSizeFormat", typeof(FileSizeFormat), typeof(PropertyGrid), new FrameworkPropertyMetadata(FileSizeFormat.BinaryUsingSI, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty GridBackgroundProperty = DependencyProperty.Register("GridBackground", typeof(Brush), typeof(PropertyGrid), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GridBorderBrushProperty = DependencyProperty.Register("GridBorderBrush", typeof(Brush), typeof(PropertyGrid), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GridBorderThicknessProperty = DependencyProperty.Register("GridBorderThickness", typeof(Thickness), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(DataGridGridLinesVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty GridStyleProperty = DependencyProperty.Register("GridStyle", typeof(Style), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HeadersVisibilityProperty = DependencyProperty.Register("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(DataGridHeadersVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ListCollectionViewProperty = DependencyProperty.Register("ListCollectionView", typeof(ListCollectionView), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty LoaderTemplateProperty = DependencyProperty.Register("LoaderTemplate", typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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
        public static DependencyProperty IsSortAscendingProperty = DependencyProperty.Register("IsSortAscending", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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
        public static DependencyProperty IsSortDescendingProperty = DependencyProperty.Register("IsSortDescending", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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

        static void OnSortChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyGrid)Object).OnSortChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NestedPropertyStringFormatProperty = DependencyProperty.Register("NestedPropertyStringFormat", typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty NestedPropertyTemplateProperty = DependencyProperty.Register("NestedPropertyTemplate", typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty NestedPropertyTemplateSelectorProperty = DependencyProperty.Register("NestedPropertyTemplateSelector", typeof(DataTemplateSelector), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(PropertyModelCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty PropertyColumnWidthProperty = DependencyProperty.Register("PropertyColumnWidth", typeof(DataGridLength), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SearchQueryProperty = DependencyProperty.Register("SearchQuery", typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static async void OnSelectedObjectChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            await Object.As<PropertyGrid>().OnSelectedObjectChanged((object)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowCategoriesProperty = DependencyProperty.Register("ShowCategories", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowCategoriesChanged));
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
            Object.As<PropertyGrid>().OnShowCategoriesChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowFeaturedProperty = DependencyProperty.Register("ShowFeatured", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ShowHeaderProperty = DependencyProperty.Register("ShowHeader", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ShowTypeProperty = DependencyProperty.Register("ShowType", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SortByNameProperty = DependencyProperty.Register("SortByName", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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
        public static DependencyProperty SortByTypeProperty = DependencyProperty.Register("SortByType", typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortChanged));
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
        public static DependencyProperty SplitterStyleProperty = DependencyProperty.Register("SplitterStyle", typeof(Style), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty TypeStringFormatProperty = DependencyProperty.Register("TypeStringFormat", typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty TypeTemplateProperty = DependencyProperty.Register("TypeTemplate", typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty TypeTemplateSelectorProperty = DependencyProperty.Register("TypeTemplateSelector", typeof(DataTemplateSelector), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ValueColumnWidthProperty = DependencyProperty.Register("ValueColumnWidth", typeof(DataGridLength), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #region PropertyGrid

        /// <summary>
        /// 
        /// </summary>
        public PropertyGrid()
        {
            Buttons = new FrameworkElementCollection();

            CommandBindings.Add(new CommandBinding(EditCollectionCommand, this.EditCollectionCommand_Executed, this.EditCollectionCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(ResetCommand, this.ResetCommand_Executed, this.ResetCommand_CanExecute));

            Properties = new PropertyModelCollection();
            ListCollectionView = new ListCollectionView(Properties);

            PropertyColumnWidth = new DataGridLength(40d, DataGridLengthUnitType.Star);
            ValueColumnWidth = new DataGridLength(60d, DataGridLengthUnitType.Star);

            InitializeComponent();

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

        ICommand nestCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand NestCommand
        {
            get
            {
                nestCommand = nestCommand ?? new RelayCommand(p =>
                {
                    nest.Push(SelectedObject);
                    SetCurrentValue(SelectedObjectProperty, p);
                }, p => !IsLoading && p is object);
                return nestCommand;
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
                rewindNestCommand = rewindNestCommand ?? new RelayCommand(p => SetCurrentValue(SelectedObjectProperty, nest.Pop()), x => !IsLoading && nest.Any());
                return rewindNestCommand;
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PropertyName"></param>
        protected void Group(string PropertyName)
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
        protected void Sort()
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

        protected virtual async Task OnSelectedObjectChanged(object Value)
        {
            if (Value != null)
            {
                Properties.Reset(Value);

                IsLoading = true;
                await SetObject(Value);
                IsLoading = false;
            }
            else if (AcceptsNullObjects)
                Properties.Reset(null);

            SelectedObjectChanged?.Invoke(this, new EventArgs<object>(Value));
        }

        protected virtual void OnShowCategoriesChanged(bool Value)
        {
            Group("Category");
            Sort();
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
