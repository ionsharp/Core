using Imagin.Common;
using System;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public abstract class Preset<T> : BaseNamable, ICloneable
    {
        public readonly T Instance;

        public Preset() : base() { }

        public Preset(string name, T instance = default) : base(name) => Instance = instance;

        object ICloneable.Clone() => Clone();
        public abstract Preset<T> Clone();
    }
}