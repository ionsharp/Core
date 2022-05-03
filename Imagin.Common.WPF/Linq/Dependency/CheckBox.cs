using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(CheckBox))]
    public static class XCheckBox
    {
        #region CheckedCommand

        public static readonly DependencyProperty CheckedCommandProperty = DependencyProperty.RegisterAttached("CheckedCommand", typeof(ICommand), typeof(XCheckBox), new FrameworkPropertyMetadata(null, OnCheckedCommandChanged));
        public static ICommand GetCheckedCommand(CheckBox i) => (ICommand)i.GetValue(CheckedCommandProperty);
        public static void SetCheckedCommand(CheckBox i, ICommand input) => i.SetValue(CheckedCommandProperty, input);
        static void OnCheckedCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is CheckBox box)
                box.RegisterHandlerAttached(e.NewValue != null, CheckedCommandProperty, i => i.Checked += CheckedCommand_Checked, i => i.Checked -= CheckedCommand_Checked);
        }

        static void CheckedCommand_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
                GetCheckedCommand(box).Execute();
        }

        #endregion

        #region UncheckedCommand

        public static readonly DependencyProperty UncheckedCommandProperty = DependencyProperty.RegisterAttached("UncheckedCommand", typeof(ICommand), typeof(XCheckBox), new FrameworkPropertyMetadata(null, OnUncheckedCommandChanged));
        public static ICommand GetUncheckedCommand(CheckBox i) => (ICommand)i.GetValue(UncheckedCommandProperty);
        public static void SetUncheckedCommand(CheckBox i, ICommand input) => i.SetValue(UncheckedCommandProperty, input);
        static void OnUncheckedCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is CheckBox box)
                box.RegisterHandlerAttached(e.NewValue != null, UncheckedCommandProperty, i => i.Unchecked += UncheckedCommand_Unchecked, i => i.Unchecked -= UncheckedCommand_Unchecked);
        }

        static void UncheckedCommand_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
                GetUncheckedCommand(box).Execute();
        }

        #endregion
    }
}