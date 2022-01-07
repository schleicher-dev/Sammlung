using System.Collections.Generic;
using System.Linq;
using Sammlung.Collections.Queues;

namespace Sammlung.CommandLine
{
    internal static class EnumerableExtensions
    {
        public static IDeque<T> ToDeque<T>(this IEnumerable<T> items)
        {
            if (items is IDeque<T> dq) return dq;
            
            var args = items.ToArray();
            IDeque<T> deque = new ArrayDeque<T>(args.Length);
            foreach (var arg in args) deque.PushRight(arg);
            return deque;
        }
    }
}