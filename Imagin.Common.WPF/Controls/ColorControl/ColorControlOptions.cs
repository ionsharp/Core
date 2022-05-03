using Imagin.Common.Analytics;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Common.Controls
{
    [DisplayName("Color view")]
    [Serializable]
    public class ColorControlOptions : Base, IColorControlOptions, ILayout, ISerialize
    {
        #region Properties

        bool autoSaveLayout = true;
        [Category(nameof(Layouts))]
        [DisplayName("AutoSave")]
        public bool AutoSaveLayout
        {
            get => autoSaveLayout;
            set => this.Change(ref autoSaveLayout, value);
        }

        [Hidden]
        [field: NonSerialized]
        public ColorControl ColorControl { get; private set; }

        IGroupWriter IColorControlOptions.Colors => colors;
        [field: NonSerialized]
        GroupWriter<StringColor> colors;
        [Hidden]
        public GroupWriter<StringColor> Colors
        {
            get => colors;
            set => this.Change(ref colors, value);
        }

        [Hidden]
        public string FilePath { get; private set; }

        string layout = string.Empty;
        [Hidden]
        public virtual string Layout
        {
            get => layout;
            set => this.Change(ref layout, value);
        }

        [NonSerialized]
        Layouts layouts = null;
        [Category(nameof(Layouts))]
        [DisplayName("Layout")]
        public virtual Layouts Layouts
        {
            get => layouts;
            set => this.Change(ref layouts, value);
        }

        [Category(nameof(Window))]
        [DisplayName("Panels")]
        public PanelCollection Panels => ColorControl?.Panels;

        #endregion

        #region ColorControlOptions

        public ColorControlOptions() : base() { }

        public ColorControlOptions(string filePath) : this() => FilePath = filePath;

        #endregion

        #region Methods

        public override string ToString() => "Color view";

        //...

        public static Result Load(string filePath, out ColorControlOptions data)
        {
            var result = BinarySerializer.Deserialize(filePath, out object options);
            data = options as ColorControlOptions ?? new ColorControlOptions(filePath);
            return result;
        }

        public Result Deserialize(string filePath, out object data) => BinarySerializer.Deserialize(filePath, out data);

        //...

        public Result Save() => Serialize(this);

        public Result Serialize(object data) => Serialize(FilePath, data);

        public Result Serialize(string filePath, object data)
        {
            OnSaved();
            return BinarySerializer.Serialize(filePath, data);
        }

        //...

        public IEnumerable<Uri> GetDefaultLayouts()
        {
            yield return Resources.Uri(InternalAssembly.Name, "Controls/ColorControl/Layouts/Default.xml");
        }

        public void OnLoaded(ColorControl colorPicker)
        {
            ColorControl = colorPicker;

            Colors = new GroupWriter<StringColor>($@"{Configuration.ApplicationProperties.GetFolderPath(Configuration.DataFolders.Documents)}\ColorControl", "Colors", "data", "colors", new Collections.Limit(250, Collections.Limit.Actions.RemoveFirst));
            Colors.Load();

            Layouts = new Layouts($@"{Configuration.ApplicationProperties.GetFolderPath(Configuration.DataFolders.Documents)}\ColorControl\Layouts", GetDefaultLayouts());
            Layouts.Update(layout);
            Layouts.Refresh();
        }

        public void OnSaved()
        {
            Layout = Layouts.Layout;
            Colors.Save();
        }

        #endregion
    }
}