using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Common.Models
{
    [Serializable]
    public abstract class Document : Content
    {
        public const SecondaryDocks DefaultDockPreference = SecondaryDocks.Left;

        bool canClose = true;
        [Hidden]
        [XmlIgnore]
        public virtual bool CanClose
        {
            get => canClose;
            set => this.Change(ref canClose, value);
        }

        [Hidden]
        [XmlIgnore]
        public virtual bool CanMinimize { get; } = true;

        [Hidden]
        [XmlIgnore]
        public virtual SecondaryDocks DockPreference { get; } = DefaultDockPreference;

        [Hidden]
        public virtual object Icon => default;

        bool isMinimized = false;
        [Hidden]
        [XmlIgnore]
        public virtual bool IsMinimized
        {
            get => isMinimized;
            set => this.Change(ref isMinimized, value);
        }

        [field: NonSerialized]
        bool isModified = false;
        [Hidden]
        [XmlIgnore]
        public virtual bool IsModified
        {
            get => isModified;
            set => this.Change(ref isModified, value);
        }

        public Document() : base() { }
        
        public abstract void Save();

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsModified):
                    this.Changed(() => Title);
                    if (IsModified)
                        Get.Where<IDockViewOptions>().If(i => i.AutoSaveDocuments, i => Save());

                    break;
            }
        }

        [field: NonSerialized]
        ICommand saveCommand;
        [Hidden]
        [XmlIgnore]
        public virtual ICommand SaveCommand => saveCommand ??= new RelayCommand(Save);
    }
}