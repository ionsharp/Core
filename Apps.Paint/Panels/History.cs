using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public sealed class HistoryPanel : Panel
    {
        public override Uri Icon => Resources.InternalImage(Images.Clock);

        public override string Title => "History";

        public HistoryPanel() : base() { }
    }
}