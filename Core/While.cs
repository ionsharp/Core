using System;
using System.Threading.Tasks;

namespace Imagin.Core;

public static class While
{
    public static async Task InvokeAsync(Func<bool> condition) { await Task.Run(() => { while (condition()) { } }); }

    public static async Task InvokeAsync<T>(Func<T> input, Predicate<T> condition, Action<T> onFinished = null)
    {
        await Task.Run(() => { while (condition(input())) { } });
        onFinished?.Invoke(input());
    }
}