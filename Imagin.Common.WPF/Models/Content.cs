using System;
using System.Xml.Serialization;

namespace Imagin.Common.Models
{
    [Serializable]
    public abstract class Content : LockableViewModel, ISubscribe, IUnsubscribe
    {
        bool canFloat = true;
        [Hidden, XmlIgnore]
        public virtual bool CanFloat
        {
            get => canFloat;
            set => this.Change(ref canFloat, value);
        }

        bool isBusy = false;
        [Hidden, XmlIgnore]
        [Serialize(false)]
        public bool IsBusy
        {
            get => isBusy;
            set => this.Change(ref isBusy, value);
        }

        [Hidden, XmlIgnore]
        public virtual string Title { get; }

        [Hidden, XmlIgnore]
        public virtual object ToolTip { get; }// = null;

        //...

        public virtual void Subscribe() { }

        public virtual void Unsubscribe() { }
    }
}