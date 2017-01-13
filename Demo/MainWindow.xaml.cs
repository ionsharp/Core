using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Extensions;
using Imagin.Common.Primitives;
using Imagin.Controls.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.NET.Demo
{
    #region FileSystemEntryModel

    public enum ServerObjectType
    {
        Folder,
        File
    }
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
        [Category("General")]
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

        ObservableCollection<FileSystemEntryModel> items = new ObservableCollection<FileSystemEntryModel>();
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

        async void Set(string Path)
        {
            await Task.Run(() => 
            {
                bool IsFolder = System.IO.Directory.Exists(Path), IsFile = System.IO.File.Exists(Path);

                if (IsFolder)
                    Set(new System.IO.DirectoryInfo(Path));
                else if (IsFile)
                    Set(new System.IO.FileInfo(Path));

                Name = !IsFolder && !IsFile ? Path : Path.GetFileName();
            });
        }

        void Set(System.IO.FileSystemInfo Info)
        {
            Path = Info.FullName;
            Accessed = Info.LastAccessTime;
            Created = Info.CreationTime;
            Modified = Info.LastWriteTime;
        }

        void Set(System.IO.DirectoryInfo Info)
        {
            Set(Info as System.IO.FileSystemInfo);
        }

        void Set(System.IO.FileInfo Info)
        {
            Size = Info.Length;

            Set(Info as System.IO.FileSystemInfo);
        }

        public FileSystemEntryModel(string path) : base()
        {
            Path = path;
            Set(path);
        }
    }

    #endregion

    #region FileSystemCollection

    public class FileSystemCollection : ConcurrentObservableCollection<FileSystemEntryModel>
    {
        string lastPath = string.Empty;
        public string LastPath
        {
            get
            {
                return lastPath;
            }
            set
            {
                lastPath = value;
                OnPropertyChanged("LastPath");
            }
        }

        public async void Set(string Path)
        {
            Clear();

            var Items = default(IEnumerable<string>);

            await Task.Run(new Action(() =>
            {
                try
                {
                    Items = Path.IsNullOrEmpty() ? System.IO.Directory.GetLogicalDrives() : System.IO.Directory.EnumerateFileSystemEntries(Path);
                }
                catch
                {
                    Items = null;
                }
            }));

            if (Items != null)
            {
                await App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    foreach (var i in Items)
                        Add(new FileSystemEntryModel(i));
                }));
            }

            LastPath = Path;
        }

        public FileSystemCollection() : base()
        {
        }
    }

    #endregion

    #region HierarchialObject

    public class HierarchialObject : NamedObject
    {
        ObservableCollection<HierarchialObject> items = null;
        public ObservableCollection<HierarchialObject> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public HierarchialObject(string Name) : base(Name)
        {
            Items = new ObservableCollection<HierarchialObject>();
        }
    }

    #endregion

    #region WildObject

    public class WildObject : NamedObject
    {
        bool boolean = false;
        [Description("Description for Boolean property.")]
        public bool Boolean
        {
            get
            {
                return boolean;
            }
            set
            {
                boolean = value;
                OnPropertyChanged("Boolean");
            }
        }

        byte _byte = (byte)0;
        [Description("Description for Byte property.")]
        public byte Byte
        {
            get
            {
                return _byte;
            }
            set
            {
                _byte = value;
                OnPropertyChanged("Byte");
            }
        }

        DateTime dateTime = DateTime.Now;
        [Description("Description for DateTime property.")]
        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                dateTime = value;
                OnPropertyChanged("DateTime");
            }
        }

        decimal _decimal = 0m;
        [Description("Description for Decimal property.")]
        public decimal Decimal
        {
            get
            {
                return _decimal;
            }
            set
            {
                _decimal = value;
                OnPropertyChanged("Decimal");
            }
        }

        double _double = 0d;
        [Description("Description for Double property.")]
        public double Double
        {
            get
            {
                return _double;
            }
            set
            {
                _double = value;
                OnPropertyChanged("Double");
            }
        }

        Guid guid = Guid.NewGuid();
        [Description("Description for Guid property.")]
        public Guid Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
                OnPropertyChanged("Guid");
            }
        }

        int _int = 0;
        [Description("Description for Int property.")]
        public int Int
        {
            get
            {
                return _int;
            }
            set
            {
                _int = value;
                OnPropertyChanged("Int");
            }
        }

        long _long = 0L;
        [Description("Description for Long property.")]
        public long Long
        {
            get
            {
                return _long;
            }
            set
            {
                _long = value;
                OnPropertyChanged("Long");
            }
        }

        NetworkCredential networkCredential = new NetworkCredential("UserName", "Password");
        [Description("Description for NetworkCredential property.")]
        public NetworkCredential NetworkCredential
        {
            get
            {
                return networkCredential;
            }
            set
            {
                networkCredential = value;
                OnPropertyChanged("NetworkCredential");
            }
        }

        Point point = new Point(0, 0);
        [Description("Description for Point property.")]
        public Point Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
                OnPropertyChanged("Point");
            }
        }

        short _short = (short)0;
        [Description("Description for Short property.")]
        public short Short
        {
            get
            {
                return _short;
            }
            set
            {
                _short = value;
                OnPropertyChanged("Short");
            }
        }

        Size size = new Size(0, 0);
        [Description("Description for Size property.")]
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

        string normalString = string.Empty;
        [Description("Description for NormalString property.")]
        public string NormalString
        {
            get
            {
                return normalString;
            }
            set
            {
                normalString = value;
                OnPropertyChanged("NormalString");
            }
        }

        string filePathString = string.Empty;
        [Description("Description for FilePathString property.")]
        [StringKind(StringKind.FilePath)]
        public string FilePathString
        {
            get
            {
                return filePathString;
            }
            set
            {
                filePathString = value;
                OnPropertyChanged("FilePathString");
            }
        }

        string folderPathString = string.Empty;
        [Description("Description for FolderPathString property.")]
        [StringKind(StringKind.FolderPath)]
        public string FolderPathString
        {
            get
            {
                return folderPathString;
            }
            set
            {
                folderPathString = value;
                OnPropertyChanged("FolderPathString");
            }
        }

        string multilineString = string.Empty;
        [Description("Description for MultilineString property.")]
        [StringKind(StringKind.Multiline)]
        public string MultilineString
        {
            get
            {
                return multilineString;
            }
            set
            {
                multilineString = value;
                OnPropertyChanged("MultilineString");
            }
        }

        string passwordString = string.Empty;
        [Description("Description for PasswordString property.")]
        [StringKind(StringKind.Password)]
        public string PasswordString
        {
            get
            {
                return passwordString;
            }
            set
            {
                passwordString = value;
                OnPropertyChanged("PasswordString");
            }
        }

        Uri uri = new Uri("http://www.google.com");
        [Description("Description for Uri property.")]
        public Uri Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
                OnPropertyChanged("Uri");
            }
        }

        Version version = new Version();
        [Description("Description for Version property.")]
        public Version Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        public WildObject(string Name) : base(Name)
        {
        }
    }

    #endregion

    #region ViewEnum

    public enum ViewEnum
    {
        List,
        Details
    }

    #endregion

    #region MainWindow

    public partial class MainWindow : BasicWindow
    {
        #region Properties

        public static DependencyProperty FileSystemCollectionProperty = DependencyProperty.Register("FileSystemCollection", typeof(FileSystemCollection), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FileSystemCollection FileSystemCollection
        {
            get
            {
                return (FileSystemCollection)GetValue(FileSystemCollectionProperty);
            }
            set
            {
                SetValue(FileSystemCollectionProperty, value);
            }
        }

        public static DependencyProperty FileSystemCollectionViewProperty = DependencyProperty.Register("FileSystemCollectionView", typeof(ListCollectionView), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ListCollectionView FileSystemCollectionView
        {
            get
            {
                return (ListCollectionView)GetValue(FileSystemCollectionViewProperty);
            }
            set
            {
                SetValue(FileSystemCollectionViewProperty, value);
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

        public static DependencyProperty ListViewProperty = DependencyProperty.Register("ListView", typeof(ViewEnum), typeof(MainWindow), new FrameworkPropertyMetadata(ViewEnum.Details, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ViewEnum ListView
        {
            get
            {
                return (ViewEnum)GetValue(ListViewProperty);
            }
            set
            {
                SetValue(ListViewProperty, value);
            }
        }

        public static DependencyProperty PropertyGridSourceProperty = DependencyProperty.Register("PropertyGridSource", typeof(ObservableCollection<WildObject>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<WildObject> PropertyGridSource
        {
            get
            {
                return (ObservableCollection<WildObject>)GetValue(PropertyGridSourceProperty);
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

            //AlignableWrapPanel
            for (int i = 1; i <= 84; i++)
            {
                PART_AlignableWrapPanel.Children.Add(new Button()
                {
                    Content = "Button {0}".F(i)
                });
            }

            //AdvancedComboBox
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

            //DataGrid/ListView/TabbedTree
            FileSystemCollection = new FileSystemCollection();
            foreach (var i in System.IO.Directory.GetLogicalDrives())
                FileSystemCollection.Add(new FileSystemEntryModel(i));

            //ListView
            FileSystemCollectionView = new ListCollectionView(FileSystemCollection);

            //PropertyGrid
            PropertyGridSource = new ObservableCollection<WildObject>();
            for (var i = 0; i < 5; i++)
                PropertyGridSource.Add(new WildObject("Wild Object " + i));
        }

        /*
        long RandomLong(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }
        */

        #endregion

        #region Handlers

        void OnGetParentPath(object sender, RoutedEventArgs e)
        {
            if (!FileSystemCollection.LastPath.IsNull() && FileSystemCollection.LastPath.DirectoryExists())
                FileSystemCollection.Set(FileSystemCollection.LastPath.GetDirectoryName());
        }

        void OnLinkPressed(object sender, RoutedEventArgs e)
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

        void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var Path = (sender as FrameworkElement).Tag.ToString();
            if (Path.DirectoryExists())
                FileSystemCollection.Set(Path);
        }

        void OnPasswordEntered(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MessageBox.Show("Entered a password!");
        }

        void OnColorSelected(object sender, RoutedEventArgs e)
        {
            PART_ColorPicker.InitialColor = PART_ColorPicker.SelectedColor;
        }

        void OnSpacerThicknessChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                PART_Spacer.Spacing = new Thickness(PART_SpacerThicknessLeft.Value, PART_SpacerThicknessTop.Value, PART_SpacerThicknessRight.Value, PART_SpacerThicknessBottom.Value);
            }
            catch
            {
            }
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            ListView = sender.As<RadioButton>().Content.ToString().ParseEnum<ViewEnum>();
        }

        #endregion
    }

    #endregion
}