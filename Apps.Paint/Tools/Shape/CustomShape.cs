using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [DisplayName("Custom shape")]
    [Icon(App.ImagePath + "CustomShape.png")]
    [Serializable]
    public class CustomShapeTool : RegionShapeTool
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("CustomShape.png");

        int shape = 0;
        [Source(nameof(Shapes))]
        [Style(Int32Style.Index)]
        public int Shape
        {
            get => shape;
            set => this.Change(ref shape, value);
        }

        ObservableCollection<Shape> shapes = new();
        [Hidden]
        public ObservableCollection<Shape> Shapes
        {
            get => shapes;
            set => this.Change(ref shapes, value);
        }

        //...

        public CustomShapeTool() : base() { }

        //...

        public override IEnumerable<Point> GetPoints()
        {
            if (Shapes[Shape] == null)
                yield break;

            var bounds = Common.Media.Shape.GetBounds(Shapes[Shape].Points);

            var oldPoints = Common.Media.Shape.Copy(Common.Media.Shape.From(Shapes[Shape].Points));
            Common.Media.Shape.Scale(oldPoints, new System.Drawing.Point(CurrentRegion.Width.Coerce(int.MaxValue, 1), CurrentRegion.Height.Coerce(int.MaxValue, 1)));

            for (int i = 0; i < oldPoints.Length * 2; i += 2)
            {
                double x = oldPoints[i]; double y = oldPoints[i + 1];
                Common.Media.Shape.Reflect(ref x, ref y, Quadrant);
                oldPoints[i] = x.Int32();
                oldPoints[i + 1] = y.Int32();
            }

            Common.Media.Shape.Translate(oldPoints, new System.Drawing.Point(CurrentRegion.X, CurrentRegion.Y));

            for (var i = 0; i < oldPoints.Length; i += 2)
                yield return new Point(oldPoints[i], oldPoints[i + 1]);
        }

        //...

        ICommand exportCommand;
        [DisplayName("Export")]
        [Index(998)]
        public virtual ICommand ExportCommand => exportCommand ??= new RelayCommand(() => _ = Get.Current<Options>().Shapes.Export(), () => true);

        ICommand importCommand;
        [DisplayName("Import")]
        [Index(999)]
        public virtual ICommand ImportCommand => importCommand ??= new RelayCommand(() => Get.Current<Options>().Shapes.Import(), () => true);
    }
}