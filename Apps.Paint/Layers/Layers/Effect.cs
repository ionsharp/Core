using Imagin.Apps.Paint.Effects;
using Imagin.Common;
using Imagin.Common.Linq;

namespace Imagin.Apps.Paint
{
    /// <summary>
    /// Applies an effect to all layers beneath (at same depth or higher).
    /// </summary>
    [DisplayName("Effect layer")]
    [Icon(App.ImagePath + "LayerEffect.png")]
    public class EffectLayer : StackableLayer
    {
        ImageEffect effect;
        public ImageEffect Effect
        {
            get => effect;
            set => this.Change(ref effect, value);
        }

        [Hidden]
        public override bool IsVisible
        {
            get => effect?.IsVisible ?? false;
            set
            {
                effect.If(i => i.IsVisible = value);
                this.Changed(() => IsVisible);
            }
        }

        [Featured, ReadOnly]
        public override string Name => effect?.Name;

        public EffectLayer(ImageEffect effect) : base(LayerType.Effect, null) => Effect = effect;

        public override Layer Clone() => new EffectLayer(Effect.Copy());
    }
}