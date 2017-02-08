using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class RichTextBoxExtensions
    {
        #region ParagraphSpacing

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ParagraphSpacingProperty = DependencyProperty.RegisterAttached("ParagraphSpacing", typeof(Thickness), typeof(RichTextBoxExtensions), new PropertyMetadata(default(Thickness), OnParagraphSpacingChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Thickness GetParagraphSpacing(RichTextBox d)
        {
            return (Thickness)d.GetValue(ParagraphSpacingProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetParagraphSpacing(RichTextBox d, Thickness value)
        {
            d.SetValue(ParagraphSpacingProperty, value);
        }
        static void OnParagraphSpacingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var RichTextBox = sender as RichTextBox;

            RichTextBox.SizeChanged -= OnParagraphSpacingUpdated;
            RichTextBox.SizeChanged += OnParagraphSpacingUpdated;

            OnParagraphSpacingUpdated(RichTextBox, null);
        }
        static void OnParagraphSpacingUpdated(object sender, SizeChangedEventArgs e)
        {
            var p = sender as RichTextBox;
            var s = GetParagraphSpacing(p);

            foreach (var i in p.Document.Blocks)
                i.As<Block>().Margin = s; //Doesn't work?
        }

        #endregion
    }
}
