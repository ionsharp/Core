using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyEnabledEventArgs : EventArgs
    {
        readonly string propertyName;
        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get
            {
                return propertyName;
            }
        }

        readonly bool isEnabled;
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="IsEnabled"></param>
        public PropertyEnabledEventArgs(string PropertyName, bool IsEnabled) : base()
        {
            propertyName = PropertyName;
            isEnabled = IsEnabled;
        }
    }
}
