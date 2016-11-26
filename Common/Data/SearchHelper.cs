using Imagin.Common.Extensions;
using System;
using System.Diagnostics;

namespace Imagin.Common.Data
{
    public class SearchHelper
    {
        Stopwatch stopwatch = new Stopwatch();
        /// <summary>
        /// Stores information about current search, if any.
        /// </summary>
        public Stopwatch Stopwatch
        {
            get
            {
                return stopwatch;
            }
            set
            {
                stopwatch = value;
            }
        }

        System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// Stores information about current search, if any.
        /// </summary>
        public System.Timers.Timer Timer
        {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
            }
        }

        SearchInfo searchInfo = default(SearchInfo);
        /// <summary>
        /// Stores information about current search, if any.
        /// </summary>
        public SearchInfo SearchInfo
        {
            get
            {
                return searchInfo;
            }
            set
            {
                searchInfo = value;
            }
        }

        public void ResetTimer(bool Start)
        {
            this.ResetTimer(Start, new Action<long>((ElapsedMilliseconds) => SearchInfo.Duration = TimeSpan.FromMilliseconds(ElapsedMilliseconds).TotalSeconds.Round(4)));
        }

        public void ResetTimer(bool Start, Action<long> Elapsed)
        {
            Timer = new System.Timers.Timer();
            Timer.Interval = 100;
            Timer.Elapsed += (a, b) => Elapsed(Stopwatch.ElapsedMilliseconds);
            if (Start) Timer.Start();
        }

        public SearchHelper()
        {
        }
    }
}
