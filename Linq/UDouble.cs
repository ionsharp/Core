namespace Imagin.Core.Linq;

public static class XUDouble
{
    public static UDouble Clamp(this UDouble value, UDouble maximum, UDouble minimum) => value > maximum ? maximum : (value < minimum ? minimum : value);
}