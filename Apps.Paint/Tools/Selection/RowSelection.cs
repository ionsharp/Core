using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Row selection")]
    [Icon(App.ImagePath + "RowSelection.png")]
    [Serializable]
    public class RowSelectionTool : SelectionTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("RowSelection.png");

        int height = 30;
        public int Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        protected override IEnumerable<Point> GetPoints(double x, double y, double height, double width)
        {
            yield return new Point(0, y);
            yield return new Point(0, y + height);
            yield return new Point(width, y + height);
            yield return new Point(width, y);
        }

        protected override Int32Region CalculateRegion(Point point) 
            => new(point.X.Int32(), point.Y.Coerce(Document.Height - height, 0).Int32(), Document.Width, height);
    }
}