using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using Imagin.Common.Text;
using Imagin.Common.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.NET.Demo
{
    public partial class MainWindow : Window
    {
        #region Models

        public class FileSystemEntryModel : NamedObject
        {
            Guid id = Guid.NewGuid();
            [Description("The date the file was last accessed.")]
            [ReadOnly(true)]
            public Guid Id
            {
                get
                {
                    return id;
                }
                set
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }

            [Description("The name of the file.")]
            [Featured(true)]
            [ReadOnly(true)]
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

            bool isExpanded = false;
            public bool IsExpanded
            {
                get
                {
                    return this.isExpanded;
                }
                set
                {
                    this.isExpanded = value;
                    OnPropertyChanged("IsExpanded");

                    if (value)
                    {
                        this.Items.Clear();
                        try
                        {
                            foreach (var i in System.IO.Directory.EnumerateFileSystemEntries(this.Path))
                                this.Items.Add(new FileSystemEntryModel(i));
                        }
                        catch
                        {
                        }
                    }
                }
            }

            bool isSelected = false;
            public bool IsSelected
            {
                get
                {
                    return this.isSelected;
                }
                set
                {
                    this.isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }

            string path = string.Empty;
            public string Path
            {
                get
                {
                    return this.path;
                }
                set
                {
                    this.path = value;
                    OnPropertyChanged("Path");
                }
            }

            long size = 0L;
            public long Size
            {
                get
                {
                    return this.size;
                }
                set
                {
                    this.size = value;
                    OnPropertyChanged("Size");
                }
            }

            ServerObjectType type;
            [Description("The type of the file system entry.")]
            [ReadOnly(true)]
            public ServerObjectType Type
            {
                get
                {
                    return type;
                }
                set
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }

            bool isReadOnly = false;
            [Description("Indicates whether or not the file is read-only.")]
            [ReadOnly(true)]
            public bool IsReadOnly
            {
                get
                {
                    return isReadOnly;
                }
                set
                {
                    isReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }

            bool isHidden = false;
            [Description("Indicates whether or not the file is hidden.")]
            [ReadOnly(true)]
            public bool IsHidden
            {
                get
                {
                    return isHidden;
                }
                set
                {
                    isHidden = value;
                    OnPropertyChanged("IsHidden");
                }
            }

            string notes = string.Empty;
            [Description("File notes.")]
            [StringRepresentation(StringRepresentation.Multiline)]
            public string Notes
            {
                get
                {
                    return notes;
                }
                set
                {
                    notes = value;
                    OnPropertyChanged("Notes");
                }
            }

            string password = string.Empty;
            [Description("A password used to lock the file.")]
            [StringRepresentation(StringRepresentation.Password)]
            public string Password
            {
                get
                {
                    return password;
                }
                set
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }

            DateTime accessed = default(DateTime);
            [Description("The date the file was last accessed.")]
            [ReadOnly(true)]
            public DateTime Accessed
            {
                get
                {
                    return this.accessed;
                }
                set
                {
                    this.accessed = value;
                    OnPropertyChanged("Accessed");
                }
            }

            DateTime created = default(DateTime);
            [Description("The date the file was created.")]
            [ReadOnly(true)]
            public DateTime Created
            {
                get
                {
                    return this.created;
                }
                set
                {
                    this.created = value;
                    OnPropertyChanged("Created");
                }
            }

            DateTime modified = default(DateTime);
            [Description("The date the file was last modified.")]
            [ReadOnly(true)]
            public DateTime Modified
            {
                get
                {
                    return this.modified;
                }
                set
                {
                    this.modified = value;
                    OnPropertyChanged("Modified");
                }
            }

            ObservableCollection<FileSystemEntryModel> items = null;
            [Browsable(false)]
            public ObservableCollection<FileSystemEntryModel> Items
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

            public override string ToString()
            {
                return Name;
            }

            void Set(System.IO.FileSystemInfo Info)
            {
                this.Path = Info.FullName;
                this.Accessed = Info.LastAccessTime;
                this.Created = Info.CreationTime;
                this.Modified = Info.LastWriteTime;
                this.IsHidden = (Info.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden;
                this.IsReadOnly = (Info.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly;
            }

            void Set(System.IO.DirectoryInfo Info)
            {
                this.Type = ServerObjectType.Folder;

                this.Set(Info as System.IO.FileSystemInfo);
            }

            void Set(System.IO.FileInfo Info)
            {
                this.Type = ServerObjectType.File;
                this.Size = Info.Length;

                this.Set(Info as System.IO.FileSystemInfo);
            }

            public FileSystemEntryModel(string Path) : base()
            {
                this.Items = new ObservableCollection<FileSystemEntryModel>();

                bool IsFolder = System.IO.Directory.Exists(Path), IsFile = System.IO.File.Exists(Path);
                if (Path.DirectoryExists())
                    this.Set(new System.IO.DirectoryInfo(Path));
                else if (Path.FileExists())
                    this.Set(new System.IO.FileInfo(Path));

                this.Name = !IsFolder && !IsFile ? this.Path : Path.GetFileName();
            }
        }

        public class HierarchialObject : NamedObject
        {
            ObservableCollection<HierarchialObject> items = null;
            public ObservableCollection<HierarchialObject> Items
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

            public HierarchialObject(string Name) : base(Name)
            {
                this.Items = new ObservableCollection<Demo.MainWindow.HierarchialObject>();
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

        string LastPath = string.Empty;

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

        public static DependencyProperty SelectedFileSystemEntriesProperty = DependencyProperty.Register("SelectedFileSystemEntries", typeof(ObservableCollection<object>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<object> SelectedFileSystemEntries
        {
            get
            {
                return (ObservableCollection<object>)GetValue(SelectedFileSystemEntriesProperty);
            }
            set
            {
                SetValue(SelectedFileSystemEntriesProperty, value);
            }
        }

        public static DependencyProperty FileSystemProperty = DependencyProperty.Register("FileSystem", typeof(ObservableCollection<FileSystemEntryModel>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<FileSystemEntryModel> FileSystem
        {
            get
            {
                return (ObservableCollection<FileSystemEntryModel>)GetValue(FileSystemProperty);
            }
            set
            {
                SetValue(FileSystemProperty, value);
            }
        }

        public static DependencyProperty HierarchialCollectionProperty = DependencyProperty.Register("HierarchialCollection", typeof(ObservableCollection<HierarchialObject>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<HierarchialObject> HierarchialCollection
        {
            get
            {
                return (ObservableCollection<HierarchialObject>)GetValue(HierarchialCollectionProperty);
            }
            set
            {
                SetValue(HierarchialCollectionProperty, value);
            }
        }

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();

            this.SelectedFileSystemEntries = new ObservableCollection<object>();

            this.FileSystem = new ObservableCollection<FileSystemEntryModel>();
            foreach (var i in System.IO.Directory.GetLogicalDrives())
                this.FileSystem.Add(new FileSystemEntryModel(i));

            this.ListViewSource = new ListCollectionView(this.FileSystem);

            for (int i = 0; i < 100; i++)
            {
                string BaseTitle = i < 25 ? "Title" : (i < 50 ? "Another Title" : (i < 75 ? "Yet Another Title" : "Yet A Longer Title"));
                this.PART_AlignableWrapPanel.Children.Add(new Button()
                {
                    Content = BaseTitle + i.ToString()
                });
            }

            HierarchialCollection = new ObservableCollection<HierarchialObject>();
            for (int i = 0; i < 10; i++)
            {
                var k = new HierarchialObject("Object " + i);
                for (int j = 0; j < 5; j++)
                {
                    var m = new HierarchialObject("Object " + (i + j));
                    m.Items.Add(new HierarchialObject("Object " + i + "a"));
                    k.Items.Add(m);
                }
                HierarchialCollection.Add(k);
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
        
        void OnGetParentPath(object sender, RoutedEventArgs e)
        {
            if (this.LastPath.IsNull()) return;

            var Parent = LastPath.DirectoryExists() ? LastPath.GetDirectoryName() : null;
            this.SetPath(Parent);
        }

        void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var Path = (sender as FrameworkElement).Tag.ToString();
            if (Path.DirectoryExists())
                this.SetPath(Path);
        }

        void OnMaskedButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked button!");
        }

        void OnPasswordEntered(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MessageBox.Show("Entered a password!");
        }

        void OnSpacerThicknessChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.PART_Spacer.Spacing = new Thickness(this.PART_SpacerThicknessLeft.Value, this.PART_SpacerThicknessTop.Value, this.PART_SpacerThicknessRight.Value, this.PART_SpacerThicknessBottom.Value);
            }
            catch
            {
            }
        }

        void SelectColor(object sender, RoutedEventArgs e)
        {
            PART_ColorPicker.InitialColor = PART_ColorPicker.SelectedColor;
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            this.View = sender.As<RadioButton>().Content.ToString().ParseEnum<ViewEnum>();
        }

        void SetPath(string Path)
        {
            this.FileSystem.Clear();

            var Items = default(IEnumerable<string>);
            try
            {
                if (Path.IsNullOrEmpty())
                    Items = System.IO.Directory.GetLogicalDrives();
                else Items = System.IO.Directory.EnumerateFileSystemEntries(Path);
            }
            catch
            {
                Items = null;
            }

            if (Items != null)
            {
                foreach (var i in Items)
                    this.FileSystem.Add(new FileSystemEntryModel(i));
            }

            this.LastPath = Path;
        }

        #endregion
    }
}
