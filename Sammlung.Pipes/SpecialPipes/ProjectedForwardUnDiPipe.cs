using Sammlung.Werkzeug;

namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="ProjectedForwardUnDiPipe{TSource,TTarget}"/> type takes a
    /// <see cref="IBiDiPipe{TSource,TTarget}"/> and projects it down to a <see cref="IUnDiPipe{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    internal class ProjectedForwardUnDiPipe<TSource, TTarget> : IUnDiPipe<TSource, TTarget>
    {
        private readonly IBiDiPipe<TSource, TTarget> _pipe;

        /// <summary>
        /// Creates a new <see cref="ProjectedForwardUnDiPipe{TSource,TTarget}"/> using the pipe to project.
        /// </summary>
        /// <param name="pipe" />
        public ProjectedForwardUnDiPipe(IBiDiPipe<TSource, TTarget> pipe)
        {
            _pipe = pipe.RequireNotNull(nameof(pipe));
        }
        
        /// <inheritdoc />
        public TTarget Process(TSource input) => _pipe.ProcessForward(input);
    }
}