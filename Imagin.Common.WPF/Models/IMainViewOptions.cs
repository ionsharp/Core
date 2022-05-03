namespace Imagin.Common.Models
{
    public interface IMainViewOptions
    {
        bool LogEnabled { get; }

        bool SaveWithDialog { get; }

        void Save();
    }
}