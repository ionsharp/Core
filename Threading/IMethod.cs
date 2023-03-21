using System;

namespace Imagin.Core.Threading;

public interface IMethod
{
    DateTime? LastActive { get; set; }
}