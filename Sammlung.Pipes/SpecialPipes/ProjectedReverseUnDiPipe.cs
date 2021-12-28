using Sammlung.Werkzeug;

namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="ProjectedReverseUnDiPipe{TSource,TTarget}"/> type takes a
    /// <see cref="IBiDiPipe{TSource,TTarget}"/> and projects it down to a <see cref="IUnDiPipe{TTarget,TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    internal class ProjectedReverseUnDiPipe<TSource, TTarget> : IUnDiPipe<TTarget, TSource>
    {
        private readonly IBiDiPipe<TSource, TTarget> _pipe;

        /// <summary>
        /// Creates a new <see cref="ProjectedReverseUnDiPipe{TSource,TTarget}"/> using the pipe to project.
        /// </summary>
        /// <param name="pipe" />
        public ProjectedReverseUnDiPipe(IBiDiPipe<TSource, TTarget> pipe)
        {
            _pipe = pipe.RequireNotNull(nameof(pipe));
        }
        
        /// <inheritdoc />
        public TSource Process(TTarget input) => _pipe.ProcessReverse(input);
    }
}