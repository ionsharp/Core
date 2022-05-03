namespace Imagin.Common.Models
{
    public interface IDockViewModel
    {
        Content ActiveContent { get; set; }

        Document ActiveDocument { get; set; }

        Panel ActivePanel { get; set; }

        DocumentCollection Documents { get; }

        PanelCollection Panels { get; }
    }
}