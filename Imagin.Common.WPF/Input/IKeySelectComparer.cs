namespace Imagin.Common.Input
{
    public interface IKeySelectComparer
    {
        bool Compare(object input, string query);
    }
}