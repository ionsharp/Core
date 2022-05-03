using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Ellipse")]
    [Icon(App.ImagePath + "Ellipse.png")]
    [Serializable]
    public class EllipseTool : RegionShapeTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Ellipse.png");

        public override IEnumerable<Point> GetPoints() => Shape.GetEllipse(CurrentRegion);
    }
}