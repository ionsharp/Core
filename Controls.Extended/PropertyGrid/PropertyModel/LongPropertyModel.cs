using System;
using Imagin.Common.Text;

namespace Imagin.Controls.Extended
{
    public sealed class LongPropertyModel : NumericPropertyModel<long>
    {
        Int64Representation int64Representation = Int64Representation.Default;
        public Int64Representation Int64Representation
        {
            get
            {
                return this.int64Representation;
            }
            set
            {
                this.int64Representation = value;
                OnPropertyChanged("Int64Representation");
            }
        }

        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? 0L : Convert.ToInt64(NewValue), null);
        }

        public LongPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
