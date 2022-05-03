using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [Icon(App.ImagePath + "Brush.png")]
    [Serializable]
    public class Brush : BaseNamable, ICloneable //, IEquatable<Brush>
    {
        //... Properties

        double angle = 0;
        [Range(0.0, 359.0, 1.0)]
        [Style(DoubleStyle.Angle)]
        public virtual double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        Matrix<byte> bytes = null;
        [Hidden]
        public Matrix<byte> Bytes
        {
            get => bytes;
            set => this.Change(ref bytes, value);
        }

        [Featured]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        double noise = 0;
        [Format(Common.RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        public virtual double Noise
        {
            get => noise;
            set => this.Change(ref noise, value);
        }

        int size = 25;
        [Range(1, 3000, 1)]
        [Format(Common.RangeFormat.Slider)]
        public virtual int Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        double xScale = 1;
        [Format(Common.RangeFormat.Both)]
        [Range(0.0, 10.0, 0.01)]
        public virtual double XScale
        {
            get => xScale;
            set => this.Change(ref xScale, value);
        }

        double yScale = 1;
        [Format(Common.RangeFormat.Both)]
        [Range(0.0, 10.0, 0.01)]
        public virtual double YScale
        {
            get => yScale;
            set => this.Change(ref yScale, value);
        }

        //... Brush

        public Brush() : this(string.Empty) { }

        public Brush(string name, Matrix<byte> bytes = null) : base(name)
        {
            Bytes = bytes;
        }

        public Brush(string name, Uri bytes) : this(name, bytes.GetImage().Bitmap(ImageExtensions.Png).WriteableBitmap()) { }

        public Brush(string name, WriteableBitmap bytes) : this(name, GetBytes(bytes)) { }

        //... Static

        public static Matrix<byte> GetBytes(ColorMatrix bitmap)
        {
            var result = new Matrix<byte>(bitmap.Rows, bitmap.Columns);
            for (uint x = 0; x < result.Columns; x++)
            {
                for (uint y = 0; y < result.Rows; y++)
                    result.SetValue(y, x, bitmap[y, x].A);
            }
            return result;
        }

        public static Matrix<byte> GetBytes(WriteableBitmap bitmap)
        {
            var result = new Matrix<byte>(bitmap.PixelHeight.UInt32(), bitmap.PixelWidth.UInt32());
            for (var y = 0; y < result.Rows; y++)
            {
                for (var x = 0; x < result.Columns; x++)
                    result.SetValue(y.UInt32(), x.UInt32(), bitmap.GetPixel(x, y).A);
            }
            return result;
        }

        //...

        async public static Task<Brush> New(string filePath)
        {
            var bitmap 
                = await ImageDocument.OpenAsync(filePath);
            var result = new Brush(System.IO.Path.GetFileNameWithoutExtension(filePath), GetBytes(bitmap))
                { Size = Math.Max(bitmap.PixelHeight, bitmap.PixelWidth) };
            return result;
        }

        public static WriteableBitmap Render(Matrix<byte> input, Color color)
        {
            var result = BitmapFactory.New(input.Columns.Int32(), input.Rows.Int32());
            for (uint x = 0; x < input.Columns; x++)
            {
                for (uint y = 0; y < input.Rows; y++)
                    result.SetPixel(x.Int32(), y.Int32(), color.A(input[y, x]));
            }
            return result;
        }

        //... Virtual

        public virtual Matrix<byte> GetBytes(int size) 
            => GetBytes(new ColorMatrix(Bytes.Transform(i => Color.FromArgb(i, 0, 0, 0))).Resize(new(size, size), Interpolations.Bilinear));

        //... Public

        object ICloneable.Clone() => Clone();
        public virtual Brush Clone() => new Brush()
        {
            Bytes = Bytes,
            Size = Size
        };
    }

    [Serializable]
    public abstract class HardBrush : Brush
    {
        double hardness = 1;
        [Range(0.0, 1.0, 0.01)]
        [Format(Common.RangeFormat.Both)]
        public virtual double Hardness
        {
            get => hardness;
            set => this.Change(ref hardness, value);
        }

        protected int GetRings(int columns, int rows)
        {
            int result = 0;
            while (columns > 1 && rows > 1)
            {
                result++;
                columns -= 1;
                rows -= 1;
            }
            return result;
        }
    }
}