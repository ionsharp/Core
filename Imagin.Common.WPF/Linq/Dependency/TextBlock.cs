using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Imagin.Common.Linq
{
    [Extends(typeof(TextBlock))]
    public static class XTextBlock
    {
        #region FontScale

        public static readonly DependencyProperty FontScaleProperty = DependencyProperty.RegisterAttached("FontScale", typeof(double), typeof(XTextBlock), new FrameworkPropertyMetadata(1.0, OnFontScaleChanged));
        public static double GetFontScale(TextBlock i) => (double)i.GetValue(FontScaleProperty);
        public static void SetFontScale(TextBlock i, double input) => i.SetValue(FontScaleProperty, input);
        static void OnFontScaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            textBlock.FontSize = GetFontScaleOrigin(textBlock) * (double)e.NewValue;
        }

        #endregion

        #region FontScaleOrigin

        public static readonly DependencyProperty FontScaleOriginProperty = DependencyProperty.RegisterAttached("FontScaleOrigin", typeof(double), typeof(XTextBlock), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, OnFontScaleOriginChanged));
        public static double GetFontScaleOrigin(TextBlock i) => (double)i.GetValue(FontScaleOriginProperty);
        public static void SetFontScaleOrigin(TextBlock i, double input) => i.SetValue(FontScaleOriginProperty, input);
        static void OnFontScaleOriginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            textBlock.FontSize = (double)e.NewValue * GetFontScale(textBlock);
        }

        #endregion

        #region SplitText

        public enum SplitTextModes
        {
            Contains,
            StartsWith
        }

        public static readonly DependencyProperty SplitTextProperty = DependencyProperty.RegisterAttached("SplitText", typeof(string), typeof(XTextBlock), new FrameworkPropertyMetadata(null, OnSplitTextChanged));
        public static string GetSplitText(TextBlock i) => (string)i.GetValue(SplitTextProperty);
        public static void SetSplitText(TextBlock i, string value) => i.SetValue(SplitTextProperty, value);
        static void OnSplitTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBlock block)
            {
                if (block.Inlines.Count > 0)
                {
                    foreach (Run i in block.Inlines)
                        i.Unbind(Run.StyleProperty);

                    block.Inlines.Clear();
                }

                var text = GetSplitText(block);
                if (text == null)
                    return;

                var key = GetSplitTextKey(block);
                if (key.NullOrEmpty())
                {
                    block.Text = text;
                    return;
                }

                var index = -1;
                switch (GetSplitTextMode(block))
                {
                    case SplitTextModes.Contains:
                        index = GetSplitTextCase(block)
                        ? text.IndexOf(key)
                        : text.ToLower().IndexOf(key.ToLower());
                        break;

                    case SplitTextModes.StartsWith:
                        index = GetSplitTextCase(block)
                        ? (text.StartsWith(key) ? 0 : -1)
                        : (text.ToLower().StartsWith(key.ToLower()) ? 0 : -1 );
                        break;
                }

                if (index < 0)
                {
                    block.Text = text;
                    return;
                }

                if (index > 0)
                    block.Inlines.Add(new Run(text.Substring(0, index)));

                var secondRun = new Run(text.Substring(index, key.Length));
                secondRun.Bind(Run.StyleProperty, new PropertyPath("(0)", SplitTextStyleProperty), block);
                block.Inlines.Add(secondRun);

                var lastIndex = index + key.Length;
                if (lastIndex < text.Length - 1)
                    block.Inlines.Add(new Run(text.Substring(lastIndex)));
            }
        }

        #endregion

        #region SplitTextCase

        public static readonly DependencyProperty SplitTextCaseProperty = DependencyProperty.RegisterAttached("SplitTextCase", typeof(bool), typeof(XTextBlock), new FrameworkPropertyMetadata(false, OnSplitTextChanged));
        public static bool GetSplitTextCase(TextBlock i) => (bool)i.GetValue(SplitTextCaseProperty);
        public static void SetSplitTextCase(TextBlock i, bool value) => i.SetValue(SplitTextCaseProperty, value);

        #endregion

        #region SplitTextKey

        public static readonly DependencyProperty SplitTextKeyProperty = DependencyProperty.RegisterAttached("SplitTextKey", typeof(string), typeof(XTextBlock), new FrameworkPropertyMetadata(null, OnSplitTextChanged));
        public static string GetSplitTextKey(TextBlock i) => (string)i.GetValue(SplitTextKeyProperty);
        public static void SetSplitTextKey(TextBlock i, string value) => i.SetValue(SplitTextKeyProperty, value);

        #endregion

        #region SplitTextMode

        public static readonly DependencyProperty SplitTextModeProperty = DependencyProperty.RegisterAttached("SplitTextMode", typeof(SplitTextModes), typeof(XTextBlock), new FrameworkPropertyMetadata(SplitTextModes.Contains, OnSplitTextChanged));
        public static SplitTextModes GetSplitTextMode(TextBlock i) => (SplitTextModes)i.GetValue(SplitTextModeProperty);
        public static void SetSplitTextMode(TextBlock i, SplitTextModes value) => i.SetValue(SplitTextModeProperty, value);

        #endregion

        #region SplitTextStyle

        public static readonly DependencyProperty SplitTextStyleProperty = DependencyProperty.RegisterAttached("SplitTextStyle", typeof(Style), typeof(XTextBlock), new FrameworkPropertyMetadata(null));
        public static Style GetSplitTextStyle(TextBlock i) => (Style)i.GetValue(SplitTextStyleProperty);
        public static void SetSplitTextStyle(TextBlock i, Style value) => i.SetValue(SplitTextStyleProperty, value);

        #endregion

        #region Update

        class TimerWrapper
        {
            TextBlock Control;

            System.Windows.Threading.DispatcherTimer Timer = new();

            public TimerWrapper(TextBlock control)
            {
                Control = control;

                Timer.Interval = GetUpdateInterval(control);
                Timer.Tick += OnUpdate;
                Timer.Start();
            }

            public void Unload()
            {
                Timer.Stop();
                Timer.Tick -= OnUpdate;
                Timer = null;

                Control = null;
            }

            void OnUpdate(object sender, EventArgs e)
            {
                var binding = System.Windows.Data.BindingOperations.GetBindingExpression(Control, TextBlock.TextProperty);
                binding?.UpdateTarget();
            }
        }

        public static readonly DependencyProperty UpdateProperty = DependencyProperty.RegisterAttached("Update", typeof(bool), typeof(XTextBlock), new FrameworkPropertyMetadata(false, OnUpdateChanged));
        public static bool GetUpdate(TextBlock i) => (bool)i.GetValue(UpdateProperty);
        public static void SetUpdate(TextBlock i, bool input) => i.SetValue(UpdateProperty, input);
        static void OnUpdateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            textBlock.RegisterHandlerAttached((bool)e.NewValue, UpdateProperty, i =>
            {
                var result = new TimerWrapper(i);
                SetUpdateTimer(i, result);
            }, i =>
            {
                GetUpdateTimer(i)?.Unload();
                SetUpdateTimer(i, null);
            });
        }

        #endregion

        #region UpdateInterval

        public static readonly DependencyProperty UpdateIntervalProperty = DependencyProperty.RegisterAttached("UpdateInterval", typeof(TimeSpan), typeof(XTextBlock), new FrameworkPropertyMetadata(1.Seconds()));
        public static TimeSpan GetUpdateInterval(TextBlock i) => (TimeSpan)i.GetValue(UpdateIntervalProperty);
        public static void SetUpdateInterval(TextBlock i, TimeSpan input) => i.SetValue(UpdateIntervalProperty, input);

        #endregion

        #region (private) UpdateTimer

        static readonly DependencyProperty UpdateTimerProperty = DependencyProperty.RegisterAttached("UpdateTimer", typeof(TimerWrapper), typeof(XTextBlock), new FrameworkPropertyMetadata(null));
        static TimerWrapper GetUpdateTimer(TextBlock i) => (TimerWrapper)i.GetValue(UpdateTimerProperty);
        static void SetUpdateTimer(TextBlock i, TimerWrapper input) => i.SetValue(UpdateTimerProperty, input);

        #endregion
    }
}