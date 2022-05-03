using Imagin.Common.Analytics;

namespace Imagin.Common
{
    public class NotSerializableWarning : Warning
    {
        public NotSerializableWarning(object input) : base($"'{input.GetType().FullName}' is not marked as serializable.") { }
    }
}