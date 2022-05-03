using Imagin.Common.Analytics;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class Console : Control, IExplorer
    {
        #region Properties

        public readonly List<BaseCommand> Commands = new();

        //...

        readonly internal Handle handleFolder = false;

        public static readonly DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(Console), new FrameworkPropertyMetadata(null));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }

        public static readonly DependencyProperty HelpButtonTemplateProperty = DependencyProperty.Register(nameof(HelpButtonTemplate), typeof(DataTemplate), typeof(Console), new FrameworkPropertyMetadata(null));
        public DataTemplate HelpButtonTemplate
        {
            get => (DataTemplate)GetValue(HelpButtonTemplateProperty);
            set => SetValue(HelpButtonTemplateProperty, value);
        }

        public static readonly DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(Console), new FrameworkPropertyMetadata(null));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static readonly DependencyProperty LineProperty = DependencyProperty.Register(nameof(Line), typeof(string), typeof(Console), new FrameworkPropertyMetadata(string.Empty));
        public string Line
        {
            get => (string)GetValue(LineProperty);
            set => SetValue(LineProperty, value);
        }

        public static readonly DependencyProperty LinePaddingProperty = DependencyProperty.Register(nameof(LinePadding), typeof(int), typeof(Console), new FrameworkPropertyMetadata(5));
        public int LinePadding
        {
            get => (int)GetValue(LinePaddingProperty);
            set => SetValue(LinePaddingProperty, value);
        }

        public static readonly DependencyProperty OutputProperty = DependencyProperty.Register(nameof(Output), typeof(string), typeof(Console), new FrameworkPropertyMetadata(string.Empty));
        public string Output
        {
            get => (string)GetValue(OutputProperty);
            set => SetValue(OutputProperty, value);
        }

        public static readonly DependencyProperty OutputBackgroundProperty = DependencyProperty.Register(nameof(OutputBackground), typeof(Brush), typeof(Console), new FrameworkPropertyMetadata(Brushes.Transparent));
        public Brush OutputBackground
        {
            get => (Brush)GetValue(OutputBackgroundProperty);
            set => SetValue(OutputBackgroundProperty, value);
        }

        public static readonly DependencyProperty OutputBackgroundImageProperty = DependencyProperty.Register(nameof(OutputBackgroundImage), typeof(ImageSource), typeof(Console), new FrameworkPropertyMetadata(null));
        public ImageSource OutputBackgroundImage
        {
            get => (ImageSource)GetValue(OutputBackgroundImageProperty);
            set => SetValue(OutputBackgroundImageProperty, value);
        }

        public static readonly DependencyProperty OutputBackgroundStretchProperty = DependencyProperty.Register(nameof(OutputBackgroundStretch), typeof(Stretch), typeof(Console), new FrameworkPropertyMetadata(Stretch.Fill));
        public Stretch OutputBackgroundStretch
        {
            get => (Stretch)GetValue(OutputBackgroundStretchProperty);
            set => SetValue(OutputBackgroundStretchProperty, value);
        }

        public static readonly DependencyProperty OutputFontFamilyProperty = DependencyProperty.Register(nameof(OutputFontFamily), typeof(FontFamily), typeof(Console), new FrameworkPropertyMetadata(default(FontFamily)));
        public FontFamily OutputFontFamily
        {
            get => (FontFamily)GetValue(OutputFontFamilyProperty);
            set => SetValue(OutputFontFamilyProperty, value);
        }

        public static readonly DependencyProperty OutputFontSizeProperty = DependencyProperty.Register(nameof(OutputFontSize), typeof(double), typeof(Console), new FrameworkPropertyMetadata(16.0));
        public double OutputFontSize
        {
            get => (double)GetValue(OutputFontSizeProperty);
            set => SetValue(OutputFontSizeProperty, value);
        }

        public static readonly DependencyProperty OutputFontSizeIncrementProperty = DependencyProperty.Register(nameof(OutputFontSizeIncrement), typeof(double), typeof(Console), new FrameworkPropertyMetadata(2.0));
        public double OutputFontSizeIncrement
        {
            get => (double)GetValue(OutputFontSizeIncrementProperty);
            set => SetValue(OutputFontSizeIncrementProperty, value);
        }

        public static readonly DependencyProperty OutputFontSizeMaximumProperty = DependencyProperty.Register(nameof(OutputFontSizeMaximum), typeof(double), typeof(Console), new FrameworkPropertyMetadata(36.0));
        public double OutputFontSizeMaximum
        {
            get => (double)GetValue(OutputFontSizeMaximumProperty);
            set => SetValue(OutputFontSizeMaximumProperty, value);
        }

        public static readonly DependencyProperty OutputFontSizeMinimumProperty = DependencyProperty.Register(nameof(OutputFontSizeMinimum), typeof(double), typeof(Console), new FrameworkPropertyMetadata(8.0));
        public double OutputFontSizeMinimum
        {
            get => (double)GetValue(OutputFontSizeMinimumProperty);
            set => SetValue(OutputFontSizeMinimumProperty, value);
        }

        public static readonly DependencyProperty OutputFontStyleProperty = DependencyProperty.Register(nameof(OutputFontStyle), typeof(FontStyle), typeof(Console), new FrameworkPropertyMetadata(FontStyles.Normal));
        public FontStyle OutputFontStyle
        {
            get => (FontStyle)GetValue(OutputFontStyleProperty);
            set => SetValue(OutputFontStyleProperty, value);
        }

        public static readonly DependencyProperty OutputForegroundProperty = DependencyProperty.Register(nameof(OutputForeground), typeof(Brush), typeof(Console), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush OutputForeground
        {
            get => (Brush)GetValue(OutputForegroundProperty);
            set => SetValue(OutputForegroundProperty, value);
        }

        public static readonly DependencyProperty OutputTextWrappingProperty = DependencyProperty.Register(nameof(OutputTextWrapping), typeof(TextWrapping), typeof(Console), new FrameworkPropertyMetadata(TextWrapping.NoWrap));
        public TextWrapping OutputTextWrapping
        {
            get => (TextWrapping)GetValue(OutputTextWrappingProperty);
            set => SetValue(OutputTextWrappingProperty, value);
        }

        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        #endregion

        #region Console

        public Console() : base()
        {
            SetCurrentValue(HistoryProperty, 
                new History(Explorer.DefaultLimit));

            GetCommands().ForEach(i =>
            {
                var command = i.Create<BaseCommand>();
                command.Console = this;
                Commands.Add(command);
            });
        }

        #endregion

        #region Methods

        IEnumerable<Type> GetCommands() => GetType().GetNestedTypes().Where(i => !i.IsAbstract && i.Inherits<BaseCommand>() && i.GetAttribute<HiddenAttribute>()?.Hidden != true);

        //...

        ICommand executeCommand;
        public ICommand ProcessCommand => executeCommand ??= new RelayCommand<string>(i =>
        {
            Execute(i ?? Line);
            SetCurrentValue(LineProperty, string.Empty);
        }, 
        i => true);

        //...

        Result Parse(string line, out BaseCommand command, out string name)
        {
            command = null;

            name = line?.Split(new char[0])?.FirstOrDefault<string>();
            if (name.NullOrEmpty())
                return new Error($"Cannot parse '{line}'.");

            foreach (var i in Commands)
            {
                foreach (var j in i.Names())
                {
                    if (j.ToLower().Equals(name.ToLower()))
                    {
                        command = i;
                        return new Success();
                    }
                }
            }
            return new Error($"Command '{name}' not found.");
        }

        void Execute(string line)
        {
            var result = Parse(line, out BaseCommand command, out string name);
            if (result is Error error)
            {
                Write($"{Path}> {error.Text}");
                return;
            }

            try
            {
                command.Execute(line);
            }
            catch (Exception e)
            {
                command.Write($"Command '{name}' failed: {e.Message}");
            }
        }

        //...

        public void Write(string input)
        {
            StringBuilder result = new();
            result.Append(Output);
            result.AppendLine(input);

            SetCurrentValue(OutputProperty, result.ToString());
        }

        public void WriteBlock(string input)
        {
            StringBuilder result = new();

            result.Append(Output);
            result.AppendLine(string.Empty);
            result.AppendLine(input);

            SetCurrentValue(OutputProperty, result.ToString());
        }

        public void WriteIndent(string input)
        {
            var result = new StringBuilder();
            result.AppendLine(string.Empty);

            input = input.PadLeft(input.Length + LinePadding, ' ');

            result.AppendLine(input);
            Write(result.ToString());
        }

        //...

        protected virtual void OnFolderChanged(Value<string> input)
        {
            handleFolder.SafeInvoke(() =>
            {
                if (Folder.Long.Exists(input.New))
                    History?.Add(input.New);
            });
        }

        #endregion
    }
}