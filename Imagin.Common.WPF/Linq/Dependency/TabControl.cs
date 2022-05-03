using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(TabControl))]
    public static class XTabControl
    {
        #region ContentVisibility

        public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.RegisterAttached("ContentVisibility", typeof(Visibility), typeof(XTabControl), new FrameworkPropertyMetadata(Visibility.Visible));
        public static Visibility GetContentVisibility(TabControl i) => (Visibility)i.GetValue(ContentVisibilityProperty);
        public static void SetContentVisibility(TabControl i, Visibility value) => i.SetValue(ContentVisibilityProperty, value);

        #endregion

        #region HeaderVisibility

        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.RegisterAttached("HeaderVisibility", typeof(Visibility), typeof(XTabControl), new FrameworkPropertyMetadata(Visibility.Visible));
        public static Visibility GetHeaderVisibility(TabControl i) => (Visibility)i.GetValue(HeaderVisibilityProperty);
        public static void SetHeaderVisibility(TabControl i, Visibility value) => i.SetValue(HeaderVisibilityProperty, value);

        #endregion

        #region IsOverflowOpen

        public static readonly DependencyProperty IsOverflowOpenProperty = DependencyProperty.RegisterAttached("IsOverflowOpen", typeof(bool), typeof(XTabControl), new FrameworkPropertyMetadata(false));
        public static bool GetIsOverflowOpen(TabControl i) => (bool)i.GetValue(IsOverflowOpenProperty);
        public static void SetIsOverflowOpen(TabControl i, bool value) => i.SetValue(IsOverflowOpenProperty, value);

        #endregion

        #region OverflowItemCommand

        public static readonly DependencyProperty OverflowItemCommandProperty = DependencyProperty.RegisterAttached("OverflowItemCommand", typeof(ICommand), typeof(XTabControl), new FrameworkPropertyMetadata(null));
        public static ICommand GetOverflowItemCommand(TabControl i) => (ICommand)i.GetValue(OverflowItemCommandProperty);
        public static void SetOverflowItemCommand(TabControl i, ICommand value) => i.SetValue(OverflowItemCommandProperty, value);

        #endregion

        #region OverflowIconTemplate

        public static readonly DependencyProperty OverflowIconTemplateProperty = DependencyProperty.RegisterAttached("OverflowIconTemplate", typeof(DataTemplate), typeof(XTabControl), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetOverflowIconTemplate(TabControl i) => (DataTemplate)i.GetValue(OverflowIconTemplateProperty);
        public static void SetOverflowIconTemplate(TabControl i, DataTemplate value) => i.SetValue(OverflowIconTemplateProperty, value);

        #endregion

        #region OverflowItemTemplate

        public static readonly DependencyProperty OverflowItemTemplateProperty = DependencyProperty.RegisterAttached("OverflowItemTemplate", typeof(DataTemplate), typeof(XTabControl), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetOverflowItemTemplate(TabControl i) => (DataTemplate)i.GetValue(OverflowItemTemplateProperty);
        public static void SetOverflowItemTemplate(TabControl i, DataTemplate value) => i.SetValue(OverflowItemTemplateProperty, value);

        #endregion
    }
}