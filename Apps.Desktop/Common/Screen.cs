using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace Imagin.Apps.Desktop
{
    [Serializable]
    public class Screen : ObservableCollection<Tile>, ILimit
    {
        public static Limit DefaultLimit = new(25, Limit.Actions.RemoveFirst);

        public int Index => Get.Current<MainViewModel>().Screens.IndexOf(this) + 1;

        Limit limit = DefaultLimit;
        [XmlIgnore]
        public Limit Limit
        {
            get => limit;
            set
            {
                limit = value;
                limit.Coerce(this);
            }
        }

        public Screen() : base() { }

        public Screen(params Tile[] tiles) : base() => tiles?.ForEach(i => Add(i));

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    limit.Coerce(this);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as Tile).Dispose();
                    break;
            }
        }
    }
}