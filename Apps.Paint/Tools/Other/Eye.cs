using Imagin.Common;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [DisplayName("Eye")]
    [Icon(App.ImagePath + "EyeDrop.png")]
    [Serializable]
    public class EyeTool : Tool
    {
        [Hidden]
        public override Cursor Cursor => Cursors.None;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("EyeDrop.png");

        [field: NonSerialized]
        Color color;
        [ReadOnly]
        public Color Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            var color = Common.Media.Display.Color();
            Color = Color.FromArgb(255, color.R, color.G, color.B);
            Get.Current<Options>().ForegroundColor = Color;
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var color = Common.Media.Display.Color();
                Color = Color.FromArgb(255, color.R, color.G, color.B);
                Get.Current<Options>().ForegroundColor = Color;
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            Color = default;
        }
    }
}