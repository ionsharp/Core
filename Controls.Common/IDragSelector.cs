namespace Imagin.Controls.Common
{
    public interface IDragSelector
    {
        double ScrollOffset
        {
            get; set;
        }

        double ScrollTolerance
        {
            get; set;
        }

        bool IsDragSelectionEnabled
        {
            get; set;
        }

        Selection Selection
        {
            get; set;
        }
    }
}
