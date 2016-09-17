using Imagin.Common;
using Imagin.Common.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public abstract class ColorSpaceModel : NamedObject
    {
        ComponentCollection components = null;
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
        
        public abstract Color GetColor();

        public ColorSpaceModel() : base()
        {
            this.Components = new ComponentCollection();
        }
    }
}
