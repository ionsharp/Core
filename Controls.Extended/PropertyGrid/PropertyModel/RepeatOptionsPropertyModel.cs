using Imagin.Common.Scheduling;

namespace Imagin.Controls.Extended
{
    public sealed class RepeatOptionsPropertyModel : PropertyModel
    {
        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? new RepeatOptions(Recurrence.None) : (RepeatOptions)NewValue, null);
        }

        public RepeatOptionsPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
