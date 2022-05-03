using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Models;
using System;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ToolPanel : Panel
    {
        public override bool CanShare => false;

        public override SecondaryDocks DockPreference => SecondaryDocks.Top;

        public override Uri Icon => Resources.ProjectImage("Tool.png");

        public override string Title => "Tool";

        public override bool TitleVisibility => false;

        Tool tool;
        public Tool Tool
        {
            get => tool;
            set => this.Change(ref tool, value);
        }

        public ToolPanel() : base() { }
    }
}