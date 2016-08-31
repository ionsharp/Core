using Imagin.Common;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using Imagin.Common.Extensions;

namespace Imagin.NET.Demo
{
    public partial class MainWindow : Window
    {
        #region Items

        public class DataGridItem : NamedEntry
        {
            string message = string.Empty;
            public string Message
            {
                get
                {
                    return this.message;
                }
                set
                {
                    this.message = value;
                    OnPropertyChanged("Message");
                }
            }

            string category = string.Empty;
            public string Category
            {
                get
                {
                    return this.category;
                }
                set
                {
                    this.category = value;
                    OnPropertyChanged("Category");
                }
            }

            public DataGridItem(string Name, string Message) : base(Name)
            {
                this.Message = Message;
            }
        }

        public class TreeViewComboBoxItem : NamedObject
        {
            string message = string.Empty;
            public string Message
            {
                get
                {
                    return this.message;
                }
                set
                {
                    this.message = value;
                    OnPropertyChanged("Message");
                }
            }

            ObservableCollection<TreeViewComboBoxItem> items = null;
            public ObservableCollection<TreeViewComboBoxItem> Items
            {
                get
                {
                    return this.items;
                }
                set
                {
                    this.items = value;
                    OnPropertyChanged("Items");
                }
            }

            public TreeViewComboBoxItem(string Name, bool AddChildren = true) : base(Name)
            {
                this.Items = new ObservableCollection<TreeViewComboBoxItem>();
                if (!AddChildren)
                    return;
                int Max = new Random().Next(0, 5);
                for (int i = 0; i < Max; i++)
                {
                    this.Items.Add(new TreeViewComboBoxItem("Child Item " + i.ToString(), false)
                    {
                        Message = "Child item " + i.ToString() + " has a message."
                    });
                }
            }
        }

        public enum ViewEnum
        {
            List, 
            Details
        }

        #endregion

        #region Properties

        public static DependencyProperty ViewProperty = DependencyProperty.Register("View", typeof(ViewEnum), typeof(MainWindow), new FrameworkPropertyMetadata(ViewEnum.Details, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ViewEnum View
        {
            get
            {
                return (ViewEnum)GetValue(ViewProperty);
            }
            set
            {
                SetValue(ViewProperty, value);
            }
        }

        public static DependencyProperty ListViewSourceProperty = DependencyProperty.Register("ListViewSource", typeof(ListCollectionView), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ListCollectionView ListViewSource
        {
            get
            {
                return (ListCollectionView)GetValue(ListViewSourceProperty);
            }
            set
            {
                SetValue(ListViewSourceProperty, value);
            }
        }

        public static DependencyProperty TreeViewSourceProperty = DependencyProperty.Register("TreeViewSource", typeof(ObservableCollection<TreeViewComboBoxItem>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<TreeViewComboBoxItem> TreeViewSource
        {
            get
            {
                return (ObservableCollection<TreeViewComboBoxItem>)GetValue(TreeViewSourceProperty);
            }
            set
            {
                SetValue(TreeViewSourceProperty, value);
            }
        }

        public static DependencyProperty DataGridSourceProperty = DependencyProperty.Register("DataGridSource", typeof(ObservableCollection<DataGridItem>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<DataGridItem> DataGridSource
        {
            get
            {
                return (ObservableCollection<DataGridItem>)GetValue(DataGridSourceProperty);
            }
            set
            {
                SetValue(DataGridSourceProperty, value);
            }
        }

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();

            this.DataGridSource = new ObservableCollection<DataGridItem>();
            for (int i = 0; i < 15; i++)
            {
                this.DataGridSource.Add(new DataGridItem("Item " + i.ToString(), "Item " + i.ToString() + " has an important message.")
                {
                    Category = i < 5 ? "Category 1" : (i < 10 ? "Category 2" : (i <= 15 ? "Category 3" : string.Empty)),
                    Date = DateTime.UtcNow.AddMinutes(i / 2)
                });
            }
            this.ListViewSource = new ListCollectionView(this.DataGridSource);
            //this.ListViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            this.TreeViewSource = new ObservableCollection<TreeViewComboBoxItem>();
            for (int i = 0; i < 15; i++)
                this.TreeViewSource.Add(new TreeViewComboBoxItem("Item " + i.ToString()));
        }

        #endregion

        #region Events

        void Link_Click(object sender, RoutedEventArgs e)
        {
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            this.View = sender.As<RadioButton>().Content.ToString().ParseEnum<ViewEnum>();
        }

        #endregion
    }
}
