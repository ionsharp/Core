using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Ellipse selection")]
    [Icon(App.ImagePath + "EllipseSelection.png")]
    [Serializable]
    public class EllipseSelectionTool : SelectionTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("EllipseSelection.png");

        protected override IEnumerable<Point> GetPoints(double x, double y, double height, double width)
            => Shape.GetEllipse(new(x.Int32(), y.Int32(), width.Int32(), height.Int32()));
    }
}