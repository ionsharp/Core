using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using Imagin.Controls.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using Imagin.Controls.Extended.Primitives;

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

        public class TreeViewItemViewModel : NamedObject
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

            ObservableCollection<TreeViewItemViewModel> items = null;
            public ObservableCollection<TreeViewItemViewModel> Items
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

            public TreeViewItemViewModel(string Name, int Depth = 0) : base(Name)
            {
                this.Items = new ObservableCollection<TreeViewItemViewModel>();
                if (Depth == 4) return;
                int Max = Random.Next(0, 10);
                for (int i = 0; i < Max; i++)
                {
                    this.Items.Add(new TreeViewItemViewModel("Child Item " + i.ToString(), Depth + 1)
                    {
                        Message = "Child item " + i.ToString() + " has a message."
                    });
                }
            }
        }

        public enum Coffee
        {
            Mild = 0,
            Moderate = 1,
            Medium = 2,
            Strong = 3,
            Black = 4,
            ADHD = 5,
            Death = 6
        }

        public class Person : NamedObject
        {
            [Category("General")]
            [Featured(true)]
            public override string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }

            DateTime born = default(DateTime);
            [Category("General")]
            public DateTime Born
            {
                get
                {
                    return born;
                }
                set
                {
                    born = value;
                    OnPropertyChanged("Born");
                }
            }

            bool isMarried = false;
            [Category("Adult Life")]
            public bool IsMarried
            {
                get
                {
                    return isMarried;
                }
                set
                {
                    isMarried = value;
                    OnPropertyChanged("IsMarried");
                }
            }

            bool hasKids = false;
            [Category("Adult Life")]
            public bool HasKids
            {
                get
                {
                    return hasKids;
                }
                set
                {
                    hasKids = value;
                    OnPropertyChanged("HasKids");
                }
            }

            int numberOfCars = 0;
            [Category("Adult Life")]
            public int NumberOfCars
            {
                get
                {
                    return numberOfCars;
                }
                set
                {
                    numberOfCars = value;
                    OnPropertyChanged("NumberOfCars");
                }
            }

            long numberOfTimesBlinked = 0L;
            [Category("Adult Life")]
            public long NumberOfTimesBlinked
            {
                get
                {
                    return numberOfTimesBlinked;
                }
                set
                {
                    numberOfTimesBlinked = value;
                    OnPropertyChanged("NumberOfTimesBlinked");
                }
            }

            double accountBalance = 0.0;
            [Category("Adult Life")]
            public double AccountBalance
            {
                get
                {
                    return accountBalance;
                }
                set
                {
                    accountBalance = value;
                    OnPropertyChanged("AccountBalance");
                }
            }

            Coffee coffee = Coffee.Medium;
            [Category("Preferences")]
            public Coffee Coffee
            {
                get
                {
                    return coffee;
                }
                set
                {
                    coffee = value;
                    OnPropertyChanged("Coffee");
                }
            }

            string journal = string.Empty;
            [Category("Personal Life")]
            [MultiLine(true)]
            public string Journal
            {
                get
                {
                    return journal;
                }
                set
                {
                    journal = value;
                    OnPropertyChanged("Journal");
                }
            }

            string passwordMostUsed = string.Empty;
            [Category("Secret")]
            [Password(true)]
            public string PasswordMostUsed
            {
                get
                {
                    return passwordMostUsed;
                }
                set
                {
                    passwordMostUsed = value;
                    OnPropertyChanged("PasswordMostUsed");
                }
            }

            string mostVisitedComputerFolder = string.Empty;
            [Category("Secret")]
            [File(true)]
            public string MostVisitedComputerFolder
            {
                get
                {
                    return mostVisitedComputerFolder;
                }
                set
                {
                    mostVisitedComputerFolder = value;
                    OnPropertyChanged("MostVisitedComputerFolder");
                }
            }

            public Person(string Name) : base(Name)
            {
            }
        }

        public enum ViewEnum
        {
            List, 
            Details
        }

        #endregion

        #region Properties

        static Random Random = new Random();
        
        public static DependencyProperty ShortValueProperty = DependencyProperty.Register("ShortValue", typeof(short), typeof(MainWindow), new FrameworkPropertyMetadata((short)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public short ShortValue
        {
            get
            {
                return (short)GetValue(ShortValueProperty);
            }
            set
            {
                SetValue(ShortValueProperty, value);
            }
        }

        public static DependencyProperty IntValueProperty = DependencyProperty.Register("IntValue", typeof(int), typeof(MainWindow), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int IntValue
        {
            get
            {
                return (int)GetValue(IntValueProperty);
            }
            set
            {
                SetValue(IntValueProperty, value);
            }
        }

        public static DependencyProperty ByteValueProperty = DependencyProperty.Register("ByteValue", typeof(byte), typeof(MainWindow), new FrameworkPropertyMetadata((byte)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public byte ByteValue
        {
            get
            {
                return (byte)GetValue(ByteValueProperty);
            }
            set
            {
                SetValue(ByteValueProperty, value);
            }
        }

        public static DependencyProperty LongValueProperty = DependencyProperty.Register("LongValueProperty", typeof(long), typeof(MainWindow), new FrameworkPropertyMetadata(0L, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public long LongValue
        {
            get
            {
                return (long)GetValue(LongValueProperty);
            }
            set
            {
                SetValue(LongValueProperty, value);
            }
        }

        public static DependencyProperty DoubleValueProperty = DependencyProperty.Register("DoubleValue", typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double DoubleValue
        {
            get
            {
                return (double)GetValue(DoubleValueProperty);
            }
            set
            {
                SetValue(DoubleValueProperty, value);
            }
        }

        public static DependencyProperty DecimalValueProperty = DependencyProperty.Register("DecimalValue", typeof(decimal), typeof(MainWindow), new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public decimal DecimalValue
        {
            get
            {
                return (decimal)GetValue(DecimalValueProperty);
            }
            set
            {
                SetValue(DecimalValueProperty, value);
            }
        }

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

        public static DependencyProperty SelectedTreeViewItemsProperty = DependencyProperty.Register("SelectedTreeViewItems", typeof(ObservableCollection<TreeViewItemViewModel>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<TreeViewItemViewModel> SelectedTreeViewItems
        {
            get
            {
                return (ObservableCollection<TreeViewItemViewModel>)GetValue(SelectedTreeViewItemsProperty);
            }
            set
            {
                SetValue(SelectedTreeViewItemsProperty, value);
            }
        }

        public static DependencyProperty TreeViewSourceProperty = DependencyProperty.Register("TreeViewSource", typeof(ObservableCollection<TreeViewItemViewModel>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<TreeViewItemViewModel> TreeViewSource
        {
            get
            {
                return (ObservableCollection<TreeViewItemViewModel>)GetValue(TreeViewSourceProperty);
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

        public static DependencyProperty PropertyGridSourceProperty = DependencyProperty.Register("PropertyGridSource", typeof(ObservableCollection<Person>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<Person> PropertyGridSource
        {
            get
            {
                return (ObservableCollection<Person>)GetValue(PropertyGridSourceProperty);
            }
            set
            {
                SetValue(PropertyGridSourceProperty, value);
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

            this.TreeViewSource = new ObservableCollection<TreeViewItemViewModel>();
            this.SelectedTreeViewItems = new ObservableCollection<TreeViewItemViewModel>();
            for (int i = 0; i < 15; i++)
                this.TreeViewSource.Add(new TreeViewItemViewModel("Item " + i.ToString()));

            for (int i = 0; i < 100; i++)
            {
                string BaseTitle = i < 25 ? "Title" : (i < 50 ? "Another Title" : (i < 75 ? "Yet Another Title" : "Yet A Longer Title"));
                this.PART_AlignableWrapPanel.Children.Add(new Button()
                {
                    Content = BaseTitle + i.ToString()
                });
            }

            this.PropertyGridSource = new ObservableCollection<Person>();
            for (int i = 0; i < 15; i++)
            {
                this.PropertyGridSource.Add(new Person("Person " + i.ToString())
                {
                    Born = DateTime.UtcNow.AddDays(-1 * (i + 18)),
                    AccountBalance = Math.Round((double)(i * 100 + (1 / (i + 1))), 3),
                    Coffee = (Coffee)Random.Next(0, 6),
                    IsMarried = Random.Next(0, 1) == 1 ? true : false,
                    HasKids = Random.Next(0, 1) == 1 ? true : false,
                    Journal = "Journal entry " + i.ToString(),
                    MostVisitedComputerFolder = Random.Next(0, 40).As<Environment.SpecialFolder>().ToString(),
                    PasswordMostUsed = "Password " + i.ToString(),
                    NumberOfCars = Random.Next(0, 3),
                    NumberOfTimesBlinked = RandomLong(9999999999, 99999999999, Random)
                });
            }
        }

        long RandomLong(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }

        #endregion

        #region Events

        void Link_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked link!");
        }

        void OnHorizontalContentAlignmentChanged(object sender, RoutedEventArgs e)
        {
            if (sender == null || !sender.As<FrameworkElement>().IsInitialized)
                return;
            this.PART_AlignableWrapPanel.HorizontalContentAlignment = (sender as RadioButton).Content.ToString().ParseEnum<HorizontalAlignment>();
        }

        void OnMaskedButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked button!");
        }

        void OnPasswordEntered(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MessageBox.Show("Entered a password!");
        }

        void SelectColor(object sender, RoutedEventArgs e)
        {
            PART_ColorPicker.InitialColor = PART_ColorPicker.SelectedColor;
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            this.View = sender.As<RadioButton>().Content.ToString().ParseEnum<ViewEnum>();
        }

        #endregion
    }
}
