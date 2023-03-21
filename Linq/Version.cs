using System;

namespace Imagin.Core.Linq;

public static class XVersion
{
    public static Version Coerce(this Version input, Version maximum, Version minimum) => input > maximum ? maximum : (input < minimum ? minimum : input);
}