using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class TimeZoneBox : ComboBox
	{
		public TimeZoneBox() : base() => SetCurrentValue(ItemsSourceProperty, new ObservableCollection<TimeZoneInfo>(TimeZoneInfo.GetSystemTimeZones()));
    }
}