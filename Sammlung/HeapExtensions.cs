using System;
using Sammlung.Interfaces;

namespace Sammlung
{
    public static class HeapExtensions
    {
        public static bool IsEmpty<TKey, TValue>(this IHeap<TKey, TValue> heap) => (heap?.Count ?? 0) == 0;
        
        public static bool Any<TKey, TValue>(this IHeap<TKey, TValue> heap) => !heap.IsEmpty();
        
        public static TValue Peek<TKey, TValue>(this IHeap<TKey, TValue> heap)
        {
            return heap.TryPeek(out var item)
                ? item
                : throw new InvalidOperationException("Cannot peek from heap. The heap may be empty.");
        }

        public static TValue Pop<TKey, TValue>(this IHeap<TKey, TValue> heap)
        {
            return heap.TryPop(out var item)
                ? item
                : throw new InvalidOperationException("Cannot peek from heap. The heap may be empty.");
        }

        public static TValue Replace<TKey, TValue>(this IHeap<TKey, TValue> heap, TKey key, TValue value)
        {
            return heap.TryReplace(key, value, out var oldValue)
                ? oldValue :
                throw new InvalidOperationException("Cannot replace root in heap. The heap may be empty.");
        }

        public static void Update<TKey, TValue>(this IHeap<TKey, TValue> heap, TValue item, TKey key)
        {
            if (!heap.TryUpdate(item, key))
                throw new InvalidOperationException("Cannot update item in heap. It may not exist.");
        }
    }
}