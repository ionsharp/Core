using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Models;
using System;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public enum Alignments
    {
        Left,
        Center,
        Right,
        JustifyLastLeft,
        JustifyLastRight,
        JustifyLastCenter,
        JustifyAll
    }

    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ParagraphPanel : Panel
    {
        Alignments alignment = Alignments.Left;
        [Visible]
        public Alignments Alignment
        {
            get => alignment;
            set => this.Change(ref alignment, value);
        }

        double indentFirstLine = 0;
        [Visible]
        public double IndentFirstLine
        {
            get => indentFirstLine;
            set => this.Change(ref indentFirstLine, value);
        }

        double indentLeft = 0;
        [Visible]
        public double IndentLeft
        {
            get => indentLeft;
            set => this.Change(ref indentLeft, value);
        }

        double indentRight = 0;
        [Visible]
        public double IndentRight
        {
            get => indentRight;
            set => this.Change(ref indentRight, value);
        }

        double spaceAfterParagraph = 0;
        [Visible]
        public double SpaceAfterParagraph
        {
            get => spaceAfterParagraph;
            set => this.Change(ref spaceAfterParagraph, value);
        }

        double spaceBeforeParagraph = 0;
        [Visible]
        public double SpaceBeforeParagraph
        {
            get => spaceBeforeParagraph;
            set => this.Change(ref spaceBeforeParagraph, value);
        }

        public override Uri Icon => Resources.ProjectImage("Paragraph.png");

        public override string Title => "Paragraph";

        public ParagraphPanel() : base() { }
    }
}