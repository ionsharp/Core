using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Selection")]
    [Icon(App.ImagePath + "Selection.png")]
    [Serializable]
    public class SelectionTool : Tool
    {
        protected Shape newSelection;

        //...

        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Selection.png");

        //...
        
        GeometryCombineMode mode = GeometryCombineMode.Union;
        public GeometryCombineMode Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        [field: NonSerialized]
        WriteableBitmap preview;
        [Hidden]
        public WriteableBitmap Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }

        //...

        protected virtual IEnumerable<Point> GetPoints(double x, double y, double height, double width)
        {
            yield return new Point(x, y);
            yield return new Point(x, y + height);
            yield return new Point(x + width, y + height);
            yield return new Point(x + width, y);
        }

        //...

        int Orientation(Vector2<int> p, Vector2<int> q, Vector2<int> r)
        {
            int result = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            return result == 0 ? 0 : result > 0 ? 1 : 2;
        }

        bool OnSegment(Vector2<int> p, Vector2<int> q, Vector2<int> r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        bool Intersects(Line<int> line1, Line<int> line2)
        {
            var p1 = new Vector2<int>(line1.X1, line1.Y1);
            var q1 = new Vector2<int>(line1.X2, line1.Y2);

            var p2 = new Vector2<int>(line2.X1, line2.Y1);
            var q2 = new Vector2<int>(line2.X2, line2.Y2);

            int o1 = 0, o2 = 0, o3 = 0, o4 = 0;
            o1 = Orientation(p1, q1, p2);
            o2 = Orientation(p1, q1, q2);
            o3 = Orientation(p2, q2, p1);
            o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
                return true;

            if (o1 == 0 && OnSegment(p1, p2, q1))
                return true;

            if (o2 == 0 && OnSegment(p1, q2, q1))
                return true;

            if (o3 == 0 && OnSegment(p2, p1, q2))
                return true;

            if (o4 == 0 && OnSegment(p2, q1, q2))
                return true;

            return false;
        }

        bool Intersects(PointCollection a, PointCollection b)
        {
            var m = GetLines(a);
            var n = GetLines(b);
            foreach (var i in m)
            {
                foreach (var j in n)
                {
                    if (Intersects(i, j))
                        return true;
                }
            }
            return false;
        }

        IEnumerable<Line<int>> GetLines(PointCollection points)
        {
            var previous = default(Point);
            foreach (var i in points)
            {
                if (previous != null)
                    yield return new Line<int>(previous.X.Int32(), previous.Y.Int32(), i.X.Int32(), i.Y.Int32());

                previous = i;
            }
        }

        //...

        protected virtual Int32Region CalculateRegion(Point point)
            => GetRegion(MouseDown.Value, point);

        //...

        public override bool OnMouseDown(Point point)
        {
            if (Document is ImageDocument document)
            {            
                //Move the selection
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    return false;
                }
                //Start a new selection (otherwise, add to the selection)
                else if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    document.Selections.Clear();
                }

                newSelection = new Shape();
                document.Selections.Add(newSelection);
            }
            base.OnMouseDown(point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var region = CalculateRegion(point);
                newSelection.Points = new(GetPoints(region.X, region.Y, region.Height, region.Width));
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            for (var i = 0; i < newSelection.Points.Count; i++)
            {
                var current = newSelection.Points[i];
                newSelection.Points[i] = current.Coerce(new Point(Document.Width, Document.Height), new Point(0, 0));
            }
            return;
            PointCollection a = null, b = null;

            if (Document is ImageDocument document)
            {
                Shape previous = null;
                for (var i = document.Selections.Count - 1; i >= 0; i--)
                {
                    if (previous != null)
                    {
                        a = previous.Points;
                        b = document.Selections[i].Points;

                        System.Windows.Media.Geometry.Combine(new Shape(a).Geometry(), new Shape(b).Geometry(), Mode, null, 0, ToleranceType.Absolute);
                        document.Selections.RemoveAt(i + 1);
                    }
                    previous = document.Selections[i];
                }
            }
        }
    }
}