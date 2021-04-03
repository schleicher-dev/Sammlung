using JetBrains.Annotations;

namespace Sammlung.Graphs
{
    [PublicAPI]
    public interface IEdge<out TVertex, out TWeight>
    {
        TVertex SourceVertex { get; }    
        TVertex TargetVertex { get; }
        TWeight Weight { get; }
    }
}