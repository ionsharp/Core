using Imagin.Core.Linq;
using System;
using System.Threading.Tasks;

namespace Imagin.Core.Threading;

public class Operation : Method
{
    public enum Statuses { Active, Inactive }

    public DateTime Added { get => Get(DateTime.Now); set => Set(value); }

    public OperationType Type { get => Get(OperationType.Create); set => Set(value); }

    public string Source { get => Get(""); set => Set(value); }

    public string Target { get => Get(""); set => Set(value); }

    public long SizeRead { get => Get(0L); set => Set(value); }

    public long Size { get => Get(0L); set => Set(value); }

    public double Speed => Duration.TotalSeconds == 0 ? 0 : SizeRead.Double() / Duration.TotalSeconds;

    public double Progress { get => Get(.0); set => Set(value); }

    public TimeSpan Duration { get => Get(TimeSpan.Zero); set => Set(value); }

    public Statuses Status { get => Get(Statuses.Inactive); set => Set(value); }

    public Operation(OperationType type, string source, string target, ManagedMethod action) : base(action)
    {
        Added = DateTime.Now;
        Type = type;
        Source = source;

        if (System.IO.File.Exists(source))
        {
            var fileInfo = new System.IO.FileInfo(source);
            Size = fileInfo.Length;
        }

        Target = target;
    }

    public override void OnPropertyChanging(PropertyChangingEventArgs e)
    {
        base.OnPropertyChanging(e);
        if (e.PropertyName == nameof(Duration))
            e.NewValue = TimeSpan.FromSeconds(e.NewValue.To<TimeSpan>().TotalSeconds.Round());
    }

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(Duration) || e.PropertyName == nameof(SizeRead))
            Update(() => Speed);
    }

    new public async Task Start()
    {
        Status = Statuses.Active;
        await base.Start();
        Status = Statuses.Inactive;
    }
}