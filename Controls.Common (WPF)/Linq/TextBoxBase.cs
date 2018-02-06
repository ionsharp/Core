using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Controls.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class TextBoxBaseExtensions
    {
        #region Properties

        #region EnableCopyCommand

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnableCopyCommandProperty = DependencyProperty.RegisterAttached("EnableCopyCommand", typeof(bool), typeof(TextBoxBaseExtensions), new PropertyMetadata(true, OnEnableCopyCommandChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetEnableCopyCommand(TextBoxBase d, bool value)
        {
            d.SetValue(EnableCopyCommandProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetEnableCopyCommand(TextBoxBase d)
        {
            return (bool)d.GetValue(EnableCopyCommandProperty);
        }
        static void OnEnableCopyCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnEnableCommandChanged(sender as UIElement, OnPreviewCopyExecuted, (bool)e.NewValue);
        }
        static void OnPreviewCopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy)
                e.Handled = true;
        }

        #endregion

        #region EnableCutCommand

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnableCutCommandProperty = DependencyProperty.RegisterAttached("EnableCutCommand", typeof(bool), typeof(TextBoxBaseExtensions), new PropertyMetadata(true, OnEnableCutCommandChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetEnableCutCommand(TextBoxBase d, bool value)
        {
            d.SetValue(EnableCutCommandProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetEnableCutCommand(TextBoxBase d)
        {
            return (bool)d.GetValue(EnableCutCommandProperty);
        }
        static void OnEnableCutCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnEnableCommandChanged(sender as UIElement, OnPreviewCutExecuted, (bool)e.NewValue);
        }
        static void OnPreviewCutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut)
                e.Handled = true;
        }

        #endregion

        #region EnablePasteCommand

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnablePasteCommandProperty = DependencyProperty.RegisterAttached("EnablePasteCommand", typeof(bool), typeof(TextBoxBaseExtensions), new PropertyMetadata(true, OnEnablePasteCommandChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetEnablePasteCommand(TextBoxBase d, bool value)
        {
            d.SetValue(EnablePasteCommandProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetEnablePasteCommand(TextBoxBase d)
        {
            return (bool)d.GetValue(EnablePasteCommandProperty);
        }
        static void OnEnablePasteCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnEnableCommandChanged(sender as UIElement, OnPreviewPasteExecuted, (bool)e.NewValue);
        }
        static void OnPreviewPasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
                e.Handled = true;
        }

        #endregion

        #endregion

        #region Methods

        static void OnEnableCommandChanged(UIElement Element, ExecutedRoutedEventHandler Handler, bool Enable)
        {
            if (Element != null)
            {
                if (Enable)
                {
                    CommandManager.RemovePreviewExecutedHandler(Element, Handler);
                }
                else CommandManager.AddPreviewExecutedHandler(Element, Handler);
            }
        }

        #endregion
    }
}
