using Imagin.Common;
using System;
using System.Windows;

namespace Imagin.Apps.Paint
{
    [DisplayName("Magic wand")]
    [Icon(App.ImagePath + "MagicWand.png")]
    [Serializable]
    public class MagicWandTool : Tool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("MagicWand.png");

        double tolerance;
        public double Tolerance
        {
            get => tolerance;
            set => this.Change(ref tolerance, value);
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            //The algorthim will basically just be a flash flood, but instead of setting pixels, we're getting pixels to construct a selection!
            //Tolerance would work fine for alpha, but what about opaque colors?
            return true;
        }
    }
}