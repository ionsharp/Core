using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DialogButtons
    {
        static string BaseLoc = "Imagin.Common:Main:{0}";

        static DialogButton[] Get(params DialogButton[] Buttons)
        {
            return Buttons;
        }

        static DialogButton Get(string label, int result, bool isDefault = false, bool isCancel = false)
        {
            return new DialogButton(BaseLoc.F(label), result, isDefault, isCancel);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] YesNo = Get(Get("Yes", 0, true), Get("No", 1, false, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] YesCancel = Get(Get("Yes", 0, true), Get("Cancel", 1, false, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] YesNoCancel = Get(Get("Yes", 0, true), Get("No", 1), Get("Cancel", 2, false, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] Ok = Get(Get("Ok", 0, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] Done = Get(Get("Done", 0, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] Continue = Get(Get("Continue", 0, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] OkCancel = Get(Get("Ok", 0, true), Get("Cancel", 1, false, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] ContinueCancel = Get(Get("Continue", 0, true), Get("Cancel", 1, false, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] AbortRetryIgnore = Get(Get("Abort", 0), Get("Retry", 1, true), Get("Ignore", 2, false, true));

        /// <summary>
        /// 
        /// </summary>
        public static DialogButton[] Cancel = Get(Get("Cancel", 0, false, true));
    }
}
