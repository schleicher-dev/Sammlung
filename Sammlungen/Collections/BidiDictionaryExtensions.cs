using Sammlungen.Exceptions;

namespace Sammlungen.Collections
{
    public static class BidiDictionaryExtensions
    {
        public static void Add<TForward, TReverse>(
            this IBidiDictionary<TForward, TReverse> bidiDict, TForward fwd, TReverse rev)
        {
            if (!bidiDict.TryAdd(fwd, rev))
                throw new DuplicateKeyException($"Either forward key '{fwd}' or reverse key '{rev}'" +
                                                $"are already in the {bidiDict.GetType().FullName}");
        }
    }
}