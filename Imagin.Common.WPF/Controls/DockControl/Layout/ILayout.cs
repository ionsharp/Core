using System;
using System.Collections.Generic;

namespace Imagin.Common.Controls
{
    public interface ILayout
    {
        IEnumerable<Uri> GetDefaultLayouts();

        Layouts Layouts { get; }
    }
}