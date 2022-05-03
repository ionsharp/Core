using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Models;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class CharacterPanel : Panel
    {
        Color color = Colors.Black;
        [Visible]
        public Color Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        FontFamily fontFamily = null;
        [Visible]
        public FontFamily FontFamily
        {
            get => fontFamily;
            set => this.Change(ref fontFamily, value);
        }

        double fontSize = 12.0;
        [Visible]
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        FontStyle fontStyle = FontStyles.Normal;
        [Visible]
        public FontStyle FontStyle
        {
            get => fontStyle;
            set => this.Change(ref fontStyle, value);
        }

        double horizontalScale = 1;
        [Visible]
        public double HorizontalScale
        {
            get => horizontalScale;
            set => this.Change(ref horizontalScale, value);
        }

        double verticalScale = 1;
        [Visible]
        public double VerticalScale
        {
            get => verticalScale;
            set => this.Change(ref verticalScale, value);
        }

        public override Uri Icon => Resources.ProjectImage("Character.png");

        public override string Title => "Character";

        public CharacterPanel() : base() { }
    }
}