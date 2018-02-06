using Imagin.Common.Linq;
using Imagin.Controls.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.NET.Demo
{
    public partial class MainWindow : BasicWindow
    {
        #region Properties

        Fruits fruits = Fruits.Kiwi;
        public Fruits Fruits
        {
            get
            {
                return fruits;
            }
            set
            {
                fruits = value;
                OnPropertyChanged("Fruits");
            }
        }
        
        public static DependencyProperty HierarchialCollectionProperty = DependencyProperty.Register("HierarchialCollection", typeof(HierarchialCollection), typeof(MainWindow), new FrameworkPropertyMetadata(default(HierarchialCollection), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public HierarchialCollection HierarchialCollection
        {
            get
            {
                return (HierarchialCollection)GetValue(HierarchialCollectionProperty);
            }
            set
            {
                SetValue(HierarchialCollectionProperty, value);
            }
        }

        public static DependencyProperty ListViewProperty = DependencyProperty.Register("ListView", typeof(View), typeof(MainWindow), new FrameworkPropertyMetadata(View.Details, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public View ListView
        {
            get
            {
                return (View)GetValue(ListViewProperty);
            }
            set
            {
                SetValue(ListViewProperty, value);
            }
        }

        public static DependencyProperty StorageCollectionProperty = DependencyProperty.Register("StorageCollection", typeof(StorageCollection), typeof(MainWindow), new FrameworkPropertyMetadata(default(StorageCollection), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public StorageCollection StorageCollection
        {
            get
            {
                return (StorageCollection)GetValue(StorageCollectionProperty);
            }
            set
            {
                SetValue(StorageCollectionProperty, value);
            }
        }

        public static DependencyProperty StorageCollectionViewProperty = DependencyProperty.Register("StorageCollectionView", typeof(ListCollectionView), typeof(MainWindow), new FrameworkPropertyMetadata(default(ListCollectionView), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ListCollectionView StorageCollectionView
        {
            get
            {
                return (ListCollectionView)GetValue(StorageCollectionViewProperty);
            }
            set
            {
                SetValue(StorageCollectionViewProperty, value);
            }
        }

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();

            foreach (var i in PART_ControlView.Controls)
            {
                if (i.Type == typeof(AlignableWrapPanel))
                {
                    for (int j = 1; j <= 32; j++)
                    {
                        i.Instance.As<AlignableWrapPanel>().Children.Add(new Button()
                        {
                            Content = "Button {0}".F(j)
                        });
                    }
                    break;
                }
            }

            SetCurrentValue(HierarchialCollectionProperty, new HierarchialCollection());
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

            SetCurrentValue(StorageCollectionProperty, new StorageCollection(string.Empty));
            SetCurrentValue(StorageCollectionViewProperty, new ListCollectionView(StorageCollection));
        }

        #endregion

        #region Handlers

        void OnColorSelected(object sender, RoutedEventArgs e)
        {
            var i = PART_ControlView.SelectedControl.Instance as ColorPicker;

            if (i != null)
                i.InitialColor = i.SelectedColor;
        }

        void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender.As<FrameworkElement>()?.Tag != null)
            {
                var Path = (sender as FrameworkElement).Tag.ToString();
                if (Path.DirectoryExists())
                    StorageCollection.Set(Path);
            }
        }

        void OnLinkPressed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked link!");
        }

        void OnMaskedButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked button!");
        }

        void OnTextBoxEntered(object sender, Controls.Common.Input.TextSubmittedEventArgs e)
        {
            MessageBox.Show("Entered text!");
        }

        void OnSelectionCanvasSelected(object sender, Common.Input.EventArgs<Common.Primitives.Selection> e)
        {
            MessageBox.Show("Made a selection!");
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            foreach (var i in PART_ControlView.Controls)
            {
                if (i.Type == typeof(System.Windows.Controls.ListView))
                {
                    SetCurrentValue(ListViewProperty, sender.As<RadioButton>().Content.ToString().ParseEnum<View>());
                    break;
                }
            }
        }

        #endregion
    }
}