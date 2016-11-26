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
            public DateTime Date
            {
                get
                {
                    return DateTime.Now;
                }
            }

            public ObservableDate() : base(true)
            {
            }

            public override void OnNotified(ElapsedEventArgs e)
            {
                base.OnNotified(e);
                OnPropertyChanged("Date");
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
