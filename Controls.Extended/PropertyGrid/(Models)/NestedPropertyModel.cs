using System;
using System.ComponentModel;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public class NestedPropertyModel : PropertyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public override Type Primitive
        {
            get
            {
                return typeof(object);
            }
        }

        internal NestedPropertyModel() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnHostPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Do nothing! :-)
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected override void OnValueChanged(object Value)
        {
            //Do nothing! :-)
        }
    }
}
