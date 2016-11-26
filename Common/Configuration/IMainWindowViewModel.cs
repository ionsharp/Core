namespace Imagin.Common.Mvvm
{
    public interface IMainWindowViewModel
    {
        IDocumentViewModelCollection GetDocuments();

        IPaneViewModelCollection GetPanes();
    }
}
