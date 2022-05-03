using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Threading;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class HistogramPanel : Panel
    {
        StringColor background = System.Windows.Media.Colors.Transparent;
        [Option, Visible]
        public System.Windows.Media.Color Background
        {
            get => background;
            set => this.Change(ref background, value);
        }

        double opacity = 0.3;
        [Format(RangeFormat.Both)]
        [Option, Visible]
        [Range(0.1, 0.9, 0.01)]
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }

        StringColor red = System.Windows.Media.Colors.Red;
        [Option, Visible]
        public System.Windows.Media.Color Red
        {
            get => red;
            set => this.Change(ref red, value);
        }

        StringColor green = System.Windows.Media.Colors.Green;
        [Option, Visible]
        public System.Windows.Media.Color Green
        {
            get => green;
            set => this.Change(ref green, value);
        }

        StringColor blue = System.Windows.Media.Colors.Blue;
        [Option, Visible]
        public System.Windows.Media.Color Blue
        {
            get => blue;
            set => this.Change(ref blue, value);
        }

        StringColor luminance = System.Windows.Media.Colors.Black;
        [Option, Visible]
        public System.Windows.Media.Color Luminance
        {
            get => luminance;
            set => this.Change(ref luminance, value);
        }

        StringColor saturation = System.Windows.Media.Colors.Magenta;
        [Option, Visible]
        public System.Windows.Media.Color Saturation
        {
            get => saturation;
            set => this.Change(ref saturation, value);
        }

        public override Uri Icon => Resources.ProjectImage("Histogram.png");

        bool showBlue = true;
        public bool ShowBlue
        {
            get => showBlue;
            set => this.Change(ref showBlue, value);
        }

        bool showGreen = true;
        public bool ShowGreen
        {
            get => showGreen;
            set => this.Change(ref showGreen, value);
        }

        bool showRed = true;
        public bool ShowRed
        {
            get => showRed;
            set => this.Change(ref showRed, value);
        }

        bool showLuminance = true;
        public bool ShowLuminance
        {
            get => showLuminance;
            set => this.Change(ref showLuminance, value);
        }

        bool showSaturation = true;
        public bool ShowSaturation
        {
            get => showSaturation;
            set => this.Change(ref showSaturation, value);
        }

        public override string Title => "Histogram";

        public HistogramPanel() : base() { }
    }
}