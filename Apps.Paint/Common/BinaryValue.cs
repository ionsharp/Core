using Imagin.Common;
using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    /*
    [Serializable]
    class WritableAdjustment
    {
        readonly string name;
        readonly List<string> names = new();
        readonly List<object> values = new();

        WritableAdjustment() : base() { }

        public WritableAdjustment(ImageEffect input) : this()
        {
            name = input.GetType().FullName;
            foreach (var i in input.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (i.DeclaringType == input.GetType() || i.DeclaringType == typeof(ImageEffect))
                {
                    if (i.GetGetMethod()?.IsPublic == true)
                    {
                        if (i.GetSetMethod()?.IsPublic == true)
                        {
                            if (i.PropertyType.HasAttribute<SerializableAttribute>())
                            {
                                names.Add(i.Name);
                                values.Add(i.GetValue(input));
                            }
                        }
                    }
                }
            }
        }

        public ImageEffect Convert()
        {
            var result = Assembly.GetEntryAssembly().GetType(name).Create<ImageEffect>();

            var j = 0;
            foreach (var i in names)
            {
                var property = result.GetType().GetProperty(i);
                property.SetValue(result, values[j]);
                j++;
            }

            return result;
        }
    }
    */

    public class WriteableBitmapToArgbMatrixConverter : Common.Converters.Converter<WriteableBitmap, Matrix<Argb>>
    {
        protected override ConverterValue<WriteableBitmap> ConvertBack(ConverterData<Matrix<Argb>> input)
            => input.Value.Convert();

        protected override ConverterValue<Matrix<Argb>> ConvertTo(ConverterData<WriteableBitmap> input)
            => input.Value.Convert();
    }

    [Serializable]
    public class BinaryValue<TA, TB, TConverter> : Base where TConverter : Common.Converters.Converter<TA, TB>
    {
        TConverter converter = default;

        TB savedValue = default;

        [NonSerialized]
        TA value;
        public TA Value
        {
            get => value;
            set => this.Change(ref this.value, value);
        }

        public BinaryValue() : base() { }

        public BinaryValue(TA value) : base() => Value = value;

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            converter ??= typeof(TConverter).Create<TConverter>();
            savedValue = (TB)converter.Convert(Value);
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            converter ??= typeof(TConverter).Create<TConverter>();
            Value = (TA)converter.ConvertBack(savedValue);
        }
    }
}