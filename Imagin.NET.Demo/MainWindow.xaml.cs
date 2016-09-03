using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Imagin.Controls.Common;

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

            this.TreeViewSource = new ObservableCollection<TreeViewComboBoxItem>();
            for (int i = 0; i < 15; i++)
                this.TreeViewSource.Add(new TreeViewComboBoxItem("Item " + i.ToString()));

            for (int i = 0; i < 100; i++)
            {
                string BaseTitle = i < 25 ? "Title" : (i < 50 ? "Another Title" : (i < 75 ? "Yet Another Title" : "Yet A Longer Title"));
                this.PART_AlignableWrapPanel.Children.Add(new Button()
                {
                    Content = BaseTitle + i.ToString()
                });
            }

            this.PropertyGridSource = new ObservableCollection<Person>();
            Random r = new Random();
            for (int i = 0; i < 15; i++)
            {
                this.PropertyGridSource.Add(new Person("Person " + i.ToString())
                {
                    Born = DateTime.UtcNow.AddDays(-1 * (i + 18)),
                    AccountBalance = Math.Round((double)(i * 100 + (1 / (i + 1))), 3),
                    Coffee = (Coffee)r.Next(0, 6),
                    IsMarried = r.Next(0, 1) == 1 ? true : false,
                    HasKids = r.Next(0, 1) == 1 ? true : false,
                    Journal = "Journal entry " + i.ToString(),
                    MostVisitedComputerFolder = r.Next(0, 40).As<Environment.SpecialFolder>().ToString(),
                    PasswordMostUsed = "Password " + i.ToString(),
                    NumberOfCars = r.Next(0, 3),
                    NumberOfTimesBlinked = RandomLong(9999999999, 99999999999, r)
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

        void OnSpacerThicknessChanged(object sender, TextChangedEventArgs e)
        {
            if (!sender.As<IntTextBox>().IsInitialized)
                return;
            List<int> Values = new List<int>();
            foreach (IntTextBox t in PART_SpacerThicknessPanel.Children)
                Values.Add(t.Text.ToInt());
            this.PART_Spacer.Spacing = new Thickness(Values[0], Values[1], Values[2], Values[3]);
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            this.View = sender.As<RadioButton>().Content.ToString().ParseEnum<ViewEnum>();
        }

        void OnPasswordEntered(object sender, Common.Events.ObjectEventArgs e)
        {
            MessageBox.Show("Entered a password!");
        }

        #endregion
    }
}
