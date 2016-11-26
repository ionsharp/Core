using Imagin.Common.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public abstract class PaneView : UserControl
    {
        #region Properties

        IPaneViewModel Model
        {
            get
            {
                return this.DataContext as IPaneViewModel;
            }
        }

        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(PaneView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTitleChanged));
        public object Title
        {
            get
            {
                return (object)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
        static void OnTitleChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((PaneView)Object).OnTitleChanged((string)e.NewValue);
        }

        public static DependencyProperty IsPaneVisibleProperty = DependencyProperty.Register("IsPaneVisible", typeof(bool), typeof(PaneView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsPaneVisibleChanged));
        public bool IsPaneVisible
        {
            get
            {
                return (bool)GetValue(IsPaneVisibleProperty);
            }
            set
            {
                SetValue(IsPaneVisibleProperty, value);
            }
        }
        static void OnIsPaneVisibleChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((PaneView)Object).OnIsPaneVisibleChanged((bool)e.NewValue);
        }

        public static DependencyProperty IconProperty = DependencyProperty.Register("IconProperty", typeof(ImageSource), typeof(PaneView), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        public ImageSource Icon
        {
            get
            {
                return (ImageSource)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }
        static void OnIconChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((PaneView)Object).OnIconChanged((ImageSource)e.NewValue);
        }

        #endregion

        #region Methods

        protected virtual void OnTitleChanged(string Value)
        {
            this.Model.Title = Value;
        }

        protected virtual void OnIsPaneVisibleChanged(bool Value)
        {
            this.Model.IsVisible = Value;
        }

        protected virtual void OnIconChanged(ImageSource Value)
        {
            this.Model.Icon = Value;
        }

        #endregion

        #region PaneView

        public PaneView() : base()
        {
        }

        #endregion
    }
}
