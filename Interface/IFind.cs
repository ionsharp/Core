namespace Imagin.Core;

public interface IFind
{
    int CaretIndex { get; set; }

    int SelectionLength { get; set; }

    int SelectionStart { get; set; }

    string Text { get; set; }
}