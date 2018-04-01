using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// A bindable variation of <see cref="System.Version"/>.
    /// </summary>
    public class Release : ObjectBase, ICloneable, IEquatable<Release>, IVariation<Version>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public const int Maximum = int.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        public const int Minimum = 0;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Version>> Changed;

        int _major = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Major
        {
            get => _major;
            set
            {
                if (!value.WithinRange(Minimum, Maximum))
                    throw new ArgumentOutOfRangeException(nameof(value));

                Property.Set(this, ref _major, value);
                OnChanged(Version);
            }
        }

        int _minor = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Minor
        {
            get => _minor;
            set
            {
                if (!value.WithinRange(Minimum, Maximum))
                    throw new ArgumentOutOfRangeException(nameof(value));

                Property.Set(this, ref _minor, value);
                OnChanged(Version);
            }
        }

        int _build = -1;
        /// <summary>
        /// 
        /// </summary>
        public int Build
        {
            get => _build;
            set
            {
                if (!value.WithinRange(Minimum, Maximum))
                    throw new ArgumentOutOfRangeException(nameof(value));

                Property.Set(this, ref _build, value);
                OnChanged(Version);
            }
        }

        int _revision = -1;
        /// <summary>
        /// 
        /// </summary>
        public int Revision
        {
            get => _revision;
            set
            {
                if (!value.WithinRange(Minimum, Maximum))
                    throw new ArgumentOutOfRangeException(nameof(value));

                Property.Set(this, ref _revision, value);
                OnChanged(Version);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Version Version
        {
            get
            {
                return new Version(_major, _minor, _build, _revision);
            }
        }

        #endregion

        #region Release

        /// <summary>
        /// 
        /// </summary>
        public Release() : this(Minimum, Minimum) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Version"></param>
        public Release(string Version) : this(new Version(Version)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public Release(Version version) : this(version.Major, version.Minor, version.Build, version.Revision) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        public Release(int major, int minor) : this(major, minor, Minimum, Minimum) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        public Release(int major, int minor, int build) : this(major, minor, build, Minimum) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        public Release(int major, int minor, int build, int revision) : base()
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        #endregion

        #region Operators

#pragma warning disable 1591
        public static implicit operator Release(Version Value) => new Release(Value);
        public static implicit operator Release(int Value) => new Release(new Version(Value.ToString()));

        public static bool operator !=(Release a, Release b) => !(a == b);
        public static bool operator !=(Release a, Version b) => !(a == b);
        public static bool operator !=(Release a, int major) => !(a == major);

        public static bool operator ==(Release a, Release b) => a.Equals_(b);
        public static bool operator ==(Release a, Version b) => a.Version == b;
        public static bool operator ==(Release a, int major) => a.Version.Major == major;

        public static bool operator <(Release a, Release b) => a.Version < b.Version;
        public static bool operator <(Release a, Version b) => a.Version < b;
        public static bool operator <(Release a, int Major) => a.Version.Major < Major;

        public static bool operator <=(Release a, Release b) => a.Version <= b.Version;
        public static bool operator <=(Release a, Version b) => a.Version <= b;
        public static bool operator <=(Release a, int major) => a.Version.Major <= major;

        public static bool operator >(Release a, Release b) => a.Version < b.Version;
        public static bool operator >(Release a, Version b) => a.Version < b;
        public static bool operator >(Release a, int major) => a.Version.Major > major;

        public static bool operator >=(Release a, Release b) => a.Version >= b.Version;
        public static bool operator >=(Release a, Version b) => a.Version >= b;
        public static bool operator >=(Release a, int major) => a.Version.Major >= major;

        public static Release operator +(Release a, Release b) => new Release(a.Major + b.Major, a.Minor + b.Minor, a.Build + b.Build, a.Revision + b.Revision);
        public static Release operator +(Release a, Version b) => new Release(a.Major + b.Major, a.Minor + b.Minor, a.Build + b.Build, a.Revision + b.Revision);
        public static Release operator +(Release a, int major) => new Release(a.Major + major, a.Minor, a.Build, a.Revision);

        public static Release operator -(Release a, Release b) => new Release(a.Major - b.Major, a.Minor - b.Minor, a.Build - b.Build, a.Revision - b.Revision);
        public static Release operator -(Release a, Version b) => new Release(a.Major - b.Major, a.Minor - b.Minor, a.Build - b.Build, a.Revision - b.Revision);
        public static Release operator -(Release a, int major) => new Release(a.Major - major, a.Minor, a.Build, a.Revision);
#pragma warning restore 1591

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Release o) => this.Equals<Release>(o);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Release)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new [] { Version }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Version.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Release Clone() => new Release(Version);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Version Get() => Version;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnChanged(Version value) => Changed?.Invoke(this, new EventArgs<Version>(value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Set(Version value)
        {
            Major = value.Major;
            Minor = value.Minor;
            Build = value.Build;
            Revision = value.Revision;
        }

        #endregion
    }
}
