using Imagin.Common.Text;

namespace Imagin.Controls.Extended
{
    public class StringPropertyModel : PropertyModel
    {
        StringRepresentation representation = StringRepresentation.Unspecified;
        public StringRepresentation Representation
        {
            get
            {
                return this.representation;
            }
            set
            {
                this.representation = value;
                OnPropertyChanged("Representation");
            }
        }

        protected override void OnValueChanged(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue == null ? string.Empty : NewValue.ToString(), null);
        }

        public StringPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
