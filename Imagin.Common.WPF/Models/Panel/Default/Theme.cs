using System;

namespace Imagin.Common.Models
{
    public class ThemePanel : Panel
    {
        [Hidden]
        public static readonly ResourceKey TemplateKey = new();

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Palette);

        [Hidden]
        public override string Title => "Theme";

        public ThemePanel() : base() { }
    }
}