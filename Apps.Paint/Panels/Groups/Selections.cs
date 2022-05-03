using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SelectionsPanel : LocalGroupPanel<Selection>
    {
        enum Category { Open, Save }

        #region Properties

        public override Uri Icon => Resources.ProjectImage("Selection.png");

        public override string Title => "Selections";

        #endregion

        #region SelectionsPanel

        public SelectionsPanel() : base(Get.Current<Options>().Selections) { }

        #endregion

        #region Methods

        protected override IEnumerable<GroupCollection<Selection>> GetDefault()
            { yield return new GroupCollection<Selection>("Default"); }

        #endregion

        #region Commands

        ICommand openSelectionCommand;
        [Category(Category.Open)]
        [DisplayName("Open")]
        [Icon(Images.Open)]
        [Tool, Visible]
        public ICommand OpenSelectionCommand => openSelectionCommand ??= new RelayCommand(() =>
        {
            var selections = Get.Current<MainViewModel>().ActiveDocument.As<ImageDocument>().Selections;

            selections.Clear();
            selections.Add(new(SelectedItem.As<Selection>().Path.Points));
        },
        () => Get.Current<MainViewModel>().ActiveDocument is ImageDocument i && SelectedItem != null);

        ICommand saveSelectionCommand;
        [Category(Category.Save)]
        [DisplayName("Save")]
        [Icon(Images.Save)]
        [Tool, Visible]
        public ICommand SaveSelectionCommand => saveSelectionCommand ??= new RelayCommand(() =>
        {
            var group = Find<SelectionsPanel>().SelectedGroup;

            var document = Get.Current<MainViewModel>().ActiveDocument.As<ImageDocument>();
            document.Selections.ForEach(i =>
            {
                var result = new Selection(i);
                //Path.Normalize(result.Path.Points, new(0, document.Width), new(0, document.Height));
                group.Add(result);
            });
        },
        () => Find<SelectionsPanel>().SelectedGroup != null && Get.Current<MainViewModel>().ActiveDocument is ImageDocument i && i.Selections.Count > 0);

        ICommand saveSelectionAsBrushCommand;
        [Category(Category.Save)]
        [DisplayName("Save as brush")]
        [Icon(App.ImagePath + "ConvertToBrush.png")]
        [Tool, Visible]
        public ICommand SaveSelectionAsBrushCommand => saveSelectionAsBrushCommand ??= new RelayCommand(() =>
        {
            var group = Find<BrushesPanel>().SelectedGroup ?? (GroupCollection<Brush>)XIList.FirstOrDefault(Get.Current<Options>().Brushes);
            if (group != null)
            {
                var selections = Get.Current<MainViewModel>().ActiveDocument.As<ImageDocument>().Selections;

                var points = selections[0].Points.Select(i => new Point(i.X.Round(), i.Y.Round())).ToArray<Point>();
                var bounds = Shape.GetBounds(points);

                var pixels = Find<LayersPanel>().SelectedLayer.As<PixelLayer>().Pixels;

                var h = bounds.Height.UInt32();
                var w = bounds.Width.UInt32();

                var x0 = bounds.X.Int32();
                var y0 = bounds.Y.Int32();

                var result = new Matrix<byte>(h, w);
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (Shape.Contains(points, new(x + x0, y + y)))
                        {
                            var oldColor = pixels.GetPixel(x + x0, y + y0);
                            result[y, x] = (oldColor.Int32().GetBrightness() * 255).Double().Round().Coerce(255).Byte();
                        }
                        else result[y, x] = 0;
                    }
                }

                group.Add(new Brush("Untitled", result));
            }
        }, () => Find<LayersPanel>().SelectedLayer is PixelLayer && Get.Current<MainViewModel>().ActiveDocument is ImageDocument i && i.Selections.Count > 0);

        ICommand saveSelectionAsMatrixCommand;
        [Category(Category.Save)]
        [DisplayName("Save as matrix")]
        [Icon(App.ImagePath + "ConvertToMatrix.png")]
        [Tool, Visible]
        public ICommand SaveSelectionAsMatrixCommand => saveSelectionAsMatrixCommand ??= new RelayCommand(() =>
        {
            var group = Find<MatricesPanel>().SelectedGroup ?? (GroupCollection<Matrix>)XIList.FirstOrDefault(Get.Current<Options>().Matrices);
            if (group != null)
            {
                var selections = Get.Current<MainViewModel>().ActiveDocument.As<ImageDocument>().Selections;

                var points = selections[0].Points.Select(i => new Point(i.X.Round(), i.Y.Round())).ToArray<Point>();
                var bounds = Shape.GetBounds(points);

                var pixels = Find<LayersPanel>().SelectedLayer.As<PixelLayer>().Pixels;

                var h = bounds.Height.UInt32();
                var w = bounds.Width.UInt32();

                var x0 = bounds.X.Int32();
                var y0 = bounds.Y.Int32();

                var result = new DoubleMatrix(h, w);
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (Shape.Contains(points, new(x + x0, y + y)))
                        {
                            var oldColor = pixels.GetPixel(x + x0, y + y0);
                            result[y, x] = oldColor.Int32().GetBrightness();
                        }
                        else result[y, x] = 0;
                    }
                }

                group.Add(new("Untitled", result));
            }
        }, () => Find<LayersPanel>().SelectedLayer is PixelLayer && Get.Current<MainViewModel>().ActiveDocument is ImageDocument i && i.Selections.Count > 0);

        ICommand saveAsPathCommand;
        [Category(Category.Save)]
        [DisplayName("Save as path")]
        [Icon(App.ImagePath + "ConvertToPath.png")]
        [Tool, Visible]
        public ICommand SaveAsPathCommand => saveAsPathCommand ??= new RelayCommand(() =>
        {
        },
        () => SelectedItem != null);

        ICommand saveAsShapeCommand;
        [Category(Category.Save)]
        [DisplayName("Save as shape")]
        [Icon(App.ImagePath + "ConvertToShape.png")]
        [Tool, Visible]
        public ICommand SaveAsShapeCommand
            => saveAsShapeCommand ??= new RelayCommand(() => Find<ShapesPanel>().SelectedGroup.Add(new Shape(Name, SelectedItem.Path)), () => SelectedItem != null);

        #endregion
    }
}