using Imagin.Core.Analytics;
using System;
using System.Threading.Tasks;

namespace Imagin.Core
{
    public static class Try
    {
        public static Result Invoke(Action @try, Action<Exception> @catch = null, Action @finally = null)
        {
            try
            {
                @try();
                return new Success();
            }
            catch (Exception e)
            {
                @catch?.Invoke(e);
                return new Error(e);
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static Result Invoke<T>(out T output, Func<T> @try, Action<Exception> @catch = null, Action @finally = null)
        {
            T temp = default;
            var result = Invoke(() => temp = @try(), @catch, @finally);
            output = temp;
            return result;
        }

        public static async Task<Result> InvokeAsync(Action @try, Action<Exception> @catch = null, Action @finally = null) => await Task.Run(() => Invoke(@try, @catch, @finally));
    }
}