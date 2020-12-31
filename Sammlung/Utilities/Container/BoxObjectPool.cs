using Sammlung.Utilities.Patterns;

namespace Sammlung.Utilities.Container
{
    internal class BoxObjectPool<TKey, TValue> : ObjectPoolBase<Box<TKey, TValue>>
    {
        /// <inheritdoc />
        protected override Box<TKey, TValue> CreateInstance() => new Box<TKey, TValue>();

        /// <inheritdoc />
        protected override Box<TKey, TValue> ResetInstance(Box<TKey, TValue> instance)
        {
            instance.Key = default;
            instance.Value = default;
            return instance;
        }

        public Box<TKey, TValue> Get(TKey key, TValue value)
        {
            var instance = Get();
            instance.Key = key;
            instance.Value = value;
            return instance;
        }
    }
}