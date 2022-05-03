using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Rounded rectangle")]
    [Icon(App.ImagePath + "RoundedRectangle.png")]
    [Serializable]
    public class RoundedRectangleTool : RegionShapeTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("RoundedRectangle.png");

        int radiusX = 5;
        public int RadiusX
        {
            get => radiusX;
            set => this.Change(ref radiusX, value);
        }

        int radiusY = 5;
        public int RadiusY
        {
            get => radiusY;
            set => this.Change(ref radiusY, value);
        }

        public override IEnumerable<Point> GetPoints() => Shape.GetRoundedRectangle(CurrentRegion, radiusX, radiusY);
    }
}