namespace Imagin.Common.Collections.Events
{
    public class ItemInsertedEventArgs<T> : NewItemEventArgs<T>
    {
        public int Index;
        public ItemInsertedEventArgs(T t, int i) : base(t)
        {
            this.Index = i;
        }
    }
}
