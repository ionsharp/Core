using System;
using System.Collections.Generic;
using System.Text;

namespace Imagin.Core;

/// <summary>Specifies an <see cref="object"/> with a description.</summary>
public interface IDescription
{
    string Description { get; set; }
}