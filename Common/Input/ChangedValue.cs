namespace Imagin.Common.Input
{
    /// <summary>
    /// Specifies a changed value.
    /// </summary>
    public class ChangedValue : ChangedValue<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        public ChangedValue(object OldValue, object NewValue) : base(OldValue, NewValue)
        {
        }
    }
}
