using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sammlung.Collections.Dictionaries;

namespace Fixtures.Sammlung.Collections.Dictionaries.Extras
{
    public delegate IBidiDictionary<int, int> BidiDictDefaultCtor();
    public delegate IBidiDictionary<int, int> BidiDictWithDictCtor(Dictionary<int, int> dict);
    public delegate IBidiDictionary<int, int> BidiDictWithEnumerableCtor(IEnumerable<KeyValuePair<int, int>> enumerable);

    [ExcludeFromCodeCoverage]
    public sealed class BidiDictConstructors : Tuple<BidiDictDefaultCtor, BidiDictWithDictCtor, BidiDictWithEnumerableCtor>
    {
        public BidiDictConstructors(BidiDictDefaultCtor item1, BidiDictWithDictCtor item2,
            BidiDictWithEnumerableCtor item3) : base(item1, item2, item3) { }
    }
}