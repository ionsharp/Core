using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Extensions;
using Imagin.Common.Text;
using Imagin.Common.Web;
using Imagin.Controls.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        Guid id = Guid.NewGuid();
        [Category("General")]
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

        [Category("General")]
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
        [Browsable(false)]
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
        [Category("Dimensions")]
        [Description("The size of the file system entry in bytes.")]
        [Int64Representation(Int64Representation.FileSize)]
        [ReadOnly(true)]
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
        [Category("General")]
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
        [Category("Attributes")]
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
        [Category("Attributes")]
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
        [Category("Misc")]
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
        [Category("Misc")]
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
        [Category("Date")]
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
        [Category("Date")]
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
        [Category("Date")]
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

        ObservableCollection<FileSystemEntryModel> items = new ObservableCollection<FileSystemEntryModel>();
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
            Path = Info.FullName;
            Accessed = Info.LastAccessTime;
            Created = Info.CreationTime;
            Modified = Info.LastWriteTime;
            IsHidden = (Info.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden;
            IsReadOnly = (Info.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly;
        }

        void Set(System.IO.DirectoryInfo Info)
        {
            Type = ServerObjectType.Folder;

            Set(Info as System.IO.FileSystemInfo);
        }

        void Set(System.IO.FileInfo Info)
        {
            Type = ServerObjectType.File;
            Size = Info.Length;

            Set(Info as System.IO.FileSystemInfo);
        }

        public FileSystemEntryModel(string Path) : base()
        {
            this.Path = Path;

            bool IsFolder = System.IO.Directory.Exists(Path), IsFile = System.IO.File.Exists(Path);

            if (IsFolder)
                Set(new System.IO.DirectoryInfo(Path));
            else if (IsFile)
                Set(new System.IO.FileInfo(Path));

            Name = !IsFolder && !IsFile ? Path : Path.GetFileName();
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