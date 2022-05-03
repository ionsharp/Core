using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    public class PathsPanel : LocalGroupPanel<Path>
    {
        enum Category { Save }

        #region Properties

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Path.png");

        [Hidden]
        public override string Title => "Paths";

        #endregion

        #region PathsPanel

        public PathsPanel() : base(Get.Current<Options>().Paths) { }

        #endregion

        #region Methods

        protected override IEnumerable<GroupCollection<Path>> GetDefault()
            { yield return new GroupCollection<Path>("Default"); }

        #endregion

        #region Commands

        ICommand saveAsBrushCommand;
        [Category(Category.Save)]
        [DisplayName("Save as brush")]
        [Icon(App.ImagePath + "ConvertToBrush.png")]
        [Tool, Visible]
        public ICommand SaveAsBrushCommand
            => saveAsBrushCommand ??= new RelayCommand(() => { }, () => SelectedItem != null);

        ICommand saveAsShapeCommand;
        [Category(Category.Save)]
        [DisplayName("Save as shape")]
        [Icon(App.ImagePath + "ConvertToShape.png")]
        [Tool, Visible]
        public ICommand SaveAsShapeCommand
            => saveAsShapeCommand ??= new RelayCommand(() => { }, () => SelectedItem != null);

        #endregion
    }
}