using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class EmptyPresenter : Presenter<ItemsControl>
    {
        public static readonly ResourceKey<Thickness> PopupMarginKey = new();

        static readonly DependencyPropertyKey IsEmptyKey = DependencyProperty.RegisterReadOnly(nameof(IsEmpty), typeof(bool), typeof(EmptyPresenter), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsEmptyProperty = IsEmptyKey.DependencyProperty;
        public bool IsEmpty
        {
            get => (bool)GetValue(IsEmptyProperty);
            private set => SetValue(IsEmptyKey, value);
        }

        static readonly DependencyProperty IsEmptyChangedProperty = DependencyProperty.Register(nameof(IsEmptyChanged), typeof(bool), typeof(EmptyPresenter), new FrameworkPropertyMetadata(false, OnIsEmptyChanged));
        bool IsEmptyChanged
        {
            get => (bool)GetValue(IsEmptyChangedProperty);
            set => SetValue(IsEmptyChangedProperty, value);
        }
        static void OnIsEmptyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is EmptyPresenter presenter)
                presenter.IsEmpty = (bool)e.NewValue;
        }

        //...

        public EmptyPresenter() : base() { }

        //...

        protected override void OnLoaded(Presenter<ItemsControl> i)
        {
            base.OnLoaded(i);
            i.Bind(EmptyPresenter.ContentTemplateProperty,
                new PropertyPath("(0)", XItemsControl.EmptyTemplateProperty),
                Control);
            i.Bind(EmptyPresenter.IsEmptyChangedProperty,
                new PropertyPath("(0)", XItemsControl.IsEmptyProperty),
                Control);

            i.MultiBind(EmptyPresenter.VisibilityProperty, BooleanToVisibilityMultiConverter.Default, Control, new PropertyPath("(0)", XItemsControl.EmptyTemplateVisibilityProperty), new PropertyPath("(0)", XItemsControl.IsEmptyProperty));
        }

        protected override void OnUnloaded(Presenter<ItemsControl> i)
        {
            base.OnUnloaded(i);
            i.Unbind
                (EmptyPresenter.ContentTemplateProperty);
            i.Unbind
                (EmptyPresenter.IsEmptyProperty);
            i.Unbind
                (EmptyPresenter.VisibilityProperty);
        }
    }
}