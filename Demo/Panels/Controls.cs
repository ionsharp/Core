using Imagin.Common;
using Imagin.Common.Models;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Demo
{
    public class ControlsPanel : DataPanel
    {
        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Briefcase);

        Element selectedElement;
        [Hidden]
        public Element SelectedElement
        {
            get => selectedElement;
            set => this.Change(ref selectedElement, value);
        }

        [Hidden]
        public override string TitleKey => "Controls";

        public ControlsPanel() : base() { }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedElement):
                    Find<InheritsPanel>().Element = selectedElement;
                    break;
            }
        }

        [Hidden]
        public override ICommand ClearCommand => base.ClearCommand;

        [Hidden]
        public override ICommand RefreshCommand => base.RefreshCommand;
    }
}