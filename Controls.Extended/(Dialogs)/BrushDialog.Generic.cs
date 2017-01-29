using Imagin.Common.Extensions;
using Imagin.Controls.Common;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BrushDialogBase<T> : BasicWindow where T : Brush
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FooterStyleProperty = DependencyProperty.Register("FooterStyle", typeof(Style), typeof(BrushDialogBase<T>), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style FooterStyle
        {
            get
            {
                return (Style)GetValue(FooterStyleProperty);
            }
            set
            {
                SetValue(FooterStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract string DefaultTitle
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public Chip<T> Chip
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public T InitialValue
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public BrushDialogBase() : base()
        {
            DataContext = this;
            Title = DefaultTitle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="initialValue"></param>
        /// <param name="chip"></param>
        public BrushDialogBase(string title, T initialValue, Chip<T> chip = null) : this()
        {
            Title = title.IsNullOrEmpty() ? Title : title;
            Value = InitialValue = initialValue;
            Chip = chip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnRevert(object sender, RoutedEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancel(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Cancel;
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSave(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Ok;
            Close();
        }
    }
}
