namespace Imagin.Common.Storage
{
    public interface IValidate
    {
        bool Validate(ItemType target, string path);
    }
}