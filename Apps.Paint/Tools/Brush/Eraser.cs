using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Eraser")]
    [Icon(App.ImagePath + "Eraser.png")]
    [Serializable]
    public class EraserTool : BrushTool
    {
        [Hidden]
        public override Uri CursorIcon => Resources.ProjectImage("EraserCursor.png");

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Eraser.png");

        [Hidden]
        public override BlendModes Mode
        {
            get => base.Mode;
            set => base.Mode = value;
        }

        protected override void Draw(Vector2<int> point, Color color)
        {
            Brush ??= new CircleBrush();
            TargetLayer.Pixels.Erase(Brush.GetBytes(Brush.Size).Transform(i => (i / 255 * Opacity * 255).Byte()), new(point.X - Brush.Size / 2, point.Y - Brush.Size / 2));
        }
    }
}