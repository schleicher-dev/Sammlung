using System.Diagnostics.CodeAnalysis;

namespace Sammlung.Graphs
{
    /// <summary>
    /// The <see cref="IEdge{TVertex,TWeight}"/> represents a weighted edge in a graph.
    /// </summary>
    /// <typeparam name="T">the vertex type</typeparam>
    /// <typeparam name="TWeight">the edge weight type</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "PublicAPI")]
    public interface IEdge<out T, out TWeight>
    {
        /// <summary>
        /// The source vertex.
        /// </summary>
        T SourceVertex { get; }
        
        /// <summary>
        /// The target vertex.
        /// </summary>
        T TargetVertex { get; }
        
        /// <summary>
        /// The weight.
        /// </summary>
        TWeight Weight { get; }
    }
}