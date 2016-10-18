using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using Imagin.Common.Text;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
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

            string type = string.Empty;
            public string Type
            {
                get
                {
                    return this.type;
                }
                set
                {
                    this.type = value;
                    OnPropertyChanged("Type");
                }
            }

            DateTime accessed = default(DateTime);
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

            public FileSystemEntryModel(string Path) : base()
            {
                this.Items = new ObservableCollection<FileSystemEntryModel>();

                this.Path = Path;
                bool IsFolder = System.IO.Directory.Exists(Path), IsFile = System.IO.File.Exists(Path);

                this.Name = !IsFolder && !IsFile ? this.Path : Path.GetFileName();
                if (!IsFolder && !IsFile) return;

                this.Type = IsFolder ? (Path.EndsWith(":") || Path.EndsWith(@":\") ? "Drive" : "Folder") : (IsFile ? string.Format("{0} File", Path.GetExtension().Replace(".", "").ToUpper()) : string.Empty);

                var Info = IsFolder ? new System.IO.DirectoryInfo(Path) : IsFile ? new System.IO.FileInfo(Path) as System.IO.FileSystemInfo : null;
                if (Info != null)
                {
                    if (IsFile)
                        this.Size = (Info as System.IO.FileInfo).Length;
                    this.Accessed = Info.LastAccessTimeUtc;
                    this.Created = Info.CreationTimeUtc;
                    this.Modified = Info.LastWriteTimeUtc;
                }
            }
        }

        public enum Rating
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five
        }

        public class FileModel : NamedObject
        {
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

            FileSystemEntry type;
            [Description("The type of the file system entry.")]
            [ReadOnly(true)]
            public FileSystemEntry Type
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

            Rating rating = Rating.Zero;
            [Description("The type of the file system entry.")]
            [ReadOnly(true)]
            public Rating Rating
            {
                get
                {
                    return rating;
                }
                set
                {
                    rating = value;
                    OnPropertyChanged("Rating");
                }
            }

            DateTime accessed = default(DateTime);
            [Description("The date the file was last accessed.")]
            [ReadOnly(true)]
            public DateTime Accessed
            {
                get
                {
                    return accessed;
                }
                set
                {
                    accessed = value;
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
                    return created;
                }
                set
                {
                    created = value;
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
                    return modified;
                }
                set
                {
                    modified = value;
                    OnPropertyChanged("Modified");
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

            long length = 0L;
            [Description("The length of the file as number of bytes.")]
            [ReadOnly(true)]
            public long Length
            {
                get
                {
                    return length;
                }
                set
                {
                    length = value;
                    OnPropertyChanged("Length");
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

            NetworkCredential credential = null;
            [Description("Network credential used to open the file.")]
            public NetworkCredential Credential
            {
                get
                {
                    return credential;
                }
                set
                {
                    credential = value;
                    OnPropertyChanged("Credential");
                }
            }

            ObservableCollection<FileModel> items = null;
            public ObservableCollection<FileModel> Items
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

            string path = string.Empty;
            [Description("Path to the file.")]
            [ReadOnly(true)]
            public string Path
            {
                get
                {
                    return path;
                }
                set
                {
                    path = value;
                    OnPropertyChanged("Path");
                }
            }

            string resourcesPath = string.Empty;
            [Description("Path to resources for the file.")]
            [StringRepresentation(StringRepresentation.FileSystemPath)]
            public string ResourcesPath
            {
                get
                {
                    return resourcesPath;
                }
                set
                {
                    resourcesPath = value;
                    OnPropertyChanged("ResourcesPath");
                }
            }

            Size size = default(Size);
            [Description("Dimensions of the file if it is an image.")]
            [ReadOnly(true)]
            public Size Size
            {
                get
                {
                    return size;
                }
                set
                {
                    size = value;
                    OnPropertyChanged("Size");
                }
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
                this.Type = FileSystemEntry.Folder;
                this.Set(Info as System.IO.FileSystemInfo);
            }

            void Set(System.IO.FileInfo Info)
            {
                this.Type = FileSystemEntry.File;
                this.Set(Info as System.IO.FileSystemInfo);
                this.Length = Info.Length;
            }

            public FileModel(string Path) : base(Path.GetFileName())
            {
                this.Items = new ObservableCollection<FileModel>();
                if (Path.DirectoryExists())
                    this.Set(new System.IO.DirectoryInfo(Path));
                else if (Path.FileExists())
                    this.Set(new System.IO.FileInfo(Path));
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

        public static DependencyProperty SelectedFileSystemEntriesProperty = DependencyProperty.Register("SelectedFileSystemEntries", typeof(ObservableCollection<FileSystemEntryModel>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<FileSystemEntryModel> SelectedFileSystemEntries
        {
            get
            {
                return (ObservableCollection<FileSystemEntryModel>)GetValue(SelectedFileSystemEntriesProperty);
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

        public static DependencyProperty PropertyGridSourceProperty = DependencyProperty.Register("PropertyGridSource", typeof(ObservableCollection<FileModel>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<FileModel> PropertyGridSource
        {
            get
            {
                return (ObservableCollection<FileModel>)GetValue(PropertyGridSourceProperty);
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

            this.FileSystem = new ObservableCollection<FileSystemEntryModel>();
            this.SelectedFileSystemEntries = new ObservableCollection<FileSystemEntryModel>();
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

            this.PropertyGridSource = new ObservableCollection<FileModel>();
            foreach (var i in System.IO.Directory.EnumerateFileSystemEntries(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
            {
                var Model = new FileModel(i);
                if (Model.Type == FileSystemEntry.Folder)
                {
                    try
                    {
                        var Items = System.IO.Directory.EnumerateFileSystemEntries(Model.Path); //This can potentially fail
                        foreach (var j in Items)
                            Model.Items.Add(new FileModel(j));
                    }
                    catch
                    {
                    }
                }
                this.PropertyGridSource.Add(Model);

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

        #endregion
    }
}
