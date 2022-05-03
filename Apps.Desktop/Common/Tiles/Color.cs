using Imagin.Common;
using Imagin.Common.Colors;
using System;
using System.Windows.Media;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Color")]
    [Serializable]
    public class ColorTile : Tile
    {
        ObservableColor color = new(ColorModels.HSB, Colors.White);
        [Hidden]
        public ObservableColor Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        public ColorTile() : base() { }
    }
}