using System.Collections.ObjectModel;
using System.Windows;

namespace Imagin.Gadgets.Search
{
    public partial class SearchGadget : Gadget
    {
        public static DependencyProperty TypeProperty = DependencyProperty.Register("SearchEngines", typeof(ObservableCollection<SearchEngine>), typeof(SearchGadget), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<SearchEngine> SearchEngines
        {
            get
            {
                return (ObservableCollection<SearchEngine>)GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        public static DependencyProperty SearchEngineProperty = DependencyProperty.Register("SearchEngine", typeof(SearchEngine), typeof(SearchGadget), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public SearchEngine SearchEngine
        {
            get
            {
                return (SearchEngine)GetValue(SearchEngineProperty);
            }
            set
            {
                SetValue(SearchEngineProperty, value);
            }
        }

        public static DependencyProperty QueryProperty = DependencyProperty.Register("Query", typeof(string), typeof(SearchGadget), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Query
        {
            get
            {
                return (string)GetValue(QueryProperty);
            }
            set
            {
                SetValue(QueryProperty, value);
            }
        }

        public SearchGadget()
        {
            InitializeComponent();

            this.SearchEngines = new ObservableCollection<SearchEngine>();
            this.SearchEngines.Add(new SearchEngine(SearchEngineType.Google));
            this.SearchEngines.Add(new SearchEngine(SearchEngineType.Bing));
            this.SearchEngines.Add(new SearchEngine(SearchEngineType.Yahoo));

            this.SearchEngine = this.SearchEngines[0];
        }

        void ExecuteQuery()
        {
            this.SearchEngine.Query(this.Query);
            this.Query = string.Empty;
        }

        void OnTextBoxPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                this.ExecuteQuery();
        }

        void OnSearchClick(object sender, RoutedEventArgs e)
        {
            this.ExecuteQuery();
        }

        void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void OnImageMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
