using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Colors;
using Imagin.Common.Models;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    [Serializable]
    public class ColorDocument : Document
    {
        public static readonly System.Windows.Media.Color DefaultOldColor = System.Windows.Media.Colors.Black;

        public static readonly System.Windows.Media.Color DefaultNewColor = System.Windows.Media.Colors.White;

        public const ColorModels DefaultModels = ColorModels.HSB | ColorModels.LABh;

        public const ColorModels DefaultSelectedModel = ColorModels.HSB;

        [field: NonSerialized]
        public event DefaultEventHandler<System.Windows.Media.Color> ColorSaved;

        //...

        byte alpha = 255;
        public byte Alpha
        {
            get => alpha;
            set => this.Change(ref alpha, value);
        }

        [field: NonSerialized]
        ObservableColor color = null;
        public ObservableColor Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        public override object Icon => Color.ActualColor;

        ColorModels models = DefaultModels;
        public ColorModels Models
        {
            get => models;
            set
            {
                this.Change(ref models, value);
                Color.If(i => i.Model = value);
            }
        }

        StringColor oldColor = DefaultOldColor;
        public System.Windows.Media.Color OldColor
        {
            get => oldColor.Value;
            set => this.Change(ref oldColor, new StringColor(value));
        }

        public override string Title => $"#{Color.ActualColor.Hexadecimal()}";

        public override object ToolTip => Color.ActualColor;

        //...

        /// <summary>
        /// Alpha <> NewColor <> ObservableColor
        /// </summary>
        public ColorDocument() : this(DefaultNewColor, DefaultModels, DefaultSelectedModel) { }

        public ColorDocument(System.Windows.Media.Color color, ColorModels models = DefaultModels, ColorModels selectedModel = DefaultSelectedModel) : base()
        {
            Color = new(selectedModel, color);
            Color.ActualColor = color; Models = models;
        }

        //...

        public override void Subscribe()
        {
            base.Subscribe();
            Color.PropertyChanged += OnColorChanged;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            Color.PropertyChanged -= OnColorChanged;
        }

        public override void Save() { }

        //...

        void OnColorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.Changed(() => Color);
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Alpha):
                    Color.If(i => i.ActualColor = Color.ActualColor.A(Alpha));
                    break;

                case nameof(Color):
                    this.Changed(() => Title);
                    this.Changed(() => ToolTip);
                    break;
            }
        }

        //...

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext input) { }

        //...

        [field: NonSerialized]
        ICommand revertCommand;
        [DisplayName("Revert")]
        [Hidden(false)]
        [Index(2)]
        public ICommand RevertCommand => revertCommand ??= new RelayCommand(() =>
        {
            var oldColor = OldColor;
            OldColor = Color.ActualColor;
            Color.ActualColor = oldColor;
        },
        () => true);

        [field: NonSerialized]
        ICommand saveCommand;
        [DisplayName("Save")]
        [Hidden(false)]
        [Index(3)]
        new public ICommand SaveCommand => saveCommand ??= new RelayCommand(() => OldColor = Color.ActualColor, () => true);

        [field: NonSerialized]
        ICommand saveColorCommand;
        [DisplayName("SaveColor")]
        [Hidden(false)]
        [Index(3)]
        public ICommand SaveColorCommand => saveColorCommand ??= new RelayCommand(() => ColorSaved?.Invoke(this, new(Color.ActualColor)), () => true);
    }
}