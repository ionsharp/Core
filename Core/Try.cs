using Imagin.Core.Analytics;
using System;
using System.Threading.Tasks;

namespace Imagin.Core;

public static class Try
{
    public static Result Invoke(Action @try, Action<Exception> @catch = null, Action @finally = null, bool research = false)
    {
        try
        {
            @try();
            return new Success();
        }
        catch (Exception e)
        {
            @catch?.Invoke(e);

            if (research)
            {
                var xcb = "http://stackoverflow.com/search?q=[c#]+" + e.Message;
                System.Diagnostics.Process.Start(xcb);
            }

            return new Error(e);
        }
        finally
        {
            @finally?.Invoke();
        }
    }

    public static Result Invoke<T>(out T output, Func<T> @try, Action<Exception> @catch = null, Action @finally = null, bool research = false)
    {
        T temp = default;
        var result = Invoke(() => temp = @try(), @catch, @finally, research);
        output = temp;
        return result;
    }

    public static async Task<Result> InvokeAsync(Action @try, Action<Exception> @catch = null, Action @finally = null, bool research = false) => await Task.Run(() => Invoke(@try, @catch, @finally, research));

    ///

    public static T Return<T>(Func<T> @try, Action<Exception> @catch = null, Action @finally = null, bool research = false)
    {
        try
        {
            return @try();
        }
        catch (Exception e)
        {
            @catch?.Invoke(e);

            if (research)
            {
                var xcb = "http://stackoverflow.com/search?q=[c#]+" + e.Message;
                System.Diagnostics.Process.Start(xcb);
            }
        }
        finally
        {
            @finally?.Invoke();
        }
        return default;
    }
}