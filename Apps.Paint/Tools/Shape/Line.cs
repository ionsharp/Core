using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [DisplayName("Line")]
    [Icon(App.ImagePath + "Line.png")]
    [Serializable]
    public class LineTool : ShapeTool
    {
        protected override PointCollection CurrentPoints => new() { MouseMove.Value, MouseDown.Value };

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Line.png");

        string stroke = $"255,0,0,0";
        public SolidColorBrush Stroke
        {
            get
            {
                var result = stroke.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref stroke, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double strokeThickness = 1.0;
        public double StrokeThickness
        {
            get => strokeThickness;
            set => this.Change(ref strokeThickness, value);
        }

        protected override ShapeLayer NewLayer() 
            => new("Untitled", Stroke, StrokeThickness);

        public override IEnumerable<Point> GetPoints()
        {
            yield return MouseMove.Value;
            yield return MouseDown.Value;
        }

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (hiddenLayer != null)
            {
                if (MouseDown != null && MouseMove != null)
                {
                    input.DrawLine(new Pen(Brushes.Black, 1 / zoom), MouseMove.Value, MouseDown.Value);
                    input.DrawLine(new Pen(Brushes.White, 0.5 / zoom), MouseMove.Value, MouseDown.Value);
                }
            }
        }
    }
}