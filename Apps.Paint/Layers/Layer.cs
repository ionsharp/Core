using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Numbers;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Imagin.Common.Data;

namespace Imagin.Apps.Paint
{
    #region (abstract) Layer

    [Serializable]
    public abstract class Layer : BaseNamable, ICloneable, ILock
    {
        #region Events

        [field: NonSerialized]
        public event LockedEventHandler Locked;

        #endregion

        #region Fields

        public const string DefaultName = "Untitled";

        public const double IndexThicknessOffset = 16;

        #endregion

        #region Properties

        [Hidden]
        public Document Document { get; set; }

        uint index = 0;
        [Hidden]
        public uint Index
        {
            get => index;
            set
            {
                this.Change(ref index, value);
                this.Changed(() => IndexThickness);
            }
        }

        [Hidden]
        public Thickness IndexThickness => new(index * IndexThicknessOffset, 0, 0, 0);

        protected bool IsParentVisible
        {
            get
            {
                var parent = Parent;
                while (parent != null)
                {
                    if (!parent.IsVisible)
                        return false;

                    parent = parent.Parent;
                }
                return true;
            }
        }

        protected bool IsParentLocked
        {
            get
            {
                var parent = Parent;
                while (parent != null)
                {
                    if (parent.IsLocked)
                        return true;

                    parent = parent.Parent;
                }
                return false;
            }
        }

        bool isLocked = false;
        [Hidden]
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                if (Parent == null || !IsParentLocked)
                {
                    this.Change(ref isLocked, value);
                    if (this is GroupLayer)
                    {
                        ((GroupLayer)this).Each<Layer>(i =>
                        {
                            i.isLocked = value;
                            i.Changed(() => i.IsLocked);
                        });
                    }
                    Locked?.Invoke(this, new(value));
                }
            }
        }

        bool isSelected = false;
        [Hidden]
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        bool isVisible = true;
        [Hidden]
        public virtual bool IsVisible
        {
            get => isVisible;
            set 
            {
                if (Parent == null || (IsParentVisible && !IsParentLocked))
                {
                    this.Change(ref isVisible, value);
                    if (this is GroupLayer)
                    {
                        ((GroupLayer)this).Each<Layer>(i =>
                        {
                            i.isVisible = value;
                            i.Changed(() => i.IsVisible);
                        });
                    }
                }
            }
        }

        [Featured]
        [Index(0)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        GroupLayer parent = null;
        [Hidden]
        public GroupLayer Parent
        {
            get => parent;
            set
            {
                this.Change(ref parent, value);

                Index = GetIndex();
                Each<Layer>(i => i.Index = i.GetIndex());
            }
        }

        LayerType type;
        [Hidden]
        public virtual LayerType Type
        {
            get => type;
            private set => this.Change(ref type, value);
        }

        //...

        #endregion

        #region Layer

        public Layer(LayerType type) : this(type, string.Empty) { }

        public Layer(LayerType type, string name) : base(name)
        {
            Type = type;
        }

        #endregion

        #region Methods

        object ICloneable.Clone() => Clone();
        public abstract Layer Clone();

        void Each<T>(Action<T> action, LayerCollection layers = null) where T : Layer
        {
            if (layers == null)
            {
                if (this is GroupLayer)
                {
                    layers = ((GroupLayer)this).Layers;
                }
                else return;
            }

            foreach (var i in layers)
            {
                if (i is T)
                    action((T)i);

                if (i is GroupLayer)
                    Each(action, ((GroupLayer)i).Layers);
            }
        }

        uint GetIndex()
        {
            uint result = 0;

            var layer = this;
            while (layer.Parent != null)
            {
                result++;
                layer = layer.Parent;
            }

            return result;
        }

        #endregion
    }

    #endregion

    #region (abstract) StackableLayer : Layer

    [Serializable]
    public abstract class StackableLayer : Layer
    {
        public StackableLayer(LayerType type, string name) : base(type, name) { }

        public StackableLayer Merge(StackableLayer layer, System.Drawing.Size size)
        {
            var a = Clone() as StackableLayer; var b = layer.Clone() as StackableLayer;
            StackableLayer Blend(StackableLayer a0, StackableLayer b0)
            {
                if (a0 is PixelLayer a1 && b0 is PixelLayer b1)
                {
                    a1.RasterizeStyle(); b1.RasterizeStyle();
                    a1.Pixels.Blend(b1.Pixels);
                }
                return a0;
            }

            //...

            //(*) EffectLayer -> EffectLayer
            if (a is EffectLayer && b is EffectLayer)
                return null;

            //(*) EffectLayer -> GroupLayer
            if (a is EffectLayer && b is GroupLayer)
                return null;

            //(*) EffectLayer -> PixelLayer
            if (a is EffectLayer && b is PixelLayer)
                return null;

            //(*) EffectLayer -> ShapeLayer
            if (a is EffectLayer && b is ShapeLayer)
                return null;

            //(*) EffectLayer -> TextLayer
            if (a is EffectLayer && b is TextLayer)
                return null;

            //(*) GroupLayer -> EffectLayer
            if (a is GroupLayer && b is EffectLayer)
            {
                //a = ?
                a.As<PixelLayer>().RasterizeStyle();
                a.As<PixelLayer>().Pixels.ForEach((x, y, color) => b.As<EffectLayer>().Effect.Apply(color));
                return a;
            }

            //(*) GroupLayer -> GroupLayer
            if (a is GroupLayer && b is GroupLayer)
            {
                //a = ?
                //b = ?
                return Blend(a, b);

            }

            //(*) GroupLayer -> PixelLayer
            if (a is GroupLayer && b is PixelLayer)
            {
                //a = ?
                return Blend(a, b);

            }

            //(*) GroupLayer -> ShapeLayer
            if (a is GroupLayer && b is ShapeLayer)
            {
                b = b.As<ShapeLayer>().Rasterize();
                //a = ?
                return Blend(a, b);

            }

            //(*) GroupLayer -> TextLayer
            if (a is GroupLayer && b is TextLayer)
            {
                b = b.As<TextLayer>().Rasterize();
                //a = ?
                return Blend(a, b);

            }

            //PixelLayer -> EffectLayer
            if (a is PixelLayer && b is EffectLayer)
            {
                a = a.Clone() as StackableLayer;
                a.As<PixelLayer>().RasterizeStyle();
                a.As<PixelLayer>().Pixels.ForEach((x, y, color) => b.As<EffectLayer>().Effect.Apply(color));
                return a;
            }

            //(*) PixelLayer -> GroupLayer
            if (a is PixelLayer && b is GroupLayer)
            {
                //b = ?
                return Blend(a, b);
            }

            //PixelLayer -> PixelLayer
            if (a is PixelLayer && b is PixelLayer)
                return Blend(a, b);

            //PixelLayer -> ShapeLayer
            if (a is PixelLayer && b is ShapeLayer)
            {
                b = b.As<ShapeLayer>().Rasterize();
                return Blend(a, b);
            }

            //PixelLayer -> TextLayer
            if (a is PixelLayer && b is TextLayer)
            {
                b = b.As<TextLayer>().Rasterize();
                return Blend(a, b);
            }

            //ShapeLayer -> EffectLayer
            if (a is ShapeLayer && b is EffectLayer)
            {
                a = a.As<ShapeLayer>().Rasterize();
                a.As<PixelLayer>().RasterizeStyle();
                a.As<PixelLayer>().Pixels.ForEach((x, y, color) => b.As<EffectLayer>().Effect.Apply(color));
                return a;
            }

            //(*) ShapeLayer -> GroupLayer
            if (a is ShapeLayer && b is GroupLayer)
            {
                a = a.As<ShapeLayer>().Rasterize();
                //b = ?
                return Blend(a, b);
            }

            //ShapeLayer -> PixelLayer
            if (a is ShapeLayer && b is PixelLayer)
            {
                a = a.As<ShapeLayer>().Rasterize();
                return Blend(a, b);
            }

            //ShapeLayer -> ShapeLayer
            if (a is ShapeLayer && b is ShapeLayer)
            {
                b.As<ShapeLayer>().Points.ForEach(i => a.As<ShapeLayer>().Points.Add(i));
                return a;
            }

            //ShapeLayer -> TextLayer
            if (a is ShapeLayer && b is TextLayer)
            {
                a = a.As<ShapeLayer>().Rasterize();
                b = b.As<TextLayer>().Rasterize();
                return Blend(a, b);
            }

            //TextLayer -> EffectLayer
            if (a is TextLayer && b is EffectLayer)
            {
                a = a.As<TextLayer>().Rasterize();
                a.As<PixelLayer>().RasterizeStyle();
                a.As<PixelLayer>().Pixels.ForEach((x, y, color) => b.As<EffectLayer>().Effect.Apply(color));
                return a;
            }

            //(*) TextLayer -> GroupLayer
            if (a is TextLayer && b is GroupLayer)
            {
                a = a.As<TextLayer>().Rasterize();
                //b = ?
                return Blend(a, b);
            }

            //TextLayer -> PixelLayer
            if (a is TextLayer && b is PixelLayer)
            {
                a = a.As<TextLayer>().Rasterize();
                return Blend(a, b);
            }

            //TextLayer -> ShapeLayer
            if (a is TextLayer && b is ShapeLayer)
            {
                a = a.As<TextLayer>().Rasterize();
                b = b.As<ShapeLayer>().Rasterize();
                return Blend(a, b);
            }

            //TextLayer -> TextLayer
            if (a is TextLayer && b is TextLayer)
            {
                a = a.As<TextLayer>().Rasterize();
                b = b.As<TextLayer>().Rasterize();
                return Blend(a, b);
            }

            return null;
        }
    }

    #endregion

    #region (abstract) StyleLayer : StackableLayer

    [Serializable]
    public abstract class StyleLayer : StackableLayer
    {
        enum Category { Effects }

        [Hidden]
        public BlendModes BlendMode
        {
            get => Style.BlendMode;
            set
            {
                Style.BlendMode = value;
                this.Changed(() => BlendMode);
            }
        }

        LayerStyle style;
        [DisplayName("Edit style")]
        [Icon(App.ImagePath + "EditLayerStyle.png")]
        [Index(0), Label(false)]
        [Tool]
        public LayerStyle Style
        {
            get => style;
            private set => this.Change(ref style, value);
        }

        public StyleLayer(LayerType type, string name) : base(type, name)
        {
            Style = new LayerStyle();
        }

        ICommand clearEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Clear effects")]
        [Icon(App.ImagePath + "ClearEffects.png")]
        [Index(2), Tool]
        public ICommand ClearEffectsCommand
            => clearEffectsCommand ??= new RelayCommand(() => Style.Effects.Clear(), () => Style.Effects.Count > 0);

        ICommand clearStyleCommand;
        [DisplayName("Clear style")]
        [Icon(App.ImagePath + "ClearStyle.png")]
        [Index(3), Tool]
        public ICommand ClearStyleCommand
            => clearStyleCommand ??= new RelayCommand(() => Style.Clear());

        ICommand copyEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Copy effects")]
        [Icon(App.ImagePath + "CopyEffects.png")]
        [Index(0), Tool]
        public ICommand CopyEffectsCommand
            => copyEffectsCommand ??= new RelayCommand(() => Copy.Set(Style.Effects));

        ICommand copyStyleCommand;
        [DisplayName("Copy style")]
        [Icon(App.ImagePath + "CopyStyle.png")]
        [Index(1), Tool]
        public ICommand CopyStyleCommand
            => copyStyleCommand ??= new RelayCommand(() => Copy.Set<LayerStyle>(Style));

        ICommand editEffectCommand;
        [Hidden]
        public ICommand EditEffectCommand
            => editEffectCommand ??= new RelayCommand<ImageEffect>(i => PropertyWindow.ShowDialog("Edit effect", i));

        ICommand deleteEffectCommand;
        [Hidden]
        public ICommand DeleteEffectCommand
            => deleteEffectCommand ??= new RelayCommand<ImageEffect>(i => Style.Effects.Remove(i));

        ICommand pasteEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Paste effects")]
        [Icon(App.ImagePath + "PasteEffects.png")]
        [Index(1), Tool]
        public ICommand PasteEffectsCommand
            => pasteEffectsCommand ??= new RelayCommand(() => Copy.Get<EffectCollection>().ForEach(i => Style.Effects.Add(i.Copy())), () => Copy.Get<EffectCollection>() != null);

        ICommand pasteStyleCommand;
        [DisplayName("Paste style")]
        [Icon(App.ImagePath + "PasteStyle.png")]
        [Index(2), Tool]
        public ICommand PasteStyleCommand
            => pasteStyleCommand ??= new RelayCommand(() => Style = Copy.Get<LayerStyle>().Clone(), () => Copy.Get<LayerStyle>() != null);
    }

    #endregion

    #region (abstract) VisualLayer : StyleLayer

    [Serializable]
    public abstract class VisualLayer : StyleLayer
    {
        enum Category { Position, Rasterize }

        #region Properties

        [Hidden]
        public abstract System.Windows.Media.PointCollection Bounds { get; }

        [Hidden]
        public bool IsRasterizable => this is RasterizableLayer;

        [NonSerialized]
        WriteableBitmap pixels;
        [Hidden]
        public virtual WriteableBitmap Pixels
        {
            get => pixels;
            set => this.Change(ref pixels, value);
        }

        [Hidden]
        public BinaryValue<WriteableBitmap, Matrix<Common.Media.Argb>, WriteableBitmapToArgbMatrixConverter> Pixells;

        [Hidden]
        public Int32Size Size => new(Pixels.PixelHeight, Pixels.PixelWidth);

        protected int x = 0;
        [Category(Category.Position)]
        [ReadOnly]
        public virtual int X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        protected int y = 0;
        [Category(Category.Position)]
        [ReadOnly]
        public virtual int Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        #endregion

        #region VisualLayer

        public VisualLayer(LayerType type, string name) : base(type, name) { }

        #endregion

        #region Methods

        public virtual void Crop(int x, int y, int height, int width)
        {
            var dx = x - X;
            var dy = y - Y;
            X -= x;
            Y -= y;
        }

        public virtual void Crop(Int32Size oldSize, Int32Size newSize, CardinalDirection direction)
        {
            /*
            if (height < Height)
            {
                //We don't have to worry about resizing layers vertically
            }
            else if (height > Height)
            {
                //Resize each layer vertically...
                foreach (var i in Layers)
                {
                    if (i is VisualLayer)
                    {
                        //...if the height of the layer is less than the new canvas height.
                        if ((i as VisualLayer).Height < height)
                        {
                            //Set i.Height to height
                        }
                    }
                }
            }

            if (width < Width)
            {
                //We don't have to worry about resizing layers horizontally
            }
            else if (width > Width)
            {
                //Resize all layers horizontally...
                foreach (var i in Layers)
                {
                    if (i is VisualLayer)
                    {
                        //...if the width of the layer is less than the new canvas width.
                        if ((i as VisualLayer).Width < width)
                        {
                            //Set i.Width to width
                        }
                    }
                }
            }

            //Now we have to apply the anchor
            foreach (var i in Layers)
            {
                switch (direction)
                {
                    case CardinalDirection.E:
                        break;
                    case CardinalDirection.N:
                        break;
                    case CardinalDirection.NE:
                        break;
                    case CardinalDirection.NW:
                        break;
                    case CardinalDirection.Origin:
                        break;
                    case CardinalDirection.S:
                        break;
                    case CardinalDirection.SE:
                        break;
                    case CardinalDirection.SW:
                        break;
                    case CardinalDirection.W:
                        break;
                }
            }
            */
        }

        ICommand rasterizeCommand;
        [Category(Category.Rasterize)]
        [DisplayName("Rasterize")]
        [Icon(App.ImagePath + "RasterizeLayer.png")]
        [Tool]
        public ICommand RasterizeCommand => rasterizeCommand ??= new RelayCommand(() =>
        {
            var result = this.As<RasterizableLayer>().Rasterize();
            Panel.Find<LayersPanel>().Layers.InsertAbove(this, result);
            Panel.Find<LayersPanel>().DeleteCommand.Execute(this);
        },
        () => this is RasterizableLayer);

        ICommand rasterizeStyleCommand;
        [Category(Category.Rasterize)]
        [DisplayName("Rasterize style")]
        [Icon(App.ImagePath + "RasterizeLayerStyle.png")]
        [Tool]
        public ICommand RasterizeStyleCommand
            => rasterizeStyleCommand ??= new RelayCommand(() => this.As<PixelLayer>().RasterizeStyle(), () => this is PixelLayer);

        #endregion
    }

    #endregion

    #region (abstract) RasterizableLayer : VisualLayer

    [Serializable]
    public abstract class RasterizableLayer : VisualLayer
    {
        object renderLock = new();

        protected readonly Handle handleRender = false;

        //...

        protected RasterizableLayer(LayerType type) : this(type, string.Empty) { }

        protected RasterizableLayer(LayerType type, string name) : base(type, name) { }

        //...

        public PixelLayer Rasterize()
        {
            var result = new PixelLayer(Name, new System.Drawing.Size(Size.Width, Size.Height)) { Parent = Parent };
            result.Style.Paste(Style);
            result.Pixels.Blend(BlendModes.Normal, Pixels, 0, 0, 1, true);
            return result;
        }

        public void Render()
        {
            handleRender.SafeInvoke(() =>
            {
                lock (renderLock)
                {
                    if (Pixels != null)
                    {
                        Pixels.Clear();
                        Render(Pixels);
                    }
                }
            });
        }

        public abstract void Render(WriteableBitmap input);
    }

    #endregion
}