using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Imagin.Common.Controls
{
    [ContentProperty(nameof(Source))]
    public class TokenBox : RichTextBox
    {
        #region Properties

        readonly Handle handle = false;

        //...

        BlockCollection Blocks => Document.Blocks;
        
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
        string CurrentText => CaretPosition?.GetTextInRun(LogicalDirection.Backward);

        //...

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(TokenBox), new FrameworkPropertyMetadata(string.Empty));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(string), typeof(TokenBox), new FrameworkPropertyMetadata(string.Empty, OnSourceChanged));
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        static void OnSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TokenBox>().OnSourceChanged(new Value<string>(e));

        public static readonly DependencyProperty TokenDelimiterProperty = DependencyProperty.Register(nameof(TokenDelimiter), typeof(char), typeof(TokenBox), new FrameworkPropertyMetadata(';', OnTokenDelimiterChanged));
        /// <summary>
        /// The <see cref="char"/> used to delimit tokens.
        /// </summary>
        public char TokenDelimiter
        {
            get => (char)GetValue(TokenDelimiterProperty);
            set => SetValue(TokenDelimiterProperty, value);
        }
        static void OnTokenDelimiterChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TokenBox>().OnTokenDelimiterChanged(new Value<char>(e));

        public static readonly DependencyProperty TokenizerProperty = DependencyProperty.Register(nameof(Tokenizer), typeof(ITokenize), typeof(TokenBox), new FrameworkPropertyMetadata(default(ITokenize)));
        /// <summary>
        /// The <see cref="ITokenize"/> that handles tokenizing.
        /// </summary>
        public ITokenize Tokenizer
        {
            get => (ITokenize)GetValue(TokenizerProperty);
            set => SetValue(TokenizerProperty, value);
        }

        public static readonly DependencyProperty TokenTriggersProperty = DependencyProperty.Register(nameof(TokenTriggers), typeof(TokenTriggerKey), typeof(TokenBox), new FrameworkPropertyMetadata(TokenTriggerKey.Return | TokenTriggerKey.Tab));
        /// <summary>
        /// Keys used to generate tokens when pressed.
        /// </summary>
        public TokenTriggerKey TokenTriggers
        {
            get => (TokenTriggerKey)GetValue(TokenTriggersProperty);
            set => SetValue(TokenTriggersProperty, value);
        }

        public static readonly DependencyProperty TokenMouseDownActionProperty = DependencyProperty.Register(nameof(TokenMouseDownAction), typeof(TokenMouseAction), typeof(TokenBox), new FrameworkPropertyMetadata(TokenMouseAction.Edit));
        /// <summary>
        /// Gets or sets the action to perform when the mouse is down on token.
        /// </summary>
        public TokenMouseAction TokenMouseDownAction
        {
            get => (TokenMouseAction)GetValue(TokenMouseDownActionProperty);
            set => SetValue(TokenMouseDownActionProperty, value);
        }

        static readonly DependencyPropertyKey TokensKey = DependencyProperty.RegisterReadOnly(nameof(Tokens), typeof(ObjectCollection), typeof(TokenBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty TokensProperty = TokensKey.DependencyProperty;
        public ObjectCollection Tokens
        {
            get => (ObjectCollection)GetValue(TokensProperty);
            private set => SetValue(TokensKey, value);
        }

        public static readonly DependencyProperty TokenStyleProperty = DependencyProperty.Register(nameof(TokenStyle), typeof(Style), typeof(TokenBox), new FrameworkPropertyMetadata(default(Style), OnTokenStyleChanged));
        public Style TokenStyle
        {
            get => (Style)GetValue(TokenStyleProperty);
            set => SetValue(TokenStyleProperty, value);
        }
        static void OnTokenStyleChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TokenBox>().OnTokenStyleChanged(new Value<Style>(e));

        #endregion

        #region TokenBox

        public TokenBox() : base()
        {
            SetCurrentValue(IsDocumentEnabledProperty,
                true);
            SetCurrentValue(TokenizerProperty, 
                new StringTokenizer());

            Tokens = new();
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
                            if (!Action(i as TParagraph, j as TInline, j.As<InlineUIContainer>()?.Child?.As<TButton>() ?? default))
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

        //...

        /// <summary>
        /// Generates an <see cref="Inline"/> element to host the given token.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        InlineUIContainer GenerateInline(object token)
        {
            OnTokenAdded(token);
            return new InlineUIContainer(new TokenButton(token)) { BaselineAlignment = BaselineAlignment.Center }; //Needed to align with run
        }

        /// <summary>
        /// Generates a <see cref="Run"/> to host the <see cref="string"/> representation of the given token.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        Run GenerateRun(object token)
        {
            OnTokenRemoved(token);
            return new Run(Tokenizer.ToString(token));
        }

        //...

        string ParseInlines()
        {
            var result = new StringBuilder();
            Enumerate<Inline, TokenButton>((inline, button) =>
            {
                if (inline is InlineUIContainer)
                    result.Append("{0}{1}".F(Tokenizer.ToString(button?.Content), TokenDelimiter));

                else if (inline is Run run)
                    result.Append(run.Text);

                return true;
            });
            return result.ToString();
        }

        //...

        /// <summary>
        /// Converts the given <see cref="TokenButton"/> to a <see cref="Run"/>.
        /// </summary>
        /// <param name="Button"></param>
        void EditToken(TokenButton input)
        {
            var result = default(Inline);
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((paragraph, inline, button) =>
            {
                if (button == input)
                {
                    result = GenerateRun(button.Content);

                    paragraph.Inlines.InsertBefore(inline, result);
                    paragraph.Inlines.Remove(inline);
                    return false;
                }
                return true;
            });
            Focus();

            if (result != null)
                Selection.Select(result.ContentStart, result.ContentEnd);
        }

        void IntersectTokens()
        {
            var tokens = new List<object>();
            Enumerate<TokenButton>(button =>
            {
                tokens.Add(button.Content);
                return true;
            });

            var result = Tokens.Intersect(tokens).ToList();

            Tokens.Clear();
            while (result.Any<object>())
            {
                foreach (var i in result)
                {
                    result.Remove(i);
                    Tokens.Add(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the <see cref="Inline"/> containing the given <see cref="TokenButton"/>.
        /// </summary>
        /// <param name="input"></param>
        void RemoveToken(TokenButton input)
        {
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((paragraph, inline, button) =>
            {
                if (button == input)
                {
                    paragraph.Inlines.Remove(inline);
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Replaces the given input text with the given token.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Token"></param>
        void ReplaceWithToken(string input, object token)
        {
            var paragraph = CaretPosition.Paragraph;

            var currentRun = CurrentRun;
            if (currentRun != null)
            {
                paragraph.Inlines.InsertBefore(currentRun, GenerateInline(token));
                if (currentRun.Text == input)
                    paragraph.Inlines.Remove(currentRun);

                else
                {
                    var tail = new Run(currentRun.Text.Substring(currentRun.Text.IndexOf(input) + input.Length));
                    paragraph.Inlines.InsertAfter(currentRun, tail);
                    paragraph.Inlines.Remove(currentRun);
                }
            }
        }

        #endregion

        #region Overrides

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key.Character() == TokenDelimiter || $"{e.Key}".TryParse(out TokenTriggerKey key) && TokenTriggers.HasAllFlags(key))
            {
                OnTokenTriggered();
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (e.Source is TokenButton button)
            {
                switch (TokenMouseDownAction)
                {
                    case TokenMouseAction.Edit:
                        EditToken(button);
                        break;
                    case TokenMouseAction.Remove:
                        RemoveToken(button);
                        break;
                }
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            handle.SafeInvoke(() =>
            {
                IntersectTokens();
                Source = ParseInlines();
            });
        }

        #endregion

        #region Virtual

        protected virtual void OnTokenDelimiterChanged(Value<char> input) => SetCurrentValue(SourceProperty, Source.Replace(input.Old, input.New));

        protected virtual void OnTokenAdded(object token) => Tokens.Add(token);

        protected virtual void OnTokenRemoved(object token) => Tokens.Remove(token);
        
        protected virtual void OnSourceChanged(Value<string> input)
        {
            handle.SafeInvoke(() =>
            {
                if (input.New != null)
                {
                    //Remove repeating delimiters
                    var clean = Regex.Replace(input.New, $"{TokenDelimiter}+", $"{TokenDelimiter}");
                    if (clean != input.New)
                        SetCurrentValue(SourceProperty, clean);
                }

                Blocks.Clear();
                Tokens.Clear();

                if (input.New?.ToString().Empty() == false)
                {
                    var paragraph = new Paragraph();
                    Tokenizer?.Tokenize(input.New, TokenDelimiter)?.ForEach(Token => paragraph.Inlines.Add(GenerateInline(Token)));
                    Blocks.Add(paragraph);
                }
            });
        }

        protected virtual void OnTokenStyleChanged(Value<Style> input)
        {
            if (input.Old != null)
                Resources.Remove(input.Old.TargetType);

            if (input.New != null)
                Resources.Add(input.New.TargetType, input.New);
        }

        protected virtual void OnTokenTriggered()
        {
            handle.SafeInvoke(() =>
            {
                var currentText = CurrentText;

                //Attempt to get token from current text
                var token = Tokenizer?.ToToken(currentText);

                //If token was created, replace current text with it
                if (token != null)
                    ReplaceWithToken(currentText, token);

                SetCurrentValue(SourceProperty, ParseInlines());
            });
        }

        #endregion

        #endregion
    }
}