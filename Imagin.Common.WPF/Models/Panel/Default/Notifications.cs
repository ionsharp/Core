using Imagin.Common.Analytics;
using Imagin.Common.Collections;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    [Serializable]
    public class NotificationsPanel : DataPanel
    {
        public static readonly ResourceKey TemplateKey = new();

        enum Category { Commands0, Text }

        #region Properties

        TimeSpan clearAfter = TimeSpan.Zero;
        [DisplayName("ClearAfter")]
        [Option]
        public TimeSpan ClearAfter
        {
            get => clearAfter;
            set => this.Change(ref clearAfter, value);
        }

        [Hidden]
        public override IList GroupNames => new StringCollection()
        {
            "None",
            nameof(Notification.Type)
        };

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Bell);

        [Hidden]
        public IList<Notification> Notifications => Data as IList<Notification>;

        [Hidden]
        public override IList SortNames => new StringCollection()
        {
            nameof(Notification.Type)
        };

        [Hidden]
        public override int TitleCount => Data?.Count<Notification>(i => !i.IsRead) ?? 0;

        [Hidden]
        public override string TitleKey => "Notifications";

        bool textWrap = true;
        [Category(Category.Text)]
        [Label(false)]
        [Icon(Images.ArrowDownLeft)]
        [Index(int.MaxValue - 1)]
        [Tool]
        [Style(BooleanStyle.Image)]
        public bool TextWrap
        {
            get => textWrap;
            set => this.Change(ref textWrap, value);
        }

        //...

        readonly BaseUpdate update = new(1.Seconds());

        #endregion

        #region NotificationsPanel

        public NotificationsPanel(ICollectionChanged input) : base()
        {
            Data = input;

            Notifications.ForEach<Notification>(i => { Unsubscribe(i); Subscribe(i); });
            update.Update += OnUpdate;
        }

        #endregion

        #region Methods

        void Subscribe(Notification input) => input.PropertyChanged += OnNotificationChanged;

        void Unsubscribe(Notification input) => input.PropertyChanged -= OnNotificationChanged;

        //...

        void OnUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (clearAfter > TimeSpan.Zero)
            {
                for (var i = Notifications.Count - 1; i >= 0; i--)
                {
                    var j = Notifications[i].As<Notification>();
                    if (DateTime.Now > j.Created + clearAfter)
                        Notifications.RemoveAt(i);
                }
            }
        }

        //...

        protected override void OnItemAdded(object input)
            => Subscribe(input as Notification);

        protected override void OnItemRemoved(object input)
            => Unsubscribe(input as Notification);

        //...

        void OnNotificationChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Notification.IsRead):
                    this.Changed(() => Title);
                    break;
            }
        }

        //...

        ICommand markAllCommand;
        [Category(Category.Commands0)]
        [DisplayName("MarkAll")]
        [Icon(Images.Read)]
        [Tool]
        public ICommand MarkAllCommand => markAllCommand
            ??= new RelayCommand(() => Notifications.ForEach<Notification>(i => i.IsRead = true), () => Notifications?.Contains<Notification>(i => !i.IsRead) == true);

        ICommand unmarkAllCommand;
        [Category(Category.Commands0)]
        [DisplayName("UnmarkAll")]
        [Icon(Images.Unread)]
        [Tool]
        public ICommand UnmarkAllCommand => unmarkAllCommand
            ??= new RelayCommand(() => Notifications.ForEach<Notification>(i => i.IsRead = false), () => Notifications?.Contains<Notification>(i => i.IsRead) == true);

        #endregion
    }
}