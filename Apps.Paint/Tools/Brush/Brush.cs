using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Brush")]
    [Icon(App.ImagePath + "Brush.png")]
    [Serializable]
    public class BrushTool : BlendTool
    {
        enum Category { Brush, Export, Import }

        #region Properties

        protected Matrix<byte> cacheBytes;

        //...

        [Hidden]
        public override Cursor Cursor => Cursors.None;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Brush.png");

        [Hidden]
        new protected PixelLayer TargetLayer => base.TargetLayer as PixelLayer;

        Brush brush = new CircleBrush();
        [Category(Category.Brush)]
        [Index(0)]
        public virtual Brush Brush
        {
            get => (brush ??= new CircleBrush());
            set => this.Change(ref brush, value);
        }

        int selectedGroupIndex = 0;
        [Hidden]
        public int SelectedGroupIndex
        {
            get => selectedGroupIndex;
            set => this.Change(ref selectedGroupIndex, value);
        }

        [Category(Category.Brush)]
        [Format(Common.RangeFormat.UpDown)]
        [Index(2)]
        [Range(1, 3000, 1)]
        [Width(64)]
        public int Size
        {
            get => brush?.Size ?? 1;
            set => brush.If(i => i.Size = value);
        }

        #endregion

        #region BrushTool

        public BrushTool() : base() { }

        #endregion

        #region Methods

        public override bool OnMouseDown(Point point)
        {
            if (base.OnMouseDown(point))
            {
                cacheBytes = Brush.GetBytes(Brush.Size);
                OnStarted();
                
                var x = (point.X + TargetLayer.X.Absolute()) / Document.Zoom;
                var y = (point.Y + TargetLayer.Y.Absolute()) / Document.Zoom;
                Draw(new Vector2<int>(x.Int32(), y.Int32()), Get.Current<Options>().ForegroundColor);
                return true;
            }
            return false;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var x = ((point.X + TargetLayer.X.Absolute()) - Brush.Size / 2) / Document.Zoom;
                var y = ((point.Y + TargetLayer.Y.Absolute()) - Brush.Size / 2) / Document.Zoom;
                Draw(new Vector2<int>(x.Int32(), y.Int32()), Get.Current<Options>().ForegroundColor);
            }
        }

        //...

        protected override bool AssertLayer() 
            => AssertLayer<PixelLayer>();

        protected virtual void Draw(Vector2<int> point, Color color) 
            => TargetLayer.Pixels.Blend(cacheBytes, point, color, Mode, Opacity);

        protected virtual void OnStarted() { }

        #endregion

        #region Commands

        ICommand exportCommand;
        [Category(Category.Export)]
        [DisplayName("Export")]
        [Icon(Images.Export)]
        [Index(0)]
        public virtual ICommand ExportCommand 
            => exportCommand ??= new RelayCommand(() => _ = Get.Current<Options>().Brushes.Export(), () => true);

        ICommand exportAllCommand;
        [Category(Category.Export)]
        [DisplayName("ExportAll")]
        [Icon(Images.ExportAll)]
        [Index(1)]
        public virtual ICommand ExportAllCommand 
            => exportAllCommand ??= new RelayCommand(() => Get.Current<Options>().Brushes.ExportAllCommand.Execute(), () => true);

        ICommand importCommand;
        [Category(Category.Import)]
        [DisplayName("Import")]
        [Icon(Images.Import)]
        [Index(0)]
        public virtual ICommand ImportCommand 
            => importCommand ??= new RelayCommand(() => Get.Current<Options>().Brushes.Import(), () => true);

        ICommand importImageCommand;
        [Category(Category.Import)]
        [DisplayName("ImportImage")]
        [Icon(Images.ImportImage)]
        [Index(1)]
        public virtual ICommand ImportImageCommand => importImageCommand ??= new RelayCommand(async () =>
        {
            string[] paths = null;
            if (StorageWindow.Show(out paths, "From image", StorageWindowModes.OpenFile, ImageFormats.Readable.Select(j => j.Extension), null, StorageWindowFilterModes.Alphabetical))
            {
                foreach (var i in paths)
                {
                    var result = await Brush.New(i);
                    Get.Current<Options>().Brushes.First().Add(result);
                }
            }
        },
        () => true);

        #endregion
    }
}