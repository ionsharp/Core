using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Numbers;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Apps.Desktop
{
    [Serializable]
    public abstract class Tile : BaseUpdate, IChange, ILock, IPoint2D, ISize, ISelect
    {
        [field: NonSerialized]
        public event ChangedEventHandler Changed;

        [field: NonSerialized]
        public event LockedEventHandler Locked;

        [field: NonSerialized]
        public event SelectedEventHandler Selected;

        bool isLocked = false;
        public bool IsLocked
        {
            get => isLocked;
            set => this.Change(ref isLocked, value);
        }

        [field: NonSerialized]
        bool isSelected = false;
        [Hidden, XmlIgnore]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                this.Change(ref isSelected, value);
                if (value)
                    OnSelected();
            }
        }

        Point2D position = new(0, 0);
        [Hidden]
        public Point2D Position
        {
            get => position;
            set => this.Change(ref position, value);
        }

        DoubleSize size = new(250d, 250d);
        [Hidden]
        public DoubleSize Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        string title = string.Empty;
        [Hidden]
        public virtual string Title
        {
            get => title;
            set => this.Change(ref title, value);
        }

        public Tile() : base() { }

        protected virtual void OnChanged() => Changed?.Invoke(this);

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(this));
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Position):
                case nameof(Size):
                case nameof(Title):
                    OnChanged();
                    break;

                case nameof(IsLocked):
                    OnChanged();
                    Locked?.Invoke(this, new(IsLocked));
                    break;
            }
        }

        [field: NonSerialized]
        ICommand deleteTileCommand;
        [Hidden, XmlIgnore]
        public ICommand DeleteTileCommand => deleteTileCommand ??= new RelayCommand<object>(i => Get.Current<MainViewModel>().Screen.Remove(i as Tile), i => i is Tile);
    }
}