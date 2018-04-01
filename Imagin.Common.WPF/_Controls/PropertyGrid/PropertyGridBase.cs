using Imagin.Common.Collections;
using Imagin.Common.Configuration;
using Imagin.Common.Converters;
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
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Splitter", Type = typeof(GridSplitter))]
    public abstract class PropertyGridBase : DataGrid
    {
        #region Fields

        Border PART_Border;

        GridSplitter PART_Splitter;

        bool presented = false;

        DataGridTemplateColumn NameColumn;

        DataGridTemplateColumn ValueColumn;

        /// <summary>
        /// 
        /// </summary>
        protected bool isNesting = false;

        /// <summary>
        /// Stores a reference to every nested property relative to the original host; properties are stored in order of depth.
        /// </summary>
        protected Stack<object> nest = new Stack<object>();

        #endregion

        #region Properties

        /// <summary>
        /// Identifies the <see cref="AcceptsNullObjects"/> <see cref="DependencyProperty"/>.
        /// </summary>
        public static DependencyProperty AcceptsNullObjectsProperty = DependencyProperty.Register("AcceptsNullObjects", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not a <see langword="null"/> object may be assigned.
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
        public static DependencyProperty HeaderButtonsProperty = DependencyProperty.Register("HeaderButtons", typeof(FrameworkElementCollection), typeof(PropertyGridBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElementCollection HeaderButtons
        {
            get
            {
                return (FrameworkElementCollection)GetValue(HeaderButtonsProperty);
            }
            set
            {
                SetValue(HeaderButtonsProperty, value);
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
        public static DependencyProperty DescriptionResizeModeProperty = DependencyProperty.Register("DescriptionResizeMode", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDescriptionResizeModeChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool DescriptionResizeMode
        {
            get
            {
                return (bool)GetValue(DescriptionResizeModeProperty);
            }
            set
            {
                SetValue(DescriptionResizeModeProperty, value);
            }
        }
        static void OnDescriptionResizeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnDescriptionResizeModeChanged((bool)e.NewValue);
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
        public static DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(PropertyGridGroupName), typeof(PropertyGridBase), new FrameworkPropertyMetadata(PropertyGridGroupName.Category, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGroupNameChanged));
        /// <summary>
        /// 
        /// </summary>
        public PropertyGridGroupName GroupName
        {
            get
            {
                return (PropertyGridGroupName)GetValue(GroupNameProperty);
            }
            set
            {
                SetValue(GroupNameProperty, value);
            }
        }
        static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnGroupNameChanged((PropertyGridGroupName)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GroupVisibilityProperty = DependencyProperty.Register("GroupVisibility", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool GroupVisibility
        {
            get
            {
                return (bool)GetValue(GroupVisibilityProperty);
            }
            set
            {
                SetValue(GroupVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(PropertyGridBase), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Visibility HeaderVisibility
        {
            get
            {
                return (Visibility)GetValue(HeaderVisibilityProperty);
            }
            set
            {
                SetValue(HeaderVisibilityProperty, value);
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
        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(PropertyGridBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(PropertyModelCollection), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(PropertyModelCollection), OnPropertiesChanged));
        /// <summary>
        /// 
        /// </summary>
        public PropertyModelCollection Properties
        {
            get
            {
                return (PropertyModelCollection)GetValue(PropertiesProperty);
            }
            private set
            {
                SetValue(PropertiesProperty, value);
            }
        }
        static void OnPropertiesChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<PropertyGridBase>().OnPropertiesChanged((PropertyModelCollection)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PropertiesViewProperty = DependencyProperty.Register("PropertiesView", typeof(ListCollectionView), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(ListCollectionView), OnPropertiesViewChanged));
        /// <summary>
        /// 
        /// </summary>
        public ListCollectionView PropertiesView
        {
            get
            {
                return (ListCollectionView)GetValue(PropertiesViewProperty);
            }
            private set
            {
                SetValue(PropertiesViewProperty, value);
            }
        }
        static void OnPropertiesViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnPropertiesViewChanged((ListCollectionView)e.NewValue);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NameColumnHeaderProperty = DependencyProperty.Register("NameColumnHeader", typeof(object), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnNameColumnHeaderChanged));
        /// <summary>
        /// 
        /// </summary>
        public object NameColumnHeader
        {
            get
            {
                return GetValue(NameColumnHeaderProperty);
            }
            set
            {
                SetValue(NameColumnHeaderProperty, value);
            }
        }
        static void OnNameColumnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnNameColumnHeaderChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PropertyColumnWidthProperty = DependencyProperty.Register("PropertyColumnWidth", typeof(DataGridLength), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnNameColumnWidthChanged));
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
        static void OnNameColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnNameColumnWidthChanged((DataGridLength)e.NewValue);
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
        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection), typeof(PropertyGridBase), new FrameworkPropertyMetadata(ListSortDirection.Ascending, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortDirectionChanged));
        /// <summary>
        /// 
        /// </summary>
        public ListSortDirection SortDirection
        {
            get
            {
                return (ListSortDirection)GetValue(SortDirectionProperty);
            }
            set
            {
                SetValue(SortDirectionProperty, value);
            }
        }
        static void OnSortDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnSortDirectionChanged((ListSortDirection)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SortNameProperty = DependencyProperty.Register("SortName", typeof(PropertyGridSortName), typeof(PropertyGridBase), new FrameworkPropertyMetadata(PropertyGridSortName.Name, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSortNameChanged));
        /// <summary>
        /// 
        /// </summary>
        public PropertyGridSortName SortName
        {
            get
            {
                return (PropertyGridSortName)GetValue(SortNameProperty);
            }
            set
            {
                SetValue(SortNameProperty, value);
            }
        }
        static void OnSortNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnSortNameChanged((PropertyGridSortName)e.NewValue);
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
        public static DependencyProperty TokenStyleProperty = DependencyProperty.Register("TokenStyle", typeof(Style), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(Style)));
        /// <summary>
        /// 
        /// </summary>
        public Style TokenStyle
        {
            get
            {
                return (Style)GetValue(TokenStyleProperty);
            }
            set
            {
                SetValue(TokenStyleProperty, value);
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
        public static DependencyProperty TypeVisibilityProperty = DependencyProperty.Register("TypeVisibility", typeof(Visibility), typeof(PropertyGridBase), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Visibility TypeVisibility
        {
            get
            {
                return (Visibility)GetValue(TypeVisibilityProperty);
            }
            set
            {
                SetValue(TypeVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueColumnHeaderProperty = DependencyProperty.Register("ValueColumnHeader", typeof(object), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueColumnHeaderChanged));
        /// <summary>
        /// 
        /// </summary>
        public object ValueColumnHeader
        {
            get
            {
                return GetValue(ValueColumnHeaderProperty);
            }
            set
            {
                SetValue(ValueColumnHeaderProperty, value);
            }
        }
        static void OnValueColumnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<PropertyGridBase>().OnValueColumnHeaderChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueColumnWidthProperty = DependencyProperty.Register("ValueColumnWidth", typeof(DataGridLength), typeof(PropertyGridBase), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueColumnWidthChanged));
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
        static void OnValueColumnWidthChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<PropertyGridBase>().OnValueColumnWidthChanged((DataGridLength)e.NewValue);
        }

        #endregion

        #region PropertyGridBase

        /// <summary>
        /// 
        /// </summary>
        public PropertyGridBase() : base()
        {
            NameColumn = new DataGridTemplateColumn()
            {
                CellTemplate = default(DataTemplate),
                SortMemberPath = "DisplayName"
            };

            ValueColumn = new DataGridTemplateColumn();

            SetCurrentValue(NameColumnHeaderProperty, Localizer.GetValue<string>("Name", "Imagin.Common.WPF"));
            SetCurrentValue(ValueColumnHeaderProperty, Localizer.GetValue<string>("Value", "Imagin.Common.WPF"));

            Columns.Add(NameColumn);
            Columns.Add(ValueColumn);

            SetCurrentValue(HeaderButtonsProperty, new FrameworkElementCollection());
            SetCurrentValue(PropertiesProperty, new PropertyModelCollection());
            SetCurrentValue(PropertyColumnWidthProperty, new DataGridLength(40d, DataGridLengthUnitType.Star));
            SetCurrentValue(ValueColumnWidthProperty, new DataGridLength(60d, DataGridLengthUnitType.Star));

            BindingOperations.SetBinding(this, SelectedValueProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("Properties.ActiveProperty"),
                Source = this
            });

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
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

        ICommand groupCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand GroupCommand
        {
            get
            {
                groupCommand = groupCommand ?? new RelayCommand<object>(p =>
                {
                    if (p is PropertyGridGroupName)
                        SetCurrentValue(GroupNameProperty, (PropertyGridGroupName)p);
                }, p => p is PropertyGridGroupName);
                return groupCommand;
            }
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

        ICommand sortCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand SortCommand
        {
            get
            {
                sortCommand = sortCommand ?? new RelayCommand<object>(p =>
                {
                    if (p is PropertyGridSortName)
                        SetCurrentValue(SortNameProperty, (PropertyGridSortName)p);

                    if (p is ListSortDirection)
                        SetCurrentValue(SortDirectionProperty, (ListSortDirection)p);
                }, p => p is PropertyGridSortName || p is ListSortDirection);
                return sortCommand;
            }
        }

        #endregion

        #region Handlers

        void OnLoaded(object sender, RoutedEventArgs e) => OnLoaded(e);

        void OnUnloaded(object sender, RoutedEventArgs e) => OnUnloaded(e);

        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Border = Template.FindName("PART_Border", this) as Border;
            PART_Splitter = Template.FindName("PART_Splitter", this) as GridSplitter;

            NameColumn.CellTemplate = (DataTemplate)PART_Border.Resources["DataTemplate.Property.Name"];
            ValueColumn.CellTemplate = (DataTemplate)PART_Border.Resources["DataTemplate.Property.Value"];

            GroupStyle.Add((GroupStyle)PART_Border.Resources["Style.Group"]);
        }

        #endregion

        #region Private

        void EvaluateSplitter()
        {
            if (PART_Splitter != null)
            {
                var result = Visibility.Visible;

                if (!DescriptionResizeMode || DescriptionVisibility == Visibility.Collapsed)
                {
                    result = Visibility.Collapsed;
                }
                else if (DescriptionVisibility == Visibility.Hidden)
                    result = Visibility.Hidden;

                PART_Splitter.Visibility = result;
            }
        }

        void Group()
        {
            if (PropertiesView != null)
            {
                PropertiesView.GroupDescriptions.Clear();

                var description = default(GroupDescription);
                switch (GroupName)
                {
                    case PropertyGridGroupName.Category:
                        description = new PropertyGroupDescription("Category", Imagin.Common.Converters.Converter<string, string>.New(i => i, i => i));
                        break;
                    case PropertyGridGroupName.Name:
                        description = new PropertyGroupDescription("DisplayName", new Imagin.Common.Converters.FirstLetterConverter());
                        break;
                    case PropertyGridGroupName.Type:
                        description = new PropertyGroupDescription("Type");
                        break;
                }

                if (description != default(GroupDescription))
                    PropertiesView.GroupDescriptions.Add(description);
            }
        }

        void Sort()
        {
            if (PropertiesView != null)
            {
                PropertiesView.SortDescriptions.Clear();

                var description = default(SortDescription);
                switch (SortName)
                {
                    case PropertyGridSortName.Name:
                        description = new SortDescription("DisplayName", SortDirection);
                        break;
                    case PropertyGridSortName.Type:
                        description = new SortDescription("Type", SortDirection);
                        break;
                }

                if (description != default(SortDescription))
                    PropertiesView.SortDescriptions.Add(description);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldItemContainerStyle"></param>
        /// <param name="newItemContainerStyle"></param>
        protected override void OnItemContainerStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
        {
            base.OnItemContainerStyleChanged(oldItemContainerStyle, newItemContainerStyle);

            if (!ItemContainerStyleChangeHandled && newItemContainerStyle != null)
            {
                ItemContainerStyleChangeHandled = true;

                var binding = Bindings.New(new StartsWithToVisibilityMultiValueConverter());
                binding.Bindings.Add(Bindings.New("DisplayName", new LocalizationConverter()));
                binding.Bindings.Add(Bindings.New("SearchQuery", this));

                var style = new Style()
                {
                    TargetType = typeof(DataGridRow),
                    BasedOn = ItemContainerStyle
                };
                style.Setters.Add(new Setter()
                {
                    Property = DataGridRow.VisibilityProperty,
                    Value = binding,
                });

                SetCurrentValue(ItemContainerStyleProperty, style);

                ItemContainerStyleChangeHandled = false;
            }
        }

        bool ItemContainerStyleChangeHandled = false;

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDescriptionResizeModeChanged(bool Value)
        {
            EvaluateSplitter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDescriptionVisibilityChanged(Visibility Value)
        {
            EvaluateSplitter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnGroupDirectionChanged(ListSortDirection value)
        {
            Group();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnGroupNameChanged(PropertyGridGroupName value)
        {
            Group();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLanguageChanged(object sender, EventArgs<CultureInfo> e)
        {
            Group();
            Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            if (!presented)
            {
                OnPresented();
                presented = true;
            }

            Application.Current.As<IApp>()?.Languages.IfNotNull(i => i.Set += OnLanguageChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnNameColumnHeaderChanged(object value)
        {
            NameColumn.Header = value;
        }

        /// <summary>
        /// Occurs when <see cref="PropertyGridBase"/> loads for the first time.
        /// </summary>
        protected virtual void OnPresented()
        {
            EvaluateSplitter();

            Group();
            Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnPropertiesChanged(PropertyModelCollection value)
        {
            SetCurrentValue(PropertiesViewProperty, new ListCollectionView(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnPropertiesViewChanged(ListCollectionView value)
        {
            SetCurrentValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUnloaded(RoutedEventArgs e)
        {
            Application.Current.As<IApp>()?.Languages.IfNotNull(i => i.Set -= OnLanguageChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnSortDirectionChanged(ListSortDirection value)
        {
            Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnSortNameChanged(PropertyGridSortName value)
        {
            Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnNameColumnWidthChanged(DataGridLength value)
        {
            NameColumn.Width = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueColumnHeaderChanged(object value)
        {
            ValueColumn.Header = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueColumnWidthChanged(DataGridLength value)
        {
            ValueColumn.Width = value;
        }

        #endregion

        #endregion
    }
}
