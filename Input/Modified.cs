using System;

namespace Imagin.Core.Input;

public class ModifiedEventArgs : EventArgs
{
    public readonly object OriginalSource;

    public readonly object OldValue;

    public readonly object NewValue;

    public readonly string PropertyName;

    public ModifiedEventArgs(object originalSource, string propertyName, object oldValue, object newValue) : base()
    {
        OriginalSource = originalSource; PropertyName = propertyName; OldValue = oldValue; NewValue = newValue;
    }
}

public delegate void ModifiedEventHandler(object sender, ModifiedEventArgs e);