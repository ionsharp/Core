using System;
using System.Windows;
using System.Windows.Markup;

namespace Demo
{
    [ContentProperty(nameof(Instance))]
    public class Element : DependencyObject
    {
        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(nameof(Category), typeof(string), typeof(Element), new FrameworkPropertyMetadata("General"));
        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.Register(nameof(Instance), typeof(object), typeof(Element), new FrameworkPropertyMetadata(null));
        public object Instance
        {
            get => GetValue(InstanceProperty);
            set => SetValue(InstanceProperty, value);
        }

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(object), typeof(Element), new FrameworkPropertyMetadata(null));
        public object Options
        {
            get => GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Type), typeof(Element), new FrameworkPropertyMetadata(null));
        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public string TypeString => Type?.Name;

        public Element() : base() { }
    }
}