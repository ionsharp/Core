using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class Presenter : ContentPresenter
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Presenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, null, OnBackgroundCoerced));
        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
        static object OnBackgroundCoerced(DependencyObject sender, object input) => (sender as Presenter).OnBackgroundCoerced(input);

        public Presenter() : base() { }

        protected override void OnRender(DrawingContext context)
        {
            base.OnRender(context);
            context.DrawRectangle(Background, null, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
        }

        protected virtual object OnBackgroundCoerced(object input) => input;
    }

    public abstract class Presenter<T> : Presenter where T : DependencyObject
    {
        protected T Control;

        public Presenter() : base() => this.RegisterHandler(OnLoaded, OnUnloaded);

        protected virtual void OnLoaded(Presenter<T> i)
        {
            i.Control = i.FindParent<T>();
            if (i.Control == null)
                throw new ParentNotFoundException<Presenter<T>, T>();
        }

        protected virtual void OnUnloaded(Presenter<T> i) { }
    }
}