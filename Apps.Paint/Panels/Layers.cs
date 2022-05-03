using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class LayersPanel : Panel
    {
        enum Category { Add, Move }

        #region Events

        public event EventHandler<EventArgs<Layer>> LayerSelected;

        #endregion

        #region Properties

        public Document ActiveDocument 
            => Get.Current<MainViewModel>().ActiveDocument;

        ListCollectionView effects = null;
        [Category(Category.Add)]
        [Command(nameof(AddEffectLayerCommand))]
        [DisplayName("Add effect layer")]
        [Label(false)]
        [Icon(App.ImagePath + "FxRound.png")]
        [Index(1)]
        [Tool, Visible]
        public ListCollectionView Effects
        {
            get
            {
                if (effects == null)
                {
                    var result = new ObservableCollection<Type>(Get.Current<MainViewModel>().Effects.SourceCollection.Select(i => i.GetType()));

                    effects = new ListCollectionView(result);
                    effects.CustomSort = TypeComparer.Default;
                    effects.GroupDescriptions.Add(new PropertyGroupDescription() { Converter = CategoryConverter.Default });
                }
                return effects;
            }
        }

        LayerType filter = LayerType.All;
        [Option, Visible]
        [Style(EnumStyle.FlagSelect)]
        public LayerType Filter
        {
            get => filter;
            set => this.Change(ref filter, value);
        }

        public override Uri Icon 
            => Resources.ProjectImage("Layers.png");
        
        LayerCollection layers;
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        double previewSize = 48;
        [Featured(AboveBelow.Below)]
        [Format(RangeFormat.Slider)]
        [Label(false)]
        [Range(28.0, 128.0, 4.0)]
        [Tool]
        [Visible]
        public double PreviewSize
        {
            get => previewSize;
            set => this.Change(ref previewSize, value);
        }

        Layer selectedLayer;
        public Layer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                this.Change(ref selectedLayer, value);
                OnLayerSelected(value);
            }
        }
        public override string Title => "Layers";

        public override TopBottom ToolBarPlacement => TopBottom.Bottom;

        #endregion

        #region LayersPanel

        public LayersPanel() : base()
        {
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnActiveDocumentChanged;
            Get.Current<MainViewModel>().DocumentRemoved += OnDocumentRemoved;
        }

        #endregion

        #region Methods

        void OnActiveDocumentChanged(object sender, EventArgs<Document> e)
        {
            Layers = e.Value?.Layers;
        }

        void OnDocumentRemoved(object sender, EventArgs<Document> e)
        {
            Layers = null;
        }

        void OnLayerSelected(Layer value)
        {
            LayerSelected?.Invoke(this, new EventArgs<Layer>(value));
        }

        //...

        public void Group()
        {
            var selectedLayer = SelectedLayer;

            var layers = SelectedLayer.Parent?.Layers ?? Layers;

            var index = layers.IndexOf(selectedLayer);
            layers.RemoveAt(index);

            var group = new GroupLayer("Untitled", selectedLayer.Parent);
            layers.Insert(index, group);

            selectedLayer.Parent = group;
            group.Layers.Add(selectedLayer);
        }

        public void MergeDown()
        {
            var layers = SelectedLayer.Parent?.Layers ?? Layers;

            var index = layers.IndexOf(SelectedLayer);
            if (index + 1 < layers.Count)
            {
                if (layers[index + 1] is VisualLayer)
                {
                    var a = (VisualLayer)layers[index + 1];
                    var b = (VisualLayer)SelectedLayer;

                    var newLayer = a.Merge(b, new System.Drawing.Size(ActiveDocument.Width, ActiveDocument.Height));
                    newLayer.Parent = SelectedLayer.Parent;

                    layers.Insert(index, newLayer);
                    layers.Remove(a);
                    layers.Remove(b);
                }
            }
        }

        public void MergeUp()
        {
            var layers = SelectedLayer.Parent?.Layers ?? Layers;

            var index = layers.IndexOf(SelectedLayer);
            if (index - 1 > -1)
            {
                if (layers[index - 1] is VisualLayer)
                {
                    var a = (VisualLayer)layers[index - 1];
                    var b = (VisualLayer)SelectedLayer;

                    var newLayer = b.Merge(a, new System.Drawing.Size(ActiveDocument.Width, ActiveDocument.Height));
                    newLayer.Parent = SelectedLayer.Parent;

                    layers.Insert(index, newLayer);
                    layers.Remove(a);
                    layers.Remove(b);
                }
            }
        }

        public void Rasterize()
        {
            var result = SelectedLayer.As<RasterizableLayer>().Rasterize();
            Layers.InsertAbove(SelectedLayer, result);
            Layers.Remove(SelectedLayer);
        }

        #endregion

        #region Commands

        ICommand addLayerCommand;
        [Category(Category.Add)]
        [DisplayName("Add layer")]
        [Icon(Images.Plus)]
        [Index(0), Tool, Visible]
        public ICommand AddLayerCommand
            => addLayerCommand ??= new RelayCommand(() => Layers.InsertAbove(SelectedLayer, ActiveDocument.NewLayers()), () => ActiveDocument != null && Layers != null);

        ICommand addEffectLayerCommand;
        public ICommand AddEffectLayerCommand
            => addEffectLayerCommand ??= new RelayCommand<Type>(i => Layers.InsertAbove(SelectedLayer, Array<Layer>.New(new EffectLayer(i.Create<ImageEffect>()))), i => ActiveDocument != null && Layers != null);

        ICommand addLayerFromFileCommand;
        [Category(Category.Add)]
        [DisplayName("Add layer from file")]
        [Icon(Images.Open)]
        [Index(2), Tool, Visible]
        public ICommand AddLayerFromFileCommand => addLayerFromFileCommand ??= new RelayCommand<ImageEffect>(async i =>
        {
            if (StorageWindow.Show(out string[] paths, "Open...", StorageWindowModes.OpenFile, ImageFormats.Readable.Select(i => i.Extension), ActiveDocument?.Path, StorageWindowFilterModes.Alphabetical))
            {
                foreach (var j in paths)
                {
                    var result = await ImageDocument.OpenAsync(j);
                    if (result == null)
                        continue;

                    var pixelLayer = new PixelLayer(System.IO.Path.GetFileNameWithoutExtension(j), result) { IsSelected = true };
                    Layers.InsertAbove(SelectedLayer, pixelLayer);
                }
            }
        },
        i => ActiveDocument != null && Layers != null);

        ICommand clearStyleCommand;
        public ICommand ClearStyleCommand => clearStyleCommand ??= new RelayCommand<StyleLayer>(i => i.Style.Clear(), i => i != null);

        ICommand cloneCommand;
        [DisplayName("Clone")]
        [Icon(Images.Clone), Tool, Visible]
        public ICommand CloneCommand => cloneCommand ??= new RelayCommand(() =>
        {
            var selectedLayer = SelectedLayer;

            var clone = selectedLayer.Clone();
            clone.Parent = selectedLayer.Parent;

            LayerCollection layers
            = selectedLayer.Parent != null
            ? selectedLayer.Parent.Layers
            : Layers;

            var index = layers.IndexOf(selectedLayer);
            layers.Insert(index, clone);
        },
        () => Get.Current<MainViewModel>().ActiveDocument != null && SelectedLayer != null);

        ICommand copyStyleCommand;
        public ICommand CopyStyleCommand 
            => copyStyleCommand ??= new RelayCommand<StyleLayer>(i => Copy.Set(i.Style), i => i != null);

        ICommand deleteCommand;
        [DisplayName("Delete")]
        [Icon(Images.Trash), Tool, Visible]
        public ICommand DeleteCommand 
            => deleteCommand ??= new RelayCommand(() => Layers.Remove(SelectedLayer), () => Layers != null && SelectedLayer != null);

        ICommand editCommand;
        public ICommand EditCommand
            => editCommand ??= new RelayCommand<StyleLayer>(i => PropertyWindow.ShowDialog("Edit layer", i), i => i != null);

        ICommand editStyleCommand;
        public ICommand EditStyleCommand
            => editStyleCommand ??= new RelayCommand<StyleLayer>(i => PropertyWindow.ShowDialog("Edit layer style", i.Style), i => i != null);

        ICommand flattenCommand;
        public ICommand FlattenCommand
            => flattenCommand ??= new RelayCommand(() => ActiveDocument.As<ImageDocument>().Flatten(), () => ActiveDocument is ImageDocument);

        ICommand groupCommand;
        [DisplayName("Group")]
        [Icon(Images.Folder), Tool, Visible]
        public ICommand GroupCommand 
            => groupCommand ??= new RelayCommand(() => Group(), () => SelectedLayer != null);

        ICommand insertAboveCommand;
        public ICommand InsertAboveCommand
            => insertAboveCommand ??= new RelayCommand(() => Layers.InsertAbove(SelectedLayer, ActiveDocument.NewLayers()), () => Layers != null);

        ICommand insertBelowCommand;
        public ICommand InsertBelowCommand
            => insertBelowCommand ??= new RelayCommand(() => Layers.InsertBelow(SelectedLayer, ActiveDocument.NewLayers()), () => Layers != null);

        ICommand mergeDownCommand;
        public ICommand MergeDownCommand 
            => mergeDownCommand ??= new RelayCommand(MergeDown, () => SelectedLayer is VisualLayer);

        ICommand mergeUpCommand;
        public ICommand MergeUpCommand 
            => mergeUpCommand ??= new RelayCommand(MergeUp, () => SelectedLayer is VisualLayer);

        ICommand mergeVisibleCommand;
        public ICommand MergeVisibleCommand 
            => mergeVisibleCommand ??= new RelayCommand(() => ActiveDocument.As<ImageDocument>().MergeVisible(), () => ActiveDocument != null);

        ICommand moveDownCommand;
        [Category(Category.Move)]
        [DisplayName("MoveDown")]
        [Icon(Images.ArrowDownRound)]
        [Index(1), Tool, Visible]
        public ICommand MoveDownCommand => moveDownCommand ??= new RelayCommand(() =>
        {
            var selectedLayer = SelectedLayer;

            LayerCollection layers
            = selectedLayer.Parent != null
            ? selectedLayer.Parent.Layers
            : Layers;

            var index = layers.IndexOf(selectedLayer);
            layers.RemoveAt(index);
            layers.Insert((index + 1).Maximum(layers.Count), selectedLayer);
        },
        () => SelectedLayer != null);

        ICommand moveInsideCommand;
        [Category(Category.Move)]
        [DisplayName("MoveInside")]
        [Icon(Images.ArrowS)]
        [Index(2), Tool, Visible]
        public ICommand MoveInsideCommand => moveInsideCommand ??= new RelayCommand(() =>
        {
            var removeLayer = SelectedLayer;

            var layers = removeLayer.Parent.Layers;
            layers.Remove(removeLayer);

            var newParent = removeLayer.Parent.Parent != null
                ? removeLayer.Parent.Parent
                : null;

            layers
                = newParent != null
                ? newParent.Layers
                : Layers;

            var index = layers.IndexOf(removeLayer.Parent);

            removeLayer.Parent = newParent;
            layers.Insert(index, removeLayer);
        },
        () => SelectedLayer?.Parent != null);

        ICommand moveOutsideCommand;
        [Category(Category.Move)]
        [DisplayName("MoveOutside")]
        [Icon(Images.ArrowN)]
        [Index(2), Tool, Visible]
        public ICommand MoveOutsideCommand => moveOutsideCommand ??= new RelayCommand(() =>
        {
            var removeLayer = SelectedLayer;

            var layers = removeLayer.Parent.Layers;
            layers.Remove(removeLayer);

            var newParent = removeLayer.Parent.Parent != null
                ? removeLayer.Parent.Parent
                : null;

            layers
                = newParent != null
                ? newParent.Layers
                : Layers;

            var index = layers.IndexOf(removeLayer.Parent);

            removeLayer.Parent = newParent;
            layers.Insert(index, removeLayer);
        },
        () => SelectedLayer?.Parent != null);

        ICommand moveUpCommand;
        [Category(Category.Move)]
        [DisplayName("MoveUp")]
        [Icon(Images.ArrowUpRound)]
        [Index(0), Tool, Visible]
        public ICommand MoveUpCommand => moveUpCommand ??= new RelayCommand(() =>
        {
            var selectedLayer = SelectedLayer;

            LayerCollection layers
            = selectedLayer.Parent != null
            ? selectedLayer.Parent.Layers
            : Layers;

            var index = layers.IndexOf(selectedLayer);
            layers.RemoveAt(index);
            layers.Insert((index - 1).Minimum(0), selectedLayer);
        },
        () => SelectedLayer != null);

        ICommand pasteStyleCommand;
        public ICommand PasteStyleCommand 
            => pasteStyleCommand ??= new RelayCommand<StyleLayer>(i => i.Style.Paste(Copy.Get<LayerStyle>()), i => i != null && Copy.Get<LayerStyle>() != null);

        ICommand rasterizeCommand;
        public ICommand RasterizeCommand 
            => rasterizeCommand ??= new RelayCommand(Rasterize, () => SelectedLayer is RasterizableLayer);

        ICommand rasterizeStyleCommand;
        public ICommand RasterizeStyleCommand 
            => rasterizeStyleCommand ??= new RelayCommand(() => SelectedLayer.As<PixelLayer>().RasterizeStyle(), () => SelectedLayer is PixelLayer);

        #endregion
    }
}