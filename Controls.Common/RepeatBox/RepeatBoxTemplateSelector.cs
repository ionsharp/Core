using Imagin.Common.Scheduling;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public class RepeatBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoneTemplate
        {
            get; set;
        }

        public DataTemplate DailyTemplate
        {
            get; set;
        }

        public DataTemplate WeeklyTemplate
        {
            get; set;
        }

        public DataTemplate MonthlyTemplate
        {
            get; set;
        }

        public DataTemplate YearlyTemplate
        {
            get; set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject Container)
        {
            var Recurrence = (Recurrence)item;
            switch (Recurrence)
            {
                case Recurrence.None:
                    return NoneTemplate;
                case Recurrence.Daily:
                    return DailyTemplate;
                case Recurrence.Weekly:
                    return WeeklyTemplate;
                case Recurrence.Monthly:
                    return MonthlyTemplate;
                case Recurrence.Yearly:
                    return YearlyTemplate;
            }
            return base.SelectTemplate(item, Container);
        }
    }
}
