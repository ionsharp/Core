using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Data;

namespace Imagin.NET.Demo
{
    public partial class MainWindow : BasicWindow
    {
        #region Properties

        Fruits _fruits = Fruits.Kiwi;
        public Fruits Fruits
        {
            get => _fruits;
            set => Property.Set(this, ref _fruits, value);
        }
        
        public static DependencyProperty HierarchialCollectionProperty = DependencyProperty.Register(nameof(HierarchialCollection), typeof(HierarchialCollection), typeof(MainWindow), new FrameworkPropertyMetadata(default(HierarchialCollection)));
        public HierarchialCollection HierarchialCollection
        {
            get => (HierarchialCollection)GetValue(HierarchialCollectionProperty);
            set => SetValue(HierarchialCollectionProperty, value);
        }

        public static DependencyProperty ListViewProperty = DependencyProperty.Register(nameof(ListView), typeof(View), typeof(MainWindow), new FrameworkPropertyMetadata(View.Details));
        public View ListView
        {
            get => (View)GetValue(ListViewProperty);
            set => SetValue(ListViewProperty, value);
        }

        public static DependencyProperty StorageCollectionProperty = DependencyProperty.Register(nameof(StorageCollection), typeof(StorageCollection), typeof(MainWindow), new FrameworkPropertyMetadata(default(StorageCollection)));
        public StorageCollection StorageCollection
        {
            get => (StorageCollection)GetValue(StorageCollectionProperty);
            set => SetValue(StorageCollectionProperty, value);
        }

        public static DependencyProperty StorageCollectionViewProperty = DependencyProperty.Register(nameof(StorageCollectionView), typeof(ListCollectionView), typeof(MainWindow), new FrameworkPropertyMetadata(default(ListCollectionView)));
        public ListCollectionView StorageCollectionView
        {
            get => (ListCollectionView)GetValue(StorageCollectionViewProperty);
            set => SetValue(StorageCollectionViewProperty, value);
        }

        #endregion

        #region Constructors

        public MainWindow() : base()
        {
            InitializeComponent();

            foreach (var i in CommonControlView.Controls)
            {
                if (i.Type == typeof(AlignableWrapPanel))
                {
                    for (int j = 1; j <= 32; j++)
                    {
                        i.Instance.As<AlignableWrapPanel>().Children.Add(new System.Windows.Controls.Button()
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
            //var i = PART_ControlView.SelectedControl.Instance as ColorPicker;

            //if (i != null)
                //i.InitialColor = i.SelectedColor;
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

        void OnTextBoxEntered(object sender, TextSubmittedEventArgs e)
        {
            MessageBox.Show("Entered text!");
        }

        void OnSelectionCanvasSelected(object sender, EventArgs<Selection> e)
        {
            MessageBox.Show("Made a selection!");
        }

        void OnViewChanged(object sender, RoutedEventArgs e)
        {
            /*
            foreach (var i in PART_ControlView.Controls)
            {
                if (i.Type == typeof(System.Windows.Controls.ListView))
                {
                    SetCurrentValue(ListViewProperty, sender.As<RadioButton>().Content.ToString().ParseEnum<View>());
                    break;
                }
            }
            */
        }

        #endregion
    }
}