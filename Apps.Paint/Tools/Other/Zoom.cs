using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [DisplayName("Zoom")]
    [Icon(Images.Zoom)]
    [Serializable]
    public class ZoomTool : Tool
    {
        #region Properties

        enum Category { Commands }

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Zoom);

        double initialValue = 0.0;

        double increment = 0.1;
        [Index(2)]
        public double Increment
        {
            get => increment;
            set => this.Change(ref increment, value);
        }

        bool scrubby = true;
        [Index(3)]
        public bool Scrubby
        {
            get => scrubby;
            set => this.Change(ref scrubby, value);
        }

        bool zoomIn = true;
        [Index(0)]
        public bool ZoomIn
        {
            get => zoomIn;
            set
            {
                this.Change(ref zoomIn, value);

                zoomOut = !value;
                this.Changed(() => ZoomOut);
            }
        }

        bool zoomOut = false;
        [Index(1)]
        public bool ZoomOut
        {
            get => zoomOut;
            set
            {
                this.Change(ref zoomOut, value);

                zoomIn = !value;
                this.Changed(() => ZoomIn);
            }
        }

        #endregion

        #region Methods

        void Do()
        {
            if (ZoomIn)
            {
                Document.Zoom += Increment;
            }
            else if (ZoomOut)
                Document.Zoom -= Increment;
        }

        public override void OnMouseDoubleClick(Point point)
        {
            base.OnMouseDoubleClick(point);
            if (!Scrubby)
                Do();
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            if (!Scrubby)
            {
                Do();
            }
            else initialValue = Document.Zoom;
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (Scrubby)
            {
                if (MouseDown != null)
                {
                    var m = 0.01;
                    var d = MouseDownAbsolute.Value.X.NearestFactor(1) - MouseMoveAbsolute.Value.X.NearestFactor(1);
                    var i = d.Absolute() * m;

                    var result = initialValue;
                    if (d < 0)
                    {
                        result = initialValue + i;
                    }
                    else if (d > 0)
                        result = initialValue - i;

                    Document.Zoom = result.Coerce(50, 0.05);
                }
            }
        }

        #endregion

        #region Commands

        [field: NonSerialized]
        ICommand oneHundredPercentCommand;
        [Category(Category.Commands)]
        [DisplayName("100%")]
        [Icon(App.ImagePath + "Zoom100.png")]
        public ICommand OneHundredPercentCommand 
            => oneHundredPercentCommand ??= new RelayCommand(() => Document.Zoom = 1, () => Document != null && Document.Zoom != 1);

        [field: NonSerialized]
        ICommand fitScreenCommand;
        [Category(Category.Commands)]
        [DisplayName("FitScreen")]
        [Icon(App.ImagePath + "ZoomFit.png")]
        public ICommand FitScreenCommand 
            => fitScreenCommand ??= new RelayCommand(() => Document.FitScreen(), () => Document != null);

        [field: NonSerialized]
        ICommand fillScreenCommand;
        [Category(Category.Commands)]
        [DisplayName("FillScreen")]
        [Icon(App.ImagePath + "ZoomFill.png")]
        public ICommand FillScreenCommand 
            => fillScreenCommand ??= new RelayCommand(() => Document.FillScreen(), () => Document != null);

        #endregion
    }
}