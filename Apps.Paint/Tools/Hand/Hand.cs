using Imagin.Common;
using System;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Hand")]
    [Icon(App.ImagePath + "Hand.png")]
    [Serializable]
    public class HandTool : Tool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Hand.png");

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null && Viewer != null)
            {
                var dx = point.X - MouseDown.Value.X;
                var dy = point.Y - MouseDown.Value.Y;

                Viewer.PART_ScrollViewer.ScrollToHorizontalOffset(Viewer.PART_ScrollViewer.HorizontalOffset - dx);
                Viewer.PART_ScrollViewer.ScrollToVerticalOffset(Viewer.PART_ScrollViewer.VerticalOffset - dy);
            }
        }
    }
}