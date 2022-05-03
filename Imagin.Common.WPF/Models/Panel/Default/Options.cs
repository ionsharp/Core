using System;

namespace Imagin.Common.Models
{
    public class OptionsPanel : Panel
    {
        public static readonly ResourceKey TemplateKey = new();

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Options);

        [Hidden]
        public override string Title => "Options";

        public OptionsPanel() : base() { }
    }
}