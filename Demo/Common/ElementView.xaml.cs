using Imagin.Common.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Demo
{
    public class ElementCollection : Imagin.Common.Collections.Generic.ObservableCollection<Element> { }

    [ContentProperty(nameof(Elements))]
    public partial class ElementView : UserControl
    {
        public static readonly DependencyProperty PanelProperty = DependencyProperty.Register(nameof(Panel), typeof(DataPanel), typeof(ElementView), new FrameworkPropertyMetadata(null, OnPanelChanged));
        public DataPanel Panel
        {
            get => (DataPanel)GetValue(PanelProperty);
            set => SetValue(PanelProperty, value);
        }
        static void OnPanelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ElementView view)
            {
                if (e.NewValue is DataPanel panel)
                    panel.Data = view.Elements;
            }
        }

        public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements), typeof(ElementCollection), typeof(ElementView), new FrameworkPropertyMetadata(null));
        public ElementCollection Elements
        {
            get => (ElementCollection)GetValue(ElementsProperty);
            set => SetValue(ElementsProperty, value);
        }

        public static readonly DependencyProperty SelectedElementProperty = DependencyProperty.Register(nameof(SelectedElement), typeof(Element), typeof(ElementView), new FrameworkPropertyMetadata(default(Element)));
        public Element SelectedElement
        {
            get => (Element)GetValue(SelectedElementProperty);
            set => SetValue(SelectedElementProperty, value);
        }
        
        public ElementView() : base()
        {
            SetCurrentValue(ElementsProperty, new ElementCollection());
            InitializeComponent();
        }
    }
}