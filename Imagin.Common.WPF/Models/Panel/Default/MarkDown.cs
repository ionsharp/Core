using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using Imagin.Common.Text;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Models
{
    [Serializable]
    public class MarkDownPanel : Panel
    {
        public static readonly ResourceKey TemplateKey = new();

        public const double FontScaleIncrement = 0.01;

        public const double FontScaleMaximum = 10;

        public const double FontScaleMinimum = 0.1;

        Bullets bulletOrdered = Bullets.NumberParenthesis;
        [DisplayName("Ordered")]
        [Tool]
        public Bullets BulletOrdered
        {
            get => bulletOrdered;
            set => this.Change(ref bulletOrdered, value);
        }

        Bullets bulletUnordered = Bullets.Square;
        [DisplayName("Unordered")]
        [Tool]
        public Bullets BulletUnordered
        {
            get => bulletUnordered;
            set => this.Change(ref bulletUnordered, value);
        }

        Encoding encoding = Encoding.Unicode;
        [Tool]
        public Encoding Encoding
        {
            get => encoding;
            set => this.Change(ref encoding, value);
        }

        FontFamily fontFamily = SystemFonts.MessageFontFamily;
        [Label(false)]
        [Tool]
        [Serialize(false)] //To do: Support (true)!
        public FontFamily FontFamily
        {
            get => fontFamily;
            set => this.Change(ref fontFamily, value);
        }

        double fontScale = 1;
        [Tool]
        [Range(FontScaleMinimum, FontScaleMaximum, FontScaleIncrement)]
        [Format(RangeFormat.Both)]
        [Width(double.NaN, 128)]
        public double FontScale
        {
            get => fontScale;
            set => this.Change(ref fontScale, value);
        }

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.MarkDown);

        string text = string.Empty;
        [Hidden]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        bool textWrap = true;
        [Label(false)]
        [Icon(Images.ArrowDownLeft)]
        [Index(int.MaxValue)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool TextWrap
        {
            get => textWrap;
            set => this.Change(ref textWrap, value);
        }

        [Hidden]
        public override string Title => "MarkDown";

        public MarkDownPanel() : base() { }

        bool Open(string filePath) 
            => Try.Invoke(() => Text = File.Long.ReadAllText(filePath, Encoding.Convert()), e => Analytics.Log.Write<MarkDownPanel>($"filePath = {filePath}; {e.Message}"));

        bool Save(string filePath) 
            => Try.Invoke(() => File.Long.WriteAllText(filePath, Text, Encoding.Convert()), e => Analytics.Log.Write<MarkDownPanel>(e));

        ICommand openCommand;
        [DisplayName("Open")]
        [Icon(Images.Open)]
        [Index(int.MinValue)]
        [Tool]
        public ICommand OpenCommand => openCommand ??= new RelayCommand(() =>
        {
            if (StorageWindow.Show(out string filePath, "Open...", StorageWindowModes.OpenFile, Array<string>.New("md")))
                Open(filePath);
        },
        () => true);

        ICommand saveCommand;
        [DisplayName("Save")]
        [Icon(Images.Save)]
        [Index(int.MinValue)]
        [Tool]
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(() =>
        {
            if (StorageWindow.Show(out string filePath, "Save...", StorageWindowModes.SaveFile, Array<string>.New("md")))
                Save(filePath);
        },
        () => true);
    }
}