using System.Windows;

namespace Imagin.Controls.Extended
{
    public sealed class SizePropertyModel : PropertyModel
    {
        bool SizeChangeHandled = false;

        bool ValueChangeHandled = false;

        double width = 0.0;
        public double Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
                OnPropertyChanged("Width");

                this.OnSizeChanged(value, false);
            }
        }

        double height = 0.0;
        public double Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                OnPropertyChanged("Height");

                this.OnSizeChanged(value, true);
            }
        }

        bool isBound = false;
        public bool IsBound
        {
            get
            {
                return this.isBound;
            }
            set
            {
                this.isBound = value;
                OnPropertyChanged("IsBound");
                if (value) this.Height = this.Width;
            }
        }

        void OnSizeChanged(double Value, bool WidthOrHeight)
        {
            if (this.IsBound)
            {
                if (SizeChangeHandled) return;

                this.SizeChangeHandled = true;
                if (WidthOrHeight)
                    this.Width = Value;
                else this.Height = Value;
                this.SizeChangeHandled = false;
            }

            this.ValueChangeHandled = true;
            this.Value = new Size(this.Width, this.Height);
            this.ValueChangeHandled = false;
        }

        protected override void OnValueChanged(object NewValue)
        {
            if (ValueChangeHandled) return;

            this.Height = ((Size)NewValue).Height;
            this.Width = ((Size)NewValue).Width;
        }

        public SizePropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
