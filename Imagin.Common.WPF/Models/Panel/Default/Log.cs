using Imagin.Common.Analytics;
using Imagin.Common.Collections;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    [Serializable]
    public class LogPanel : DataPanel
    {
        public static readonly ResourceKey TemplateKey = new();

        enum Category { Category0, Commands0, Filter, Group, Sort, Text, View }

        [Serializable]
        public enum Views { Rows, Text }

        #region Converters

        static IMultiValueConverter countConverter;
        public static IMultiValueConverter CountConverter => countConverter ??= new MultiConverter<string>(i =>
        {
            if (i.Values?.Length == 3)
            {
                if (i.Values[0] is ICollectionChanged a)
                {
                    if (i.Values[2] is ResultTypes b)
                        return $"{a.Count<LogEntry>(i => i.Result.Type == b)}";
                }
            }
            return $"0";
        });

        static IMultiValueConverter visibilityConverter;
        public static IMultiValueConverter VisibilityConverter => visibilityConverter ??= new MultiConverter<Visibility>(i =>
        {
            if (i.Values?.Length >= 3)
            {
                if (i.Values[0] is LogEntry logEntry)
                {
                    if (i.Values[1] is ResultTypes filterType)
                    {
                        if (i.Values[2] is TraceLevel filterLevel)
                        {
                            if (filterType == ResultTypes.None)
                                return Visibility.Collapsed;

                            if (logEntry.Result.Type != ResultTypes.All)
                            {
                                if (!filterType.HasFlag(logEntry.Result.Type))
                                    return Visibility.Collapsed;
                            }
                            if (!filterLevel.HasFlag(logEntry.Level))
                                return Visibility.Collapsed;

                            if (i.Values.Length >= 4)
                            {
                                if (i.Values[3] is string search)
                                {
                                    if (!search.NullOrEmpty())
                                    {
                                        if (!logEntry.Result.Text.ToLower().StartsWith(search.ToLower()))
                                            return Visibility.Collapsed;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Visibility.Visible;
        });

        #endregion

        #region Properties

        [Hidden]
        public int ErrorCount 
            => Data.Count<LogEntry>(i => i.Result is Error);

        [Hidden]
        public int MessageCount 
            => Data.Count<LogEntry>(i => i.Result is Message);

        [Hidden]
        public int SuccessCount 
            => Data.Count<LogEntry>(i => i.Result is Success);

        [Hidden]
        public int WarningCount 
            => Data.Count<LogEntry>(i => i.Result is Warning);

        //...

        [Hidden]
        public ResultTypes Filter
        {
            get
            {
                var result = ResultTypes.None;
                if (FilterError)
                    result = result.AddFlag(ResultTypes.Error);

                if (FilterMessage)
                    result = result.AddFlag(ResultTypes.Message);

                if (FilterSuccess)
                    result = result.AddFlag(ResultTypes.Success);

                if (FilterWarning)
                    result = result.AddFlag(ResultTypes.Warning);

                return result;
            }
        }

        bool filterError = true;
        [Category(Category.Filter)]
        [Content("Errors")]
        [ContentTrigger(nameof(ErrorCount), "Errors ({0})")]
        [Label(false)]
        [Icon(Images.XRound, ThemeKeys.ResultError)]
        [Index(-5)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool FilterError
        {
            get => filterError;
            set => this.Change(ref filterError, value);
        }

        bool filterMessage = true;
        [Category(Category.Filter)]
        [Content("Messages")]
        [ContentTrigger(nameof(MessageCount), "Messages ({0})")]
        [Label(false)]
        [Icon(Images.Info, ThemeKeys.ResultMessage)]
        [Index(-4)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool FilterMessage
        {
            get => filterMessage;
            set => this.Change(ref filterMessage, value);
        }

        bool filterSuccess = true;
        [Category(Category.Filter)]
        [Content("Success")]
        [ContentTrigger(nameof(SuccessCount), "Success ({0})")]
        [Label(false)]
        [Icon(Images.CheckmarkRound, ThemeKeys.ResultSuccess)]
        [Index(-3)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool FilterSuccess
        {
            get => filterSuccess;
            set => this.Change(ref filterSuccess, value);
        }

        bool filterWarning = true;
        [Category(Category.Filter)]
        [Content("Warnings")]
        [ContentTrigger(nameof(WarningCount), "Warnings ({0})")]
        [Label(false)]
        [Icon(Images.Warning, ThemeKeys.ResultWarning)]
        [Index(-2)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool FilterWarning
        {
            get => filterWarning;
            set => this.Change(ref filterWarning, value);
        }

        TraceLevel filterLevel = TraceLevel.All;
        [Category(Category.Category0)]
        [DisplayName("Level")]
        [Tool]
        [Style(EnumStyle.FlagSelect)]
        public TraceLevel FilterLevel
        {
            get => filterLevel;
            set => this.Change(ref filterLevel, value);
        }

        [Hidden]
        public override IList GroupNames => new Collections.ObjectModel.StringCollection()
        {
            "None",
            nameof(LogEntry.Added),
            nameof(LogEntry.Level),
            nameof(LogEntry.Member),
            nameof(LogEntry.Result.Text),
            nameof(LogEntry.Result.Type),
            nameof(LogEntry.Sender),
        };

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Log);

        string search = string.Empty;
        [Command(nameof(SearchCommand))]
        [Label(false)]
        [Featured(AboveBelow.Below)]
        [Icon(Images.Search)]
        [Index(int.MaxValue)]
        [Tool]
        [Placeholder("Search...")]
        [Style(StringStyle.Search)]
        [Suggestions(nameof(SearchHistory), nameof(SearchSuggestionCommand))]
        [UpdateSourceTrigger(UpdateSourceTrigger.LostFocus)]
        [Width(300)]
        public string Search
        {
            get => search;
            set => this.Change(ref search, value);
        }

        Collections.ObjectModel.StringCollection searchHistory = new();
        [Hidden]
        public Collections.ObjectModel.StringCollection SearchHistory
        {
            get => searchHistory;
            private set => this.Change(ref searchHistory, value);
        }

        [Hidden]
        public override IList SortNames => new Collections.ObjectModel.StringCollection()
        {
            nameof(LogEntry.Added),
            nameof(LogEntry.Level),
            nameof(LogEntry.Member),
            nameof(LogEntry.Result.Text),
            nameof(LogEntry.Result.Type),
            nameof(LogEntry.Sender),
        };

        string text = null;
        [Hidden]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        bool textWrap = false;
        [Category(Category.Text)]
        [Label(false)]
        [Icon(Images.ArrowDownLeft)]
        [Index(int.MaxValue - 1)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool TextWrap
        {
            get => textWrap;
            set => this.Change(ref textWrap, value);
        }

        [Hidden]
        public override string TitleKey => "Log";

        Views view = Views.Text;
        [Category(Category.View)]
        [Option]
        public Views View
        {
            get => view;
            set => this.Change(ref view, value);
        }

        #endregion

        #region LogPanel

        public LogPanel(ICollectionChanged input) : base(input) { }

        #endregion

        #region Methods

        void OnLogChanged(object sender, NotifyCollectionChangedEventArgs e) => UpdateText();

        //...
        
        string Format(LogEntry i)
        {
            var result = $"({i.Result.Type}) ";
            if (i.Result is Error parent)
            {
                result += $"{parent.Name}: {parent.Text}";
                result += $"{parent.StackTrace}";

                if (parent.Inner is Error child)
                {
                    result += $" {child.Name}: {child.Text}";
                    result += $" {child.StackTrace}";
                }
            }
            else result += i.Result.Text;
            return result;
        }

        void UpdateSearch(string input)
        {
            if (!input.NullOrEmpty())
            {
                if (SearchHistory.Contains(input))
                    SearchHistory.Remove(input);

                SearchHistory.Insert(0, input);
            }
        }

        string GetText() => Data.Select<object, LogEntry>(i => i as LogEntry).ToString("\n", Format);

        void UpdateText()
        {
            //i => Filter.HasFlag(i.Result.Type) && FilterLevel.HasFlag(i.Level) && (Search.NullOrEmpty() || i.Result.Text.ToLower().StartsWith(Search.ToLower()))
            if (View == Views.Text)
            {
                Text = Data?.Count > 0
                ? GetText()
                : string.Empty;
            }
        }

        //...

        void Subscribe(ICollectionChanged input)
        {
            Unsubscribe(input);
            if (input != null)
            {
                UpdateText();
                input.CollectionChanged += OnLogChanged;
            }
        }

        void Unsubscribe(ICollectionChanged input)
        {
            if (input != null)
            {
                Text = string.Empty;
                input.CollectionChanged -= OnLogChanged;
            }
        }

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Data):
                    Unsubscribe(Data);
                    Subscribe(Data);
                    break;

                case nameof(Count):
                    this.Changed(() => ErrorCount);
                    this.Changed(() => MessageCount);
                    this.Changed(() => SuccessCount);
                    this.Changed(() => WarningCount);
                    break;

                case nameof(Filter):
                case nameof(FilterLevel):
                    UpdateText();
                    break;

                case nameof(FilterError):
                case nameof(FilterMessage):
                case nameof(FilterSuccess):
                case nameof(FilterWarning):
                    this.Changed(() => Filter);
                    break;

                case nameof(View):
                    UpdateText();
                    break;
            }
        }

        [Category(Category.Commands0)]
        [DisplayName("Clear")]
        [Icon(Images.Trash)]
        [Index(2)]
        [Tool]
        new public ICommand ClearCommand => base.ClearCommand;

        ICommand cutCommand;
        [Category(Category.Commands0)]
        [DisplayName("Cut")]
        [Icon(Images.Cut)]
        [Index(0)]
        [Tool]
        public ICommand CutCommand
            => cutCommand ??= new RelayCommand(() => { CopyCommand.Execute(); ClearCommand.Execute(); }, () => Data?.Count > 0);

        ICommand cutSingleCommand;
        [Hidden]
        public ICommand CutSingleCommand
            => cutSingleCommand ??= new RelayCommand<LogEntry>(i => { CopySingleCommand.Execute(i); Data.Remove(i); }, i => i != null);

        ICommand copyCommand;
        [Category(Category.Commands0)]
        [DisplayName("Copy")]
        [Icon(Images.Copy)]
        [Index(1)]
        [Tool]
        public ICommand CopyCommand => copyCommand ??= new RelayCommand(() =>
        {
            switch (View)
            {
                case Views.Rows:
                    Clipboard.SetText(GetText());
                    break;
                case Views.Text:
                    Clipboard.SetText(Text);
                    break;
            }
        },
        () => Data?.Count > 0);

        ICommand copySingleCommand;
        [Hidden]
        public ICommand CopySingleCommand
            => copySingleCommand ??= new RelayCommand<LogEntry>(i => Clipboard.SetText(Format(i)), i => i != null);

        ICommand searchCommand;
        [Hidden]
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(() =>
        {
            UpdateSearch(Search);
            UpdateText();
        },
        () => !Search.NullOrEmpty());

        ICommand searchSuggestionCommand;
        [Hidden]
        public ICommand SearchSuggestionCommand => searchSuggestionCommand ??= new RelayCommand<string>(i =>
        {
            UpdateSearch(i);
            UpdateText();
        });

        #endregion
    }
}