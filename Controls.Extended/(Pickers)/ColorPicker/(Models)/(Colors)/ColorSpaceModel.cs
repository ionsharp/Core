using Imagin.Common;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ColorSpaceModel : NamedObject
    {
        ComponentCollection components = new ComponentCollection();
        /// <summary>
        /// 
        /// </summary>
        public ComponentCollection Components
        {
            get
            {
                return this.components;
            }
            set
            {
                this.components = value;
                this.OnPropertyChanged("Components");
            }
        }

        Orientation orientation = Orientation.Vertical;
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
                this.OnPropertyChanged("Orientation");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Color GetColor();

        /// <summary>
        /// 
        /// </summary>
        public ColorSpaceModel() : base()
        {
        }
    }
}
