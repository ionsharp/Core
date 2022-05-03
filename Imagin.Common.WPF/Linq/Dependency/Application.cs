using Imagin.Common.Configuration;
using System.Runtime.InteropServices;
using System.Windows;

namespace Imagin.Common.Linq
{
    public static class XApplication
    {
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        [DllImport("user32")]
        public static extern void LockWorkStation();

        static void Hibernate()
            => SetSuspendState(true, true, true);

        static void Lock()
            => LockWorkStation();

        static void LogOff()
            => ExitWindowsEx(0, 0);

        static void Restart()
            => System.Diagnostics.Process.Start("shutdown", "/r /t 0");

        static void Shutdown()
            => System.Diagnostics.Process.Start("shutdown", "/s /t 0");

        static void Sleep()
            => SetSuspendState(false, true, true);

        public static void Exit(this Application input, ExitMethod method)
        {
            switch (method)
            {
                case ExitMethod.None: break;
                case ExitMethod.Exit:
                    input.Shutdown(0);
                    break;

                case ExitMethod.Hibernate:
                    Hibernate();
                    break;

                case ExitMethod.Lock:
                    Lock();
                    break;

                case ExitMethod.LogOff:
                    LogOff();
                    break;

                case ExitMethod.Restart:
                    Restart();
                    break;

                case ExitMethod.Shutdown:
                    Shutdown();
                    break;

                case ExitMethod.Sleep:
                    Sleep();
                    break;
            }
        }
    }
}