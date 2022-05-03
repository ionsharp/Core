using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Image")]
    [Serializable]
    public class ImageTile : FolderTile
    {
        TimeSpan interval = TimeSpan.FromSeconds(3);
        public TimeSpan Interval
        {
            get => interval;
            set => this.Change(ref interval, value.Coerce(TimeSpan.MaxValue, TimeSpan.FromSeconds(3)));
        }

        Transitions transition = Transitions.Random;
        public Transitions Transition
        {
            get => transition;
            set => this.Change(ref transition, value);
        }

        public ImageTile() : base() { }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Interval):
                case nameof(Transition):
                    OnChanged();
                    break;
            }
        }

        [field: NonSerialized]
        ICommand transitionCommand;
        [Hidden, XmlIgnore]
        public ICommand TransitionCommand => transitionCommand ??= new RelayCommand<object>(i => Transition = (Transitions)i, i => i is Transitions);
    }
}