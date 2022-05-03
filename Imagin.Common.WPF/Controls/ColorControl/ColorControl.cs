using Imagin.Common.Collections.Generic;
using Imagin.Common.Configuration;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class ColorControl : Control
    {
        public static readonly ResourceKey<PanelTemplateSelector> PanelTemplateSelectorKey = new();

        readonly ColorsPanel colorsPanel;

        public static readonly DependencyProperty ActiveDocumentProperty = DependencyProperty.Register(nameof(ActiveDocument), typeof(ColorDocument), typeof(ColorControl), new FrameworkPropertyMetadata(null));
        public ColorDocument ActiveDocument
        {
            get => (ColorDocument)GetValue(ActiveDocumentProperty);
            set => SetValue(ActiveDocumentProperty, value);
        }

        public static readonly DependencyProperty AlphaPanelProperty = DependencyProperty.Register(nameof(AlphaPanel), typeof(AlphaPanel), typeof(ColorControl), new FrameworkPropertyMetadata(null));
        public AlphaPanel AlphaPanel
        {
            get => (AlphaPanel)GetValue(AlphaPanelProperty);
            private set => SetValue(AlphaPanelProperty, value);
        }

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(IColorControlOptions), typeof(ColorControl), new FrameworkPropertyMetadata(null, OnOptionsChanged));
        public IColorControlOptions Options
        {
            get => (IColorControlOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }
        static void OnOptionsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ColorControl>().OnOptionsChanged(e.NewValue as IColorControlOptions);

        public static readonly DependencyProperty OptionsPanelProperty = DependencyProperty.Register(nameof(OptionsPanel), typeof(OptionsPanel), typeof(ColorControl), new FrameworkPropertyMetadata(null));
        public OptionsPanel OptionsPanel
        {
            get => (OptionsPanel)GetValue(OptionsPanelProperty);
            set => SetValue(OptionsPanelProperty, value);
        }

        public static readonly DependencyProperty DocumentsProperty = DependencyProperty.Register(nameof(Documents), typeof(DocumentCollection), typeof(ColorControl), new FrameworkPropertyMetadata(null));
        public DocumentCollection Documents
        {
            get => (DocumentCollection)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }

        static readonly DependencyPropertyKey PanelsKey = DependencyProperty.RegisterReadOnly(nameof(Panels), typeof(PanelCollection), typeof(ColorControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty PanelsProperty = PanelsKey.DependencyProperty;
        public PanelCollection Panels
        {
            get => (PanelCollection)GetValue(PanelsProperty);
            private set => SetValue(PanelsKey, value);
        }

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(nameof(SaveCommand), typeof(ICommand), typeof(ColorControl), new FrameworkPropertyMetadata(null));
        public ICommand SaveCommand
        {
            get => (ICommand)GetValue(SaveCommandProperty);
            set => SetValue(SaveCommandProperty, value);
        }

        readonly ColorControlOptions defaultOptions = null;

        public ColorControl() : base()
        {
            this.RegisterHandler(OnLoaded, OnUnloaded);

            ColorControlOptions.Load($@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(ColorControl)}\Options.data", out defaultOptions);

            SetCurrentValue(DocumentsProperty,
                new DocumentCollection());

            var panels = new PanelCollection();
            SetCurrentValue(AlphaPanelProperty, new AlphaPanel());

            panels.Add
                (AlphaPanel);

            colorsPanel = new ColorsPanel(defaultOptions.Colors, () => ActiveDocument?.OldColor ?? System.Windows.Media.Colors.White, () => ActiveDocument?.Color.ActualColor ?? System.Windows.Media.Colors.White);
            panels.Add
                (colorsPanel);
            panels.Add
                (new ComponentPanel());

            SetCurrentValue(OptionsPanelProperty, new OptionsPanel());

            panels.Add
                (OptionsPanel);
            panels.Add
                (new PropertiesPanel());

            Panels = panels;

            SetCurrentValue(OptionsProperty, defaultOptions);
        }

        void OnColorSaved(object sender, EventArgs<Color> e)
        {
            colorsPanel?.SelectedGroup.If(i => i.Add(e.Value));
        }

        void OnDocumentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems[0].As<ColorDocument>().ColorSaved += OnColorSaved;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems[0].As<ColorDocument>().ColorSaved -= OnColorSaved;
                    break;
            }
        }

        void OnLoaded()
        {
            Documents.CollectionChanged += OnDocumentsChanged;
            Documents.ForEach(i => i.As<ColorDocument>().ColorSaved += OnColorSaved);
        }

        void OnUnloaded()
        {
            Documents.CollectionChanged -= OnDocumentsChanged;
            Documents.ForEach(i => i.As<ColorDocument>().ColorSaved -= OnColorSaved);
        }

        protected virtual void OnOptionsChanged(IColorControlOptions input)
        {
            if (input != null)
            {
                input.OnLoaded(this);
                colorsPanel?.Initialize(input.Colors, () => ActiveDocument?.OldColor ?? System.Windows.Media.Colors.White, () => ActiveDocument?.Color.ActualColor ?? System.Windows.Media.Colors.White);
                colorsPanel.Selected += OnColorSelected;
            }
        }

        void OnColorSelected(object sender, EventArgs<Media.StringColor> e)
            => ActiveDocument.If(i => i.Color.ActualColor = e.Value);

        ICommand saveOldColorCommand;
        public ICommand SaveOldColorCommand => saveOldColorCommand ??= new RelayCommand(() => colorsPanel?.SelectedGroup.If<GroupCollection<StringColor>>(i => i.Add(ActiveDocument.OldColor)), () => ActiveDocument != null);

        ICommand saveNewColorCommand;
        public ICommand SaveNewColorCommand => saveNewColorCommand ??= new RelayCommand(() => colorsPanel?.SelectedGroup.If<GroupCollection<StringColor>>(i => i.Add(ActiveDocument.Color.ActualColor)), () => ActiveDocument != null);
    }
}