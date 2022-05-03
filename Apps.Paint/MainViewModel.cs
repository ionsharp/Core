using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    public class MainViewModel : DockViewModel<MainWindow, Document>
    {
        #region Properties

        ListCollectionView effects = null;
        public ListCollectionView Effects
        {
            get => effects;
            set => this.Change(ref effects, value);
        }

        Layer selectedLayer;
        public Layer SelectedLayer
        {
            get => selectedLayer;
            set => this.Change(ref selectedLayer, value);
        }

        Tool selectedTool;
        public Tool SelectedTool
        {
            get => selectedTool;
            set => this.Change(ref selectedTool, value);
        }

        #endregion

        #region MainViewModel
        
        public MainViewModel() : base()
        {
            var effects = new ObservableCollection<ImageEffect>();
            EffectCollection.Types.ForEach(i => effects.Add(i.Create<ImageEffect>()));

            Effects = new(effects);
            Effects.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ImageEffect.Category)));
            Effects.SortDescriptions.Add(new SortDescription(nameof(ImageEffect.Category), ListSortDirection.Ascending));
            Effects.SortDescriptions.Add(new SortDescription(nameof(ImageEffect.Name), ListSortDirection.Ascending));

            //...

            Panel.Find<BrushesPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);

            Panel.Find<ColorsPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);
            Panel.Find<ColorsPanel>()
                .Selected += (sender, e) => Get.Current<Options>().ForegroundColor = e.Value;
            
            Panel.Find<CurvesPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);

            Panel.Find<ExplorePanel>()
                .OpenedFile += (sender, e) => _ = OpenAsync(e.Value);

            Panel.Find<GradientsPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);
            Panel.Find<LayersPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);
            Panel.Find<MatricesPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);
            Panel.Find<SelectionsPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);
            Panel.Find<ShapesPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);
            Panel.Find<StylesPanel>().If
                (i => i.PropertyChanged += OnPanelChanged);

            Panel.Find<ToolsPanel>()
                .ToolSelected += OnToolSelected;

            //... Tests ...

            int newHeight = 540; int newWidth = 300;

            var document = new ImageDocument($"Test ({newWidth}x{newHeight})", new(newWidth, newHeight), 72);
            document.Zoom = 0.5;

            int count = 9;
            BlendModes[] blend = new BlendModes[9]
            {
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal,
                BlendModes.Normal
            };

            for (var i = 1; i < count + 1; i++)
            {
                var image = Resources.ProjectUri($@"Stock\{i}.jpg").GetImage().Bitmap(ImageExtensions.Jpg).WriteableBitmap().Resize(newWidth, newHeight, Interpolations.Bilinear); ;

                ImageEffect[] e = null;
                /*
                if (i == 1)
                    e = new ImageEffect[] { new GradientOverlayEffect() { Opacity = 0.5, Type = GradientType.Circle } };
                */

                var layer = new PixelLayer($"Layer {i}", image, e);
                layer.Style.BlendMode = blend[i - 1];

                document.Layers.Add(layer);
            }

            /*
            var shapeLayer = new RegionShapeLayer("Untitled", Brushes.Green, Brushes.Blue, 4, new(newWidth, newHeight));
            shapeLayer.Height = 96; shapeLayer.Width = 96;
            shapeLayer.X = 10; shapeLayer.Y = 10;
            shapeLayer.Points = new(Shape.GetEllipse(new(10, 10, 96, 96), true));
            shapeLayer.Render();

            shapeLayer.Style.Effects.Add(new GradientOverlayEffect() { Type = GradientType.Diamond });

            document.Layers.Insert(0, shapeLayer);
            shapeLayer.IsSelected = true;
            */
            Documents.Add(document);
        }

        #endregion

        #region Methods

        void OnPanelChanged(object sender, PropertyChangedEventArgs e)
        {
            object result = null;
            if (sender is LayersPanel layersPanel)
            {
                if (e.PropertyName == nameof(LayersPanel.SelectedLayer))
                {
                    SelectedLayer = layersPanel.SelectedLayer;
                    result = layersPanel.SelectedLayer;
                }
            }
            else if (e.PropertyName == "SelectedItem")
            {
                dynamic i = sender;
                result = i.SelectedItem;
            }
            Panel.Find<PropertiesPanel>().Select(result);
        }

        void OnToolSelected(object sender, EventArgs<Tool> e)
        {
            SelectedTool = e.Value;
            Panel.Find<ToolPanel>().Tool = e.Value;

            Get.Current<Options>().Tools.ForEach<Tool>(i =>
            {
                if (!ReferenceEquals(i, e.Value))
                    i.IsSelected = false;
            });
        }

        //...

        void Capture(System.Drawing.Bitmap bitmap)
        {
            if (Get.Current<Options>().CaptureClipboard)
            {
                Clipboard.SetImage(bitmap.BitmapSource());
            }
            else if (Get.Current<Options>().CaptureFile)
            {
                var result = Document.SaveAs(bitmap);
                if (!result)
                {
                    Dialog.Show(nameof(Capture), "Couldn't capture file.", DialogImage.Error, Buttons.Ok);
                    return;
                }
            }
            else if (Get.Current<Options>().CaptureLayer)
            {
                if (Panel.Find<LayersPanel>().Layers != null)
                {
                    var newLayer = new PixelLayer("Capture", bitmap.WriteableBitmap());
                    Panel.Find<LayersPanel>().Layers.InsertAbove(Panel.Find<LayersPanel>().SelectedLayer, Array<Layer>.New(newLayer));
                }
            }
        }

        //...

        protected void OnOpened(Document document)
        {
            Documents.Add(document);
            Get.Current<Options>().RecentFiles.Add(document.Path);
        }

        //...

        public override IEnumerable<Panel> GetPanels()
        {
            yield return new BrushesPanel();
            yield return new CharacterPanel();
            yield return new ColorPanel();
            yield return new ColorsPanel(Get.Current<Options>().Colors, () => Get.Current<Options>().BackgroundColor, () => Get.Current<Options>().ForegroundColor);
            yield return new CurvesPanel();
            yield return new ExplorePanel();
            yield return new GradientsPanel();
            yield return new HistogramPanel();
            yield return new HistoryPanel();
            yield return new LayersPanel();
            yield return new MatricesPanel();
            yield return new NotesPanel();
            yield return new ParagraphPanel();
            yield return new PathsPanel();
            yield return new PropertiesPanel();
            yield return new SelectionsPanel();
            yield return new ShapesPanel(); 
            yield return new StylesPanel();
            yield return new ToolPanel();
            yield return new ToolsPanel();
        }

        //...

        public void Open(string filePath, params ImageEffect[] effects)
        {
            var fileExtension = System.IO.Path.GetExtension(filePath).Substring(1);

            //If the file is already open, just activate it
            if (Documents.FirstOrDefault(i => i.As<Document>().Path == filePath) is Document existingDocument)
            {
                ActiveContent = existingDocument;
                return;
            }

            if (ImageFormats.IsReadable(fileExtension))
            {
                Document document = null;
                switch (fileExtension)
                {
                    case "image":
                        BinarySerializer.Deserialize(filePath, out document);
                        if (document == null)
                        {
                            Document.AssertInvalid(filePath);
                            return;
                        }
                        break;

                    default:
                        var result = ImageDocument.Open(filePath);
                        if (result == null)
                        {
                            Document.AssertInvalid(filePath);
                            return;
                        }

                        document = new ImageDocument(result.WriteableBitmap(), filePath, effects);
                        break;
                }
                OnOpened(document);
            }
            else Dialog.Show("Open", "The file extension '{0}' is not supported.".F(fileExtension), DialogImage.Error, Buttons.Ok);
        }

        public async Task OpenAsync()
        {
            if (StorageWindow.Show(out string[] paths, "Open...", StorageWindowModes.OpenFile, ImageFormats.Readable.Select(i => i.Extension), ActiveDocument?.Path, StorageWindowFilterModes.Alphabetical))
            {
                foreach (var i in paths)
                    await OpenAsync(i);
            }
        }

        public async Task OpenAsync(string filePath, params ImageEffect[] effects)
        {
            var fileExtension = System.IO.Path.GetExtension(filePath).Substring(1);

            //If the file is already open, just activate it
            if (Documents.FirstOrDefault(i => i.As<Document>().Path == filePath) is Document existingDocument)
            {
                ActiveContent = existingDocument;
                return;
            }

            if (ImageFormats.IsReadable(fileExtension))
            {
                Document document = null;

                switch (fileExtension)
                {
                    case "image":
                        BinarySerializer.Deserialize(filePath, out document);
                        if (document == null)
                        {
                            Document.AssertInvalid(filePath);
                            return;
                        }
                        break;

                    default:
                        var result = await ImageDocument.OpenAsync(filePath);
                        if (result == null)
                        {
                            Document.AssertInvalid(filePath);
                            return;
                        }

                        document = new ImageDocument(result, filePath, effects);
                        break;
                }
                OnOpened(document);
            }
            else Dialog.Show("Open", "The file extension '{0}' is not supported.".F(fileExtension), DialogImage.Error, Buttons.Ok);
        }

        public async Task OpenAsync(IList<string> filePaths)
        {
            if (filePaths?.Count > 0)
            {
                foreach (var i in filePaths)
                    await OpenAsync(i);
            }
        }

        public IEnumerable<WriteableBitmap> OpenFiles()
        {
            var result = new List<WriteableBitmap>();
            if (StorageWindow.Show(out string[] paths, "Open...", StorageWindowModes.OpenFile, ImageFormats.Readable.Select(i => i.Extension), ActiveDocument?.Path, StorageWindowFilterModes.Alphabetical))
            {
                foreach (var i in paths)
                {
                    var extension = System.IO.Path.GetExtension(i).Substring(1);
                    if (ImageFormats.IsReadable(extension))
                    {
                        var j = ImageDocument.Open(i);
                        result.Add(j.WriteableBitmap());
                    }
                }
            }
            return result;
        }

        #endregion

        #region Commands

        #region <File>

        ICommand newCommand;
        public ICommand NewCommand 
            => newCommand ??= new RelayCommand<Type>(i => Document.New(i).If(j => Documents.Add(j)), i => i != null);

        ICommand newFromTemplateCommand;
        public ICommand NewFromTemplateCommand => newFromTemplateCommand ??= new RelayCommand<string>(i => Document.NewFromTemplate(i).If(j => Documents.Add(j)), i =>
        {
            var result = false;
            Try.Invoke(() => result = File.Long.Exists(i));
            return result;
        });

        ICommand openCommand;
        public ICommand OpenCommand => openCommand ??= new RelayCommand(async () => await OpenAsync());

        ICommand openRecentFileCommand;
        public ICommand OpenRecentFileCommand => openRecentFileCommand ??= new RelayCommand<string>(i => _ = OpenAsync(i), i =>
        {
            var result = false;
            Try.Invoke(() => result = File.Long.Exists(i));
            return result;
        });
        
        ICommand propertiesCommand;
        public ICommand PropertiesCommand => propertiesCommand ??= new RelayCommand(() => Computer.Properties.Show(ActiveDocument.Path), () => ActiveDocument != null && System.IO.File.Exists(ActiveDocument.Path));

        ICommand showInWindowsExplorerCommand;
        public ICommand ShowInWindowsExplorerCommand => showInWindowsExplorerCommand ??= new RelayCommand(() => Computer.ShowInWindowsExplorer(ActiveDocument.Path), () => ActiveDocument != null && File.Long.Exists(ActiveDocument.Path));

        #endregion

        #region <Edit>

        ICommand undoCommand;
        public ICommand UndoCommand 
            => undoCommand ??= new RelayCommand(() => ActiveDocument.History.Undo(), () => ActiveDocument != null && ActiveDocument.History.U.Any());

        ICommand redoCommand;
        public ICommand RedoCommand 
            => redoCommand ??= new RelayCommand(() => ActiveDocument.History.Redo(), () => ActiveDocument != null && ActiveDocument.History.R.Any());

        ICommand repeatCommand;
        public ICommand RepeatCommand 
            => repeatCommand ??= new RelayCommand(() => ActiveDocument.History.Repeat(), () => ActiveDocument != null && ActiveDocument.History.Any());

        ICommand copyCommand;
        public ICommand CopyCommand 
            => copyCommand ??= new RelayCommand(() => ActiveDocument.Copy(),  () => SelectedLayer is VisualLayer);

        ICommand copyMergedCommand;
        public ICommand CopyMergedCommand 
            => copyMergedCommand ??= new RelayCommand(() => ActiveDocument.CopyMerged(), () => ActiveDocument != null);

        ICommand cutCommand;
        public ICommand CutCommand 
            => cutCommand ??= new RelayCommand(() => ActiveDocument.Cut(), () => SelectedLayer is VisualLayer);

        ICommand cutMergedCommand;
        public ICommand CutMergedCommand 
            => cutMergedCommand ??= new RelayCommand(() => ActiveDocument.CutMerged(), () => ActiveDocument != null);

        ICommand pasteCommand;
        public ICommand PasteCommand 
            => pasteCommand ??= new RelayCommand(() => ActiveDocument.Paste(), () => ActiveDocument != null && Clipboard.ContainsImage());

        ICommand pasteNewLayerCommand;
        public ICommand PasteNewLayerCommand
            => pasteNewLayerCommand ??= new RelayCommand(() => ActiveDocument.PasteNewLayer(), () => SelectedLayer is VisualLayer);

        ICommand clearCommand;
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(() =>
        {
            if (ActiveDocument is ImageDocument document)
            {
                if (SelectedLayer is PixelLayer pixelLayer)
                {
                    foreach (var i in document.Selections)
                        pixelLayer.Pixels.FillPolygon(Shape.From(i.Points), Colors.Transparent, null);
                }

                else
                {
                    if (Dialog.Show("Are you sure you want to delete this?", "Clear", DialogImage.Warning, Buttons.YesNo) == 0)
                    {
                        var layers
                            = SelectedLayer.Parent != null
                            ? SelectedLayer.Parent.Layers
                            : Panel.Find<LayersPanel>().Layers;

                        layers.Remove(SelectedLayer);
                    }
                }
            }
        }, () => SelectedLayer is VisualLayer);

        #endregion

        #region <Capture>

        ICommand captureWindowCommand;
        public ICommand CaptureWindowCommand => captureWindowCommand ??= new RelayCommand(() => Capture(Computer.Screen.CaptureForegroundWindow()), () => true);

        ICommand captureForegroundWindowCommand;
        public ICommand CaptureForegroundWindowCommand => captureForegroundWindowCommand ??= new RelayCommand(() =>
        {
            var bitmap = default(System.Drawing.Bitmap);

            var methods = new Action[3];
            methods[0] = new Action(() => View.Hide());
            methods[1] = new Action(() => bitmap = Computer.Screen.CaptureForegroundWindow());
            methods[2] = new Action(() => View.Show());

            for (int i = 0; i < methods.Count(); i++)
                methods[i]();

            Capture(bitmap);

        }, () => true);

        ICommand captureScreenCommand;
        public ICommand CaptureScreenCommand => captureScreenCommand ??= new RelayCommand(() =>
        {
            var bitmap = default(System.Drawing.Bitmap);

            var methods = new Action[3];
            methods[0] = new Action(() => View.Hide());
            methods[1] = new Action(() => bitmap = Computer.Screen.CaptureDesktop());
            methods[2] = new Action(() => View.Show());

            for (int i = 0; i < methods.Count(); i++)
                methods[i]();

            Capture(bitmap);

        }, () => true);

        #endregion

        #region <Image>

        ICommand imageCloneCommand;
        public ICommand ImageCloneCommand => imageCloneCommand ??= new RelayCommand(() => 
        {
            var clone = ActiveDocument.As<Document>().Clone();
            clone.Name += " (Clone)";

            Documents.Insert(Documents.IndexOf(clone), clone);
        }, 
        () => ActiveDocument != null);

        ICommand imageResizeCommand;
        public ICommand ImageResizeCommand => imageResizeCommand ??= new RelayCommand(() =>
        {
            var options = new ImageResizeOptions(ActiveDocument as ImageDocument);
            PropertyWindow.ShowDialog("Resize", options, out int result, i => i.HeaderVisibility = Visibility.Collapsed, Buttons.SaveCancel);
            result.If(i => i == 0, i => ActiveDocument.As<ImageDocument>().Resize(options));
        }, 
        () => ActiveDocument is ImageDocument);

        ICommand imageRotateCommand;
        public ICommand ImageRotateCommand => imageRotateCommand ??= new RelayCommand(() =>
        {
            var options = new ImageRotateOptions();
            PropertyWindow.ShowDialog("Rotate", options, out int result, i => i.HeaderVisibility = Visibility.Collapsed, Buttons.SaveCancel);
            result.If(i => i == 0, i => ActiveDocument.As<ImageDocument>().Rotate(options));

        }, 
        () => ActiveDocument is ImageDocument);

        ICommand imageTrimCommand;
        public ICommand ImageTrimCommand => imageTrimCommand ??= new RelayCommand(() =>
        {
            var options = new ImageTrimOptions();
            PropertyWindow.ShowDialog("Trim", options, out int result, i => i.HeaderVisibility = Visibility.Collapsed, Buttons.SaveCancel);
            result.If(i => i == 0, i => ActiveDocument.As<ImageDocument>().Trim(options));
        }, 
        () => ActiveDocument is ImageDocument);

        #endregion

        #region <Effect>

        ICommand addEffectCommand;
        public ICommand AddEffectCommand => addEffectCommand ??= new RelayCommand<ImageEffect>(i => SelectedLayer.As<StyleLayer>().Style.Effects.Add((ImageEffect)i.Clone()), i => SelectedLayer is StyleLayer);

        #endregion

        #region <Transform>

        ICommand flipHorizontalCommand;
        public ICommand FlipHorizontalCommand => flipHorizontalCommand ??= new RelayCommand(() =>
        {
            if (SelectedLayer is PixelLayer layer)
                layer.Pixels = layer.Pixels.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);
        }, 
        () => SelectedLayer is PixelLayer);

        ICommand flipVerticalCommand;
        public ICommand FlipVerticalCommand => flipVerticalCommand ??= new RelayCommand(() =>
        {
            if (SelectedLayer is PixelLayer layer)
                layer.Pixels = layer.Pixels.Flip(WriteableBitmapExtensions.FlipMode.Vertical);
        }, 
        () => SelectedLayer is PixelLayer);

        ICommand rotateCommand;
        public ICommand RotateCommand => rotateCommand ??= new RelayCommand<object>(i =>
        {
            if (SelectedLayer is PixelLayer layer)
                layer.Pixels = layer.Pixels.RotateFree(i.ToString().Int32());
        }, 
        i => i != null && SelectedLayer is PixelLayer);

        ICommand rotateTransformCommand;
        public ICommand RotateTransformCommand 
            => rotateTransformCommand ??= new RelayCommand(() => { }, () => SelectedLayer is VisualLayer);

        ICommand scaleTransformCommand;
        public ICommand ScaleTransformCommand 
            => scaleTransformCommand ??= new RelayCommand(() => { }, () => SelectedLayer is VisualLayer);

        ICommand skewTransformCommand;
        public ICommand SkewTransformCommand 
            => skewTransformCommand ??= new RelayCommand(() => { }, () => SelectedLayer is VisualLayer);

        ICommand distortTransformCommand;
        public ICommand DistortTransformCommand
            => distortTransformCommand ??= new RelayCommand(() => { }, () => SelectedLayer is VisualLayer);

        ICommand perspectiveTransformCommand;
        public ICommand PerspectiveTransformCommand 
            => perspectiveTransformCommand ??= new RelayCommand(() => { }, () => SelectedLayer is VisualLayer);

        ICommand warpTransformCommand;
        public ICommand WarpTransformCommand 
            => warpTransformCommand ??= new RelayCommand(() => { }, () => SelectedLayer is VisualLayer);

        #endregion

        #region <Select>

        ICommand selectAllCommand;
        public ICommand SelectAllCommand => selectAllCommand ??= new RelayCommand(() =>
        {
            ActiveDocument.Selections.Clear();
            ActiveDocument.Selections.Add(new Shape(new Point(0, 0), new Point(ActiveDocument.Width, 0), new Point(ActiveDocument.Width, ActiveDocument.Height), new Point(0, ActiveDocument.Height)));
        }, () => ActiveDocument != null);

        ICommand invertSelectionCommand;
        public ICommand InvertSelectionCommand => invertSelectionCommand ??= new RelayCommand(() =>
        {
        }, () => ActiveDocument?.Selections.Count > 0);

        #endregion

        #region Other

        ICommand addDocumentPresetCategoryCommand;
        public ICommand AddDocumentPresetCategoryCommand => addDocumentPresetCategoryCommand ??= new RelayCommand<string>(i => Get.Current<Options>().DocumentPresetCategories.Add(i), i => i is string);

        #endregion

        #endregion
    }
}