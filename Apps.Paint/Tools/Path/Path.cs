using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [DisplayName("Path")]
    [Icon(App.ImagePath + "Path.png")]
    [Serializable]
    public class PathTool : FreePathTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Path.png");
    }

    [DisplayName("Free path")]
    [Icon(App.ImagePath + "PathFree.png")]
    [Serializable]
    public class FreePathTool : Tool
    {
        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("PathFree.png");

        PointCollection points = new();

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (points.Count > 1)
            {
                var geometry = new Shape(points).Geometry();
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1 / zoom), geometry);
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, 0.5 / zoom), geometry);
            }
        }

        public override bool OnMouseDown(Point point)
        {
            points.Add(point);
            return base.OnMouseDown(point);
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
                points.Add(point);
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            //if (TargetPathGroup != null)
            //{
                //TargetPathGroup.Paths.Add(new(points));
            //}
            //else
            //{
                var newPathGroup = new PathGroup("Untitled");
                newPathGroup.Paths.Add(new(points));

                Document.If<ImageDocument>(i => i.Paths.ForEach(j => j.IsSelected = false));
                Document.If<ImageDocument>(i => i.Paths.Insert(0, newPathGroup));

                newPathGroup.IsSelected = true;
            //}
            points = new PointCollection();
        }
    }
}