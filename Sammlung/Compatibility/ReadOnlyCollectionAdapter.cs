using System.Collections;
using System.Collections.Generic;

namespace Sammlung.Compatibility
{
    public class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _collection;

        public ReadOnlyCollectionAdapter(ICollection<T> collection)
        {
            _collection = collection;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _collection).GetEnumerator();
        }

        /// <inheritdoc />
        public int Count => _collection.Count;
    }
}