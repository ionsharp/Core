using Imagin.Common;
using Imagin.Common.Media;
using System;

namespace Imagin.Apps.Paint
{
    [Icon(App.ImagePath + "Selection.png")]
    [Serializable]
    public class Selection : Base
    {
        Shape path = null;
        public Shape Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        public Selection() : base() { }

        public Selection(Shape path) : base() => Path = new Shape(path.Points);
    }
}