using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [DisplayName("Rotate hand")]
    [Icon(App.ImagePath + "HandRotate.png")]
    [Serializable]
    public class HandRotateTool : HandTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("HandRotate.png");

        Point origin => new(ImageViewer.ActualWidth / 2.0, ImageViewer.ActualHeight / 2.0);

        [Format(RangeFormat.Both)]
        [Range(-180.0, 180.0, 1.0)]
        public double Angle
        {
            get => Document?.Angle ?? 0;
            set
            {
                Document.Angle = value;
                this.Changed(() => Angle);
            }
        }

        double snap = 30;
        [Range(1.0, 90.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Snap
        {
            get => snap;
            set => this.Change(ref snap, value);
        }

        //...

        void Update(Point target)
        {
            var vector2 = target - origin;
            var vector1 = new Point(0, 1);

            var angleInRadians = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
            var angleInDegrees = angleInRadians.GetDegree();

            Document.Angle = ModifierKeys.Shift.Pressed() ? angleInDegrees.NearestFactor(snap) : angleInDegrees;
            this.Changed(() => Angle);
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            ShowCompass = true;
            Update(MouseDownAbsolute ?? point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            if (MouseDown != null)
                Update(MouseMoveAbsolute ?? point);
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            ShowCompass = false;
        }
    }
}