using Imagin.Common.Converters;
using Imagin.Common.Data;
using System;

namespace Imagin.Common.Controls
{
    internal class SourceFilter
    {
        public readonly bool Ignore;

        public readonly object Source;

        public readonly Type Filter;

        public SourceFilter(object source, Type filter, bool ignore) : base()
        {
            Source = source;
            Filter = filter;
            Ignore = ignore;
        }
    }

    internal class SourceFilterUpdate { }

    /// <summary>
    /// Filters an object before <see cref="PropertyGrid"/> reads it.
    /// Avoids <see cref="PropertyGrid"/> loading multiple times when <see cref="PropertyGrid.FilterAttribute"/> changes.
    /// Note: This is not a bug. This is by design!
    /// </summary>
    public class SourceFilterBinding : LocalBinding
    {
        public readonly Type Filter;

        public bool Ignore { get; set; }

        public SourceFilterBinding(Type filter) : this(".", filter) { }

        public SourceFilterBinding(string path, Type filter) : base(path)
        {
            Filter = filter;
            Converter = new SimpleConverter<object, SourceFilter>(i => new(i, Filter, Ignore));
        }
    }
}