using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public abstract class UpDown : TextBox
    {
        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(UpDown), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }
            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        public UpDown() : base()
        {
            this.DefaultStyleKey = typeof(UpDown);
            this.CommandBindings.Add(new CommandBinding(Up, Up_Executed, Up_CanExecute));
            this.CommandBindings.Add(new CommandBinding(Down, Down_Executed, Down_CanExecute));
        }

        public static readonly RoutedUICommand Up = new RoutedUICommand("Up", "Up", typeof(UpDown));
        protected abstract void Up_Executed(object sender, ExecutedRoutedEventArgs e);
        protected abstract void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e);
        
        public static readonly RoutedUICommand Down = new RoutedUICommand("Down", "Down", typeof(UpDown));
        protected abstract void Down_Executed(object sender, ExecutedRoutedEventArgs e);
        protected abstract void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e);
    }
}
