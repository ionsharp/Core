namespace Imagin.Common.Mvvm
{
    public interface IDocumentViewModel
    {
        string Title
        {
            get;
        }

        string ToolTip
        {
            get;
        }

        void Save();
    }
}
