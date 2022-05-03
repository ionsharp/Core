using System;
using System.Threading.Tasks;

namespace Imagin.Common
{
    public class While
    {
        public static async Task InvokeAsync<T>(Func<T> input, Predicate<T> condition, Action<T> onFinished = null)
        {
            await Task.Run(() => { while (condition(input())) { } });
            onFinished?.Invoke(input());
        }
    }
}