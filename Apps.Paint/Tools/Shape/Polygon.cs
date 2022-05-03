using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Polygon")]
    [Icon(App.ImagePath + "Polygon.png")]
    [Serializable]
    public class PolygonTool : RegionShapeTool
    {
        double angle = 0;
        public double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Polygon.png");

        int indent = 2;
        public int Indent
        {
            get => indent;
            set => this.Change(ref indent, value);
        }

        uint sides = 5;
        public uint Sides
        {
            get => sides;
            set => this.Change(ref sides, value);
        }

        bool star = false;
        public bool Star
        {
            get => star;
            set => this.Change(ref star, value);
        }

        public override IEnumerable<Point> GetPoints()
            => Star ? Shape.GetStar(CurrentRegion, Angle, Sides, Indent, Quadrant) : Shape.GetPolygon(CurrentRegion, Angle, Sides, Quadrant);
    }
}