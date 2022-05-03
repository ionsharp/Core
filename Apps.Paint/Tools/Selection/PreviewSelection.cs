using Imagin.Common;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public abstract class PreviewSelectionTool : Tool
    {
        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        PointCollection points = new();
        [Hidden]
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        PointCollection preview = new();
        [Hidden]
        public PointCollection Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }
    }
}