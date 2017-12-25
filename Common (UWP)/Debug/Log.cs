using Imagin.Common.Config;
using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// 
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 
        /// </summary>
        static IApp App
        {
            get
            {
                return Application.Current as IApp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Action"></param>
        static void Write(Action<IApp> Action)
        {
            if (App != null) Action(App);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        public static void Write(object Message, LogEntryType Type = LogEntryType.Message)
        {
            Write(i => i.Log.Write(Message, Type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Source"></param>
        /// <param name="Type"></param>
        public static void Write(object Message, object Source, LogEntryType Type = LogEntryType.Message)
        {
            Write(i => i.Log.Write(Message, Source, Type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Format"></param>
        public static void Write(string Message, params object[] Format)
        {
            Write(i => i.Log.Write(Message.F(Format)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        /// <param name="Format"></param>
        public static void Write(string Message, LogEntryType Type, params object[] Format)
        {
            Write(i => i.Log.Write(Message.F(Format), Type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Source"></param>
        /// <param name="Type"></param>
        /// <param name="Format"></param>
        public static void Write(string Message, object Source, LogEntryType Type, params object[] Format)
        {
            Write(i => i.Log.Write(Message.F(Format), Source, Type));
        }
    }
}
