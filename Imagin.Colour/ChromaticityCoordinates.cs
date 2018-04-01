namespace Imagin.Colour
{
    /// <summary>
    /// 
    /// </summary>
    public struct ChromaticityCoordinates
    {
        readonly ChromacityPoint _r;
        /// <summary>
        /// 
        /// </summary>
        public ChromacityPoint R => _r;

        readonly ChromacityPoint _g;
        /// <summary>
        /// 
        /// </summary>
        public ChromacityPoint G => _g;

        readonly ChromacityPoint _b;
        /// <summary>
        /// 
        /// </summary>
        public ChromacityPoint B => _b;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public ChromaticityCoordinates(ChromacityPoint r, ChromacityPoint g, ChromacityPoint b)
        {
            _r = r;
            _g = g;
            _b = b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ChromaticityCoordinates other) => _r.Equals(other._r) && _g.Equals(other._g) && _b.Equals(other._b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o))
                return false;

            return o is ChromaticityCoordinates && Equals((ChromaticityCoordinates)o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ChromaticityCoordinates left, ChromaticityCoordinates right) => left.Equals(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ChromaticityCoordinates left, ChromaticityCoordinates right) => !left.Equals(right);
    }
}