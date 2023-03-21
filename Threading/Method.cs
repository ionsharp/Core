using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imagin.Core.Threading;

public delegate void ManagedMethod<T>(T input, CancellationToken token);

public delegate Task UnmanagedMethod<T>(T input, CancellationToken token);

///

public delegate void ManagedMethod(CancellationToken token);

public delegate Task UnmanagedMethod(CancellationToken token);

///

public delegate void TaskCompletedEventHandler(Method task);

///

/// <summary>Invokes a <see cref="TaskManagement.Managed"/> or <see cref="TaskManagement.Unmanaged"/> action and is cancellable.</summary>
[Serializable]
public class Method : Base
{
    [field: NonSerialized]
    public event TaskCompletedEventHandler Completed;

    ///

    [field: NonSerialized]
    readonly ManagedMethod ManagedAction;

    [field: NonSerialized]
    readonly UnmanagedMethod UnmanagedAction;

    ///

    [field: NonSerialized]
    protected CancellationTokenSource tokenSource;

    [Hide]
    [XmlIgnore]
    public bool IsCancelled => tokenSource.IsCancellationRequested;

    [field: NonSerialized]
    protected bool restart = false;

    public bool Restarts { get => Get(false); set => Set(value); }

    [Hide]
    [XmlIgnore]
    public bool Started { get => Get(false, false); set => Set(value, false); }

    ///

    Method() : base() { }

    protected Method(bool restarts) : this() 
        => Restarts = restarts;

    public Method(UnmanagedMethod action, bool restarts = false) : this(restarts)
    {
        UnmanagedAction = action;
    }

    public Method(ManagedMethod action, bool restarts = false) : this(restarts)
    {
        ManagedAction = action;
    }

    ///

    protected virtual void OnCompleted() => Completed?.Invoke(this);

    ///

    public void Cancel()
    {
        tokenSource?.Cancel();
        pause = false;
    }

    async public Task CancelAsync()
    {
        Cancel();
        await Task.Run(() => { while (Started) { } });
    }

    bool pause = false;

    public void Pause()
    {
        pause = true;
        tokenSource?.Cancel();
    }

    public async Task Start()
    {
        if (pause)
            pause = false;

        if (Started)
        {
            restart = Restarts;
            Cancel();
            return;
        }

        Started = true;

        tokenSource = new CancellationTokenSource();
        if (UnmanagedAction != null)
            await UnmanagedAction.Invoke(tokenSource.Token);

        if (ManagedAction != null)
            await Task.Run(() => ManagedAction.Invoke(tokenSource.Token), tokenSource.Token);

        Started = false;
        OnCompleted();

        if (restart)
        {
            restart = false;
            await Start();
        }
    }
}

/// <summary>A task that cancels with both a <see cref="TaskManagement.Managed"/> or <see cref="TaskManagement.Unmanaged"/> action, and a parameter of type <see cref="{T}"/>.</summary>
public class Method<T> : Method
{
    class Parameter
    {
        public readonly TaskManagement Execution;

        public readonly T Value;

        public Parameter(T i, TaskManagement e) { Value = i; Execution = e; }
    }

    ///

    [field: NonSerialized]
    readonly ManagedMethod<T> ManagedAction;

    [field: NonSerialized]
    readonly UnmanagedMethod<T> UnmanagedAction;

    Parameter parameter;

    ///

    public Method(ManagedMethod<T> managedAction, UnmanagedMethod<T> unmanagedAction, bool restarts = false) : base(restarts)
    {
        ManagedAction = managedAction; UnmanagedAction = unmanagedAction;
    }

    ///

    bool CheckStart(T input, TaskManagement e)
    {
        if (Started)
        {
            restart
                = Restarts;
            parameter
                = new Parameter(input, e);

            Cancel();
            return false;
        }
        return true;
    }

    void CheckRestart()
    {
        if (restart)
        {
            restart = false;

            var temp = parameter;
            parameter = default;

            _ = Start(temp.Value, temp.Execution);
        }
    }

    ///

    async public Task Start(T input, TaskManagement execution = TaskManagement.Unmanaged)
    {
        if (execution == TaskManagement.Unmanaged)
        {
            if (CheckStart(input, TaskManagement.Unmanaged))
            {
                Started = true;

                tokenSource = new CancellationTokenSource();
                await UnmanagedAction.Invoke(input, tokenSource.Token);

                Started = false;

                OnCompleted();
                CheckRestart();
            }
        }
        else if (execution == TaskManagement.Managed)
        {
            if (CheckStart(input, TaskManagement.Managed))
            {
                Started = true;

                tokenSource = new CancellationTokenSource();
                await Task.Run(() => ManagedAction.Invoke(input, tokenSource.Token), tokenSource.Token);

                Started = false;

                OnCompleted();
                CheckRestart();
            }
        }
    }
}