using Imagin.Controls.Common;
using System.Collections.ObjectModel;
using System.Windows;

namespace Imagin.Gadgets.Search
{
    public partial class SearchGadget : Gadget
    {
        public static DependencyProperty ProvidersProperty = DependencyProperty.Register("Providers", typeof(ObservableCollection<SearchProvider>), typeof(SearchGadget), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<SearchProvider> Providers
        {
            get
            {
                return (ObservableCollection<SearchProvider>)GetValue(ProvidersProperty);
            }
            set
            {
                SetValue(ProvidersProperty, value);
            }
        }

        public static DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(SearchProvider), typeof(SearchGadget), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public SearchProvider Provider
        {
            get
            {
                return (SearchProvider)GetValue(ProviderProperty);
            }
            set
            {
                SetValue(ProviderProperty, value);
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

        public SearchGadget() : base()
        {
            Providers = new ObservableCollection<SearchProvider>();

            Providers.Add(new SearchProvider("Bing", @"http://www.bing.com/search?q="));
            Providers.Add(new SearchProvider("Google", @"https://www.google.com/#q="));
            Providers.Add(new SearchProvider("Yahoo", @"https://search.yahoo.com/search;?p="));

            Provider = Providers[1];

            InitializeComponent();
        }

        void ExecuteQuery()
        {
            var query = Query;
            Provider.Query(query);
            Query = string.Empty;
        }

        void OnEntered(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ExecuteQuery();
        }

        void OnSearched(object sender, RoutedEventArgs e)
        {
            ExecuteQuery();
        }
    }
}
