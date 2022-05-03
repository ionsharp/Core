using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    [Serializable]
    public abstract class DockViewOptions<T> : MainViewOptions, IDockViewOptions, ILayout where T : class, IDockViewModel
    {
        enum Category
        {
            Documents,
            Layouts,
            Window
        }

        Dictionary<string, PanelOptions> PanelOptions = new();

        //...

        protected virtual bool SaveDocuments => true;

        //...

        bool autoSaveDocuments = false;
        [Category(nameof(Category.Documents))]
        [DisplayName("AutoSave")]
        public bool AutoSaveDocuments
        {
            get => autoSaveDocuments;
            set => this.Change(ref autoSaveDocuments, value);
        }

        bool autoSaveLayout = true;
        [Category(nameof(Category.Layouts))]
        [DisplayName("AutoSave")]
        public bool AutoSaveLayout
        {
            get => autoSaveLayout;
            set => this.Change(ref autoSaveLayout, value);
        }

        List<Document> documents = new();
        [Hidden]
        public virtual List<Document> Documents
        {
            get => documents;
            set => this.Change(ref documents, value);
        }

        string layout = string.Empty;
        [Hidden]
        public virtual string Layout
        {
            get => layout;
            set => this.Change(ref layout, value);
        }

        [NonSerialized]
        Layouts layouts = null;
        [Category(nameof(Category.Layouts))]
        [DisplayName("Layout")]
        public virtual Layouts Layouts
        {
            get => layouts;
            set => this.Change(ref layouts, value);
        }

        [Category(nameof(Category.Window))]
        [DisplayName("Panels")]
        public virtual PanelCollection Panels => Get.Where<T>().Panels;

        //...

        public DockViewOptions() : base() { }

        //...

        protected override void OnApplicationLoaded()
        {
            base.OnApplicationLoaded();
            if (Documents.Count > 0)
            {
                Documents.ForEach(i => Get.Where<T>().Documents.Add(i));
                Documents.Clear();
            }
            foreach (var i in Panels)
            {
                if (PanelOptions.ContainsKey(i.Name))
                    PanelOptions[i.Name].Load(i);
            }
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            Layouts = new Layouts($@"{Get.Where<IApplication>().Properties.FolderPath}\Layouts", GetDefaultLayouts());
            Layouts.Update(layout);
            Layouts.Refresh();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Layout = Layouts?.Layout;

            if (SaveDocuments)
            {
                Documents.Clear();
                Get.Where<T>()?.Documents.ForEach(i => Documents.Add(i));
            }

            PanelOptions.Clear();
            foreach (var i in Panels)
            {
                PanelOptions.Add(i.Name, new());
                PanelOptions[i.Name].Save(i);
            }
        }

        //...

        public abstract IEnumerable<Uri> GetDefaultLayouts();

        //...

        [field: NonSerialized]
        ICommand openLayoutsFolder;
        [Category(nameof(Layouts))]
        [DisplayName("Open")]
        public virtual ICommand OpenLayoutsFolder => openLayoutsFolder ??= new RelayCommand(() => Storage.Computer.OpenInWindowsExplorer(Layouts.Path), () => true);

        [field: NonSerialized]
        ICommand resetLayout;
        [Category(nameof(Layouts))]
        [DisplayName("Reset")]
        public virtual ICommand ResetLayout => resetLayout ??= new RelayCommand(() => Layouts.Layout = null, () => true);

        [field: NonSerialized]
        ICommand saveLayout;
        [Hidden]
        public virtual ICommand SaveLayout => saveLayout ??= new RelayCommand(() => Layouts.SaveLayoutCommand?.Execute(), () => Layouts != null);
    }
}