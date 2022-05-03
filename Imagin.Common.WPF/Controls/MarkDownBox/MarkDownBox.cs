using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    #region (enum) BlockTypes

    public enum BlockTypes
    {
        [Type(typeof(Paragraph))]
        Default,
        [Type(typeof(CodeBlock))]
        Code,
        [Type(typeof(HeaderBlockAtx))]
        HeaderAtx,
        [Type(typeof(HeaderBlockStx))]
        HeaderStx,
        [Type(typeof(LineBlock))]
        Line,
        [Type(typeof(List))]
        ListOrdered,
        [Type(typeof(List))]
        ListUnordered,
        [Type(typeof(QuoteBlock))]
        Quote,
        [Type(typeof(Table))]
        Table
    }

    #endregion

    #region (enum) ListTypes

    public enum ListTypes
    {
        Ordered,
        Unordered
    }

    #endregion

    //...

    #region MarkDownBoxExtensions

    public static class MarkDownBoxExtensions
    {
        public static string[] GetColumns(this string input)
            => input.Split(Array<char>.New('|'), StringSplitOptions.RemoveEmptyEntries);

        //...

        public static BlockTypes GetLogicalType(this Block input)
        {
            if (input is List list)
                return list.IsOrdered() ? BlockTypes.ListOrdered : BlockTypes.ListUnordered;

            return input.GetType().GetAttribute<TypeAttribute>()?.Type.As<BlockTypes>() ?? BlockTypes.Default;
        }

        public static string GetText(this Block input)
            => new TextRange(input.ContentStart, input.ContentEnd).Text;

        public static string GetText(this BlockCollection input)
        {
            var result = new StringBuilder();
            foreach (var i in input)
                result.Append(i.GetText());

            return result.ToString();
        }

        //...

        public static Block Check(Block checkBlock, BlockTypes checkType)
        {
            string checkLine = checkBlock.GetText();
            switch (checkType)
            {
                case BlockTypes.Code:
                    return CodeBlock.Check(checkBlock, checkLine);

                case BlockTypes.HeaderAtx:
                    return HeaderBlockAtx.Check(checkBlock, checkType, checkLine);

                case BlockTypes.HeaderStx:
                    return HeaderBlockStx.Check(checkBlock, checkType, checkLine);

                case BlockTypes.Line:
                    return LineBlock.Check(checkBlock, checkLine);

                case BlockTypes.ListOrdered:
                    return Check(ListTypes.Ordered, checkBlock, checkLine);

                case BlockTypes.ListUnordered:
                    return Check(ListTypes.Unordered, checkBlock, checkLine);

                case BlockTypes.Quote:
                    return QuoteBlock.Check(checkBlock, checkLine);

                case BlockTypes.Table:
                    return CheckTable(checkBlock, checkLine);
            }
            return null;
        }

        public static Block Check(ListTypes type, Block block, string line)
        {
            if (type == ListTypes.Ordered)
            {
                var j = false;
                var k = false;

                foreach (var i in line)
                {
                    if (k)
                    {
                        if (i == '.')
                        {
                            j = true;
                            k = false;
                            continue;
                        }
                    }

                    if (!j)
                    {
                        if (!char.IsDigit(i))
                            break;

                        k = true;
                    }

                    if (j)
                    {
                        if (i == ' ')
                            return block;

                        break;
                    }
                }
            }
            if (type == ListTypes.Unordered)
                return line.StartsWith("* ") || line.StartsWith("- ") ? block : null;

            return null;
        }

        /// <summary>
        /// 1 (or more) dash(es) and one (or two) pipe(s) for each column: |-|-|-|-| 
        /// </summary>
        public static Block CheckTable(Block input, string line)
        {
            Block result = null;

            string firstLine = null;
            string secondLine = null;

            int GetSecondColumns(string w)
            {
                var result = 0;
                if (w.Length > 0)
                {
                    bool p = false;
                    foreach (var j in w)
                    {
                        if (j == '|')
                        {
                            if (p)
                                return 0;

                            p = true;
                        }
                        else if (j == '-')
                        {
                            if (result == 0 || p)
                                result++;

                            p = false;
                        }
                        else return 0;
                    }
                    return result;
                }
                return 0;
            }

            //1) If first line of table
            if (input.NextBlock != null && input.NextBlock.GetLogicalType() == BlockTypes.Default)
            {
                result = input;

                firstLine = input.GetText();
                secondLine = input.NextBlock.GetText();

                var columns = GetColumns(firstLine);
                if (columns.Length > 0 && (columns.Length != 1 || line.EndsWith("|")) && columns.Length == GetSecondColumns(secondLine))
                    return result;
            }

            //2) If second line of table
            /*
            else if (input.PreviousBlock != null && input.PreviousBlock.GetLogicalType() == BlockTypes.Default)
            {
                result = input.PreviousBlock;

                firstLine = input.PreviousBlock.GetText();
                secondLine = input.GetText();
            }
            */

            //3) If any other line of table


            //4) If previous block is a table and this a (potentially) new row
            return null;
        }

        //...

        public static string Convert(this Block input)
        {
            var result = new StringBuilder();
            if (input is Paragraph paragraph)
            {
                switch (GetLogicalType(input))
                {
                    case BlockTypes.Code:
                    case BlockTypes.HeaderAtx:
                        goto default;

                    case BlockTypes.Line:
                        result.Append("---");
                        break;

                    case BlockTypes.ListOrdered:
                    case BlockTypes.ListUnordered:
                        foreach (var i in (input as List).ListItems)
                            result.AppendLine($"- {i.Blocks.GetText()}");

                        break;

                    case BlockTypes.Quote:
                        result.Append($"> ");
                        goto default;

                    case BlockTypes.Table:
                        foreach (var i in (input as System.Windows.Documents.Table).RowGroups)
                        {
                        }
                        break;

                    default:
                        foreach (var i in paragraph.Inlines)
                        {
                            if (i is Run run)
                                result.Append(run.Text);
                        }

                        if (input is HeaderBlockStx headerBlock)
                            result.AppendLine(headerBlock.Level == 1 ? "===" : "---");

                        break;
                }
            }
            return result.ToString();
        }

        //...

        public static bool IsOrdered(this List input)
        {
            switch (input.MarkerStyle)
            {
                case TextMarkerStyle.Box:
                case TextMarkerStyle.Circle:
                case TextMarkerStyle.Square:
                case TextMarkerStyle.Disc:
                case TextMarkerStyle.None:
                    return false;

                case TextMarkerStyle.Decimal:
                case TextMarkerStyle.LowerLatin:
                case TextMarkerStyle.LowerRoman:
                case TextMarkerStyle.UpperLatin:
                case TextMarkerStyle.UpperRoman:
                    return true;
            }
            throw new NotSupportedException();
        }

        public static string Trim(this List input, string line)
            => input.IsOrdered() ? line.Substring(line.FirstIndex('.') + 2) : line.Substring(2);
    }

    #endregion

    //...

    #region (abstract) MarkDownBlock

    public abstract class MarkDownBlock : Paragraph
    {
        #region Properties

        public virtual bool CanParse => false;

        public string OriginalLine { get; }

        #endregion

        #region Parsing

        #region (private) (class) ParseElement

        class ParseElement
        {
            public readonly bool Correct = false;

            public bool Started = false;

            public readonly string Prefix = "";

            public readonly string[] Suffixes = null;

            public readonly Func<string, Inline> Parse = null;

            public string Text = "";

            public ParseElement(bool correct, Func<string, Inline> parse, string prefix, params string[] suffixes)
            {
                Correct = correct;
                Prefix = prefix;
                Suffixes = suffixes;
                Parse = parse;
            }
        }

        #endregion

        static string ParseAttribute(string input, string name)
        {
            var j = "";
            var start = false;

            var result = "";
            foreach (var i in input)
            {
                j += i;
                if (j.ToLower().EndsWith($"{name.ToLower()}=\""))
                {
                    start = true;
                    continue;
                }

                if (start)
                {
                    if (i == '"')
                        break;

                    result += i;
                }
            }
            return result;
        }

        static string ParseBlock(string input, string element)
        {
            var j = "";
            var k = "";
            var l = $"</{element}>";

            var a = false;
            foreach (var i in input)
            {
                j += i;
                if (j.EndsWith(">"))
                {
                    a = true;
                    continue;
                }

                if (a)
                {
                    k += i;
                    if (k.EndsWith(l))
                        return k.Substring(0, k.Length - l.Length);
                }
            }

            return null;
        }

        //...

        /// <summary>
        /// Stuff like this: ![alt text](image.jpg)
        /// </summary>
        static ImageInline ParseImage(string input)
        {
            var result = SplitLink(input);
            return result != null ? new ImageInline()
            {
                Child = new Border()
                {
                    Background = Brushes.LightGray,
                    Child = new Image()
                    {
                        Height = double.NaN,
                        Source = new ImageSourceConverter().ConvertFromString(result[1]) as ImageSource,
                        Width = double.NaN
                    },
                    MinHeight = 16,
                    MinWidth = 16
                },
                ToolTip = result[0].NullOrEmpty() ? result[1] : result[0]
            } 
            : default;
        }

        /// <summary>
        /// Stuff like this: <img src="" height="" width="">.
        /// </summary>
        static ImageInline ParseImageHTML(string input)
        {
            var h = ParseAttribute(input, "height").Double();
            h = h == 0 ? double.NaN : h;

            var w = ParseAttribute(input, "width").Double();
            w = w == 0 ? double.NaN : w;

            return new ImageInline()
            {
                Child = new Image()
                {
                    Height = h,
                    Source = new ImageSourceConverter().ConvertFromString(ParseAttribute(input, "src")) as ImageSource,
                    Width = w,
                }
            };
        }

        /// <summary>
        /// Stuff like this: [title](https://www.example.com)
        /// </summary>
        static LinkInline ParseLink(string input)
        {
            var result = SplitLink(input);
            return ParseLink(result[0], result[1]);
        }

        static LinkInline ParseLink(string text, string url)
        {
            var result = new LinkInline(text);

            var onLinkMouseLeftButtonDown = new MouseButtonEventHandler((s, e) => Process.Start(url));
            result.MouseLeftButtonDown += onLinkMouseLeftButtonDown;

            RoutedEventHandler onLinkUnloaded = null;
            onLinkUnloaded = new RoutedEventHandler((s, e) =>
            {
                result.Unloaded -= onLinkUnloaded;
                result.MouseLeftButtonDown -= onLinkMouseLeftButtonDown;
            });

            result.Unloaded += onLinkUnloaded;
            return result;
        }

        /// <summary>
        /// Stuff like this: https://www.example.com
        /// </summary>
        static LinkInline ParseLinkDirect(string input) => ParseLink(input, input);

        /// <summary>
        /// Stuff like this: <a href=""></a>
        /// </summary>
        static LinkInline ParseLinkHTML(string input)
        {
            var url = ParseAttribute(input, "href");
            return ParseLink(ParseBlock(input, "a") ?? url, url);
        }

        /// <summary>
        /// Stuff like this: `text`
        /// </summary>
        static TickInline ParseTick(string input) => new(input.Substring(1, input.Length - 2));

        //...

        static string[] SplitLink(string input)
        {
            var c = "";
            var u = "";
            var s = false;

            var sc = false;
            var su = false;

            foreach (var i in input)
            {
                if (i == '[')
                {
                    sc = true;
                    continue;
                }

                if (i == ']')
                {
                    sc = false;
                    su = true;
                    continue;
                }

                if (sc)
                    c += i;

                if (su)
                {
                    if (i == ')')
                    {
                        s = true;
                        break;
                    }

                    if (i == '(')
                        continue;

                    u += i;
                }
            }
            return s ? Array<string>.New(c, u) : null;
        }

        //...

        static void ApplyStyle(Run run, int style)
        {
            style %= 4;
            style = style < 0 ? 0 : style;
            switch (style)
            {
                case 0:
                    return;
                case 1:
                    run.FontStyle = FontStyles.Italic;
                    return;
                case 2:
                    run.FontWeight = FontWeights.Bold;
                    return;
                case 3:
                    run.FontStyle = FontStyles.Italic;
                    goto case 2;
            }
        }

        //...

        static List<TextInline> Emphasize(TextInline input, ref int style)
        {
            var result = new List<TextInline>();

            TextInline run = null;
            bool decreased = false, increased = false;

            int index = 0;

            var text = "";
            for (var i = 0; i < input.Text.Length; i++)
            {
                text += input.Text[i];
                if ((style == 0 && text[text.Length - 1] == '*') || (increased && text[text.Length - 1] == '*') || (text.Length >= 3 && text.Substring(text.Length - 3, 2) == " *" && text[text.Length - 1] != ' '))
                {
                    text = text.Substring(0, text.Length - 1);

                    if (!text.NullOrEmpty())
                    {
                        run = new TextInline(text);
                        ApplyStyle(run, style);
                        result.Add(run);
                    }

                    text = "";
                    style++;

                    increased = true;
                    decreased = false;
                }
                else
                {
                    var dCondition = -1;
                    if (decreased && text[text.Length - 1] == '*')
                        dCondition = 0;

                    else if (text.Length >= 2 && text.Substring(text.Length - 2, 2) == "* ")
                        dCondition = 1;

                    else if (text.Length >= 2 && text.Substring(text.Length - 2, 2) == "**")
                        dCondition = 2;

                    else if (style > 0 && input.Text.Length > index + 1 && !input.Text.Substring(index + 1).Contains("*") && text[text.Length - 1] == '*')
                        dCondition = 3;

                    if (dCondition >= 0)
                    {
                        if (dCondition == 0)
                        {
                            text = "";
                        }
                        else
                        {
                            if (dCondition == 1)
                                text = text.Substring(0, text.Length - 2) + " ";

                            if (dCondition == 2)
                                text = text.Substring(0, text.Length - 2);

                            if (dCondition == 3)
                                text = text.Substring(0, text.Length - 1);

                            if (!text.NullOrEmpty())
                            {
                                run = new TextInline(text);
                                ApplyStyle(run, style);
                                result.Add(run);
                            }
                            text = "";
                        }

                        style--;

                        decreased = true;
                        increased = false;

                        if (dCondition == 2)
                        {
                            i--;
                            index--;
                        }
                    }
                    else
                    {
                        decreased = false;
                        increased = false;
                    }
                }

                index++;
            }

            if (!text.NullOrEmpty())
            {
                run = new TextInline(text);
                ApplyStyle(run, style);
                result.Add(run);
            }

            var finalResult = new List<TextInline>();

            bool strike = false;
            foreach (var i in result)
            {
                var strikeText = "";
                foreach (var j in i.Text)
                {
                    strikeText += j;
                    if (strikeText.EndsWith("~~"))
                    {
                        var strikeInline = new TextInline(strikeText.Substring(0, strikeText.Length - 2));
                        if (strike)
                            strikeInline.TextDecorations.Add(System.Windows.TextDecorations.Strikethrough);

                        finalResult.Add(strikeInline);

                        strikeText = "";
                        strike = !strike;
                    }
                }

                if (!strikeText.NullOrEmpty())
                    finalResult.Add(new TextInline(strikeText));
            }

            return finalResult;
        }

        //...

        static IEnumerable<Inline> Parse(string line)
        {
            var style = 0;

            var result = ParseElements(line);
            foreach (var i in result)
            {
                if (i is TextInline textInline)
                {
                    foreach (var j in Emphasize(textInline, ref style))
                        yield return j;

                    continue;
                }

                if (i is ImageInline || i is LinkInline || i is TickInline)
                    yield return i;
            }

            yield break;
        }

        static IEnumerable<Inline> ParseElements(string line)
        {
            List<ParseElement> checkElements = new()
            {
                //Image
                new ParseElement(false, ParseImage, "![", ")"),
                //Image (HTML)
                new ParseElement(false, ParseImageHTML, "<img", ">", "/>"),
                //Link
                new ParseElement(false, ParseLink, "[", ")"),
                //Link (Direct)
                new ParseElement(true, ParseLinkDirect, "http://", " "),
                //Link (Direct)
                new ParseElement(true, ParseLinkDirect, "https://", " "),
                //Link (HTML)
                new ParseElement(false, ParseLinkHTML, "<a ", "/>", "</a>"),
                //Tick
                new ParseElement(false, ParseTick, "`", "`"),
            };

            var result = "";
            foreach (var i in line)
            {
                var skip = false;
                foreach (var j in checkElements)
                {
                    if (j.Started)
                    {
                        j.Text += i;
                        if (j.Text.EndsWithAny(j.Suffixes))
                        {
                            j.Started = false;
                            yield return j.Parse(j.Text) ?? new TextInline(j.Text);
                            j.Text = "";
                        }
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;

                result += i;
                foreach (var j in checkElements)
                {
                    if (result.EndsWith(j.Prefix))
                    {
                        j.Started = true;
                        j.Text = j.Prefix;

                        result = result.Substring(0, result.Length - j.Prefix.Length);
                        if (!result.NullOrEmpty())
                            yield return new TextInline(result);

                        result = "";
                    }
                }
            }

            foreach (var j in checkElements)
            {
                if (!j.Text.NullOrEmpty())
                {
                    if (j.Correct)
                    {
                        yield return j.Parse(j.Text);
                        continue;
                    }
                    yield return new TextInline(j.Text);
                }
            }
            if (!result.NullOrEmpty())
                yield return new TextInline(result);

            yield break;
        }

        //...

        public static void Parse(Paragraph input)
        {
            if (input is MarkDownBlock block)
            {
                if (!block.CanParse)
                    return;
            }

            var oldRun
                = input.Inlines.FirstOrDefault<Run>();
            var oldText
                = oldRun.Text;

            input.Inlines.Remove(oldRun);
            foreach (var i in Parse(oldText))
                input.Inlines.Add(i);
        }

        #endregion

        #region Methods

        public virtual string Trim(string input) => input;
        
        #endregion
    }

    #endregion

    #region CodeBlock

    [Type(BlockTypes.Code)]
    public class CodeBlock : MarkDownBlock
    {
        public static Block Check(Block checkBlock, string checkLine)
            => checkLine.StartsWith("    ") ? checkBlock : null;
    }

    //...

    #endregion

    #region (abstract) HeaderBlock

    public abstract class HeaderBlock : MarkDownBlock 
    {
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(nameof(Level), typeof(int), typeof(HeaderBlock), new FrameworkPropertyMetadata(1));
        public int Level
        {
            get => (int)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public static int GetAtxLevel(string input) => input.StartRepeats('#');

        public static int GetStxLevel(string input) => input.TrimWhitespace().OnlyContains('-') ? 2 : input.TrimWhitespace().OnlyContains('=') ? 1 : 0;
    }

    #endregion

    #region HeaderBlockAtx

    [Type(BlockTypes.HeaderAtx)]
    public class HeaderBlockAtx : HeaderBlock
    {
        public override bool CanParse => true;

        public static Block Check(Block checkBlock, BlockTypes checkType, string checkLine)
        {
            return checkLine.StartsWith("# ") 
                || checkLine.StartsWith("## ") 
                || checkLine.StartsWith("### ") 
                || checkLine.StartsWith("#### ") 
                || checkLine.StartsWith("##### ") 
                || checkLine.StartsWith("###### ")
                ? checkBlock : null;
        }
    }

    #endregion

    #region HeaderBlockStx

    [Type(BlockTypes.HeaderStx)]
    public class HeaderBlockStx : HeaderBlock
    {
        public override bool CanParse => true;

        public static Block Check(Block sourceBlock, BlockTypes checkType, string sourceLine)
        {
            //We can move forward if 1) the current and next block match OR 2) the current and previous block match
            if (sourceBlock.NextBlock?.GetLogicalType() == BlockTypes.Default)
            {
                if (!sourceLine.TrimWhitespace().NullOrEmpty() && HeaderBlock.GetStxLevel(sourceBlock.NextBlock.GetText()) > 0)
                    return sourceBlock;
            }
            if (sourceBlock.PreviousBlock?.GetLogicalType() == BlockTypes.Default)
            {
                if (!sourceBlock.PreviousBlock.GetText().TrimWhitespace().NullOrEmpty() && HeaderBlock.GetStxLevel(sourceLine) > 0)
                    return sourceBlock.PreviousBlock;
            }
            return null;
        }
    }

    //...

    #endregion

    #region LineBlock

    [Type(BlockTypes.Line)]
    public class LineBlock : MarkDownBlock 
    {
        /// <summary>
        /// * (3+) or - (3+) with spacing anywhere. Must rule out <see cref="HeaderBlockStx"/> first!
        /// </summary>
        public static Block Check(Block sourceBlock, string sourceLine)
        {
            if (sourceBlock.PreviousBlock == null || (sourceBlock.PreviousBlock.GetLogicalType() == BlockTypes.Default && sourceBlock.PreviousBlock.GetText().TrimWhitespace().NullOrEmpty()))
            {
                int a = 0, d = 0;
                foreach (var i in sourceLine)
                {
                    if (i == ' ')
                    {
                    }
                    else if (i == '*')
                        a++;

                    else if (i == '-')
                        d++;

                    else return null;
                }
                if ((a == 0 && d >= 3) || (a >= 3 && d == 0))
                    return sourceBlock;
            }
            return null;
        }

        public LineBlock() : base()
        {
            var line = new LineElement() { Orientation = Orientation.Horizontal };
            line.Bind(LineElement.WidthProperty, nameof(MarkDownBox.ActualWidth), this);

            Inlines.Add(new InlineUIContainer(line));
        }
    }

    //...

    #endregion

    #region QuoteBlock

    [Type(BlockTypes.Quote)]
    public class QuoteBlock : MarkDownBlock
    {
        public override bool CanParse => true;

        public static Block Check(Block sourceBlock, string sourceLine)
            => sourceLine.StartsWith("> ") ? sourceBlock : null;

        public override string Trim(string input) => input.Substring(2);
    }

    #endregion

    #region TableHeader

    public class TableHeader : TableRow { }

    #endregion

    #region TableHeaderCell

    public class TableHeaderCell : TableCell
    {
        public TableHeaderCell(Block input) : base(input) { }
    }

    #endregion

    //...

    #region MarkDownBox

    public class MarkDownBox : RichTextBox
    {
        #region Properties

        readonly Handle handle = false;

        Block GetCurrent()
            => Document.Blocks.Where(x => x.ContentStart.CompareTo(CaretPosition) == -1 && x.ContentEnd.CompareTo(CaretPosition) == 1).FirstOrDefault() as Paragraph;

        //...
        
        public static readonly DependencyProperty BulletOrderedProperty = DependencyProperty.Register(nameof(BulletOrdered), typeof(Bullets), typeof(MarkDownBox), new FrameworkPropertyMetadata(Bullets.NumberPeriod));
        public Bullets BulletOrdered
        {
            get => (Bullets)GetValue(BulletOrderedProperty);
            set => SetValue(BulletOrderedProperty, value);
        }

        public static readonly DependencyProperty BulletUnorderedProperty = DependencyProperty.Register(nameof(BulletUnordered), typeof(Bullets), typeof(MarkDownBox), new FrameworkPropertyMetadata(Bullets.Circle));
        public Bullets BulletUnordered
        {
            get => (Bullets)GetValue(BulletUnorderedProperty);
            set => SetValue(BulletUnorderedProperty, value);
        }

        public static readonly DependencyProperty FontScaleProperty = DependencyProperty.Register(nameof(FontScale), typeof(double), typeof(MarkDownBox), new FrameworkPropertyMetadata(1.0));
        public double FontScale
        {
            get => (double)GetValue(FontScaleProperty);
            set => SetValue(FontScaleProperty, value);
        }

        public static readonly DependencyProperty ListStyleProperty = DependencyProperty.Register(nameof(ListStyle), typeof(Style), typeof(MarkDownBox), new FrameworkPropertyMetadata(null, OnStyleChanged));
        public Style ListStyle
        {
            get => (Style)GetValue(ListStyleProperty);
            set => SetValue(ListStyleProperty, value);
        }

        public static readonly DependencyProperty ParagraphStyleProperty = DependencyProperty.Register(nameof(ParagraphStyle), typeof(Style), typeof(MarkDownBox), new FrameworkPropertyMetadata(null, OnStyleChanged));
        public Style ParagraphStyle
        {
            get => (Style)GetValue(ParagraphStyleProperty);
            set => SetValue(ParagraphStyleProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(string), typeof(MarkDownBox), new FrameworkPropertyMetadata(null, OnSourceChanged));
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<MarkDownBox>().OnSourceChanged(e);

        public static readonly DependencyProperty TextWrapProperty = DependencyProperty.Register(nameof(TextWrap), typeof(TextWrapping), typeof(MarkDownBox), new FrameworkPropertyMetadata(TextWrapping.Wrap));
        public TextWrapping TextWrap
        {
            get => (TextWrapping)GetValue(TextWrapProperty);
            set => SetValue(TextWrapProperty, value);
        }

        #endregion

        #region MarkDownBox

        public MarkDownBox() : base() { }

        static void OnStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<MarkDownBox>().OnStyleChanged(e);

        #endregion

        #region Methods

        //...

        Paragraph Create(Paragraph a, BlockTypes type)
        {
            var result = type.GetAttribute<TypeAttribute>().Type.As<Type>().Create<Paragraph>();
            Document.Blocks.InsertBefore(a, result);

            for (var i = a.Inlines.Count - 1; i >= 0; i--)
            {
                if (a.Inlines.ElementAt(i) is Run run)
                {
                    var j = run.Text;
                    j = result is MarkDownBlock k ? k.Trim(j) : j;
                    result.As<Paragraph>().Inlines.Add(new Run(j));
                }
            }

            Document.Blocks.Remove(a);

            if (result is HeaderBlockAtx headerBlockAtx)
                headerBlockAtx.Level = HeaderBlockAtx.GetAtxLevel(headerBlockAtx.GetText());

            if (result is HeaderBlockStx headerBlockStx)
            {
                headerBlockStx.Level = HeaderBlockStx.GetStxLevel(headerBlockStx.NextBlock.GetText());
                Document.Blocks.Remove(headerBlockStx.NextBlock);
            }

            MarkDownBlock.Parse(result);
            return result;
        }

        bool CreateList(Paragraph a, BlockTypes bType)
        {
            if (a.PreviousBlock is List previousList && previousList.GetLogicalType() == bType)
            {
                var c = new Paragraph(new Run(previousList.Trim(a.GetText())));
                MarkDownBlock.Parse(c);

                previousList.ListItems.Add(new ListItem(c));
                Document.Blocks.Remove(a);
                return true;
            }
            else
            {
                var result = new List();
                result.As<List>().MarkerStyle = bType == BlockTypes.ListOrdered ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc;

                Document.Blocks.InsertBefore(a, result);

                var newItem = new ListItem();
                result.As<List>().ListItems.Add(newItem);

                var newParagraph = new Paragraph();
                for (var i = a.Inlines.Count - 1; i >= 0; i--)
                {
                    if (a.Inlines.ElementAt(i) is Run run)
                        newParagraph.Inlines.Add(new Run(result.As<List>().Trim(run.Text)));
                }

                MarkDownBlock.Parse(newParagraph);
                newItem.Blocks.Add(newParagraph);

                Document.Blocks.Remove(a);
                return false;
            }
        }

        void CreateList(Paragraph a, BlockTypes bType, ref int index)
        {
            if (CreateList(a, bType))
                index--;
        }

        void CreateTable(Paragraph a)
        {
            var result = new System.Windows.Documents.Table();
            Document.Blocks.InsertBefore(a, result);

            var columns = MarkDownBoxExtensions.GetColumns(a.GetText());
            Document.Blocks.Remove(a);

            var lines = new List<string>();
            for (int i = Document.Blocks.Count() - 1; i >= 0; i--)
            {
                var t = Document.Blocks.ElementAt(i);
                if (ReferenceEquals(t, result))
                    break;

                lines.Insert(0, t.GetText());
            }

            var rowHead = new TableRowGroup();
            result.RowGroups.Add(rowHead);

            var tableHeader = new TableHeader();
            rowHead.Rows.Add(tableHeader);

            foreach (var j in columns)
                tableHeader.Cells.Add(new TableHeaderCell(new Paragraph(new Run(j))));

            var rowBody = new TableRowGroup();
            for (var i = 1; i < lines.Count; i++)
            {
                var c = MarkDownBoxExtensions.GetColumns(lines[i]);
                if (lines[i].EndsWith("|") || c.Length > 0)
                {
                    var row = new TableRow();

                    var column = 0;
                    foreach (var k in columns)
                    {
                        var newCell = new Paragraph(new Run(c[column]));
                        MarkDownBlock.Parse(newCell);

                        row.Cells.Add(new(newCell));
                        column++;
                    }

                    rowBody.Rows.Add(row);

                    continue;
                }
                break;
            }
            result.RowGroups.Add(rowBody);

            foreach (var j in columns)
                result.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Auto) });

            Document.Blocks.Remove(result.NextBlock);
            for (var i = 0; i < rowBody.Rows.Count; i++)
                Document.Blocks.Remove(result.NextBlock);

        }

        //...

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var current = GetCurrent();
            if (e.Key == Key.Back)
            {
                if (current is HeaderBlockStx || current is QuoteBlock)
                {
                    if (CaretPosition == current.ContentStart)
                    {
                        e.Handled = true;
                        var newBlock = Create(current as Paragraph, BlockTypes.Default);
                        CaretPosition = newBlock.ContentStart;
                    }
                }
            }
        }

        void OnTextChanged()
        {
            var current = GetCurrent();
            if (current != null)
            {
                switch (current.GetLogicalType())
                {
                    case BlockTypes.Default:
                        foreach (BlockTypes i in typeof(BlockTypes).GetEnumValues())
                        {
                            if (MarkDownBoxExtensions.Check(current, i) is Paragraph replaceBlock)
                            {
                                switch (i)
                                {
                                    case BlockTypes.Line:
                                        Document.Blocks.InsertBefore(replaceBlock, new LineBlock());
                                        Document.Blocks.Remove(replaceBlock);
                                        break;

                                    case BlockTypes.ListOrdered:
                                    case BlockTypes.ListUnordered:
                                        CreateList(replaceBlock, i);
                                        break;

                                    case BlockTypes.Table:
                                        break;

                                    default:
                                        Create(replaceBlock, i);
                                        break;
                                }
                                break;
                            }
                        }
                        break;

                    case BlockTypes.HeaderAtx:
                    case BlockTypes.Code:
                        if (MarkDownBoxExtensions.Check(current, current.GetLogicalType()) == null)
                        {
                            var newBlock = Create(current as Paragraph, BlockTypes.Default);
                            CaretPosition = newBlock.ContentStart;

                            OnTextChanged();
                        }
                        else if (current is HeaderBlockAtx headerBlock)
                            headerBlock.Level = current.GetText().StartRepeats('#');

                        break;
                }
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            handle.SafeInvoke(() =>
            {
                OnTextChanged();

                var result = new StringBuilder();
                foreach (var i in Document.Blocks)
                    result.AppendLine(i.Convert());

                SetCurrentValue(SourceProperty, result.ToString());
            });
        }

        protected virtual void OnSourceChanged(Value<string> input) => handle.SafeInvoke(() =>
        {
            Document.Blocks.Clear();

            var lines = input.New?.GetLines();
            if (lines == null || lines.Count() == 0)
                return;

            lines.ForEach(i =>
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(i));
                Document.Blocks.Add(paragraph);
            });

            for (var i = 0; i < Document.Blocks.Count; i++)
            {
                if (i >= Document.Blocks.Count)
                    break;

                var checkBlock = Document.Blocks.ElementAt(i);
                Block replaceBlock = null;

                foreach (BlockTypes checkType in typeof(BlockTypes).GetEnumValues())
                {
                    replaceBlock = MarkDownBoxExtensions.Check(checkBlock, checkType);
                    if (replaceBlock != null)
                    {
                        switch (checkType)
                        {
                            case BlockTypes.Line:
                                Document.Blocks.InsertBefore(replaceBlock, new LineBlock());
                                Document.Blocks.Remove(replaceBlock);
                                break;

                            case BlockTypes.ListOrdered:
                            case BlockTypes.ListUnordered:
                                CreateList(replaceBlock as Paragraph, checkType, ref i);
                                break;

                            case BlockTypes.Table:
                                CreateTable(replaceBlock as Paragraph);
                                break;

                            default:
                                Create(replaceBlock as Paragraph, checkType);
                                break;
                        }
                        break;
                    }
                }

                if (replaceBlock == null)
                    MarkDownBlock.Parse(checkBlock as Paragraph);
            }
        });

        protected virtual void OnStyleChanged(Value<Style> input)
        {
            if (input.Old != null)
                Resources.Remove(input.Old.TargetType);

            if (input.New != null)
                Resources.Add(input.New.TargetType, input.New);
        }

        #endregion
    }

    #endregion
}