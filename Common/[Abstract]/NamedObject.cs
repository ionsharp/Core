using Imagin.Common.Input;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// A named, abstract object.
    /// </summary>
    public class NamedObject : BindableObject, INamable
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<string>> NameChanged;

        /// <summary>
        /// 
        /// </summary>
        protected string name = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get => name;
            set
            {
                name = OnPreviewNameChanged(name, value);
                OnPropertyChanged("Name");
                OnNameChanged(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NamedObject() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public NamedObject(string name) : base()
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnNameChanged(string Value)
        {
            NameChanged?.Invoke(this, new EventArgs<string>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        protected virtual string OnPreviewNameChanged(string OldValue, string NewValue)
        {
            return NewValue;
        }
    }
}
