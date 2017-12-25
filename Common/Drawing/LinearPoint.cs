namespace Imagin.Common.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public struct LinearPoint
    {
        /// <summary>
        /// 
        /// </summary>
        public double DistanceX
        {
            get
            {
                if (x1 > x2)
                {
                    return x1 - x2;
                }
                else if (x1 < x2)
                {
                    return x2 - x1;
                }
                else return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double DistanceY
        {
            get
            {
                if (y1 > y2)
                {
                    return y1 - y2;
                }
                else if (y1 < y2)
                {
                    return y2 - y1;
                }
                else return 0;
            }
        }

        readonly double x1;
        /// <summary>
        /// 
        /// </summary>
        public double X1
        {
            get
            {
                return x1;
            }
        }

        readonly double x2;
        /// <summary>
        /// 
        /// </summary>
        public double X2
        {
            get
            {
                return x2;
            }
        }

        readonly double y1;
        /// <summary>
        /// 
        /// </summary>
        public double Y1
        {
            get
            {
                return y1;
            }
        }

        readonly double y2;
        /// <summary>
        /// 
        /// </summary>
        public double Y2
        {
            get
            {
                return y2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        public LinearPoint(double X1, double Y1, double X2, double Y2)
        {
            x1 = X1;
            y1 = Y1;
            x2 = X2;
            y2 = Y2;
        }

        static bool Compare(LinearPoint a, LinearPoint b)
        {
            return a.X1 == b.X1 && a.X2 == b.X2 && a.Y1 == b.Y1 && a.Y2 == b.Y2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public override bool Equals(object a)
        {
            if (a is LinearPoint)
                return Compare(this, (LinearPoint)a);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Equals(LinearPoint a)
        {
            return Equals((object)a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x1.GetHashCode() ^ y1.GetHashCode() ^ x2.GetHashCode() ^ y2.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(LinearPoint a, LinearPoint b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if ((object)a == null || (object)b == null)
                return false;

            return Compare(a, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(LinearPoint a, LinearPoint b)
        {
            return !(a == b);
        }
    }
}
