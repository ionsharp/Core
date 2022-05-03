using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [DisplayName("Text")]
    [Icon(Images.Rename)]
    [Serializable]
    public class TextTool : Tool
    {
        Int32Region region;

        Shape selection;

        //...

        [Hidden]
        public override Cursor Cursor => Cursors.Cross;

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Rename);

        SolidColorBrush fontColor = Brushes.Black;
        public SolidColorBrush FontColor
        {
            get => fontColor;
            set => this.Change(ref fontColor, value);
        }

        FontFamily fontFamily = new("Arial");
        public FontFamily FontFamily
        {
            get => fontFamily;
            set => this.Change(ref fontFamily, value);
        }

        double fontSize = 12.0;
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
        public System.Drawing.FontStyle FontStyle
        {
            get => fontStyle;
            set => this.Change(ref fontStyle, value);
        }

        public ObservableCollection<System.Drawing.FontStyle> FontStyles => new()
        {
            System.Drawing.FontStyle.Bold,
            System.Drawing.FontStyle.Italic,
            System.Drawing.FontStyle.Regular,
            System.Drawing.FontStyle.Strikeout,
            System.Drawing.FontStyle.Underline,
        };

        TextAlignment horizontalTextAlignment = TextAlignment.Left;
        public TextAlignment HorizontalTextAlignment
        {
            get => horizontalTextAlignment;
            set => this.Change(ref horizontalTextAlignment, value);
        }

        VerticalAlignment verticalTextAlignment = VerticalAlignment.Top;
        public VerticalAlignment VerticalTextAlignment
        {
            get => verticalTextAlignment;
            set => this.Change(ref verticalTextAlignment, value);
        }

        //...

        protected TextLayer NewLayer() => new TextLayer("Untitled", fontColor, fontFamily, fontSize, FontStyle, "Some text", horizontalTextAlignment, verticalTextAlignment, new(Document.Height, Document.Width));

        public override bool OnMouseDown(Point point)
        {
            MouseDown = point;

            selection = new Shape();
            Document.Selections.Add(selection);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            MouseMove = point;
            if (MouseDown != null)
            {
                region = GetRegion(MouseDown.Value, point);
                selection.Points = new PointCollection()
                {
                    new Point(region.X, region.Y),
                    new Point(region.X + region.Width, region.Y),
                    new Point(region.X + region.Width, region.Y + region.Height),
                    new Point(region.X, region.Y + region.Height),
                };
            }
        }

        public override void OnMouseUp(Point point)
        {
            if (region == null)
                return;

            if (Document is ImageDocument document)
            {
                document.Layers.ForEach(i => i.IsSelected = false);

                var newLayer = NewLayer();
                newLayer.IsSelected = true;
                newLayer.Region = region;

                LayerCollection layers = TargetLayer == null || TargetLayer.Parent == null ? document.Layers : TargetLayer.Parent.Layers;
                newLayer.Parent = TargetLayer?.Parent;

                var index = TargetLayer == null || TargetLayer.Parent == null ? 0 : layers.IndexOf(TargetLayer);
                layers.Insert(index, newLayer);

                Document.Selections.Remove(selection);
                selection = null;

                region = null;
                MouseDown = null;
                MouseMove = null;
            }
        }
    }
}