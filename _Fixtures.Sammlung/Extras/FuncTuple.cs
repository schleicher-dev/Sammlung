using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sammlung;
using Sammlung.Interfaces;

namespace _Fixtures.Sammlung.Extras
{
    [ExcludeFromCodeCoverage]
    public class FuncTuple
    {
        public static FuncTuple Create(DefCreatorFunc zf, DictCreatorFunc df, EnumCreatorFunc ef)
        {
            return new FuncTuple {DefaultCreatorFunc = zf, DictionaryCreatorFunc = df, EnumerableCreatorFunc = ef};
        }

        public delegate IBidiDictionary<int, int> DefCreatorFunc();
        public delegate IBidiDictionary<int, int> DictCreatorFunc(Dictionary<int, int> dict);

        public delegate IBidiDictionary<int, int> EnumCreatorFunc(IEnumerable<KeyValuePair<int, int>> enumerable);
        
        public DefCreatorFunc DefaultCreatorFunc { get; set; }
        public DictCreatorFunc DictionaryCreatorFunc { get; set; }
        public EnumCreatorFunc EnumerableCreatorFunc { get; set; }

        public void Deconstruct(out DefCreatorFunc zf, out DictCreatorFunc df, out EnumCreatorFunc ef)
        {
            zf = DefaultCreatorFunc;
            df = DictionaryCreatorFunc;
            ef = EnumerableCreatorFunc;
        }
    }
}