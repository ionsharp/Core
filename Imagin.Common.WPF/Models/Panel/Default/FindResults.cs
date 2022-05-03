using Imagin.Common.Collections.Generic;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Common.Models
{
    public class FindResult : Base
    {
        int column;
        public int Column
        {
            get => column;
            set => this.Change(ref column, value);
        }

        int index;
        public int Index
        {
            get => index;
            set => this.Change(ref index, value);
        }

        int line;
        public int Line
        {
            get => line;
            set => this.Change(ref line, value);
        }

        IFind file;
        public IFind File
        {
            get => file;
            set => this.Change(ref file, value);
        }

        string text;
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        public FindResult(IFind target, int index, string text, int line, int column) : base()
        {
            File
                = target;
            Index
                = index;
            Text
                = text;
            Line
                = line;
            Column
                = column;
        }
    }

    public class FindResultCollection : ObservableCollection<FindResult>
    {
        public string FindText { get; private set; }

        public FindResultCollection(string findText)
        {
            FindText = findText;
        }
    }

    public class FindResultsPanel : DataPanel
    {
        public static readonly ResourceKey TemplateKey = new();

        #region (IMultiValueConverter) VisibilityConverter

        public static readonly IMultiValueConverter VisibilityConverter = new MultiConverter<Visibility>(i =>
        {
            if (i.Values?.Length >= 2)
            {
                if (i.Values[0] is FindResult result)
                {
                    if (i.Values[1] is FindSource source)
                    {
                        if (source == FindSource.CurrentDocument)
                        {
                            var activeDocument = Get.Where<IDockViewModel>()?.ActiveContent as Document;
                            if (!ReferenceEquals(result.File, activeDocument))
                                return Visibility.Collapsed;
                        }
                        if (i.Values.Length >= 3)
                        {
                            if (i.Values[2] is string search)
                            {
                                if (search.Length > 0)
                                {
                                    if (!result.Text.ToLower().Contains(search.ToLower()))
                                        return Visibility.Collapsed;
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

        FindSource filterSource;
        [Label(false)]
        [Tool]
        public FindSource FilterSource
        {
            get => filterSource;
            set => this.Change(ref filterSource, value);
        }

        [Hidden]
        public override IList GroupNames => new StringCollection()
        {
            "None",
            nameof(FindResult.File)
        };

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Search);

        bool keepResults;
        [Tool]
        public bool KeepResults
        {
            get => keepResults;
            set => this.Change(ref keepResults, value);
        }

        [field: NonSerialized]
        FindResultCollection results = null;
        [Hidden, XmlIgnore]
        public FindResultCollection Results
        {
            get => results;
            set
            {
                this.Change(ref results, value);
                Data = value;
                this.Changed(() => Title);
            }
        }

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

        StringCollection searchHistory = new();
        [Hidden]
        public StringCollection SearchHistory
        {
            get => searchHistory;
            set => this.Change(ref searchHistory, value);
        }

        [Hidden]
        public override IList SortNames => new StringCollection()
        {
            nameof(FindResult.File),
            nameof(FindResult.Line),
            nameof(FindResult.Text)
        };

        bool textWrap = true;
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
        public override string TitleKey => "Find";

        [Hidden]
        public override string TitleSuffix => $" \"{results.FindText}\"";

        #endregion

        #region FindResultsPanel

        public FindResultsPanel() : base() { }

        public FindResultsPanel(FindResultCollection results) : this() => Results = results;

        #endregion

        #region Methods

        void UpdateSearch(string input)
        {
            if (!input.NullOrEmpty())
            {
                if (SearchHistory.Contains(input))
                    SearchHistory.Remove(input);

                SearchHistory.Insert(0, input);
            }
        }

        #endregion

        #region Commands

        [Hidden]
        public override ICommand ClearCommand => base.ClearCommand;

        ICommand copyCommand;
        [DisplayName("Copy")]
        [Icon(Images.Copy)]
        [Tool]
        public ICommand CopyCommand => copyCommand ??= new RelayCommand(() =>
        {
            var result = new StringBuilder();
            foreach (var i in Results)
                result.AppendLine($"{i.Line}: {i.Text}");

            Clipboard.SetText(result.ToString());
        }, 
        () => Results.Count > 0);

        ICommand openResultCommand;
        [Hidden]
        public ICommand OpenResultCommand => openResultCommand ??= new RelayCommand<FindResult>(i =>
        {
            Get.Where<IDockViewModel>().ActiveContent = i.File as Content;
            //Scroll to and select matched text
        },
        i => i != null);

        ICommand searchCommand;
        [Hidden]
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(() => UpdateSearch(Search), () => !Search.NullOrEmpty());

        ICommand searchSuggestionCommand;
        [Hidden]
        public ICommand SearchSuggestionCommand => searchSuggestionCommand ??= new RelayCommand<string>(i => UpdateSearch(i));

        #endregion
    }
}