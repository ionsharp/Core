using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class FindControl : Control
    {
        #region Properties

        public Document ActiveDocument 
            => Get.Where<IDockViewModel>()?.ActiveDocument;

        public DocumentCollection Documents 
            => Get.Where<IDockViewModel>()?.Documents;

        //...

        #region FindText

        public static readonly DependencyProperty FindTextProperty = DependencyProperty.Register(nameof(FindText), typeof(string), typeof(FindControl), new FrameworkPropertyMetadata(string.Empty));
        public string FindText
        {
            get => (string)GetValue(FindTextProperty);
            set => SetValue(FindTextProperty, value);
        }

        #endregion

        #region MatchCase

        public static readonly DependencyProperty MatchCaseProperty = DependencyProperty.Register(nameof(MatchCase), typeof(bool), typeof(FindControl), new FrameworkPropertyMetadata(false));
        public bool MatchCase
        {
            get => (bool)GetValue(MatchCaseProperty);
            set => SetValue(MatchCaseProperty, value);
        }

        #endregion

        #region MatchWord

        public static readonly DependencyProperty MatchWordProperty = DependencyProperty.Register(nameof(MatchWord), typeof(bool), typeof(FindControl), new FrameworkPropertyMetadata(false));
        public bool MatchWord
        {
            get => (bool)GetValue(MatchWordProperty);
            set => SetValue(MatchWordProperty, value);
        }

        #endregion

        #region Model

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(IDockViewModel), typeof(FindControl), new FrameworkPropertyMetadata(null));
        public IDockViewModel Model
        {
            get => (IDockViewModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        #endregion

        #region ReplaceText

        public static readonly DependencyProperty ReplaceTextProperty = DependencyProperty.Register(nameof(ReplaceText), typeof(string), typeof(FindControl), new FrameworkPropertyMetadata(string.Empty));
        public string ReplaceText
        {
            get => (string)GetValue(ReplaceTextProperty);
            set => SetValue(ReplaceTextProperty, value);
        }

        #endregion

        #region ResultsCommand

        public static readonly DependencyProperty ResultsCommandProperty = DependencyProperty.Register(nameof(ResultsCommand), typeof(ICommand), typeof(FindControl), new FrameworkPropertyMetadata(null));
        public ICommand ResultsCommand
        {
            get => (ICommand)GetValue(ResultsCommandProperty);
            set => SetValue(ResultsCommandProperty, value);
        }

        #endregion

        #region Source

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(FindSource), typeof(FindControl), new FrameworkPropertyMetadata(FindSource.CurrentDocument));
        public FindSource Source
        {
            get => (FindSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        #endregion

        #endregion

        #region FindReplaceControl

        public FindControl() : base() { }

        #endregion

        #region Methods

        #region Old code

        /*
        void NoMatches() => Dialog.Show(DialogTitle, "No matches...", DialogImage.Exclamation, Buttons.Ok);

        //...

        bool Supported(Document input) => input is IFind;

        IEnumerable<FindMatch> FindAll(bool hello)
        {
            OriginalSelection = null;
            switch (Source)
            {
                case FindSource.CurrentDocument:

                    if (Supported(ActiveDocument))
                    {
                        foreach (var i in FindAll((IFind)ActiveDocument))
                            yield return new FindMatch((IFind)ActiveDocument, i);
                    }

                    break;

                case FindSource.AllDocuments:
                    foreach (var i in Documents)
                    {
                        if (Supported(i))
                        {
                            foreach (var j in FindAll((IFind)i))
                                yield return new FindMatch((IFind)i, j);
                        }
                    }
                    break;
            }
        }

        IEnumerable<int> FindAll(IFind document)
        {
            int start = 0;

            var next = 0;
            while (true)
            {
                next = FindNext(new FindRegion(document, start, FindText.Length), true)?.Start ?? -1;

                if (next == -1)
                    break;

                yield return next;
                start = next + FindText.Length;
            }
            yield break;
        }

        //...

        string Replace(int index, string input)
        {
            if (index < 0)
                return input;

            return $"{input.Substring(0, index)}{ReplaceText}{input.Substring(index + FindText.Length, input.Length - (index + FindText.Length))}";
        }

        //...
        */

        /*

        FindRegion FindNext(FindRegion input, bool skip)
        {
            switch (Source)
            {
                case FindSource.CurrentDocument:
                    return NextCurrent(input, true);

                case FindSource.AllDocuments:
                    return NextAll(input, skip);

                case FindSource.Selection:
                    break;
            }
            return null;
        }

        FindRegion FindPrevious(FindRegion input)
        {
            switch (Source)
            {
                case FindSource.CurrentDocument:
                    return PreviousCurrent(input, true);

                case FindSource.AllDocuments:
                    return PreviousAll(input);

                case FindSource.Selection:
                    break;
            }
            return null;
        }

        //...

        /// <summary>
        /// Searches from the end of the current selection to the end of the document, then from the beginning of the same document to the beginning of the current selection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FindRegion NextCurrent(FindRegion input, bool circular)
        {
            /*
            if (MatchWord)
            {
                var a = document.Text[result + FindText.Length];
                var b = document.Text[result - 1];
                if (a.ToString().AlphaNumeric() || b.ToString().AlphaNumeric())
                    result = -1;
            }

            var start = input.Start + input.Length;
            //If we're already at the end of the document
            if (input.Start == input.Document.Text.Length)
            {
                //If we're allowed to circle back
                if (circular)
                {
                    //Circle back now versus later
                    start = 0;
                    circular = false;
                }
            }

            var j = 0;
            for (int i = start, count = input.Document.Text.Length; i < count; i++)
            {
                //Get the next character
                var character = input.Document.Text[i];
                //If we did not match all characters
                if (j < FindText.Length)
                {
                    var a = character;
                    var b = FindText[j];

                    if (!MatchCase)
                    {
                        a = $"{a}".ToLower()[0];
                        b = $"{b}".ToLower()[0];
                    }

                    if (a == b)
                    {
                        j++;
                    }
                    else
                    {
                        j = 0;
                    }
                }
                //If we did match all characters
                if (j == FindText.Length)
                {
                    return new FindRegion(input.Document, i - FindText.Length + 1, FindText.Length);
                }

                //This will be null when calling FindAll()
                if (OriginalSelection != null)
                {
                    //We didn't match all characters!
                    //If we reached the beginning of the original selection
                    if (OriginalSelection.Start - 1 >= 0)
                    {
                        if (i == OriginalSelection.Start - 1)
                            break;
                    }
                    else
                    {
                        if (i == input.Document.Text.Length - 1)
                        {
                            break;
                        }
                    }
                }

                //Circle back!
                if (i == input.Document.Text.Length - 1)
                {
                    if (!circular)
                        break;

                    i = -1;
                    j = 0;
                    count = input.Start;
                }
            }
            return null;
        }

        /// <summary>
        /// Searches from the beginning of the current selection to the beginning of the document, then from the end of the same document to the end of the current selection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FindRegion PreviousCurrent(FindRegion input, bool circular)
        {
            var start = input.Start - 1;
            //If we're already at the beginning of the document
            if (input.Start == 0)
            {
                //If we're allowed to circle back
                if (circular)
                {
                    //Circle back now versus later
                    start = input.Document.Text.Length - 1;
                    circular = false;
                }
            }

            var j = 0;
            for (int i = start, count = 0; i >= count; i--)
            {
                //Get the next character
                var character = input.Document.Text[i];
                //If we did not match all characters
                if (j >= 0)
                {
                    var a = character;
                    var b = FindText[j];

                    if (!MatchCase)
                    {
                        a = $"{a}".ToLower()[0];
                        b = $"{b}".ToLower()[0];
                    }

                    if (a == b)
                    {
                        j--;
                    }
                    else
                    {
                        j = FindText.Length - 1;
                    }
                }
                //If we did match all characters
                if (j < 0)
                {
                    return new FindRegion(input.Document, i, FindText.Length);
                }

                //We didn't match all characters!
                //If we reached the end of the original selection
                if (i == OriginalSelection.Start + OriginalSelection.Length)
                    break;

                //Circle back!
                if (i == 0)
                {
                    if (!circular)
                        break;

                    i = input.Document.Text.Length;
                    j = FindText.Length - 1;
                    count = input.Start + input.Length;
                }
            }
            return null;
        }

        //...

        FindRegion NextAll(FindRegion input, bool skip)
        {
            var document = input.Document;
            while (document != null)
            {
                var result
                    = document == input.Document
                    ? NextCurrent(input, false)
                    : NextCurrent(new FindRegion(document, 0, 0), false);

                if (result != null)
                    return result;

                if (skip)
                    break;

                document = NextDocument(document);
                if (document == input.Document)
                    break;
            }
            return null;
        }

        FindRegion PreviousAll(FindRegion input)
        {
            var document = input.Document;
            while (document != null)
            {
                var result
                    = document == input.Document
                    ? PreviousCurrent(input, false)
                    : PreviousCurrent(new FindRegion(document, 0, 0), false);

                if (result != null)
                    return result;

                document = PreviousDocument(document);
                if (document == input.Document)
                    break;
            }
            return null;
        }

        //...

        IFind PreviousDocument(IFind input)
        {
            bool circular = Documents.IndexOf(input as Document) < Documents.Count - 1;
            for (int i = Documents.IndexOf(input as Document), count = 0; i >= count; i--)
            {
                var document = (Document)Documents[i];
                if (document != input)
                {
                    if (Supported(document))
                    {
                        return document as IFind;
                    }
                }
                if (i == 0)
                {
                    i = Documents.Count;
                    count = Documents.IndexOf(input as Document) + 1;
                }
            }
            return null;
        }

        IFind NextDocument(IFind input)
        {
            bool circular = Documents.IndexOf(input as Document) > 0;
            for (int i = Documents.IndexOf(input as Document), count = Documents.Count; i < count; i++)
            {
                var document = (Document)Documents[i];
                if (document != input)
                {
                    if (Supported(document))
                        return document as IFind;
                }
                if (circular)
                {
                    if (i == Documents.Count - 1)
                    {
                        i = -1;
                        count = Documents.IndexOf(input as Document);
                    }
                }
            }
            return null;
        }

        //...
        */

        #endregion

        bool Can()
        {
            if (!FindText.NullOrEmpty())
            {
                if (Documents?.Any<Document>() == true)
                {
                    switch (Source)
                    {
                        case FindSource.AllDocuments:
                            foreach (var i in Documents)
                            {
                                if (i is IFind)
                                    return true;
                            }
                            return false;

                        case FindSource.CurrentDocument:
                            return ActiveDocument is IFind;

                        case FindSource.Selection:
                            return ActiveDocument is IFind j && j.SelectionLength > 0;
                    }
                }
            }
            return false;
        }

        static List<int> FindAll(string input, string findText, bool matchCase)
        {
            input 
                = matchCase 
                ? input : input.ToLower();
            findText 
                = matchCase 
                ? findText : findText.ToLower();

            List<int> indexes = new();
            for (int index = 0; ; index += findText.Length)
            {
                index = input.IndexOf(findText, index);
                if (index == -1)
                    return indexes;

                indexes.Add(index);
            }
        }

        IEnumerable<FindResult> FindAll(IFind input)
        {
            var targetText = input.Text;
            if (Source == FindSource.Selection)
                targetText = input.Text.Substring(input.SelectionStart, input.SelectionLength);

            List<int> matches = null, breaks = null;

            Try.Invoke
                (() => matches = FindAll(targetText, FindText, MatchCase));
            Try.Invoke
                (() => breaks = FindAll(targetText, $"{'\n'}", true));

            if (matches != null)
            {
                foreach (var i in matches)
                {
                    int line = 0, lineIndex = 0;
                    int? nextLineIndex = null;

                    string lineText = null;
                    if (breaks != null)
                    {
                        foreach (var j in breaks)
                        {
                            if (j < i)
                            {
                                line++;
                                lineIndex = j;
                            }
                            else
                            {
                                nextLineIndex = j;
                                break;
                            }
                        }

                        lineText = nextLineIndex == null
                        ? targetText.Substring(lineIndex)
                        : targetText.Substring(lineIndex, (nextLineIndex.Value - lineIndex).Coerce(int.MaxValue));
                    }
                    else lineText = targetText;

                    yield return new FindResult(input, i, lineText.Replace(System.Environment.NewLine, "").Trim(), line + 1, i - lineIndex + 1);
                }
            }
            yield break;
        }

        FindResultCollection FindAll(bool report)
        {
            FindResultCollection results = new(FindText);
            switch (Source)
            {
                case FindSource.AllDocuments:
                    foreach (var i in Documents)
                    {
                        if (i is IFind j)
                            FindAll(j).ForEach(k => results.Add(k));
                    }
                    break;

                case FindSource.CurrentDocument:
                case FindSource.Selection:
                    if (ActiveDocument is IFind m)
                        FindAll(m).ForEach(n => results.Add(n));

                    break;
            }

            if (report)
            {
                if (results.Count == 0)
                    Dialog.Show("Find".Translate(), $"0 occurences.", DialogImage.Information, Buttons.Ok);

                else OnFindAll(results);
            }

            return results;
        }

        //...

        void ReplaceAll()
        {
            var results = FindAll(false);
            foreach (var i in results)
                i.File.Text = i.File.Text.Substring(0, i.Index) + ReplaceText + i.File.Text.Substring(i.Index + FindText.Length);

            Dialog.Show("Replace".Translate(), $"{results.Count} occurences replaced.", DialogImage.Information, Buttons.Ok);
        }

        void FindReplace(bool direction, bool replace)
        {
            /*
            //Determine what the original and current selection should be. 
            //The original selection should be changed externally. 
            //The current selection is always determined internally.
            switch (Source)
            {
                case FindSource.CurrentDocument:

                    if (OriginalSelection == null || OriginalSelection.Document != ActiveDocument)
                        OriginalSelection = new FindRegion((IFind)ActiveDocument, 0, 0);

                    CurrentSelection = CurrentSelection ?? OriginalSelection;
                    break;

                case FindSource.AllDocuments:

                    if (OriginalSelection == null)
                    {
                        var firstDocument = Documents.FirstOrDefault(i => Supported(i)) as Document;
                        OriginalSelection = new FindRegion((IFind)firstDocument, 0, 0);
                    }

                    CurrentSelection = CurrentSelection ?? OriginalSelection;
                    break;

                case FindSource.Selection:

                    if (OriginalSelection == null || OriginalSelection.Length <= 0)
                    {
                        Dialog.Show(DialogTitle, "Selection cannot have zero length!", DialogImage.Exclamation, Buttons.Ok);
                        return;
                    }

                    CurrentSelection = CurrentSelection ?? new FindRegion(OriginalSelection.Document, OriginalSelection.Start, 0);
                    break;
            }

            //If the current selection is invalid in the following ways, we cannot find a new one based on it
            if (CurrentSelection == null || CurrentSelection.Document is not Document || !Supported(CurrentSelection.Document as Document))
                return;

            //Get a new selection based on the current selection
            switch (direction)
            {
                case true:
                    CurrentSelection = FindNext(CurrentSelection, false);
                    break;

                case false:
                    CurrentSelection = FindPrevious(CurrentSelection);
                    break;
            }

            //We found a match
            if (CurrentSelection != null)
            {
                //Get the new document in focus (if applicable)
                switch (Source)
                {
                    case FindSource.CurrentDocument:
                    case FindSource.Selection:
                        break;

                    case FindSource.AllDocuments:

                        if (CurrentSelection.Document != ActiveDocument)
                            Model.ActiveContent = (Document)CurrentSelection.Document;

                        break;
                }
                //Set the new selection in the new document (and/or replace)
                switch (replace)
                {
                    case true:
                        CurrentSelection.Document.Text = Replace(CurrentSelection.Start, CurrentSelection.Document.Text);
                        CurrentSelection.Document.SelectionStart = CurrentSelection.Start;
                        CurrentSelection.Document.SelectionLength = ReplaceText.Length;
                        break;

                    case false:
                        CurrentSelection.Document.SelectionStart = CurrentSelection.Start;
                        CurrentSelection.Document.SelectionLength = FindText.Length;
                        break;
                }
                //Do nothing more...
                return;
            }

            //We didn't find a match...

            //Get the original document in focus (if applicable)
            switch (Source)
            {
                case FindSource.CurrentDocument:
                case FindSource.Selection:
                    break;

                case FindSource.AllDocuments:

                    if (OriginalSelection.Document != ActiveDocument)
                        Model.ActiveContent = (Document)OriginalSelection.Document;

                    break;
            }

            //Show a dialog
            NoMatches();

            //Set the original selection in the original document
            OriginalSelection.Document.SelectionStart = OriginalSelection.Start;
            OriginalSelection.Document.SelectionLength = 0;
            OriginalSelection.Document.CaretIndex = OriginalSelection.Start;
            */
        }

        //...

        protected virtual void OnFindAll(FindResultCollection input) => ResultsCommand?.Execute(input);

        #endregion

        #region Commands

        ICommand findAllCommand;
        public ICommand FindAllCommand => findAllCommand ??= new RelayCommand(() => FindAll(true), () => Can());

        ICommand findNextCommand;
        public ICommand FindNextCommand => findNextCommand ??= new RelayCommand(() => FindReplace(true, false), () => Can());

        ICommand findPreviousCommand;
        public ICommand FindPreviousCommand => findPreviousCommand ??= new RelayCommand(() => FindReplace(false, false), () => Can());

        //...

        ICommand replaceAllCommand;
        public ICommand ReplaceAllCommand => replaceAllCommand ??= new RelayCommand(() => ReplaceAll(), () => Can());

        ICommand replaceNextCommand;
        public ICommand ReplaceNextCommand => replaceNextCommand ??= new RelayCommand(() => FindReplace(true, true), () => Can());

        ICommand replacePreviousCommand;
        public ICommand ReplacePreviousCommand => replacePreviousCommand ??= new RelayCommand(() => FindReplace(false, true), () => Can());


        #endregion
    }

    public class FindBox : FindControl
    {
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(FindBox), new FrameworkPropertyMetadata(null));
        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public FindBox() : base() { }
    }
}