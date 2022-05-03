using Imagin.Common.Analytics;
using Imagin.Common.Threading;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(RichTextBox))]
    public static class XRichTextBox
    {
        #region (enum) Actions

        enum Actions
        {
            Load,
            Save
        }

        #endregion

        #region (class) ActionData

        class ActionData
        {
            public readonly Actions Action;

            public readonly RichTextBox Control;

            public readonly string Path;

            public ActionData(RichTextBox control, Actions action, string path)
            {
                Control
                    = control;
                Action
                    = action;
                Path
                    = path;
            }
        }

        #endregion

        #region Properties

        #region AutoSave

        public static readonly DependencyProperty AutoSaveProperty = DependencyProperty.RegisterAttached("AutoSave", typeof(bool), typeof(XRichTextBox), new FrameworkPropertyMetadata(false));
        public static bool GetAutoSave(RichTextBox i) => (bool)i.GetValue(AutoSaveProperty);
        public static void SetAutoSave(RichTextBox i, bool input) => i.SetValue(AutoSaveProperty, input);

        #endregion

        #region Queue

        static readonly DependencyProperty QueueProperty = DependencyProperty.RegisterAttached("Queue", typeof(CancelTask<ActionData>), typeof(XRichTextBox), new FrameworkPropertyMetadata(null));
        static CancelTask<ActionData> GetQueue(RichTextBox i) => i.GetValueOrSetDefault(QueueProperty, () => new CancelTask<ActionData>(null, OnAction, false));

        #endregion

        #region (ReadOnly) Loading

        static readonly DependencyPropertyKey LoadingKey = DependencyProperty.RegisterAttachedReadOnly("Loading", typeof(bool), typeof(XRichTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty LoadingProperty = LoadingKey.DependencyProperty;
        public static bool GetLoading(RichTextBox i) => (bool)i.GetValue(LoadingProperty);
        static void SetLoading(RichTextBox i, bool input) => i.SetValue(LoadingKey, input);

        #endregion

        #region (ReadOnly) IsModified

        static readonly DependencyPropertyKey IsModifiedKey = DependencyProperty.RegisterAttachedReadOnly("IsModified", typeof(bool), typeof(XRichTextBox), new FrameworkPropertyMetadata(false, OnIsModifiedChanged));
        public static readonly DependencyProperty IsModifiedProperty = IsModifiedKey.DependencyProperty;
        public static bool GetIsModified(RichTextBox i) => (bool)i.GetValue(IsModifiedProperty);
        static void OnIsModifiedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is RichTextBox box)
                box.RaiseEvent(new(ModifiedEvent, box));
        }

        #endregion

        #region (RoutedEvent) Modified

        public static readonly RoutedEvent ModifiedEvent = EventManager.RegisterRoutedEvent("Modified", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RichTextBox));
        public static void AddModifiedHandler(DependencyObject i, RoutedEventHandler handler)
            => i.As<RichTextBox>().AddHandler(ModifiedEvent, handler);
        public static void RemoveModifiedHandler(DependencyObject i, RoutedEventHandler handler)
            => i.As<RichTextBox>().RemoveHandler(ModifiedEvent, handler);

        #endregion

        #region (ReadOnly) Saving

        static readonly DependencyPropertyKey SavingKey = DependencyProperty.RegisterAttachedReadOnly("Saving", typeof(bool), typeof(XRichTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty SavingProperty = SavingKey.DependencyProperty;
        public static bool GetSaving(RichTextBox i) => (bool)i.GetValue(SavingProperty);
        static void SetSaving(RichTextBox i, bool input) => i.SetValue(SavingKey, input);

        #endregion

        #region Path

        public static readonly DependencyProperty PathProperty = DependencyProperty.RegisterAttached("Path", typeof(string), typeof(XRichTextBox), new FrameworkPropertyMetadata(null, OnPathChanged));
        public static string GetPath(RichTextBox i) => (string)i.GetValue(PathProperty);
        public static void SetPath(RichTextBox i, string input) => i.SetValue(PathProperty, input);
        static void OnPathChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is RichTextBox box)
                box.Load();
        }

        #endregion

        #endregion

        #region XRichTextBox

        static XRichTextBox()
        {
            EventManager.RegisterClassHandler(typeof(RichTextBox), RichTextBox.PreviewKeyDownEvent,
               new KeyEventHandler(OnPreviewKeyDown), true);
            EventManager.RegisterClassHandler(typeof(RichTextBox), RichTextBox.TextChangedEvent,
               new TextChangedEventHandler(OnTextChanged), true);
        }

        async static Task OnAction(ActionData data, CancellationToken token)
        {
            switch (data.Action)
            {
                case Actions.Load:
                    SetLoading(data.Control, true);
                    await Dispatch.InvokeAsync(() => data.Control.Load(data.Path));
                    SetLoading(data.Control, false);
                    break;

                case Actions.Save:
                    SetSaving(data.Control, true);
                    await Dispatch.InvokeAsync(() => data.Control.Save(data.Path));
                    SetSaving(data.Control, false);
                    break;
            }
            data.Control.SetValue(IsModifiedKey, false);
        }

        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is RichTextBox box)
            {
                if (ModifierKeys.Control.Pressed())
                {
                    if (e.Key == Key.S)
                        box.Save();
                }
            }
        }

        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox box)
            {
                box.SetValue(IsModifiedKey, true);
                if (GetAutoSave(box))
                    box.Save();
            }
        }

        #endregion

        #region Methods

        public static bool Empty(this RichTextBox input)
        {
            string result = new TextRange(input.Document.ContentStart, input.Document.ContentEnd).Text;
            if (!result.NullOrWhiteSpace() && !result.NullOrEmpty())
                return false;

            if (input.Document.Blocks.OfType<BlockUIContainer>().Any())
                return false;

            var p = input.Document.Blocks.OfType<Paragraph>();
            foreach (var i in p)
            {
                if (i.Inlines.Contains(j => j is InlineUIContainer || (j is Run k && !k.Text.NullOrWhiteSpace())))
                    return false;
            }
            return true;
        }

        //...

        public static Result Load(this RichTextBox input, string filePath)
        {
            Result result = true;
            try
            {
                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate))
                {
                    var textRange = new TextRange(input.Document.ContentStart, input.Document.ContentEnd);
                    textRange.Load(fileStream, DataFormats.Rtf);
                }
            }
            catch (Exception e)
            {
                Log.Write<TextBlock>(e);
                result = e;
            }

            return result;
        }

        public static void Load(this RichTextBox input) => _ = GetQueue(input).StartAsync(new ActionData(input, Actions.Load, GetPath(input)));

        //...

        public static Result Save(this RichTextBox input, string filePath)
        {
            Result result = true;
            try
            {
                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    var textRange = new TextRange(input.Document.ContentStart, input.Document.ContentEnd);
                    textRange.Save(fileStream, DataFormats.Rtf);
                }
            }
            catch (Exception e)
            {
                Log.Write<RichTextBox>(e);
                result = e;
            }
            return result;
        }

        public static void Save(this RichTextBox input) => _ = GetQueue(input).StartAsync(new ActionData(input, Actions.Save, GetPath(input)));

        #endregion
    }
}