using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Colors;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class Options : DockViewOptions<MainViewModel>, IFavorite
    {
        enum Category { Canvas, Color, Colors, Display, Explorer }

        #region Properties

        protected override WindowState DefaultWindowState
            => WindowState.Maximized;

        #region Canvas

        string canvasBackground = $"255,{System.Windows.Media.Colors.DarkGray.R},{System.Windows.Media.Colors.DarkGray.G},{System.Windows.Media.Colors.DarkGray.B}";
        [Category(Category.Canvas)]
        [DisplayName("Background")]
        public Color CanvasBackground
        {
            get
            {
                var result = canvasBackground.Split(',');
                return System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref canvasBackground, $"{value.A},{value.R},{value.G},{value.B}");
        }

        string canvasGridLines = $"255,{System.Windows.Media.Colors.DarkGray.R},{System.Windows.Media.Colors.DarkGray.G},{System.Windows.Media.Colors.DarkGray.B}";
        [Category(Category.Canvas)]
        [DisplayName("Grid lines")]
        public Color CanvasGridLines
        {
            get
            {
                var result = canvasGridLines.Split(',');
                return System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref canvasGridLines, $"{value.A},{value.R},{value.G},{value.B}");
        }

        bool viewGridLines = true;
        [Hidden]
        public bool ViewGridLines
        {
            get => viewGridLines;
            set => this.Change(ref viewGridLines, value);
        }

        bool viewRulers = false;
        [Hidden]
        public bool ViewRulers
        {
            get => viewRulers;
            set => this.Change(ref viewRulers, value);
        }

        #endregion

        #region Capture

        bool captureClipboard = true;
        [Hidden]
        public bool CaptureClipboard
        {
            get => captureClipboard;
            set => this.Change(ref captureClipboard, value);
        }

        bool captureFile = false;
        [Hidden]
        public bool CaptureFile
        {
            get => captureFile;
            set => this.Change(ref captureFile, value);
        }

        bool captureLayer = false;
        [Hidden]
        public bool CaptureLayer
        {
            get => captureLayer;
            set => this.Change(ref captureLayer, value);
        }

        #endregion

        #region Color

        Components colorComponent = Components.A;
        [Category(Category.Color)]
        [DisplayName("Component")]
        public Components ColorComponent
        {
            get => colorComponent;
            set => this.Change(ref colorComponent, value);
        }

        ColorModels colorModel = ColorModels.HCV;
        [Category(Category.Color)]
        [DisplayName("Model")]
        public ColorModels ColorModel
        {
            get => colorModel;
            set => this.Change(ref colorModel, value);
        }

        #endregion

        #region Colors

        StringColor backgroundColor = System.Windows.Media.Colors.White;
        [Category(Category.Colors)]
        [DisplayName("Background")]
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => this.Change(ref backgroundColor, value);
        }

        StringColor foregroundColor = System.Windows.Media.Colors.Black;
        [Category(Category.Colors)]
        [DisplayName("Foreground")]
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => this.Change(ref foregroundColor, value);
        }

        #endregion

        #region Display

        GraphicUnit graphicalUnit = GraphicUnit.Pixel;
        [Category(Category.Display)]
        [DisplayName("Graphic unit")]
        public GraphicUnit GraphicUnit
        {
            get => graphicalUnit;
            set => this.Change(ref graphicalUnit, value);
        }

        #endregion

        #region Documents

        StringCollection documentPresetCategories = new();
        [Hidden]
        public StringCollection DocumentPresetCategories
        {
            get => documentPresetCategories;
            set => this.Change(ref documentPresetCategories, value);
        }

        ObservableCollection<DocumentPreset> documentPresets = new(DocumentPreset.Default());
        [Hidden]
        public ObservableCollection<DocumentPreset> DocumentPresets
        {
            get => documentPresets ?? (documentPresets = new ObservableCollection<DocumentPreset>());
            set => this.Change(ref documentPresets, value);
        }

        DocumentPreset documentPreset = null;
        [Hidden]
        public DocumentPreset DocumentPreset
        {
            get => documentPreset;
            set => this.Change(ref documentPreset, value);
        }

        #endregion

        #region Explorer

        [Hidden]
        public Favorites Favorites => explorerOptions.Favorites;

        ExplorerOptions explorerOptions = new();
        [Category(Category.Explorer)]
        [DisplayName("Options")]
        public ExplorerOptions ExplorerOptions
        {
            get => explorerOptions;
            set => this.Change(ref explorerOptions, value);
        }

        #endregion

        #region Groups

        [field: NonSerialized]
        GroupWriter<Brush> brushes;
        [Hidden]
        public GroupWriter<Brush> Brushes
        {
            get => brushes;
            set => this.Change(ref brushes, value);
        }
        
        [field: NonSerialized]
        GroupWriter<StringColor> colors;
        [Hidden]
        public GroupWriter<StringColor> Colors
        {
            get => colors;
            set => this.Change(ref colors, value);
        }

        [field: NonSerialized]
        GroupWriter<Curve> curves;
        [Hidden]
        public GroupWriter<Curve> Curves
        {
            get => curves;
            set => this.Change(ref curves, value);
        }

        [field: NonSerialized]
        GroupWriter<Gradient> gradients;
        [Hidden]
        public GroupWriter<Gradient> Gradients
        {
            get => gradients;
            set => this.Change(ref gradients, value);
        }

        [field: NonSerialized]
        GroupWriter<Matrix> matrices;
        [Hidden]
        public GroupWriter<Matrix> Matrices
        {
            get => matrices;
            set => this.Change(ref matrices, value);
        }

        [field: NonSerialized]
        GroupWriter<Path> paths;
        [Hidden]
        public GroupWriter<Path> Paths
        {
            get => paths;
            set => this.Change(ref paths, value);
        }

        [field: NonSerialized]
        GroupWriter<Selection> selections;
        [Hidden]
        public GroupWriter<Selection> Selections
        {
            get => selections;
            set => this.Change(ref selections, value);
        }

        [field: NonSerialized]
        GroupWriter<Shape> shapes;
        [Hidden]
        public GroupWriter<Shape> Shapes
        {
            get => shapes;
            set => this.Change(ref shapes, value);
        }

        [field: NonSerialized]
        GroupWriter<LayerStyle> styles;
        [Hidden]
        public GroupWriter<LayerStyle> Styles
        {
            get => styles;
            set => this.Change(ref styles, value);
        }

        #endregion

        #region Tools

        #endregion

        #region Other

        ObservableCollection<string> recentFiles = new();
        [Hidden]
        public ObservableCollection<string> RecentFiles
        {
            get => recentFiles;
            set => this.Change(ref recentFiles, value);
        }

        [field: NonSerialized]
        PathCollection templates;
        [Hidden]
        public PathCollection Templates
        {
            get => templates;
            set => this.Change(ref templates, value);
        }

        ObservableCollection<ToolPreset> toolPresets = new();
        [Hidden]
        public ObservableCollection<ToolPreset> ToolPresets
        {
            get => toolPresets;
            set => this.Change(ref toolPresets, value);
        }

        [field: NonSerialized]
        ToolCollection tools = new();
        [Hidden]
        public ToolCollection Tools
        {
            get => tools;
            set => this.Change(ref tools, value);
        }

        #endregion

        #endregion

        #region Methods

        protected override IEnumerable<IWriter> GetData()
        {
            yield return 
                ExplorerOptions.Favorites;
            yield return 
                Brushes;
            yield return 
                Colors;
            yield return
                Curves;
            yield return
                Gradients;
            yield return
                Matrices;
            yield return
                Paths;
            yield return
                Selections;
            yield return
                Shapes;
            yield return
                Styles;
        }

        protected override void OnLoaded()
        {
            ExplorerOptions.Favorites
                = new Favorites($@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(Explorer)}", new Limit(250, Limit.Actions.RemoveFirst));
            
            Brushes
                = new GroupWriter<Brush>(
                    Get.Current<App>().Properties.FolderPath, "Brushes",
                    "brushes", "brushes",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Colors
                = new GroupWriter<StringColor>(
                    Get.Current<App>().Properties.FolderPath, "Colors",
                    "colors", "colors",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Curves
                = new GroupWriter<Curve>(
                    Get.Current<App>().Properties.FolderPath, "Curves",
                    "curves", "curves",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Gradients
                = new GroupWriter<Gradient>(
                    Get.Current<App>().Properties.FolderPath, "Gradients",
                    "gradients", "gradients",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Matrices
                = new GroupWriter<Matrix>(
                    Get.Current<App>().Properties.FolderPath, "Matrices",
                    "matrices", "matrices",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Paths
                = new GroupWriter<Path>(
                    Get.Current<App>().Properties.FolderPath, "Paths",
                    "paths", "path",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Selections
                = new GroupWriter<Selection>(
                    Get.Current<App>().Properties.FolderPath, "Selections",
                    "selections", "selection",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Shapes
                = new GroupWriter<Shape>(
                    Get.Current<App>().Properties.FolderPath, "Shapes",
                    "shapes", "shapes",
                    new Limit(250, Limit.Actions.RemoveFirst));
            Styles
                = new GroupWriter<LayerStyle>(
                    Get.Current<App>().Properties.FolderPath, "Styles",
                    "styles", "styles",
                    new Limit(250, Limit.Actions.RemoveFirst));

            Templates
                = new PathCollection(Get.Current<App>().Properties.GetFolderPath("Templates"), new Filter(ItemType.File, "template"));
            Templates.Refresh();
            base.OnLoaded();
        }

        public override IEnumerable<Uri> GetDefaultLayouts()
        {
            yield return Resources.ProjectUri(Layouts.DefaultFolderPath + "Layout1.xml");
            yield return Resources.ProjectUri(Layouts.DefaultFolderPath + "Layout2.xml");
            yield return Resources.ProjectUri(Layouts.DefaultFolderPath + "Layout3.xml");
            yield return Resources.ProjectUri(Layouts.DefaultFolderPath + "Layout4.xml");
        }

        #endregion
    }
}