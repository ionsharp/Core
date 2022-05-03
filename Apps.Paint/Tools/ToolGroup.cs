using Imagin.Common;
using Imagin.Common.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class ToolGroup : Base
    {
        [field: NonSerialized]
        public event EventHandler MenuShown;

        bool isMenuVisible = new();
        public bool IsMenuVisible
        {
            get => isMenuVisible;
            set => this.Change(ref isMenuVisible, value);
        }

        Tool selectedTool = null;
        public Tool SelectedTool
        {
            get => selectedTool;
            set => this.Change(ref selectedTool, value);
        }

        ObservableCollection<Tool> tools = new();
        public ObservableCollection<Tool> Tools
        {
            get => tools;
            set => this.Change(ref tools, value);
        }

        public ToolGroup(params Tool[] tools) : base()
        {
            if (tools?.Length > 0)
            {
                SelectedTool = tools[0];
                foreach (var i in tools)
                    Tools.Add(i);
            }
        }

        protected virtual void OnMenuShown() => MenuShown?.Invoke(this, EventArgs.Empty);

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsMenuVisible):
                    if (IsMenuVisible)
                        OnMenuShown();

                    break;
            }
        }
    }
}