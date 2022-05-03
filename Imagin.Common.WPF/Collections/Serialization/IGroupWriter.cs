using System;
using System.Collections;
using System.Windows.Input;

namespace Imagin.Common.Collections.Serialization
{
    public interface IGroupWriter : IList
    {
        ICommand ClearCommand { get; }

        ICommand ExportCommand { get; }

        ICommand ExportAllCommand { get; }

        ICommand ImportCommand { get; }

        Type GetItemType();
    }
}