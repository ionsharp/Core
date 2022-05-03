using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Polygonal lasso")]
    [Icon(App.ImagePath + "PolygonalLasso.png")]
    [Serializable]
    public class PolygonalLassoTool : PreviewSelectionTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("PolygonalLasso.png");

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (Preview?.Count > 1)
            {
                var geometry = new Shape(Preview).Geometry();
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1 / zoom), geometry);
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, 0.5 / zoom), geometry);
            }
        }

        public override void OnMouseDoubleClick(Point point)
        {
            var newSelection = new Shape();
            Shape.Round(Points).ForEach(i => newSelection.Points.Add(i.Int32().Double()));

            for (var i = 0; i < newSelection.Points.Count; i++)
            {
                var current = newSelection.Points[i];
                newSelection.Points[i] = current.Coerce(new Point(Document.Width, Document.Height), new Point(0, 0));
            }

            Document.Selections.Add(newSelection);
            Points.Clear();
            Preview = null;
        }

        public override bool OnMouseDown(Point point)
        {            
            if (!Points.Any<Point>())
            {            
                //Move the selection
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    return false;
                }
                //Start a new selection (otherwise, add to the selection)
                else if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    Document.Selections.Clear();
                }

                Points.Add(point);
                Points.Add(point);

                Preview = new PointCollection(Points);
            }
            else
            {
                if (Points.Last<Point>() == point)
                    point = new Point(point.X + 1, point.Y + 1);

                Points.Add(point);
                Preview = new PointCollection(Points);
            }

            base.OnMouseDown(point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                Points.Remove(Points.Last<Point>());
                Points.Add(point);

                Preview = new PointCollection(Points);
            }
        }
    }
}