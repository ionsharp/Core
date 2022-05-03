using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Imagin.Common.Behavior
{
    public class UpdateTemplateBehavior : Behavior<ContentPresenter>
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(UpdateTemplateBehavior), new FrameworkPropertyMetadata(null));
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(UpdateTemplateBehavior), new FrameworkPropertyMetadata(null));
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public UpdateTemplateBehavior() : base() { }

        protected override void OnAttached()
        {
            base.OnAttached(); 
            Update();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.Property.Name)
            {
                case nameof(Content):
                case nameof(Value):
                    Update();
                    break;
            }
        }

        void Update()
        {
            if (AssociatedObject != null)
            {
                if (Content != null)
                {
                    AssociatedObject
                        .Unbind(ContentPresenter.ContentProperty);
                    AssociatedObject.Content
                        = null;

                    AssociatedObject
                        .Bind(ContentPresenter.ContentProperty, nameof(Content), this);
                }
            }
        }
    }
}