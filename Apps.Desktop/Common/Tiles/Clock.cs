using Imagin.Common;
using System;
using System.Timers;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Clock")]
    [Serializable]
    public class ClockTile : Tile
    {
        string format = "ddd, MMM d, yyyy • h:mm:ss tt";
        public string Format
        {
            get => format;
            set => this.Change(ref format, value);
        }

        [Hidden]
        public string DateTime => System.DateTime.Now.ToString(format);

        public ClockTile() : base()
        {
            timer.Enabled = true;
        }

        protected override void OnUpdate(ElapsedEventArgs e)
        {
            base.OnUpdate(e);
            XIPropertyChanged.Changed(this, () => DateTime);
        }
    }
}