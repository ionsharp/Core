using Imagin.Common.Input;
using System;
using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="INamable"/>.
    /// </summary>
    public class NamedObject : ObjectBase, INamable
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<string>> NameChanged;

        /// <summary>
        /// 
        /// </summary>
        string _name = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get => _name;
            set
            {
                Property.Set(this, ref _name, OnPreviewNameChanged(_name, value));
                OnNameChanged(_name);
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
        public NamedObject(string name) : base() => Name = name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnNameChanged(string Value) => NameChanged?.Invoke(this, new EventArgs<string>(Value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        protected virtual string OnPreviewNameChanged(string OldValue, string NewValue) => NewValue;
    }
}
