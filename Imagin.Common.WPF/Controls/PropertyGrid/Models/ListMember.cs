using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class ListMemberModel : MemberModel
    {
        public IList ActualValue => Value as IList;

        public ObservableCollection<ListItemModel> Items { get; private set; } = new();

        public int Length
        {
            get => ActualValue.Count;
            set
            {
                Resize(value);
                this.Changed(() => Length);
            }
        }

        int selectedIndex = -1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => this.Change(ref selectedIndex, value);
        }

        //...

        public ListMemberModel(MemberData data) : base(data)
        {
            if (ItemType == null)
                Dispatch.Invoke(() => Log.Write<ListMemberModel>(new MemberMissingAttributeWarning<ItemTypeAttribute>(Name, ActualValue?.GetType() ?? Type)));
        }

        //...

        object CreateItem(Type type)
        {
            type ??= ItemType;
            return type == typeof(string) ? "" : type.Create<object>();
        }

        void CreateItem(int index, object i)
        {
            ListItemModel result = new(this, new(Collection, new(new MemberRouteSource(i), null), null, new(null)));
            if (index == -1)
                Items.Add(result);

            else Items.Insert(index, result);

            result.Depth = 0;
            result.UpdateValue();
            result.Subscribe();
        }

        //...

        object GetSelectedItem()
            => ActualValue is IList i && i.Count > SelectedIndex && SelectedIndex >= 0 ? i[SelectedIndex] : null;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Changed(() => Length);
            Try.Invoke(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.ForEach(i => CreateItem(e.NewStartingIndex, i));
                        break;

                    case NotifyCollectionChangedAction.Move:
                        var item = ActualValue[e.OldStartingIndex];
                        ActualValue.RemoveAt(e.OldStartingIndex);
                        ActualValue.Insert(e.NewStartingIndex, item);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        e.OldItems?.ForEach(i =>
                        {
                            Items[e.OldStartingIndex].Unsubscribe();
                            Items.RemoveAt(e.OldStartingIndex);
                        });
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        if (!ItemType.IsValueType && ItemType != typeof(string))
                        {
                            e.OldItems?.ForEach(i =>
                            {
                                Items[e.OldStartingIndex].Unsubscribe();
                                Items.RemoveAt(e.OldStartingIndex);
                            });
                            goto case NotifyCollectionChangedAction.Add;
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        Items.ForEach(i => i.Unsubscribe());
                        Items.Clear();
                        break;
                }
            },
            e => Log.Write<ListMemberModel>(e));
        }

        //...

        protected virtual void InsertAbove(int index, Type type)
            => Try.Invoke(() => CreateItem(type).If(i => ActualValue.Insert(index == -1 ? 0 : index, i)), e => Log.Write<ListMemberModel>(e));

        protected virtual void InsertBelow(int index, Type type) 
            => Try.Invoke(() => CreateItem(type).If(i =>
            {
                var newIndex = index + 1;
                if (index != -1 && newIndex < ActualValue.Count)
                    ActualValue.Insert(newIndex, i);

                else ActualValue.Add(i);
            }), e => Log.Write<ListMemberModel>(e));

        //...

        public void Resize(int length)
        {
            if (length == ActualValue?.Count)
                return;

            Try.Invoke(() =>
            {
                if (length == 0)
                    ActualValue.Clear();

                else if (length > ActualValue.Count)
                {
                    var j = length - ActualValue.Count;
                    for (var i = 0; i < j; i++)
                        ActualValue.Add(CreateItem(null));
                }
                else
                {
                    var j = ActualValue.Count - length;
                    for (var i = ActualValue.Count - 1; i >= length; i--)
                        ActualValue.RemoveAt(i);
                }
            },
            e => Log.Write<ListMemberModel>(e));
        }

        //...

        public override void Subscribe()
        {
            base.Subscribe();

            Items.ForEach(i => i.Unsubscribe());
            Items.Clear();

            foreach (var i in ActualValue)
                CreateItem(-1, i);

            (ActualValue as INotifyCollectionChanged).CollectionChanged -= OnCollectionChanged;
            (ActualValue as INotifyCollectionChanged).CollectionChanged += OnCollectionChanged;

            this.Changed(() => Length);
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Items.ForEach(i => i.Unsubscribe());
            Items.Clear();

            (ActualValue as INotifyCollectionChanged).CollectionChanged -= OnCollectionChanged;
        }

        //...

        ICommand insertAboveCommand;
        public ICommand InsertAboveCommand
            => insertAboveCommand
            ??= new RelayCommand<Type>(i => InsertAbove(SelectedIndex, i),
                i => ActualValue != null);

        ICommand insertBelowCommand;
        public ICommand InsertBelowCommand
            => insertBelowCommand
            ??= new RelayCommand<Type>(i => InsertBelow(SelectedIndex, i),
                i => ActualValue != null);

        ICommand moveDownCommand;
        public ICommand MoveDownCommand
            => moveDownCommand
            ??= new RelayCommand(() => Try.Invoke(() => ActualValue.MoveDown(SelectedIndex, true)),
                () => GetSelectedItem() != null);

        ICommand moveUpCommand;
        public ICommand MoveUpCommand
            => moveUpCommand
            ??= new RelayCommand(() => Try.Invoke(() => ActualValue.MoveUp(SelectedIndex, true)),
                () => GetSelectedItem() != null);

        ICommand removeCommand;
        public ICommand RemoveCommand
            => removeCommand
            ??= new RelayCommand(() => Try.Invoke(() => ActualValue.RemoveAt(SelectedIndex)),
                () => GetSelectedItem() != null);

        ICommand resetCommand;
        public ICommand ResetCommand
            => resetCommand
            ??= new RelayCommand(() => Try.Invoke(() => GetSelectedItem().If<IReset>(i => i.Reset())),
                () => GetSelectedItem() is IReset);
    }
}