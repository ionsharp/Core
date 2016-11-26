namespace Imagin.Controls.Extended
{
    public abstract class NumericPropertyModel<T> : NumericPropertyModel
    {
        T maximum = default(T);
        public T Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                this.maximum = value;
                OnPropertyChanged("Maximum");
            }
        }

        T minimum = default(T);
        public T Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                this.minimum = value;
                OnPropertyChanged("Minimum");
            }
        }

        internal override void SetConstraint(object Minimum, object Maximum)
        {
            this.Maximum = (T)Maximum;
            this.Minimum = (T)Minimum;
        }

        public NumericPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
