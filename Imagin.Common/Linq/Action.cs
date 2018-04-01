using Imagin.Common.Debug;
using System;
using System.Threading.Tasks;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Invoke the given action if predicate.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Predicate"></param>
        public static void InvokeIf(this Action Value, Func<Action, bool> Predicate)
        {
            if (Predicate(Value))
                Value.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Result Try(this Action Value)
        {
            try
            {
                Value();
                return new Success() as Result;
            }
            catch (Exception e)
            {
                return new Error(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static async Task<Result> TryAsync(this Action Value)
        {
            return await Task.Run(() => Value.Try());
        }
    }
}
