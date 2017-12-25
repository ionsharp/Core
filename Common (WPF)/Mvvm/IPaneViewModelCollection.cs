namespace Imagin.Common.Mvvm
{
    public interface IPaneViewModelCollection
    {
        void Add(IPaneViewModel Item);

        void Remove(IPaneViewModel Item);
    }
}
