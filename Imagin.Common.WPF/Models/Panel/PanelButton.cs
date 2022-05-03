using System;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    public class PanelButton : BaseNamable
    {
        ICommand command = default;
        public ICommand Command
        {
            get => command;
            set => this.Change(ref command, value);
        }

        Uri icon = default;
        public Uri Icon
        {
            get => icon;
            set => this.Change(ref icon, value);
        }

        public PanelButton(string name, ICommand command, Uri icon) : base(name)
        {
            Command = command;
            Icon = icon;
        }
    }
}