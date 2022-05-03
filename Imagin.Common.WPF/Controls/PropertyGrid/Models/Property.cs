using Imagin.Common.Linq;
using System;
using System.Reflection;

namespace Imagin.Common.Controls
{
    public class PropertyModel : MemberModel
    {
        DependencyValue internalValue;

        public override bool CanWrite => Member.GetSetMethod(true) != null;

        new public PropertyInfo Member => (PropertyInfo)base.Member;

        public override Type Type => Member.PropertyType;

        public PropertyModel(MemberData data) : base(data) { }

        protected override void SetValue(object source, object value) => Member.SetValue(source, value, null);

        protected override object GetValue(object input) => Member.GetValue(input);

        public override void Subscribe()
        {
            base.Subscribe();
            return;
            internalValue = new();
            //internalValue.Bind(DependencyValue.ValueProperty, new PropertyPath("(0)", Property), Source.First);
            //internalValue.ValueChanged += OnValueChanged;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            if (internalValue != null)
            {
                //internalValue.ValueChanged -= OnValueChanged;
                internalValue.Unbind(DependencyValue.ValueProperty);
                internalValue = null;
            }
        }
    }
}