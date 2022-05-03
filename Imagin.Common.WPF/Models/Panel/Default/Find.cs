using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    [MemberVisibility(MemberVisibility.Explicit, MemberVisibility.Explicit)]
    public class FindPanel : Panel, IFrameworkReference
    {
        public static readonly ReferenceKey<FindControl> ControlKey = new();

        public static readonly ResourceKey TemplateKey = new();

        FindControl Control;

        string findText = string.Empty;
        public string FindText
        {
            get => findText;
            set => this.Change(ref findText, value);
        }

        public override Uri Icon => Resources.InternalImage(Images.Search);

        bool matchCase = false;
        public bool MatchCase
        {
            get => matchCase;
            set => this.Change(ref matchCase, value);
        }

        bool matchWord = false;
        public bool MatchWord
        {
            get => matchWord;
            set => this.Change(ref matchWord, value);
        }

        FindSource source = FindSource.CurrentDocument;
        public FindSource Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        string replaceText = string.Empty;
        public string ReplaceText
        {
            get => replaceText;
            set => this.Change(ref replaceText, value ?? string.Empty);
        }

        public override string Title => "Find";

        public FindPanel() : base() { }

        void IFrameworkReference.SetReference(IFrameworkKey key, System.Windows.FrameworkElement element)
        {
            if (key == ControlKey)
                Control = element as FindControl;
        }

        //...

        ICommand findAllCommand;
        public ICommand FindAllCommand 
            => findAllCommand ??= new RelayCommand(() => Control.FindAllCommand.Execute(), () => Control?.FindAllCommand.CanExecute(null) == true);

        ICommand findNextCommand;
        public ICommand FindNextCommand
            => findNextCommand ??= new RelayCommand(() => Control.FindNextCommand.Execute(), () => Control?.FindNextCommand.CanExecute(null) == true);

        ICommand findPreviousCommand;
        public ICommand FindPreviousCommand 
            => findPreviousCommand ??= new RelayCommand(() => Control.FindNextCommand.Execute(), () => Control?.FindNextCommand.CanExecute(null) == true);

        //...

        ICommand replaceAllCommand;
        public ICommand ReplaceAllCommand 
            => replaceAllCommand ??= new RelayCommand(() => Control.ReplaceAllCommand.Execute(), () => Control?.ReplaceAllCommand.CanExecute(null) == true);

        ICommand replaceNextCommand;
        public ICommand ReplaceNextCommand 
            => replaceNextCommand ??= new RelayCommand(() => Control.ReplaceNextCommand.Execute(), () => Control?.ReplaceNextCommand.CanExecute(null) == true);

        ICommand replacePreviousCommand;
        public ICommand ReplacePreviousCommand 
            => replacePreviousCommand ??= new RelayCommand(() => Control.ReplacePreviousCommand.Execute(), () => Control?.ReplacePreviousCommand.CanExecute(null) == true);

        //...

        ICommand resultsCommand;
        public ICommand ResultsCommand => resultsCommand ??= new RelayCommand<FindResultCollection>(i =>
        {
            var dockViewModel = Get.Where<IDockViewModel>();
            if (dockViewModel.Panels.FirstOrDefault<FindResultsPanel>() is FindResultsPanel oldPanel)
            {
                if (!oldPanel.KeepResults)
                {
                    oldPanel.Results = i;
                    return;
                }
            }
            dockViewModel.Panels.Add(new FindResultsPanel(i));
        }, 
        i => i != null);
    }
}