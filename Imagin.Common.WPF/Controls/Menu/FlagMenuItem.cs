using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Controls
{
    public class FlagMenuItem : EnumMenuItem
    {
        public FlagMenuItem() : base() { }

        protected override bool Compare(Enum a, Enum b) => a?.HasFlag(b) == true;

        protected override void OnChecked(BaseCheckable<Enum> input)
        {
            handle.SafeInvoke(() =>
            {
                if (!Value.HasFlag(input.Value))
                    SetCurrentValue(ValueProperty, Value.AddFlag(input.Value));
            });
        }

        protected override void OnValueChanged(Value<Enum> input)
            => handle.SafeInvoke(() => items.ForEach(i => i.IsChecked = input.New.HasFlag(i.Value)));

        protected override void Subscribe(BaseCheckable<Enum> input)
        {
            base.Subscribe(input);
            input.Unchecked += OnUnchecked;
        }

        protected override void Unsubscribe(BaseCheckable<Enum> input)
        {
            base.Unsubscribe(input);
            input.Unchecked -= OnUnchecked;
        }

        //...

        void OnUnchecked(object sender, EventArgs e)
        {
            handle.SafeInvoke(() =>
            {
                if (sender is BaseCheckable<Enum> i)
                {
                    if (Value.HasFlag(i.Value))
                        SetCurrentValue(ValueProperty, Value.RemoveFlag(i.Value));
                }
            });
        }
    }
}