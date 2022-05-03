using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class GradientsPanel : LocalGroupPanel<Gradient>
    {
        public override Uri Icon => Resources.InternalImage(Images.Gradient);

        public override string Title => "Gradients";

        public GradientsPanel() : base(Get.Current<Options>().Gradients) { }

        protected override IEnumerable<GroupCollection<Gradient>> GetDefault()
            { yield return new DefaultGradients(); }

        ICommand resetCommand;
        public ICommand ResetCommand => resetCommand ??= new RelayCommand<Gradient>(i => i.Reset(), i => i != null);
    }
}