namespace Imagin.Common.Collections.Events
{
    public class ItemAddedEventArgs<T> : NewItemEventArgs<T>
    {
        public ItemAddedEventArgs(T t) : base(t)
        {
        }
    }
}
