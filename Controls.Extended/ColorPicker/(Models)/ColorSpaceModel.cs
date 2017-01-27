using Imagin.Common;
using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ColorSpaceModel : NamedObject 
    {
        /// <summary>
        /// 
        /// </summary>
        public event SelectedEventHandler Selected;

        ComponentCollection components = new ComponentCollection();
        /// <summary>
        /// 
        /// </summary>
        public ComponentCollection Components
        {
            get
            {
                return components;
            }
            set
            {
                components = value;
                OnPropertyChanged("Components");
            }
        }

        Illuminant illuminant = Illuminant.Default;
        /// <summary>
        /// 
        /// </summary>
        public Illuminant Illuminant
        {
            get
            {
                return illuminant;
            }
            set
            {
                illuminant = value;
                OnPropertyChanged("Illuminant");
            }
        }

        ObserverAngle observer = ObserverAngle.Two;
        /// <summary>
        /// 
        /// </summary>
        public ObserverAngle Observer
        {
            get
            {
                return observer;
            }
            set
            {
                observer = value;
                OnPropertyChanged("Observer");
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
                return orientation;
            }
            set
            {
                orientation = value;
                OnPropertyChanged("Orientation");
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
            Components.ItemAdded += OnComponentAdded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnComponentAdded(object sender, EventArgs<ComponentModel> e)
        {
            if (e.Value is ISelectable)
                e.Value.As<ISelectable>().Selected += OnSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelected(SelectedEventArgs e)
        {
            Selected?.Invoke(e);
        }
    }
}
