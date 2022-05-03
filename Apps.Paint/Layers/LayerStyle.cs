using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Layer style")]
    [Icon(App.ImagePath + "LayerStyle.png")]
    [Serializable]
    public class LayerStyle : BaseNamable, ICloneable
    {
        enum Category { Alpha, Blend, Channels, Effects, Mask, Save, Style }

        #region Methods

        #endregion

        #region Properties

        [Hidden]
        public bool Clip => mask == LayerMasks.Clip;

        [Hidden]
        public bool HasMask => mask != LayerMasks.None;

        [Hidden]
        public bool KnockOut => mask == LayerMasks.DeepPunch || mask == LayerMasks.ShallowPunch;

        //...

        BlendModes blendMode = BlendModes.Normal;
        [Featured]
        public BlendModes BlendMode
        {
            get => blendMode;
            set => this.Change(ref blendMode, value);
        }

        RedGreenBlue channels = RedGreenBlue.All;
        [Category(Category.Channels)]
        [Style(EnumStyle.FlagCheck)]
        public RedGreenBlue Channels
        {
            get => channels;
            set => this.Change(ref channels, value);
        }

        EffectCollection effects = new();
        [Category(Category.Effects)]
        [ItemStyle(ObjectStyle.Button)]
        [ItemType(typeof(ImageEffect))]
        [Style(CollectionStyle.Default)]
        public EffectCollection Effects
        {
            get => effects;
            set => this.Change(ref effects, value);
        }

        double fill = 1;
        [Category(Category.Alpha)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Fill
        {
            get => fill;
            set => this.Change(ref fill, value);
        }

        LayerMasks mask = LayerMasks.None;
        [Category(Category.Mask)]
        public LayerMasks Mask
        {
            get => mask;
            set
            {
                this.Change(ref mask, value);
                this.Changed(() => Clip);
                this.Changed(() => KnockOut);
                this.Changed(() => HasMask);
            }
        }

        [Hidden]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        double opacity = 1;
        [Category(Category.Alpha)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }

        //...

        #endregion

        #region LayerStyle

        public LayerStyle() : base() { }

        public LayerStyle(string name, params ImageEffect[] input) : base(name)
        {
            foreach (var i in input)
                effects.Add(i);
        }

        #endregion

        #region Methods

        public void Clear()
        {
            BlendMode
                = BlendModes.Normal;
            Channels
                = RedGreenBlue.All;
            Effects
                .Clear();
            Fill
                = 1;
            Opacity
                = 1;
            Mask
                = LayerMasks.None;
        }

        public void Paste(LayerStyle copy)
        {
            BlendMode
               = copy.BlendMode;
            Channels
                = copy.Channels;
            Effects
                = copy.Effects.Clone();
            Fill
                = copy.Fill;
            Opacity
                = copy.Opacity;
            Mask
                = copy.Mask;
        }

        object ICloneable.Clone() => Clone();
        public LayerStyle Clone()
        {
            var result = new LayerStyle()
            {
                BlendMode
                    = BlendMode,
                Channels
                    = Channels,
                Fill
                    = Fill,
                Mask
                    = Mask,
                Name
                    = Name,
                Opacity
                    = Opacity
            };

            foreach (var i in effects)
                result.Effects.Add(i.Copy());

            return result;
        }

        public Color Apply(Color color)
        {
            var result = color;
            effects.ForEach(i => result = i.Apply(result));
            return result;
        }

        public void Apply(WriteableBitmap OldBitmap, WriteableBitmap NewBitmap)
        {
            OldBitmap.ForEach((x, y, oldColor) =>
            {
                var newColor = Apply(oldColor);

                if (OldBitmap != NewBitmap)
                    NewBitmap.SetPixel(x, y, newColor);

                return OldBitmap != NewBitmap ? oldColor : newColor;
            });
        }

        public void Apply(ColorMatrix oldMatrix, ColorMatrix newMatrix)
        {
            oldMatrix.Each((y, x, oldColor) =>
            {
                var newColor = Apply(oldColor);
                newMatrix.SetValue(y.UInt32(), x.UInt32(), newColor);
                return oldColor;
            });
        }

        public void Apply(WriteableBitmap input) => input.ForEach((x, y, oldColor) => Apply(oldColor));

        #endregion

        #region Commands

        ICommand clearCommand;
        [DisplayName("Clear")]
        [Icon(Images.XRound)]
        [Index(2), Tool]
        public ICommand ClearCommand 
            => clearCommand ??= new RelayCommand(Clear);

        ICommand clearEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Clear effects")]
        [Icon(App.ImagePath + "ClearEffects.png")]
        [Index(2), Tool]
        public ICommand ClearEffectsCommand
            => clearEffectsCommand ??= new RelayCommand(() => Effects.Clear(), () => Effects.Count > 0);

        ICommand copyCommand;
        [DisplayName("Copy")]
        [Icon(Images.Copy)]
        [Index(0), Tool]
        public ICommand CopyCommand
            => copyCommand ??= new RelayCommand(() => Copy.Set(this));

        ICommand copyEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Copy effects")]
        [Icon(App.ImagePath + "CopyEffects.png")]
        [Index(0), Tool]
        public ICommand CopyEffectsCommand
            => copyEffectsCommand ??= new RelayCommand(() => Copy.Set(Effects));

        ICommand pasteCommand;
        [DisplayName("Paste")]
        [Icon(Images.Paste)]
        [Index(1), Tool]
        public ICommand PasteCommand
            => pasteCommand ??= new RelayCommand(() => Paste(Copy.Get<LayerStyle>()),
            () => Copy.Get<LayerStyle>() != null);

        ICommand pasteEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Paste effects")]
        [Icon(App.ImagePath + "PasteEffects.png")]
        [Index(1), Tool]
        public ICommand PasteEffectsCommand
            => pasteEffectsCommand ??= new RelayCommand(() => Copy.Get<EffectCollection>().ForEach(i => Effects.Add(i.Copy())), () => Copy.Get<EffectCollection>() != null);

        ICommand saveCommand;
        [Category(Category.Save)]
        [DisplayName("Save")]
        [Icon(Images.Save)]
        [Tool]
        public ICommand SaveCommand
            => saveCommand ??= new RelayCommand(() => Panel.Find<StylesPanel>().SelectedGroup.Add(Clone()), () => Panel.Find<StylesPanel>()?.SelectedGroup != null);

        #endregion
    }
}