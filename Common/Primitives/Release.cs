using System;
using Imagin.Common.Input;
using System.Xml.Serialization;
using Imagin.Common.Extensions;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Release : AbstractObject, ICloneable, IVariant<Version>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public const int MaxValue = int.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        public const int MinValue = 0;

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Version>> Changed;

        int major = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Major
        {
            get
            {
                return major;
            }
            set
            {
                major = value.Coerce(MaxValue, MinValue);
                OnChanged(default(Version));
                OnPropertyChanged("Major");
            }
        }

        int minor = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Minor
        {
            get
            {
                return minor;
            }
            set
            {
                minor = value.Coerce(MaxValue, MinValue);
                OnChanged(default(Version));
                OnPropertyChanged("Minor");
            }
        }

        int build = -1;
        /// <summary>
        /// 
        /// </summary>
        public int Build
        {
            get
            {
                return build;
            }
            set
            {
                build = value.Coerce(MaxValue, MinValue);
                OnChanged(default(Version));
                OnPropertyChanged("Build");
            }
        }

        int revision = -1;
        /// <summary>
        /// 
        /// </summary>
        public int Revision
        {
            get
            {
                return revision;
            }
            set
            {
                revision = value.Coerce(MaxValue, MinValue);
                OnChanged(default(Version));
                OnPropertyChanged("Revision");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Version Version
        {
            get
            {
                return new Version(major, minor, build, revision);
            }
        }

        #endregion

        #region Release

        /// <summary>
        /// 
        /// </summary>
        public Release() : this(MinValue, MinValue)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Version"></param>
        public Release(string Version) : this(new Version(Version))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public Release(Version version) : this(version.Major, version.Minor, version.Build, version.Revision)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Major"></param>
        /// <param name="Minor"></param>
        public Release(int Major, int Minor) : this(Major, Minor, MinValue, MinValue)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Major"></param>
        /// <param name="Minor"></param>
        /// <param name="Build"></param>
        public Release(int Major, int Minor, int Build) : this(Major, Minor, Build, MinValue)
        {
        }

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

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override bool Equals(object Value)
        {
            if (Value is Release)
            {
                return this == (Release)Value;
            }
            else if (Value is Version)
            {
                return Version == (Version)Value;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Version.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Release Clone()
        {
            return new Release(Version);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Version Get()
        {
            return Version;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(Version Value)
        {
            Changed?.Invoke(this, new EventArgs<Version>(Version));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void Set(Version Value)
        {
            Major = Value.Major;
            Minor = Value.Minor;
            Build = Value.Build;
            Revision = Value.Revision;
        }

        #endregion

        #region Operators

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Release(Version Value)
        {
            return new Release(Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Release(int Value)
        {
            return new Release(new Version(Value.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Release a, Release b)
        {
            return a.Version != b.Version;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Release a, Version b)
        {
            return a.Version != b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static bool operator !=(Release a, int Major)
        {
            return a.Version.Major != Major;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Release a, Release b)
        {
            return a.Version == b.Version;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Release a, Version b)
        {
            return a.Version == b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static bool operator ==(Release a, int Major)
        {
            return a.Version.Major == Major;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(Release a, Release b)
        {
            return a.Version < b.Version;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(Release a, Version b)
        {
            return a.Version < b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static bool operator <(Release a, int Major)
        {
            return a.Version.Major < Major;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(Release a, Release b)
        {
            return a.Version <= b.Version;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(Release a, Version b)
        {
            return a.Version <= b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static bool operator <=(Release a, int Major)
        {
            return a.Version.Major <= Major;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Release a, Release b)
        {
            return a.Version < b.Version;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Release a, Version b)
        {
            return a.Version < b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static bool operator >(Release a, int Major)
        {
            return a.Version.Major > Major;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(Release a, Release b)
        {
            return a.Version >= b.Version;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(Release a, Version b)
        {
            return a.Version >= b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static bool operator >=(Release a, int Major)
        {
            return a.Version.Major >= Major;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Release operator +(Release a, Release b)
        {
            return new Release(a.Major + b.Major, a.Minor + b.Minor, a.Build + b.Build, a.Revision + b.Revision);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Release operator +(Release a, Version b)
        {
            return new Release(a.Major + b.Major, a.Minor + b.Minor, a.Build + b.Build, a.Revision + b.Revision);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static Release operator +(Release a, int Major)
        {
            return new Release(a.Major + Major, a.Minor, a.Build, a.Revision);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Release operator -(Release a, Release b)
        {
            return new Release(a.Major - b.Major, a.Minor - b.Minor, a.Build - b.Build, a.Revision - b.Revision);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Release operator -(Release a, Version b)
        {
            return new Release(a.Major - b.Major, a.Minor - b.Minor, a.Build - b.Build, a.Revision - b.Revision);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Major"></param>
        /// <returns></returns>
        public static Release operator -(Release a, int Major)
        {
            return new Release(a.Major - Major, a.Minor, a.Build, a.Revision);
        }

        #endregion
    }
}
