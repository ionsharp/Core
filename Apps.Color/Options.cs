using Imagin.Common;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Controls;
using Imagin.Common.Models;
using System;

namespace Imagin.Apps.Color
{
    [Serializable]
    public class Options : MainViewOptions, IColorControlOptions
    {
        [Hidden]
        public bool AutoSaveLayout => ColorControlOptions.AutoSaveLayout;

        [Hidden]
        IGroupWriter IColorControlOptions.Colors => ColorControlOptions.Colors;

        ColorControlOptions colorControlOptions = new();
        [Category(nameof(ColorControl))]
        [DisplayName("Options")]
        public ColorControlOptions ColorControlOptions
        {
            get => colorControlOptions;
            set => this.Change(ref colorControlOptions, value);
        }

        DocumentCollection documents = new();
        [Hidden]
        public DocumentCollection Documents
        {
            get => documents;
            set => this.Change(ref documents, value);
        }

        [Hidden]
        public Layouts Layouts => ColorControlOptions.Layouts;

        //...

        protected override void OnSaving()
        {
            base.OnSaving();
            ColorControlOptions.OnSaved();
        }

        public void OnLoaded(ColorControl colorPicker) => ColorControlOptions?.OnLoaded(colorPicker);
    }
}