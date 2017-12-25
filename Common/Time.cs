using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct Time
    {
        short hour;
        /// <summary>
        /// 
        /// </summary>
        public short Hour
        {
            get
            {
                return hour;
            }
            set
            {
                hour = value.Coerce(23);
            }
        }

        short minute;
        /// <summary>
        /// 
        /// </summary>
        public short Minute
        {
            get
            {
                return minute;
            }
            set
            {
                minute = value.Coerce(59);
            }
        }

        short second;
        /// <summary>
        /// 
        /// </summary>
        public short Second
        {
            get
            {
                return second;
            }
            set
            {
                second = value.Coerce(59);
            }
        }

        short millisecond;
        /// <summary>
        /// 
        /// </summary>
        public short Millisecond
        {
            get
            {
                return second;
            }
            set
            {
                second = value.Coerce(999);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Meridiem
        {
            get
            {
                if (hour < 12)
                {
                    return "am";
                }
                else if (hour >= 12)
                    return "pm";

                return string.Empty;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hour"></param>
        /// <param name="Minute"></param>
        /// <param name="Second"></param>
        /// <param name="Millisecond"></param>
        public Time(short Hour, short Minute = 0, short Second = 0, short Millisecond = 0)
        {
            hour = Hour;
            minute = Minute;
            second = Second;
            millisecond = Millisecond;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        /// <returns></returns>
        public string ToString(string Format = "{0}:{1} {m}")
        {
            var Result = Format.Replace("{m}", Meridiem);

            var h = default(short);
            if (hour == 0)
            {
                h = 12;
            }
            else if (hour > 0 && hour < 13)
            {
                h = hour;
            }
            else if (hour > 12)
                h = (hour - 12).ToInt16();

            Result = Result.F(h, Minute, Second, Millisecond);
            return Result;
        }
    }
}
