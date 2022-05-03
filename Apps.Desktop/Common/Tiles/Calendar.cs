using Imagin.Common;
using System;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Calendar")]
    [Serializable]
    public class CalendarTile : Tile
    {
        DateTime date = DateTime.Now;
        [Hidden]
        public DateTime Date
        {
            get => date;
            set => this.Change(ref date, value);
        }

        public CalendarTile() : base() { }
    }
}