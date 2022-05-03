using Imagin.Common.Collections;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Models;

namespace Imagin.Common.Analytics
{
    public class LogWriter : XmlWriter<LogEntry>, ILog
    {
        public static Limit DefaultLimit = new(5000, Limit.Actions.ClearAndArchive);

        public bool Enabled => Get.Where<IMainViewOptions>()?.LogEnabled == true;

        public LogWriter(string folderPath, Limit limit) : base(nameof(Log), folderPath, nameof(Log), "xml", "xml", limit, typeof(Error), typeof(LogEntry), typeof(Message), typeof(Success), typeof(Warning), typeof(Result), typeof(ResultTypes), typeof(TraceLevel))
            => Get.Register<ILog>(this);
    }
}