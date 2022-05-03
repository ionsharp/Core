namespace Imagin.Common.Media
{
    public class Geometry
    {
        public static System.Windows.Media.Geometry TriangleDown => System.Windows.Media.Geometry.Parse("M0,0 L0,2 L4,6 L8,2 L8,0 L4,4 z");

        public static System.Windows.Media.Geometry TriangleRight => System.Windows.Media.Geometry.Parse("M0,0 L0,8 L4,4 z");
    }
}