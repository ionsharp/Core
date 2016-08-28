namespace Imagin.Common.Collections.Events
{
    public class ItemReplacedEventArgs<T> : NewItemEventArgs<T>
    {
        public T OldItem;
        public ItemReplacedEventArgs(T Old, T New) : base(New)
        {
            this.OldItem = Old;
        }
    }
}
