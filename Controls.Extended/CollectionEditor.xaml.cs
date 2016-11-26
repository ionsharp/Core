using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Extended
{
    public partial class CollectionEditor : UserControl
    {
        public static DependencyProperty CollectionProperty = DependencyProperty.Register("Collection", typeof(object), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCollectionChanged));
        public object Collection
        {
            get
            {
                return (object)GetValue(CollectionProperty);
            }
            set
            {
                SetValue(CollectionProperty, value);
            }
        }
        static void OnCollectionChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<CollectionEditor>().OnCollectionChanged((object)e.NewValue);
        }

        public static DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(CollectionEditor), new FrameworkPropertyMetadata(DataGridGridLinesVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataGridGridLinesVisibility GridLinesVisibility
        {
            get
            {
                return (DataGridGridLinesVisibility)GetValue(GridLinesVisibilityProperty);
            }
            set
            {
                SetValue(GridLinesVisibilityProperty, value);
            }
        }

        public static DependencyProperty HeadersVisibilityProperty = DependencyProperty.Register("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(CollectionEditor), new FrameworkPropertyMetadata(DataGridHeadersVisibility.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataGridHeadersVisibility HeadersVisibility
        {
            get
            {
                return (DataGridHeadersVisibility)GetValue(HeadersVisibilityProperty);
            }
            set
            {
                SetValue(HeadersVisibilityProperty, value);
            }
        }

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object SelectedItem
        {
            get
            {
                return (object)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        protected virtual void OnCollectionChanged(object Value)
        {
        }

        public CollectionEditor()
        {
            InitializeComponent();
        }
    }
}
