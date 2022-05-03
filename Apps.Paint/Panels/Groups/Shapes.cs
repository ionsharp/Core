using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    #region PolygonShapes

    public class PolygonShapes : GroupCollection<Shape>
    {
        public PolygonShapes() : base("Polygons")
        {
            var size = new Size(20, 20);
            var origin = new Point(size.Width, size.Height);

            var polygonNames = new string[9]
            {
                "Triangle",
                "Rectangle",
                "Pentagon",
                "Hexagon",
                "Heptagon",
                "Octagon",
                "Nonagon",
                "Decagon",
                "Hexadecagon",
            };
            for (uint i = 3; i <= 11; i++)
            {
                var n = i;
                i = i == 11 ? 16 : i;

                var shape = new Shape(polygonNames[n - 3], Shape.GetPolygon(270.0.GetRadian(), origin, size, i, 0));
                shape.Translate();
                shape.Normalize();
                Add(shape);
            }
        }
    }

    #endregion

    #region StarShapes

    public class StarShapes : GroupCollection<Shape>
    {
        public StarShapes() : base("Stars")
        {
            var size = new Size(20, 20);
            var origin = new Point(size.Width, size.Height);

            for (uint i = 3; i <= 16; i++)
            {
                var shape = new Shape($"{i}-Star", Shape.GetStar(270.0.GetRadian(), i, 2, new Rect(origin, size), 0));
                shape.Translate();
                shape.Normalize();
                Add(shape);
            }
        }
    }

    #endregion

    #region ShapesPanel

    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ShapesPanel : LocalGroupPanel<Shape>
    {
        enum Category { Save }

        #region Properties

        public override Uri Icon => Resources.ProjectImage("CustomShape.png");

        public override string Title => "Shapes";

        #endregion

        #region ShapesPanel

        public ShapesPanel() : base(Get.Current<Options>().Shapes) { }

        #endregion

        #region Methods

        protected override IEnumerable<GroupCollection<Shape>> GetDefault()
        {
            yield return new PolygonShapes();
            yield return new StarShapes();
        }

        #endregion

        #region Commands

        ICommand saveAsBrushCommand;
        [Category(Category.Save)]
        [DisplayName("Save as brush")]
        [Icon(App.ImagePath + "ConvertToBrush.png")]
        [Tool, Visible]
        public ICommand SaveAsBrushCommand
            => saveAsBrushCommand ??= new RelayCommand(() => Find<BrushesPanel>().SelectedGroup.Add(new ShapeBrush(Name, SelectedItem)), () => SelectedItem != null && Find<BrushesPanel>().SelectedGroup != null);

        ICommand saveAsPathCommand;
        [Category(Category.Save)]
        [DisplayName("Save as path")]
        [Icon(App.ImagePath + "ConvertToPath.png")]
        [Tool, Visible]
        public ICommand SaveAsPathCommand => saveAsPathCommand ??= new RelayCommand(() =>
        {
        }, 
        () => SelectedItem != null && Find<PathsPanel>().SelectedGroup != null);

        #endregion
    }

    #endregion
}