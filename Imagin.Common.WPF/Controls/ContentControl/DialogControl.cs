using System.Windows.Media.Effects;

namespace Imagin.Common.Controls
{
    public class DialogControl : ContentControl<DialogReference>
    {
        public static readonly ResourceKey<DropShadowEffect> DropShadowEffectKey = new();

        public DialogControl() : base() { }
    }
}