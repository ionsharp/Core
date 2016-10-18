using System;
using System.Reflection;
using System.Collections.ObjectModel;
using Imagin.Common.Collections;

namespace Imagin.Controls.Extended
{
    public sealed class EnumPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object Value)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, Value, null);
        }

        public EnumPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
