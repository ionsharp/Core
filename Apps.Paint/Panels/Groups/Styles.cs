using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class StyleGroup : GroupCollection<LayerStyle>
    {
        public StyleGroup(string name) : base(name) { }
    }


    #region DefaultStyles

    [Serializable]
    public sealed class DefaultStyles : StyleGroup
    {
        public DefaultStyles() : base("Default")
        {
            var result = new List<LayerStyle>()
            {
                new LayerStyle("Baked",
                    new ShadingEffect(100, 100, 60),
                    new TintEffect(20, 30, 60),
                    new GammaEffect(0.7f)),

                new LayerStyle("Blue",
                    new BalanceEffect(0, 0, 30),
                    new ShadingEffect(0, 70, 80),
                    new TintEffect(0, 0, 40)),

                new LayerStyle("Castaway",
                    new TintEffect(30, 0, 0),
                    new GammaEffect(0.7f),
                    new SaturationEffect(-30)),

                new LayerStyle("Cheshire",
                    new GammaEffect(0.6f),
                    new TintEffect(0, 0, 40)),

                new LayerStyle("Cold",
                    new BalanceEffect(10, 0, 20),
                    new ShadingEffect(0, 10, 20),
                    new SaturationEffect(-10)),

                new LayerStyle("DarkSepia",
                    new AdjustEffect(),
                    new BrightnessEffect(-40),
                    new ContrastEffect(25),
                    new TintEffect(45, 25, 40)),

                new LayerStyle("Frostbite",
                    new SaturationEffect(-30),
                    new TintEffect(0, 0, 20),
                    new GammaEffect(0.8f)),

                new LayerStyle("Green",
                    new BalanceEffect(0, 30, 0),
                    new ShadingEffect(0, 80, 70),
                    new TintEffect(0, 40, 0)),

                new LayerStyle("Independence",
                    new GammaEffect(0.7f),
                    new TintEffect(0, 0, 30)),

                new LayerStyle("Jungle",
                    new GammaEffect(0.7f)),

                new LayerStyle("Light Purple",
                    new TintEffect(65, 0, 70)),

                new LayerStyle("Mardi Gras",
                    new GammaEffect(0.6f)),

                new LayerStyle("Marietta",
                    new GammaEffect(0.7f),
                    new TintEffect(0, 0, 30),
                    new BalanceEffect(32, 32, 32),
                    new ShadingEffect(100, 100, 50)),

                new LayerStyle("Metro",
                    new TintEffect(0, 0, 60),
                    new BrightnessEffect(-20),
                    new ContrastEffect(25),
                    new BalanceEffect(-25, -35, 50),
                    new TintEffect(35, 0, 70),
                    new BrightnessEffect(-20),
                    new ContrastEffect(15)),

                new LayerStyle("Perpetual",
                    new ShadingEffect(100, 100, 60),
                    new GammaEffect(0.7f),
                    new BalanceEffect(-60, 20, 100)),

                new LayerStyle("Purple",
                    new TintEffect(65, 0, 70),
                    new BalanceEffect(-5, -50, 100),
                    new BrightnessEffect(-20),
                    new ContrastEffect(25)),

                new LayerStyle("Red",
                    new BalanceEffect(30, 0, 0),
                    new ShadingEffect(80, 70, 0),
                    new TintEffect(40, 0, 0)),

                new LayerStyle("Rusty",
                    new TintEffect(25, 15, 5)),

                new LayerStyle("SciFi",
                    new BrightnessEffect(-40),
                    new ContrastEffect(25),
                    new TintEffect(45, 25, 40)),

                new LayerStyle("Sepia",
                    new AdjustEffect()),

                new LayerStyle("Submarine",
                    new GammaEffect(0.8f),
                    new TintEffect(0, 0, 15)),

                new LayerStyle("Vintage",
                    new TintEffect(10, 30, 60),
                    new GammaEffect(0.5f)),

                new LayerStyle("Warm",
                    new BalanceEffect(20, 0, 10),
                    new ShadingEffect(0, 20, 10),
                    new SaturationEffect(10)),

                new LayerStyle("Western",
                    new BalanceEffect(50, 0, -50))
            };
            result.ForEach(i => Add(i));
        }
    }

    #endregion

    #region StylesPanel

    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class StylesPanel : LocalGroupPanel<LayerStyle>
    {
        enum Category { Apply, Effects, General, Save }

        #region Properties

        int columns = 7;
        [Format(RangeFormat.Both)]
        [Option]
        [Range(1, 16, 1)]
        [Visible]
        public int Columns
        {
            get => columns;
            set => this.Change(ref columns, value);
        }

        public override Uri Icon => Resources.ProjectImage("LayerStyle.png");

        public override string Title => "Styles";

        #endregion

        #region StylesPanel

        public StylesPanel() : base(Get.Current<Options>().Styles) { }

        #endregion

        #region Methods

        protected override IEnumerable<GroupCollection<LayerStyle>> GetDefault()
            { yield return new DefaultStyles(); }

        #endregion

        #region Commands

        ICommand applyCommand;
        [Featured(AboveBelow.Below), DisplayName("Apply")]
        [Icon(App.ImagePath + "ApplyFilter.png")]
        [Tool, Visible]
        public ICommand ApplyCommand => applyCommand ??= new RelayCommand<LayerStyle>(i =>
        {
            i ??= SelectedItem;
            if (Find<LayersPanel>().SelectedLayer is PixelLayer j)
                i.Apply(j.Pixels);
        },
        i => (i != null || SelectedItem != null) && Find<LayersPanel>()?.SelectedLayer is PixelLayer);

        ICommand clearCommand;
        [DisplayName("Clear")]
        [Icon(Images.XRound)]
        [Tool, Visible]
        public ICommand ClearCommand
            => clearCommand ??= new RelayCommand(() => SelectedItem.Clear(), () => SelectedItem != null);

        ICommand clearEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Clear effects")]
        [Icon(App.ImagePath + "ClearEffects.png")]
        [Tool, Visible]
        public ICommand ClearEffectsCommand
            => clearEffectsCommand ??= new RelayCommand(() => SelectedItem.As<LayerStyle>().Effects.Clear(), () => SelectedItem != null);

        ICommand copyCommand;
        [DisplayName("Copy")]
        [Icon(Images.Copy)]
        [Tool, Visible]
        public ICommand CopyCommand
            => copyCommand ??= new RelayCommand(() => Copy.Set(SelectedItem), () => SelectedItem != null);

        ICommand copyEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Copy effects")]
        [Icon(App.ImagePath + "CopyEffects.png")]
        [Tool, Visible]
        public ICommand CopyEffectsCommand
            => copyEffectsCommand ??= new RelayCommand(() => Copy.Set(SelectedItem.As<LayerStyle>().Effects), () => SelectedItem != null);

        ICommand pasteCommand;
        [DisplayName("Paste")]
        [Icon(Images.Paste)]
        [Tool, Visible]
        public ICommand PasteCommand
            => pasteCommand ??= new RelayCommand(() => SelectedItem.Paste(Copy.Get<LayerStyle>()),
            () => SelectedItem != null && Copy.Get<LayerStyle>() != null);

        ICommand pasteEffectsCommand;
        [Category(Category.Effects)]
        [DisplayName("Paste effects")]
        [Icon(App.ImagePath + "PasteEffects.png")]
        [Tool, Visible]
        public ICommand PasteEffectsCommand
            => pasteEffectsCommand ??= new RelayCommand(() => Copy.Get<EffectCollection>().ForEach(i => SelectedItem.As<LayerStyle>().Effects.Add(i.Copy())), () => SelectedItem != null && Copy.Get<EffectCollection>() != null);

        #endregion
    }

    #endregion
}