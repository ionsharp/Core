using Imagin.Common;
using Imagin.Controls.Common;
using System;
using System.Timers;

namespace Imagin.Gadgets
{
    public partial class ClockGadget : Gadget
    {
        public class ObservableDate : NotifiableObject
        {
            DateTime date = DateTime.Now;
            public DateTime Date
            {
                get
                {
                    return date;
                }
                set
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }

            public ObservableDate() : base(true)
            {
            }

            public override void OnNotified(ElapsedEventArgs e)
            {
                base.OnNotified(e);
                this.Date = DateTime.Now;
            }
        }

        public static ObservableDate Date
        {
            get
            {
                return new ObservableDate();
            }
        }

        public ClockGadget()
        {
            InitializeComponent();
        }
    }
}
