using Imagin.Common.Linq;
using System;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class ToolPreset : Preset<Tool>
    {
        public ToolPreset() : base() { }

        public ToolPreset(string name, Tool instance = default) : base(name, instance) { }

        public override Preset<Tool> Clone() => new ToolPreset(Name, Instance.SmartClone());
    }
}