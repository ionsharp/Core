using System;

namespace Imagin.Controls.Common
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
        /// <param name="Value"></param>
        protected override void OnValueChanged(object Value)
        {
            //Do nothing! :-)
        }
    }
}
