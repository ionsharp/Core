using Imagin.Common.Text;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class BoolPropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? false : (bool)NewValue;
        }

        public BoolPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class BytePropertyModel : NumericPropertyModel<byte>
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? (byte)0 : (byte)NewValue;
        }

        public BytePropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class CollectionPropertyModel : PropertyModel
    {
        public CollectionPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class DateTimePropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? default(DateTime) : (DateTime)NewValue;
        }

        public DateTimePropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class DecimalPropertyModel : NumericPropertyModel<decimal>
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? 0m : (decimal)NewValue;
        }

        public DecimalPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class DoublePropertyModel : NumericPropertyModel<double>
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? 0d : Convert.ToDouble(NewValue);
        }

        public DoublePropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class EnumPropertyModel : PropertyModel
    {
        public EnumPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class GuidPropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? default(Guid) : (Guid)NewValue;
        }

        public GuidPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class IntPropertyModel : NumericPropertyModel<int>
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? 0 : Convert.ToInt32(NewValue);
        }

        public IntPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class LinearGradientPropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? default(LinearGradientBrush) : (LinearGradientBrush)NewValue;
        }

        public LinearGradientPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

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

        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? 0L : Convert.ToInt64(NewValue);
        }

        public LongPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class NetworkCredentialPropertyModel : PropertyModel
    {
        public NetworkCredentialPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public abstract class NumericPropertyModel : PropertyModel
    {
        internal abstract void SetConstraint(object Minimum, object Maximum);

        public NumericPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

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

    public sealed class ShortPropertyModel : NumericPropertyModel<short>
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? (short)0 : (short)NewValue;
        }

        public ShortPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class SizePropertyModel : PropertyModel
    {
        double width = 0.0;
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");

                OnSizeChanged(value, false);
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
            if (IsBound)
            {
                if (WidthOrHeight)
                    Width = Value;
                else Height = Value;
            }

            this.Value = new Size(Width, Height);
        }

        public SizePropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class SolidColorBrushPropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? default(SolidColorBrush) : (SolidColorBrush)NewValue;
        }

        public SolidColorBrushPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

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

        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? string.Empty : NewValue.ToString();
        }

        public StringPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class UriPropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? new Uri(string.Empty) : (Uri)NewValue;
        }

        public UriPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }

    public sealed class VersionPropertyModel : PropertyModel
    {
        protected override object OnPreviewValueChanged(object NewValue)
        {
            return NewValue == null ? new Version() : (Version)NewValue;
        }

        public VersionPropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base(Name, Value, Category, Description, IsReadOnly, IsFeatured)
        {
        }
    }
}
