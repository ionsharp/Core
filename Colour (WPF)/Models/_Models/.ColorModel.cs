using Imagin.Colour.Controls.Collections;
using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ColorModel : NamedObject, IChangeable
    {
        /// <summary>
        /// 
        /// </summary>
        public event ChangedEventHandler Changed;

        /// <summary>
        /// 
        /// </summary>
        public event SelectedEventHandler Selected;

        readonly ComponentCollection _components = new ComponentCollection();
        /// <summary>
        /// 
        /// </summary>
        public ComponentCollection Components => _components;

        /// <summary>
        /// 
        /// </summary>
        public virtual Orientation Orientation => Orientation.Vertical;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Color GetColor();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="components"></param>
        public ColorModel(params Component[] components) : base()
        {
            _components.ItemAdded += OnComponentAdded;
            components.ForEach(i => _components.Add(i));
        }

        void OnComponentAdded(object sender, EventArgs<Component> e)
        {
            e.Value.ColorSpace = this;

            e.Value.ValueChanged -= OnValueChanged;
            e.Value.ValueChanged += OnValueChanged;

            if (e.Value is ISelectable)
            {
                e.Value.As<ISelectable>().Selected -= OnSelected;
                e.Value.As<ISelectable>().Selected += OnSelected;
            }
        }

        void OnSelected(object sender, SelectedEventArgs e) => Selected?.Invoke(this, new SelectedEventArgs(e.Value));

        void OnValueChanged(object sender, EventArgs<double> e) => Changed?.Invoke(this, new EventArgs());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ColorModel New(ColorModels value)
        {
            switch (value)
            {
                case ColorModels.CMYK:
                    return new CMYKViewModel();
                case ColorModels.HCG:
                    return new HCGViewModel();
                case ColorModels.HSB:
                    return new HSBViewModel();
                case ColorModels.HSI:
                    return new HSIViewModel();
                case ColorModels.HSL:
                    return new HSLViewModel();
                case ColorModels.HSM:
                    return new HSMViewModel();
                case ColorModels.HSP:
                    return new HSPViewModel();
                case ColorModels.HunterLab:
                    return new HunterLabViewModel();
                case ColorModels.HWB:
                    return new HWBViewModel();
                case ColorModels.Lab:
                    return new LabViewModel();
                case ColorModels.LChab:
                    return new LChabViewModel();
                case ColorModels.LChuv:
                    return new LChuvViewModel();
                case ColorModels.LMS:
                    return new LMSViewModel();
                case ColorModels.Luv:
                    return new LuvViewModel();
                case ColorModels.RGB:
                    return new RGBViewModel();
                case ColorModels.TSL:
                    return new TSLViewModel();
                case ColorModels.xyY:
                    return new xyYViewModel();
                case ColorModels.XYZ:
                    return new XYZViewModel();
                case ColorModels.YCoCg:
                    return new YCoCgViewModel();
                case ColorModels.YES:
                    return new YESViewModel();
                case ColorModels.YIQ:
                    return new YIQViewModel();
                case ColorModels.YUV:
                    return new YUVViewModel();
            }
            return default(ColorModel);
        }
    }
}
