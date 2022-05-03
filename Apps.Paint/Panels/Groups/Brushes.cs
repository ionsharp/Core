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
    #region BasicBrushes

    [Serializable]
    public class BasicBrushes : GroupCollection<Brush>
    {
        public BasicBrushes() : base("Basic")
        {
            Add(new CircleBrush());
            Add(new SquareBrush());
        }
    }

    #endregion

    #region OvalBrushes

    [Serializable]
    public class OvalBrushes : GroupCollection<Brush>
    {
        public OvalBrushes() : base("Ovals")
        {
            Add(new CircleBrush()
            {
                Angle = -45,
                XScale = 0.25,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = -22.5,
                XScale = 0.25,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = -+0,
                XScale = 0.25,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +22.5,
                XScale = 0.25,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +45,
                XScale = 0.25,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +90,
                XScale = 0.25,
                YScale = 1
            });

            Add(new CircleBrush()
            {
                Angle = -45,
                XScale = 0.5,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = -22.5,
                XScale = 0.5,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = -+0,
                XScale = 0.5,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +22.5,
                XScale = 0.5,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +45,
                XScale = 0.5,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +90,
                XScale = 0.5,
                YScale = 1
            });

            Add(new CircleBrush()
            {
                Angle = -45,
                XScale = 0.75,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = -22.5,
                XScale = 0.75,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = -+0,
                XScale = 0.75,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +22.5,
                XScale = 0.75,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +45,
                XScale = 0.75,
                YScale = 1
            });
            Add(new CircleBrush()
            {
                Angle = +90,
                XScale = 0.75,
                YScale = 1
            });
        }
    }

    #endregion

    #region PolygonBrushes

    [Serializable]
    public class PolygonBrushes : GroupCollection<Brush>
    {
        public PolygonBrushes() : base("Polygons")
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

                var brush = new ShapeBrush(polygonNames[n - 3], Shape.GetPolygon(270.0.GetRadian(), origin, size, i, 0));
                brush.Path.Translate();
                brush.Path.Normalize();
                Add(brush);
            }
        }
    }

    #endregion

    #region StarBrushes

    [Serializable]
    public class StarBrushes : GroupCollection<Brush>
    {
        public StarBrushes() : base("Stars")
        {
            var size = new Size(20, 20);
            var origin = new Point(size.Width, size.Height);

            for (uint i = 3; i <= 16; i++)
            {
                var brush = new ShapeBrush($"{i}-Star", Shape.GetStar(270.0.GetRadian(), i, 2, new Rect(origin, size), 0));
                brush.Path.Translate();
                brush.Path.Normalize();
                Add(brush);
            }
        }
    }

    #endregion

    //...

    #region BrushesPanel

    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class BrushesPanel : LocalGroupPanel<Brush>
    {
        enum Category { Save }

        #region Properties

        public override Uri Icon => Resources.ProjectImage("Brush.png");

        public override string Title => "Brushes";

        #endregion

        #region BrushesPanel

        public BrushesPanel() : base(Get.Current<Options>().Brushes) { }

        #endregion

        #region Methods

        protected override IEnumerable<GroupCollection<Brush>> GetDefault()
        {
            yield return new BasicBrushes();
            yield return new OvalBrushes();
            yield return new PolygonBrushes();
            yield return new StarBrushes();
        }

        #endregion

        #region Commands

        ICommand saveAsMatrixCommand;
        [Category(Category.Save)]
        [DisplayName("Save as matrix")]
        [Icon(App.ImagePath + "ConvertToMatrix.png")]
        [Tool]
        public ICommand SaveAsMatrixCommand => saveAsMatrixCommand ??= new RelayCommand(() =>
        {
            Find<MatricesPanel>().SelectedGroup.Add(new Matrix());
        }, 
        () => SelectedItem != null && Find<MatricesPanel>().SelectedGroup != null);

        ICommand saveAsPathCommand;
        [Category(Category.Save)]
        [DisplayName("Save as path")]
        [Icon(App.ImagePath + "ConvertToPath.png")]
        [Tool, Visible]
        public ICommand SaveAsPathCommand => saveAsPathCommand ??= new RelayCommand(() =>
        {
        }, 
        () => SelectedItem != null && Find<PathsPanel>().SelectedGroup != null);

        ICommand saveAsShapeCommand;
        [Category(Category.Save)]
        [DisplayName("Save as shape")]
        [Icon(App.ImagePath + "ConvertToShape.png")]
        [Tool, Visible]
        public ICommand SaveAsShapeCommand => saveAsShapeCommand ??= new RelayCommand(() =>
        {
            Find<ShapesPanel>().SelectedGroup.Add(new Shape());
        }, 
        () => SelectedItem != null && Find<ShapesPanel>().SelectedGroup != null);

        #endregion
    }

    #endregion
}