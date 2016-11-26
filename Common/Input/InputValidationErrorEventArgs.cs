using System;

namespace Imagin.Common.Input
{
    public delegate void InputValidationErrorEventHandler(object sender, InputValidationErrorEventArgs e);

    public class InputValidationErrorEventArgs : EventArgs
    {
        #region Properties

        Exception exception;
        public Exception Exception
        {
            get
            {
                return exception;
            }
            private set
            {
                exception = value;
            }
        }

        bool _throwException;
        public bool ThrowException
        {
            get
            {
                return _throwException;
            }
            set
            {
                _throwException = value;
            }
        }

        #endregion

        #region InputValidationErrorEventArgs

        public InputValidationErrorEventArgs(Exception e)
        {
            Exception = e;
        }

        #endregion
    }
}
