using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    public abstract class DataPanel : Panel, IFrameworkReference
    {
        public static readonly ReferenceKey<DataGrid> DataGridReferenceKey = new();

        enum Category { Group, Sort }

        #region Events

        public event EventHandler<EventArgs<IList>> SelectionChanged;

        #endregion

        #region Properties

        Bullets bullet = Bullets.NumberParenthesis;
        [Option]
        public Bullets Bullet
        {
            get => bullet;
            set => this.Change(ref bullet, value);
        }

        BooleanList columns = new();
        [Hidden]
        public BooleanList Columns
        {
            get => columns;
            set => this.Change(ref columns, value);
        }

        [Hidden]
        public virtual int Count => Data?.Count ?? 0;

        ICollectionChanged data;
        [Hidden]
        [Serialize(false)]
        public ICollectionChanged Data
        {
            get => data;
            set
            {
                if (data != null)
                    data.CollectionChanged -= OnItemsChanged;

                if (value != null)
                {
                    value.CollectionChanged
                        -= OnItemsChanged;
                    value.CollectionChanged
                        += OnItemsChanged;
                }

                var oldValue = data;
                var newValue = value;

                this.Change(ref data, newValue);
            }
        }

        [Hidden]
        public DataGrid DataGrid { get; private set; }

        ListSortDirection groupDirection = ListSortDirection.Ascending;
        [Category(Category.Group)]
        [Option]
        public ListSortDirection GroupDirection
        {
            get => groupDirection;
            set => this.Change(ref groupDirection, value);
        }

        [Hidden]
        public string GroupName => GroupNames?.Count > GroupNameIndex && GroupNameIndex >= 0 ? (string)GroupNames[GroupNameIndex] : null;

        int groupNameIndex = 0;
        [Category(Category.Group), Option]
        [DisplayName(nameof(GroupName))]
        [Source(nameof(GroupNames))]
        [Style(Int32Style.Index)]
        public virtual int GroupNameIndex
        {
            get => groupNameIndex;
            set => this.Change(ref groupNameIndex, value);
        }

        [Hidden]
        public virtual IList GroupNames { get; }

        ICollectionChanged selectedItems;
        [Hidden]
        public ICollectionChanged SelectedItems
        {
            get => selectedItems;
            private set
            {
                if (selectedItems != null)
                    selectedItems.CollectionChanged -= OnSelectionChanged;

                selectedItems = value;
                if (selectedItems != null)
                    selectedItems.CollectionChanged += OnSelectionChanged;
            }
        }

        ListSortDirection sortDirection = ListSortDirection.Descending;
        [Category(Category.Sort)]
        [Option]
        public ListSortDirection SortDirection
        {
            get => sortDirection;
            set => this.Change(ref sortDirection, value);
        }

        [Hidden]
        public string SortName => SortNames?.Count > SortNameIndex && SortNameIndex >= 0 ? (string)SortNames[SortNameIndex] : null;

        int sortNameIndex = 0;
        [Category(Category.Sort), Option]
        [DisplayName(nameof(SortName))]
        [Source(nameof(SortNames))]
        [Style(Int32Style.Index)]
        public virtual int SortNameIndex
        {
            get => sortNameIndex;
            set => this.Change(ref sortNameIndex, value);
        }

        [Hidden]
        public virtual IList SortNames { get; }

        [Hidden]
        public sealed override string Title
        {
            get
            {
                var result = TitleKey.Translate();
                if (TitleCount == 0)
                    return result;

                return $"{result}{TitleSuffix} ({TitleCount})";
            }
        }

        [Hidden]
        public virtual int TitleCount => Data?.Count ?? 0;

        /// <summary>
        /// A key used to localize the title.
        /// </summary>
        [Hidden]
        public abstract string TitleKey { get; }

        [Hidden]
        public sealed override bool TitleLocalized => false;

        /// <summary>
        /// A suffix to append to the title after localization.
        /// </summary>
        [Hidden]
        public virtual string TitleSuffix { get; }

        #endregion

        #region DataPanel

        public DataPanel() : base()
        {
            Get.Where<MainViewOptions>().PropertyChanged += OnLanguageChanged;
        }

        public DataPanel(ICollectionChanged input) : this()
        {
            Data = input;
        }

        void IFrameworkReference.SetReference(IFrameworkKey key, FrameworkElement element)
        {
            if (key == DataGridReferenceKey)
            {
                DataGrid = element as DataGrid;
                DataGrid.If(i => i.Unloaded += OnDataGridUnloaded);

                SelectedItems = XSelector.GetSelectedItems(DataGrid);
            }
        }

        #endregion

        #region Methods

        void OnDataGridUnloaded(object sender, RoutedEventArgs e)
        {
            DataGrid.Unloaded -= OnDataGridUnloaded;
            DataGrid = null;

            SelectedItems = null;
        }

        void OnLanguageChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Changed(() => Title);
        }

        void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Changed(() => Count);
            this.Changed(() => Title);

            OnItemsChanged();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnItemAdded(e.NewItems[0]);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    OnItemRemoved(e.OldItems[0]);
                    break;
            }
        }

        void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var result = new List<object>();
            (sender as ICollectionChanged).ForEach<object>(i => result.Add(i));
            SelectionChanged?.Invoke(this, new EventArgs<IList>(result));
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Data):
                    this.Changed(() => Count);
                    this.Changed(() => Title);
                    break;

                case nameof(GroupNameIndex):
                    this.Changed(() => GroupName);
                    break;

                case nameof(SortNameIndex):
                    this.Changed(() => SortName);
                    break;
            }
        }

        protected virtual void OnItemsChanged() { }

        protected virtual void OnItemRemoved(object input) { }

        protected virtual void OnItemAdded(object input) { }

        ICommand clearCommand;
        [DisplayName("Clear")]
        [Icon(Images.Trash)]
        [Tool]
        public virtual ICommand ClearCommand => clearCommand ??= new RelayCommand(() => Data.Clear(), () => Data?.Count > 0);

        ICommand refreshCommand;
        [DisplayName("Refresh")]
        [Featured(AboveBelow.Below)]
        [Icon(Images.Refresh)]
        [Index(int.MaxValue)]
        [Tool]
        public virtual ICommand RefreshCommand => refreshCommand ??= new RelayCommand(() => DataGrid.ItemsSource.As<ListCollectionView>().Refresh(), () => DataGrid?.ItemsSource is ListCollectionView);

        ICommand removeCommand;
        [Hidden]
        public ICommand RemoveCommand => removeCommand ??= new RelayCommand<object>(i => Data.Remove(i), i => i != null && Data?.Contains(i) == true);

        #endregion
    }
}