using Imagin.Common;
using System;
using System.Timers;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Count down")]
    [Serializable]
    public class CountDownTile : Tile
    {
        DateTime date = DateTime.Now;
        [Hidden]
        public DateTime Date
        {
            get => date;
            set => this.Change(ref date, value);
        }

        public CountDownTile() : base()
        {
            timer.Enabled = true;
        }

        protected override void OnUpdate(ElapsedEventArgs e)
        {
            base.OnUpdate(e);
            XIPropertyChanged.Changed(this, () => Date);
        }
    }
}