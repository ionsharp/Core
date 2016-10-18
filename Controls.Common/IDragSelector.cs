namespace Imagin.Controls.Common
{
    public interface IDragSelector
    {
        double DragScrollOffset
        {
            get; set;
        }

        double DragScrollTolerance
        {
            get; set;
        }

        Selection DragSelection
        {
            get; set;
        }

        bool IsDragSelectionEnabled
        {
            get; set;
        }
    }
}
