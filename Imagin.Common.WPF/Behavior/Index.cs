using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Interactivity;

namespace Imagin.Common.Behavior
{
    public class IndexBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(nameof(Index), typeof(int), typeof(IndexBehavior), new FrameworkPropertyMetadata(-1, OnPropertyChanged));
        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static readonly DependencyProperty IndexorProperty = DependencyProperty.Register(nameof(Indexor), typeof(object), typeof(IndexBehavior), new FrameworkPropertyMetadata(null, OnIndexorChanged));
        public object Indexor
        {
            get => GetValue(IndexorProperty);
            set => SetValue(IndexorProperty, value);
        }
        static void OnIndexorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e)
        {
            if (i is IndexBehavior behavior)
            {
                if (e.NewValue is not Array && e.NewValue is not IList)
                {
                    Log.Write<IndexBehavior>(new Warning());
                    return;
                }

                behavior.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(string), typeof(IndexBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public string Property
        {
            get => (string)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(IndexBehavior), new FrameworkPropertyMetadata(null, OnValueChanged, OnValueCoerced));
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<IndexBehavior>().OnValueChanged(e);
        static object OnValueCoerced(DependencyObject sender, object input) => input ?? sender.As<IndexBehavior>().Value;

        //...

        protected override void OnAttached() => OnPropertyChanged();

        static void OnPropertyChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<IndexBehavior>().OnPropertyChanged();
        protected virtual void OnPropertyChanged()
        {
            if (AssociatedObject != null)
            {
                if (Property != null)
                {
                    this.Unbind(ValueProperty); this.Bind(ValueProperty, Property, AssociatedObject);
                }
            }
        }

        protected virtual void OnValueChanged(Value<object> input)
        {
            if (input.New != null)
            {
                if (Index >= 0)
                {
                    if (Indexor is Array a)
                    {
                        if (a.Length > Index)
                            a.SetValue(input.New, Index);
                    }
                    else if (Indexor is IList b)
                    {
                        if (b.Count > Index)
                            b[Index] = input.New;
                    }
                }
            }
        }
    }
}