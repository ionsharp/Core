using System;

namespace Imagin.Common.Configuration
{
    public class StartTask
    {
        public readonly bool Dispatch;

        public readonly Action Action;

        public readonly string Message;

        public StartTask(bool dispatch, string message, Action action)
        {
            Dispatch = dispatch;
            Message = message;
            Action = action;
        }
    }
}