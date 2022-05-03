using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Collections.Generic
{
    public class ImmutableCollection<T> : BaseImmutableCollection<T>
    {
        /// <summary>
        /// The collection wrapped by this class to restrict access.
        /// </summary>
        readonly IList<T> source;

        public override int Count => source.Count;

        public ImmutableCollection(IEnumerable<T> source) => this.source = new List<T>(source);

        public ImmutableCollection() : this(Enumerable.Empty<T>()) { }

        public override bool Contains(T Item) => source.Contains(Item);

        public override void CopyTo(T[] Array, int ArrayIndex) => source.CopyTo(Array, ArrayIndex);

        public override IEnumerator<T> GetEnumerator() => source.GetEnumerator();
    }
}