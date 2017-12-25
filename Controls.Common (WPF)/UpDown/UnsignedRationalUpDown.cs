using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnsignedRationalUpDown<T> : RationalUpDown<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public UnsignedRationalUpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            switch (e.Text)
            {
                default:
                    e.Handled = !Expression.IsMatch(e.Text);
                    break;
            }
        }
    }
}
