using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Reflection;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Colors
{
    /// <summary>
    /// A normalized [0, 1] color with three components.
    /// </summary>
    public class ObservableColor : Base
    {
        enum Category { Value }

        readonly Handle handle = false;

        readonly Action<Color> OnChanged;

        //...

        Color actualColor = System.Windows.Media.Colors.White;
        [Format(ColorFormat.TextBox), DisplayName("Color")]
        [Featured, Label(false), Visible]
        public Color ActualColor
        {
            get => actualColor;
            set => this.Change(ref actualColor, value);
        }

        //...

        Components component = Components.A;
        [Index(1), Label(false), Tool]
        public Components Component
        {
            get => component;
            set => this.Change(ref component, value);
        }

        ColorModels model = ColorModels.HSB;
        [Index(0), Label(false), Localize(false), Tool]
        public ColorModels Model
        {
            get => model;
            set => this.Change(ref model, value);
        }

        ColorProfiles profile = ColorProfiles.sRGB;
        [Index(-1), Label(false), Localize(false), Tool]
        public ColorProfiles Profile
        {
            get => profile;
            set => this.Change(ref profile, value);
        }

        //...

        [Category(Category.Value), PropertyTrigger(nameof(Controls.MemberModel.DisplayName), nameof(NameX))]
        [Index(0), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus), Visible]
        public string DisplayX
        {
            get => GetDisplayValue(0);
            set => SetDisplayValue(value, 0);
        }

        [Category(Category.Value), PropertyTrigger(nameof(Controls.MemberModel.DisplayName), nameof(NameY))]
        [Index(1), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus), Visible]
        public string DisplayY
        {
            get => GetDisplayValue(1);
            set => SetDisplayValue(value, 1);
        }

        [Category(Category.Value), PropertyTrigger(nameof(Controls.MemberModel.DisplayName), nameof(NameZ))]
        [Index(2), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus), Visible]
        public string DisplayZ
        {
            get => GetDisplayValue(2);
            set => SetDisplayValue(value, 2);
        }

        //...

        [Hidden]
        public string NameX => $"({Model.GetComponent(0).Symbol}) {Model.GetComponent(0).Name}";

        [Hidden]
        public string NameY => $"({Model.GetComponent(1).Symbol}) {Model.GetComponent(1).Name}";

        [Hidden]
        public string NameZ => $"({Model.GetComponent(2).Symbol}) {Model.GetComponent(2).Name}";

        //...

        [Hidden]
        public Vector Maximum => new(Model.GetComponents()[0].Maximum, Model.GetComponents()[1].Maximum, Model.GetComponents()[2].Maximum);

        [Hidden]
        public Vector Minimum => new(Model.GetComponents()[0].Minimum, Model.GetComponents()[1].Minimum, Model.GetComponents()[2].Minimum);

        [Hidden]
        public Vector Value => new(x, y, z);

        //...

        One x = default;
        [Hidden]
        public One X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        One y = default;
        [Hidden]
        public One Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        One z = default;
        [Hidden]
        public One Z
        {
            get => z;
            set => this.Change(ref z, value);
        }

        //...

        public ObservableColor(ColorModels model, Color defaultColor, Action<Color> onChanged = null) : base()
        {
            Model = model; ActualColor = defaultColor; OnChanged = onChanged;
        }

        //...

        string GetDisplayValue(int index)
        {
            One result = default;
            switch (Component)
            {
                case Components.A:
                    var a = new One[] { z, x, y };
                    result = a[index];
                    break;
                case Components.B:
                    var b = new One[] { x, z, y };
                    result = b[index];
                    break;
                case Components.C:
                    var c = new One[] { x, y, z };
                    result = c[index];
                    break;
            }

            var aRange = new DoubleRange(0, 1);
            var bRange = new DoubleRange(Minimum[index], Maximum[index]);

            if (Model == ColorModels.RGB || Model == ColorModels.XYZ)
            {

            }

            return aRange.Convert(bRange.Minimum, bRange.Maximum, result).Round(2).ToString();
        }

        void SetDisplayValue(string input, int index)
        {
            var aRange = new DoubleRange(0, 1);
            var bRange = new DoubleRange(Minimum[index], Maximum[index]);

            if (Model == ColorModels.RGB || Model == ColorModels.XYZ)
            {

            }

            var result = (One)bRange.Convert(aRange.Minimum, aRange.Maximum, input?.Double() ?? 0);
            switch (Component)
            {
                case Components.A:
                    switch (index)
                    {
                        case 0: Z = result; break;
                        case 1: X = result; break;
                        case 2: Y = result; break;
                    }
                    break;
                case Components.B:
                    switch (index)
                    {
                        case 0: X = result; break;
                        case 1: Z = result; break;
                        case 2: Y = result; break;
                    }
                    break;
                case Components.C:
                    switch (index)
                    {
                        case 0: X = result; break;
                        case 1: Y = result; break;
                        case 2: Z = result; break;
                    }
                    break;
            }

            switch (index)
            {
                case 0: this.Changed(() => DisplayX); break;
                case 1: this.Changed(() => DisplayY); break;
                case 2: this.Changed(() => DisplayZ); break;
            }
        }

        //... ActualColor <Binding> [X, Y, Z]

        /// <summary>
        /// Converts from <see cref="Model"/> to <see cref="RGB"/> based on <see cref="Component"/>.
        /// </summary>
        void ConvertTo()
        {
            handle.SafeInvoke(() =>
            {
                Vector3<double> result = null;
                if (Component == Components.A)
                    result = new(z, x, y);

                if (Component == Components.B)
                    result = new(x, z, y);

                if (Component == Components.C)
                    result = new(x, y, z);

                var xyz = ColorVector.Create(Model, result);
                ActualColor = xyz.Convert();
            });
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="Model"/> based on <see cref="Component"/>.
        /// </summary>
        void ConvertFrom()
        {
            handle.SafeInvoke(() =>
            {
                var rgb = new RGB(ActualColor);

                var result = ColorVector.Create(Model, rgb).Value;
                var xyz = new Vector3<double>(result[0] / 255, result[1] / 255, result[2] / 255);

                switch (Component)
                {
                    case Components.A: X = xyz.Y; Y = xyz.Z; Z = xyz.X; break;
                    case Components.B: X = xyz.X; Y = xyz.Z; Z = xyz.Y; break;
                    case Components.C: X = xyz.X; Y = xyz.Y; Z = xyz.Z; break;
                }
            });
        }

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Component):
                case nameof(Model):
                    this.Changed(() => DisplayX);
                    this.Changed(() => DisplayY);
                    this.Changed(() => DisplayZ);

                    this.Changed(() => NameX);
                    this.Changed(() => NameY);
                    this.Changed(() => NameZ);
                    break;

                case nameof(X):
                    switch (Component)
                    {
                        case Components.A: this.Changed(() => DisplayY); break;
                        case Components.B: this.Changed(() => DisplayX); break;
                        case Components.C: this.Changed(() => DisplayX); break;
                    }
                    ConvertTo();
                    break;

                case nameof(Y):
                    switch (Component)
                    {
                        case Components.A: this.Changed(() => DisplayZ); break;
                        case Components.B: this.Changed(() => DisplayZ); break;
                        case Components.C: this.Changed(() => DisplayY); break;
                    }
                    ConvertTo();
                    break;

                case nameof(Z):
                    switch (Component)
                    {
                        case Components.A: this.Changed(() => DisplayX); break;
                        case Components.B: this.Changed(() => DisplayY); break;
                        case Components.C: this.Changed(() => DisplayZ); break;
                    }
                    ConvertTo();
                    break;

                case nameof(ActualColor):
                    OnChanged?.Invoke(ActualColor);
                    ConvertFrom();
                    break;
            }
        }
    }
}