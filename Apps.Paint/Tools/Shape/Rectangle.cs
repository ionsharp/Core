using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Rectangle")]
    [Icon(App.ImagePath + "Rectangle.png")]
    [Serializable]
    public class RectangleTool : RegionShapeTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Rectangle.png");

        public override IEnumerable<Point> GetPoints() => Shape.GetRectangle(CurrentRegion);
    }
}