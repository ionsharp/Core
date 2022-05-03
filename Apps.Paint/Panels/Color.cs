using Imagin.Common;
using Imagin.Common.Colors;
using Imagin.Common.Controls;
using Imagin.Common.Models;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ColorPanel : Panel
    {
        #region Properties

        public override Uri Icon => Resources.ProjectImage("Color.png");

        public override string Title => "Color";

        //...

        ObservableColor color;
        public ObservableColor Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        Components component = Components.A;
        [Index(1), Tool, Visible]
        public Components Component
        {
            get => component;
            set => this.Change(ref component, value);
        }

        ColorModels model = ColorModels.HSB;
        [Index(0), Tool, Visible]
        public ColorModels Model
        {
            get => model;
            set
            {
                this.Change(ref model, value);
                Component = Components.A;
            }
        }

        #endregion

        #region ColorPanel

        public ColorPanel() : base()
            => Color = new ObservableColor(Model, Get.Current<Options>().ForegroundColor, i => Get.Current<Options>().ForegroundColor = i);

        #endregion

        #region Methods

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Component):
                    color.Component
                        = Component;
                    break;

                case nameof(Model):
                    color.Model
                        = Model;
                    break;
            }
        }

        #endregion

    }
}