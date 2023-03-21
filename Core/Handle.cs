using System;
using System.Threading.Tasks;

namespace Imagin.Core;

[Serializable]
public class Handle
{
    public delegate Task AsyncDelegate();

    bool value;

    Handle(bool input) => value = input;

    public Handle() { }

    public static implicit operator Handle(bool input) => new(input);

    public void Invoke(Action action)
    {
        value = true;
        action();
        value = false;
    }

    public void SafeInvoke(Action @unsafe, Action safe = null)
    {
        if (!value)
        {
            value = true;
            @unsafe();
            value = false;
        }
        else safe?.Invoke();
    }

    async public Task SafeInvokeAsync(AsyncDelegate @unsafe, AsyncDelegate safe = null)
    {
        if (!value)
        {
            value = true;
            await @unsafe();
            value = false;
        }
        else if (safe != null)
            await safe();
    }
}