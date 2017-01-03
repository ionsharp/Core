using System;
using System.Windows.Media;
using System.Xml.Serialization;
using Imagin.Common.Extensions;

namespace Imagin.Common.Serialization
{
    [Serializable]
    public class WritableColor : AbstractObject
    {
        #region Properties

        string hex = "00000000";
        public string Hex
        {
            get
            {
                return hex;
            }
            set
            {
                hex = value.StartsWith("#") ? value.Substring(1) : value;
            }
        }

        #endregion

        #region WritableColor

        public WritableColor()
        {
        }

        public WritableColor(string hex)
        {
            Hex = hex;
        }

        public WritableColor(Color Color)
        {
            Hex = Color.ToHexWithAlpha();
        }

        public WritableColor(SolidColorBrush SolidColorBrush)
        {
            Hex = SolidColorBrush.Color.ToHexWithAlpha();
        }

        #endregion
    }
}
