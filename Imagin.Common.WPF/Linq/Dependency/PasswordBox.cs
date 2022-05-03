using Imagin.Common.Converters;
using Imagin.Common.Input;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(PasswordBox))]
    public static class XPasswordBox
    {
        #region Properties

        #region ClearCommand

        public static readonly RoutedUICommand ClearCommand = new(nameof(ClearCommand), nameof(ClearCommand), typeof(XPasswordBox));
        static void OnClearCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                passwordBox.Password = null;
        }
        static void OnClearCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                e.CanExecute = !passwordBox.Password.NullOrEmpty() && passwordBox.IsEnabled;
        }

        #endregion

        #region CopyCommand

        public static readonly DependencyProperty CopyCommandProperty = DependencyProperty.RegisterAttached("CopyCommand", typeof(ICommand), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static ICommand GetCopyCommand(PasswordBox i) => (ICommand)i.GetValue(CopyCommandProperty);
        public static void SetCopyCommand(PasswordBox i, ICommand input) => i.SetValue(CopyCommandProperty, input);

        #endregion

        #region CutCommand

        public static readonly DependencyProperty CutCommandProperty = DependencyProperty.RegisterAttached("CutCommand", typeof(ICommand), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static ICommand GetCutCommand(PasswordBox i) => (ICommand)i.GetValue(CutCommandProperty);
        public static void SetCutCommand(PasswordBox i, ICommand input) => i.SetValue(CutCommandProperty, input);

        #endregion

        #region GenerateButtonTemplate

        public static readonly DependencyProperty GenerateButtonTemplateProperty = DependencyProperty.RegisterAttached("GenerateButtonTemplate", typeof(DataTemplate), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetGenerateButtonTemplate(PasswordBox i) => (DataTemplate)i.GetValue(GenerateButtonTemplateProperty);
        public static void SetGenerateButtonTemplate(PasswordBox i, DataTemplate input) => i.SetValue(GenerateButtonTemplateProperty, input);

        #endregion

        #region GenerateButtonVisibility

        public static readonly DependencyProperty GenerateButtonVisibilityProperty = DependencyProperty.RegisterAttached("GenerateButtonVisibility", typeof(Visibility), typeof(XPasswordBox), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetGenerateButtonVisibility(PasswordBox i) => (Visibility)i.GetValue(GenerateButtonVisibilityProperty);
        public static void SetGenerateButtonVisibility(PasswordBox i, Visibility input) => i.SetValue(GenerateButtonVisibilityProperty, input);

        #endregion

        #region GenerateCommand

        public static readonly RoutedUICommand GenerateCommand = new(nameof(GenerateCommand), nameof(GenerateCommand), typeof(XPasswordBox));
        static void OnGenerateCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is PasswordBox i)
            {
                var result = Random.String(GetGenerateCharacters(i), GetGenerateLength(i).Minimum, GetGenerateLength(i).Maximum);
                result = GetGenerateDistinct(i) ? string.Concat(result.Distinct()) : result;
                i.Password = result;
            }
        }
        static void OnGenerateCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        #endregion

        #region GenerateCharacters

        public static readonly DependencyProperty GenerateCharactersProperty = DependencyProperty.RegisterAttached("GenerateCharacters", typeof(string), typeof(XPasswordBox), new FrameworkPropertyMetadata(string.Empty));
        public static string GetGenerateCharacters(PasswordBox i) => (string)i.GetValue(GenerateCharactersProperty);
        public static void SetGenerateCharacters(PasswordBox i, string input) => i.SetValue(GenerateCharactersProperty, input);

        #endregion

        #region GenerateDistinct

        public static readonly DependencyProperty GenerateDistinctProperty = DependencyProperty.RegisterAttached("GenerateDistinct", typeof(bool), typeof(XPasswordBox), new FrameworkPropertyMetadata(false));
        public static bool GetGenerateDistinct(PasswordBox i) => (bool)i.GetValue(GenerateDistinctProperty);
        public static void SetGenerateDistinct(PasswordBox i, bool input) => i.SetValue(GenerateDistinctProperty, input);

        #endregion

        #region GenerateLength

        public static readonly DependencyProperty GenerateLengthProperty = DependencyProperty.RegisterAttached("GenerateLength", typeof(Range<int>), typeof(XPasswordBox), new FrameworkPropertyMetadata(new Range<int>(0, 0)));
        [TypeConverter(typeof(Int32RangeTypeConverter))]
        public static Range<int> GetGenerateLength(PasswordBox i) => (Range<int>)i.GetValue(GenerateLengthProperty);
        public static void SetGenerateLength(PasswordBox i, Range<int> input) => i.SetValue(GenerateLengthProperty, input);

        #endregion

        #region Mask

        public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask", typeof(bool), typeof(XPasswordBox), new FrameworkPropertyMetadata(true));
        public static bool GetMask(PasswordBox i) => (bool)i.GetValue(MaskProperty);
        public static void SetMask(PasswordBox i, bool input) => i.SetValue(MaskProperty, input);

        #endregion

        #region PasteCommand

        public static readonly DependencyProperty PasteCommandProperty = DependencyProperty.RegisterAttached("PasteCommand", typeof(ICommand), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static ICommand GetPasteCommand(PasswordBox i) => (ICommand)i.GetValue(PasteCommandProperty);
        public static void SetPasteCommand(PasswordBox i, ICommand input) => i.SetValue(PasteCommandProperty, input);

        #endregion

        #region Password

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(XPasswordBox), new FrameworkPropertyMetadata(null, OnPasswordChanged));
        public static string GetPassword(PasswordBox i) => (string)i.GetValue(PasswordProperty);
        public static void SetPassword(PasswordBox i, string input) => i.SetValue(PasswordProperty, input);
        static void OnPasswordChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                GetPasswordHandle(passwordBox).SafeInvoke(() =>
                {
                    if (e.NewValue is string i)
                        passwordBox.Password = i;

                    passwordBox.Password = null;
                });
            }
        }

        #endregion

        #region PasswordHandle

        public static readonly DependencyProperty PasswordHandleProperty = DependencyProperty.RegisterAttached("PasswordHandle", typeof(Handle), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static Handle GetPasswordHandle(PasswordBox i) => i.GetValueOrSetDefault<Handle>(PasswordHandleProperty, () => false);

        #endregion

        #region Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(object), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static object GetPlaceholder(PasswordBox i) => (object)i.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(PasswordBox i, object input) => i.SetValue(PlaceholderProperty, input);

        #endregion
        
        #region PlaceholderTemplate

        public static readonly DependencyProperty PlaceholderTemplateProperty = DependencyProperty.RegisterAttached("PlaceholderTemplate", typeof(DataTemplate), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetPlaceholderTemplate(PasswordBox i) => (DataTemplate)i.GetValue(PlaceholderTemplateProperty);
        public static void SetPlaceholderTemplate(PasswordBox i, DataTemplate input) => i.SetValue(PlaceholderTemplateProperty, input);

        #endregion

        #region ToggleTemplate

        public static readonly DependencyProperty ToggleTemplateProperty = DependencyProperty.RegisterAttached("ToggleTemplate", typeof(DataTemplate), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetToggleTemplate(PasswordBox i) => (DataTemplate)i.GetValue(ToggleTemplateProperty);
        public static void SetToggleTemplate(PasswordBox i, DataTemplate input) => i.SetValue(ToggleTemplateProperty, input);

        #endregion

        #region ToggleButtonTemplate

        public static readonly DependencyProperty ToggleButtonTemplateProperty = DependencyProperty.RegisterAttached("ToggleButtonTemplate", typeof(DataTemplate), typeof(XPasswordBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetToggleButtonTemplate(PasswordBox i) => (DataTemplate)i.GetValue(ToggleButtonTemplateProperty);
        public static void SetToggleButtonTemplate(PasswordBox i, DataTemplate input) => i.SetValue(ToggleButtonTemplateProperty, input);

        #endregion

        #region ToggleButtonVisibility

        public static readonly DependencyProperty ToggleButtonVisibilityProperty = DependencyProperty.RegisterAttached("ToggleButtonVisibility", typeof(Visibility), typeof(XPasswordBox), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetToggleButtonVisibility(PasswordBox i) => (Visibility)i.GetValue(ToggleButtonVisibilityProperty);
        public static void SetToggleButtonVisibility(PasswordBox i, Visibility input) => i.SetValue(ToggleButtonVisibilityProperty, input);

        #endregion

        #region TextTrimming

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.RegisterAttached("TextTrimming", typeof(TextTrimming), typeof(XPasswordBox), new FrameworkPropertyMetadata(TextTrimming.None));
        public static TextTrimming GetTextTrimming(PasswordBox i) => (TextTrimming)i.GetValue(TextTrimmingProperty);
        public static void SetTextTrimming(PasswordBox i, TextTrimming input) => i.SetValue(TextTrimmingProperty, input);

        #endregion

        #endregion

        #region XPasswordBox

        static XPasswordBox()
        {
            EventManager.RegisterClassHandler(typeof(PasswordBox), PasswordBox.LoadedEvent,
                new RoutedEventHandler(OnLoaded), true);
            EventManager.RegisterClassHandler(typeof(PasswordBox), PasswordBox.PasswordChangedEvent,
                new RoutedEventHandler(OnActualPasswordChanged), true);
        }

        static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                if (!box.CommandBindings.Contains<CommandBinding>(j => j.Command == ClearCommand))
                {
                    box.CommandBindings.Add(new CommandBinding
                       (ClearCommand,
                       OnClearCommandExecuted,
                       OnClearCommandCanExecute));
                }

                if (!box.CommandBindings.Contains<CommandBinding>(j => j.Command == GenerateCommand))
                {
                    box.CommandBindings.Add(new CommandBinding
                       (GenerateCommand,
                       OnGenerateCommandExecuted,
                       OnGenerateCommandCanExecute));
                }

                //...

                if (GetCopyCommand(box) == null)
                {
                    SetCopyCommand(box, new RelayCommand<PasswordBox>(i =>
                    {
                        i.Focus();
                        i.SelectAll();
                        Clipboard.SetText(i.Password);
                    }, i => i?.Password?.Length > 0));
                }

                if (GetCutCommand(box) == null)
                {
                    SetCutCommand(box, new RelayCommand<PasswordBox>(i =>
                    {
                        Clipboard.SetText(i.Password);
                        i.Password = null;
                    }, i => i?.Password?.Length > 0));
                }

                if (GetPasteCommand(box) == null)
                    SetPasteCommand(box, new RelayCommand<PasswordBox>(i => i.Paste(), i => i is PasswordBox));

                OnActualPasswordChanged(box, new());
            }
        }

        static void OnActualPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
                GetPasswordHandle(box).SafeInvoke(() => SetPassword(box, box.Password));
        }

        #endregion
    }
}