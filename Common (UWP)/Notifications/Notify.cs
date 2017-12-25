using System;
using Windows.UI.Notifications;

namespace Imagin.Common
{
    public static class Notify
    {
        public static void Toast(string Title, string Content, int? Expiration)
        {
            var Notifier = ToastNotificationManager.CreateToastNotifier();

            var Xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var Nodes = Xml.GetElementsByTagName("text");

            Nodes.Item(0).AppendChild(Xml.CreateTextNode(Title));
            Nodes.Item(1).AppendChild(Xml.CreateTextNode(Content));

            var Root = Xml.SelectSingleNode("/toast");

            var Audio = Xml.CreateElement("audio");
            Audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            var Toast = new ToastNotification(Xml);

            if (Expiration != null)
                Toast.ExpirationTime = DateTime.Now.AddSeconds(Expiration.Value);

            Notifier.Show(Toast);
        }
    }
}
