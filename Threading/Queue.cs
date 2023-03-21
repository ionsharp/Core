using Imagin.Core.Collections.ObjectModel;
using System;

namespace Imagin.Core.Threading;

public class Queue : ObservableCollection<Operation>
{
    readonly IMethod method;

    public Operation Current { get; private set; }

    public Queue(IMethod method)
    {
        this.method = method;
    }

    async void Assign(Operation operation)
    {
        Current = operation;
        await Current.Start();

        Remove(Current);
        Current = null;

        method.LastActive = DateTime.Now;
        Scan();
    }

    void Scan()
    {
        if (Current == null)
        {
            if (Count > 0)
            {
                Assign(this[0]);
            }
        }
    }

    protected override void OnAdded(Operation input)
    {
        base.OnAdded(input);
        Scan();
    }

    public void Add(OperationType type, string source, string target, ManagedMethod action)
    {
        Add(new Operation(type, source, target, action));
    }

    new public void Clear()
    {
        foreach (var i in this)
            i.Cancel();

        base.Clear();
    }
}