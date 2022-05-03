using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [DisplayName("Crop")]
    [Icon(App.ImagePath + "Crop.png")]
    [Serializable]
    public class CropTool : Tool
    {
        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Crop.png");

        StringColor background = new StringColor(new Hexadecimal("AA000000").Color());
        public virtual StringColor Background
        {
            get => background;
            set => this.Change(ref background, value);
        }

        ICommand cropCommand;
        [DisplayName("Crop")]
        public ICommand CropCommand => cropCommand ??= new RelayCommand(() =>
        {
            if (Document is ImageDocument i)
            {
                i.Crop(new(i.CropX, i.CropY, i.CropWidth, i.CropHeight));
                i.CropX = 0; i.CropY = 0;
                i.CropHeight = Document.Height; i.CropWidth = Document.Width;
            }
        },
        () => Document is ImageDocument);

        ICommand resetCommand;
        [DisplayName("Reset")]
        public ICommand ResetCommand => resetCommand ??= new RelayCommand(() =>
        {
            if (Document is ImageDocument i)
            {
                i.CropX = 0; i.CropY = 0;
                i.CropHeight = Document.Height; i.CropWidth = Document.Width;
            }
        },
        () => Document is ImageDocument);
    }
}