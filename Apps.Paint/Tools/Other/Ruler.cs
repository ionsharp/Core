using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [DisplayName("Ruler")]
    [Icon(App.ImagePath + "Ruler.png")]
    [Serializable]
    public class RulerTool : Tool
    {
        #region Properties

        double? angle = null;
        [ReadOnly]
        public virtual double? Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Ruler.png");

        double? length = null;
        [ReadOnly]
        public virtual double? Length
        {
            get => length;
            set => this.Change(ref length, value);
        }

        double? height = null;
        [ReadOnly]
        public virtual double? Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        double snap = 30;
        [Index(-1)]
        [Range(1.0, 90.0, 1.0)]
        [Format(RangeFormat.Both)]
        public virtual double Snap
        {
            get => snap;
            set => this.Change(ref snap, value);
        }

        double? width = null;
        [ReadOnly]
        public virtual double? Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        double? x = null;
        [ReadOnly]
        public virtual double? X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        double? y = null;
        [ReadOnly]
        public virtual double? Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        double x2;
        [ReadOnly]
        public virtual double X2
        {
            get => x2;
            set => this.Change(ref x2, value);
        }

        double y2;
        [ReadOnly]
        public virtual double Y2
        {
            get => y2;
            set => this.Change(ref y2, value);
        }

        #endregion

        #region Methods

        protected void Draw()
        {
            if (MouseDown == null)
                return;

            X = MouseDown.Value.X.Int32();
            Y = MouseDown.Value.Y.Int32();

            if (Angle != null && Length != null)
            {
                var newAngle = -(Angle.Value - 90).GetRadian();
                var x = X.Value + Math.Sin(newAngle) * Length.Value;
                var y = Y.Value + Math.Cos(newAngle) * Length.Value;

                X2 = x;
                Y2 = y;
            }
            else
            {
                X2 = X.Value;
                Y2 = Y.Value;
            }
        }

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (x != null && y != null)
            {
                input.DrawLine(new Pen(Brushes.Black, 1 / zoom), new Point(x.Value / zoom, y.Value / zoom), new Point(x2 / zoom, y2 / zoom));
                input.DrawLine(new Pen(Brushes.White, 0.5 / zoom), new Point(x.Value / zoom, y.Value / zoom), new Point(x2 / zoom, y2 / zoom));
            }
        }

        public override bool OnMouseDown(Point point)
        {
            if (base.OnMouseDown(point))
            {
                Height = Width = Angle = Length = 0;
                Draw();
                return true;
            }
            return false;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (X != null && Y != null)
            {
                Height 
                    = point.Y - y;
                Width 
                    = point.X - x;

                Angle 
                    = Math.Atan2(point.Y - Y.Value, point.X - X.Value).GetDegree().NearestFactor(Snap);
                Length 
                    = point.Distance(new Point(x.Value, y.Value));

                Draw();
            }
        }

        #endregion
    }
}