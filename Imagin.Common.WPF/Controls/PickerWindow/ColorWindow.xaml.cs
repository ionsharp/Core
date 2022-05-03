using Imagin.Common.Configuration;
using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Colors;
using Imagin.Common.Models;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class ColorWindow : PickerWindow
    {
        public static readonly DependencyProperty ActiveDocumentProperty = DependencyProperty.Register(nameof(ActiveDocument), typeof(ColorDocument), typeof(ColorWindow), new FrameworkPropertyMetadata(null));
        public ColorDocument ActiveDocument
        {
            get => (ColorDocument)GetValue(ActiveDocumentProperty);
            set => SetValue(ActiveDocumentProperty, value);
        }
        
        public static readonly DependencyProperty DocumentsProperty = DependencyProperty.Register(nameof(Documents), typeof(DocumentCollection), typeof(ColorWindow), new FrameworkPropertyMetadata(null));
        public DocumentCollection Documents
        {
            get => (DocumentCollection)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(ColorControlOptions), typeof(ColorWindow), new FrameworkPropertyMetadata(null));
        public ColorControlOptions Options
        {
            get => (ColorControlOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        readonly ColorControlOptions options = null;

        public ColorWindow() : base()
        {
            SetCurrentValue(DocumentsProperty, new DocumentCollection());

            ColorControlOptions.Load($@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(ColorWindow)}\Options.data", out options);
            SetCurrentValue(OptionsProperty, options);

            var document = new ColorDocument(System.Windows.Media.Colors.White, ColorModels.RGB)
            {
                CanClose = false,
                CanFloat = false
            };

            Documents.Add(document);
            SetCurrentValue(ActiveDocumentProperty, document);

            this.Bind(ValueProperty, $"{nameof(ActiveDocument)}.{nameof(ColorDocument.Color)}.{nameof(ObservableColor.ActualColor)}", this, System.Windows.Data.BindingMode.TwoWay, ColorToSolidColorBrushConverter.Default);
            InitializeComponent();
        }

        public ColorWindow(string title, Color color) : this()
        {
            SetCurrentValue(TitleProperty,
                title);
            SetCurrentValue(ValueProperty,
                color);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            options.Save();
        }
    }
}