using System.Windows;
using System.Windows.Documents;

namespace Imagin.Common.Linq
{
    [Extends(typeof(List))]
    public static class XList
    {
        #region FontScale

        public static readonly DependencyProperty FontScaleProperty = DependencyProperty.RegisterAttached("FontScale", typeof(double), typeof(XList), new FrameworkPropertyMetadata(1.0, OnFontScaleChanged));
        public static double GetFontScale(List i) => (double)i.GetValue(FontScaleProperty);
        public static void SetFontScale(List i, double input) => i.SetValue(FontScaleProperty, input);
        static void OnFontScaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var list = sender as List;
            list.FontSize = GetFontScaleOrigin(list) * (double)e.NewValue;
        }

        #endregion

        #region FontScaleOrigin

        public static readonly DependencyProperty FontScaleOriginProperty = DependencyProperty.RegisterAttached("FontScaleOrigin", typeof(double), typeof(XList), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, OnFontScaleOriginChanged));
        public static double GetFontScaleOrigin(List i) => (double)i.GetValue(FontScaleOriginProperty);
        public static void SetFontScaleOrigin(List i, double input) => i.SetValue(FontScaleOriginProperty, input);
        static void OnFontScaleOriginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var list = sender as List;
            list.FontSize = (double)e.NewValue * GetFontScale(list);
        }

        #endregion
    }

    [Extends(typeof(Paragraph))]
    public static class XParagraph
    {
        #region FontScale

        public static readonly DependencyProperty FontScaleProperty = DependencyProperty.RegisterAttached("FontScale", typeof(double), typeof(XParagraph), new FrameworkPropertyMetadata(1.0, OnFontScaleChanged));
        public static double GetFontScale(Paragraph i) => (double)i.GetValue(FontScaleProperty);
        public static void SetFontScale(Paragraph i, double input) => i.SetValue(FontScaleProperty, input);
        static void OnFontScaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var paragraph = sender as Paragraph;
            paragraph.FontSize = GetFontScaleOrigin(paragraph) * (double)e.NewValue;
        }

        #endregion

        #region FontScaleOrigin

        public static readonly DependencyProperty FontScaleOriginProperty = DependencyProperty.RegisterAttached("FontScaleOrigin", typeof(double), typeof(XParagraph), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, OnFontScaleOriginChanged));
        public static double GetFontScaleOrigin(Paragraph i) => (double)i.GetValue(FontScaleOriginProperty);
        public static void SetFontScaleOrigin(Paragraph i, double input) => i.SetValue(FontScaleOriginProperty, input);
        static void OnFontScaleOriginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var paragraph = sender as Paragraph;
            paragraph.FontSize = (double)e.NewValue * GetFontScale(paragraph);
        }

        #endregion
    }
}