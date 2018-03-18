using Imagin.Common;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Colour.Controls
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
        public bool IsCancel = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSave = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FooterStyleProperty = DependencyProperty.Register(nameof(FooterStyle), typeof(Style), typeof(BrushDialogBase<T>), new FrameworkPropertyMetadata(default(Style)));
        /// <summary>
        /// 
        /// </summary>
        public Style FooterStyle
        {
            get => (Style)GetValue(FooterStyleProperty);
            set => SetValue(FooterStyleProperty, value);
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
            IsCancel = true;
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSave(object sender, RoutedEventArgs e)
        {
            IsSave = true;
            Close();
        }
    }
}