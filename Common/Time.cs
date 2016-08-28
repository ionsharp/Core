using System;

namespace Imagin.Common
{
    public static class Time
    {
        /// <summary>
        /// Gets time remaining for a process using elapsed time, total number of bytes to receive, and number of bytes received.
        /// </summary>
        public static long Remaining(double TimeElapsed, long Total, long Recieved)
        {
            double LinesProcessed = ((double)Recieved / (double)Total) * 100d;
            //Avoids /0 exception
            LinesProcessed = LinesProcessed == 0 ? 1 : LinesProcessed;
            return Convert.ToInt64(Math.Round((TimeElapsed / LinesProcessed) * (100d - LinesProcessed)));
        }
    }
}
