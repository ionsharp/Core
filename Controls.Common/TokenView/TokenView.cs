using Imagin.Common.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Tokens")]
    public class TokenView : RichTextBox
    {
        #region Properties

        bool TextChangeHandled = false;

        bool TokensChangeHandled = false;

        BlockCollection Blocks
        {
            get
            {
                return Document.Blocks;
            }
        }
        
        Run CurrentRun
        {
            get
            {
                var Paragraph = CaretPosition.Paragraph;
                return Paragraph.Inlines.FirstOrDefault(Inline =>
                {
                    var Run = Inline.As<Run>();
                    var Text = CurrentText;

                    if (Run != null && (Run.Text.StartsWith(Text) || Run.Text.EndsWith(Text)))
                        return true;

                    return false;
                }) as Run;
            }
        }

        /// <summary>
        /// Gets the current input text.
        /// </summary>
        string CurrentText
        {
            get
            {
                return CaretPosition?.GetTextInRun(LogicalDirection.Backward);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsCopyPasteEnabledProperty = DependencyProperty.Register("IsCopyPasteEnabled", typeof(bool), typeof(TokenView), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// The <see cref="char"/> used to delimit tokens.
        /// </summary>
        public bool IsCopyPasteEnabled
        {
            get
            {
                return (bool)GetValue(IsCopyPasteEnabledProperty);
            }
            set
            {
                SetValue(IsCopyPasteEnabledProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokenDelimiterProperty = DependencyProperty.Register("TokenDelimiter", typeof(char), typeof(TokenView), new FrameworkPropertyMetadata(';', FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokenDelimiterChanged));
        /// <summary>
        /// The <see cref="char"/> used to delimit tokens.
        /// </summary>
        public char TokenDelimiter
        {
            get
            {
                return (char)GetValue(TokenDelimiterProperty);
            }
            set
            {
                SetValue(TokenDelimiterProperty, value);
            }
        }
        static void OnTokenDelimiterChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<TokenView>().OnTokenDelimiterChanged((char)e.OldValue, (char)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokenizerProperty = DependencyProperty.Register("Tokenizer", typeof(ITokenizer), typeof(TokenView), new FrameworkPropertyMetadata(default(ITokenizer), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// The <see cref="ITokenizer"/> that handles tokenizing.
        /// </summary>
        public ITokenizer Tokenizer
        {
            get
            {
                return (ITokenizer)GetValue(TokenizerProperty);
            }
            set
            {
                SetValue(TokenizerProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokenTriggersProperty = DependencyProperty.Register("TokenTriggers", typeof(TokenTriggerKey), typeof(TokenView), new FrameworkPropertyMetadata(TokenTriggerKey.Return | TokenTriggerKey.Tab, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Keys used to generate tokens when pressed.
        /// </summary>
        public TokenTriggerKey TokenTriggers
        {
            get
            {
                return (TokenTriggerKey)GetValue(TokenTriggersProperty);
            }
            set
            {
                SetValue(TokenTriggersProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokenMouseDownActionProperty = DependencyProperty.Register("TokenMouseDownAction", typeof(TokenMouseAction), typeof(TokenView), new FrameworkPropertyMetadata(TokenMouseAction.Edit, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the action to perform when the mouse is down on token.
        /// </summary>
        public TokenMouseAction TokenMouseDownAction
        {
            get
            {
                return (TokenMouseAction)GetValue(TokenMouseDownActionProperty);
            }
            set
            {
                SetValue(TokenMouseDownActionProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokensProperty = DependencyProperty.Register("Tokens", typeof(string), typeof(TokenView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokensChanged));
        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        public string Tokens
        {
            get
            {
                return (string)GetValue(TokensProperty);
            }
            set
            {
                SetValue(TokensProperty, value);
            }
        }
        static void OnTokensChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<TokenView>().OnTokensChanged((string)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokensSourceProperty = DependencyProperty.Register("TokensSource", typeof(object), typeof(TokenView), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokensSourceChanged));
        /// <summary>
        /// 
        /// </summary>
        public object TokensSource
        {
            get
            {
                return GetValue(TokensSourceProperty);
            }
            set
            {
                SetValue(TokensSourceProperty, value);
            }
        }
        static void OnTokensSourceChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<TokenView>().OnTokensSourceChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TokenStyleProperty = DependencyProperty.Register("TokenStyle", typeof(Style), typeof(TokenView), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokenStyleChanged));
        /// <summary>
        /// 
        /// </summary>
        public Style TokenStyle
        {
            get
            {
                return (Style)GetValue(TokenStyleProperty);
            }
            set
            {
                SetValue(TokenStyleProperty, value);
            }
        }
        static void OnTokenStyleChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<TokenView>().OnTokenStyleChanged((Style)e.NewValue);
        }

        #endregion

        #region TokenView

        /// <summary>
        /// 
        /// </summary>
        public TokenView() : base()
        {
            CommandManager.AddPreviewExecutedHandler(this, OnPreviewExecuted);
            SetCurrentValue(IsDocumentEnabledProperty, true);
            SetCurrentValue(TokenizerProperty, new StringTokenizer());
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// For each token, perform action exposing corresponding <see cref="TokenButton"/>.
        /// </summary>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="Action"></param>
        void Enumerate<TButton>(Func<TButton, bool> Action) where TButton : TokenButton
        {
            Enumerate<InlineUIContainer, TButton>((i, j) => Action(j));
        }

        /// <summary>
        /// For each token, perform action exposing corresponding <see cref="TokenButton"/> and <see cref="Inline"/>.
        /// </summary>
        /// <typeparam name="TInline"></typeparam>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="Action"></param>
        void Enumerate<TInline, TButton>(Func<TInline, TButton, bool> Action) where TInline : Inline where TButton : TokenButton
        {
            Enumerate<Paragraph, TInline, TButton>((p, i, b) => Action(i, b));
        }

        /// <summary>
        /// For each token, perform action exposing corresponding <see cref="TokenButton"/>, <see cref="Inline"/>, and <see cref="Paragraph"/>.
        /// </summary>
        /// <typeparam name="TParagraph"></typeparam>
        /// <typeparam name="TInline"></typeparam>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="Action"></param>
        void Enumerate<TParagraph, TInline, TButton>(Func<TParagraph, TInline, TButton, bool> Action) where TParagraph : Paragraph where TInline : Inline where TButton : TokenButton
        {
            foreach (var i in Blocks)
            {
                if (i is TParagraph)
                {
                    var Continue = true;
                    foreach (var j in i.As<TParagraph>().Inlines)
                    {
                        if (j is TInline)
                        {
                            if (!Action(i as TParagraph, j as TInline, j.As<InlineUIContainer>()?.Child?.As<TButton>() ?? default(TButton)))
                                Continue = false;
                        }
                        if (!Continue)
                            break;
                    }
                    if (!Continue)
                        break;
                }
            }
        }

        //...............................................................................

        InlineUIContainer GetTokenContainer(object Token)
        {
            var Button = new TokenButton(Token);

            if (TokenStyle != null)
                Button.Style = TokenStyle;

            //BaselineAlignment is needed to align with run
            var Result = new InlineUIContainer(Button)
            {
                BaselineAlignment = BaselineAlignment.Center
            };

            return Result;
        }

        Run GetTokenRun(object Token)
        {
            return new Run(Tokenizer.ToString(Token));
        }

        string GetTokenString()
        {
            var Result = new StringBuilder();
            Enumerate<Inline, TokenButton>((i, b) =>
            {
                if (i is InlineUIContainer)
                {
                    Result.Append("{0}{1}".F(Tokenizer.ToString(b?.Content), TokenDelimiter));
                }
                else if (i is Run)
                    Result.Append(i.As<Run>().Text);

                return true;
            });
            return Result.ToString();
        }

        /// <summary>
        /// Replaces the given input text with the given token.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Token"></param>
        void ReplaceWithToken(string Text, object Token)
        {
            TextChangeHandled = true;

            var Paragraph = CaretPosition.Paragraph;

            var currentRun = CurrentRun;
            if (currentRun != null)
            {
                Paragraph.Inlines.InsertBefore(currentRun, GetTokenContainer(Token));
                if (currentRun.Text == Text)
                {
                    Paragraph.Inlines.Remove(currentRun);
                }
                else
                {
                    var Tail = new Run(currentRun.Text.Substring(currentRun.Text.IndexOf(Text) + Text.Length));
                    Paragraph.Inlines.InsertAfter(currentRun, Tail);
                    Paragraph.Inlines.Remove(currentRun);
                }
            }

            TextChangeHandled = false;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            var Field = e.Key.ToString();
            if (e.Key.ToChar() == TokenDelimiter)
            {
                OnTokenTriggered();
                e.Handled = true;
                return;
            }

            var TriggerKey = default(TokenTriggerKey);
            if (Field.TryParseEnum(out TriggerKey) && TokenTriggers.Has(TriggerKey))
            {
                OnTokenTriggered(); 
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            var t = e.Source as TokenButton;
            if (t != null)
            {
                switch (TokenMouseDownAction)
                {
                    case TokenMouseAction.Edit:
                        EditToken(t);
                        e.Handled = true;
                        break;
                    case TokenMouseAction.Remove:
                        RemoveToken(t);
                        e.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (!TextChangeHandled)
            {
                TokensChangeHandled = true;
                Tokens = GetTokenString();
                TokensChangeHandled = false;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Converts the element that hosts the given token to a <see cref="Run"/>.
        /// </summary>
        /// <param name="Token"></param>
        public void EditToken(object Token)
        {
            var Inline = default(Inline);
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b?.Content == Token)
                {
                    Inline = GetTokenRun(Token);

                    p.Inlines.InsertBefore(i, Inline);
                    p.Inlines.Remove(i);

                    return false;
                }
                return true;
            });
            Focus();

            if (Inline != null)
                Selection.Select(Inline.ContentStart, Inline.ContentEnd);
        }

        /// <summary>
        /// Converts the given token element to a <see cref="Run"/>.
        /// </summary>
        /// <param name="Button"></param>
        public void EditToken(TokenButton Button)
        {
            var Inline = default(Inline);
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b == Button)
                {
                    Inline = GetTokenRun(b.Content);

                    p.Inlines.InsertBefore(i, Inline);
                    p.Inlines.Remove(i);

                    return false;
                }
                return true;
            });
            Focus();

            if (Inline != null)
                Selection.Select(Inline.ContentStart, Inline.ContentEnd);
        }

        /// <summary>
        /// Removes the <see cref="Inline"/> that hosts the <see cref="TokenButton"/> corresponding to the given token.
        /// </summary>
        /// <param name="Token"></param>
        public void RemoveToken(object Token)
        {
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b?.Content == Token)
                {
                    p.Inlines.Remove(i);
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Removes the <see cref="Inline"/> that hosts the given <see cref="TokenButton"/>.
        /// </summary>
        /// <param name="Button"></param>
        public void RemoveToken(TokenButton Button)
        {
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b == Button)
                {
                    p.Inlines.Remove(i);
                    return false;
                }
                return true;
            });
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsCopyPasteEnabled && e.Command.EqualsAny(ApplicationCommands.Copy, ApplicationCommands.Cut, ApplicationCommands.Paste))
                e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnTokenDelimiterChanged(char OldValue, char NewValue)
        {
            SetCurrentValue(TokensProperty, Tokens.Replace(OldValue, NewValue));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual async void OnTokensChanged(string Value)
        {
            if (!TokensChangeHandled)
            {
                TextChangeHandled = true;

                await Dispatcher.BeginInvoke(() => Blocks.Clear());
                if (Value?.ToString().IsEmpty() == false)
                {
                    var p = new Paragraph();
                    await Dispatcher.BeginInvoke(() => Tokenizer?.GenerateFrom(Value, TokenDelimiter)?.ForEach(Token => p.Inlines.Add(GetTokenContainer(Token))));
                    Blocks.Add(p);
                }

                TextChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnTokensSourceChanged(object Value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnTokenStyleChanged(Style Value)
        {
            if (Value != null)
            {
                Enumerate<TokenButton>(i =>
                {
                    i.Style = Value;
                    return true;
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        protected virtual void OnTokenTriggered()
        {
            if (!TextChangeHandled)
            {
                var currentText = CurrentText;

                //Attempt to get a token from the current text
                var Token = Tokenizer?.ParseToken(currentText);

                //If a token was created, replace the current text with it
                if (Token != null)
                    ReplaceWithToken(currentText, Token);

                TokensChangeHandled = true;
                Tokens = GetTokenString();
                TokensChangeHandled = false;
            }
        }

        #endregion

        #endregion
    }
}