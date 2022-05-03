using Imagin.Common.Collections;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Linq;
using System.Collections.Specialized;

namespace Imagin.Common.Analytics
{
    public class NotificationWriter : XmlWriter<Notification>
    {
        public static Limit DefaultLimit = new(50, Limit.Actions.ClearAndArchive);

        public NotificationWriter(string folderPath, Limit limit) : base("Notifications", folderPath, "Notifications", "xml", "xml", limit, typeof(Notification), typeof(Result)) { }
        
        void OnExpired(object sender, System.EventArgs e)
        {
            Remove(sender as Notification);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems[0].As<Notification>().Expired += OnExpired;
                    break;

                case NotifyCollectionChangedAction.Remove:
                    e.OldItems[0].As<Notification>().Expired -= OnExpired;
                    break;
            }
        }
    }
}