using Sammlung.Werkzeug;

namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="CompositeBiDiPipe{TSource,TTarget}"/> which combines two <see cref="IUnDiPipe{TSource,TTarget}"/>
    /// instances to form a <see cref="IBiDiPipe{TSource,TTarget}"/> instance.
    /// </summary>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    internal class CompositeBiDiPipe<TSource, TTarget> : IBiDiPipe<TSource, TTarget>
    {
        private readonly IUnDiPipe<TSource, TTarget> _fwdPipe;
        private readonly IUnDiPipe<TTarget, TSource> _revPipe;

        /// <summary>
        /// Creates a new <see cref="CompositeBiDiPipe{TSource,TTarget}"/> using the forward and reverse
        /// <see cref="IUnDiPipe{TSource,TTarget}"/> instances.
        /// </summary>
        /// <param name="fwdPipe">the forward pipe</param>
        /// <param name="revPipe"></param>
        public CompositeBiDiPipe(IUnDiPipe<TSource, TTarget> fwdPipe, IUnDiPipe<TTarget, TSource> revPipe)
        {
            _fwdPipe = fwdPipe.RequireNotNull(nameof(fwdPipe));
            _revPipe = revPipe.RequireNotNull(nameof(revPipe));
        }

        /// <inheritdoc />
        public TTarget ProcessForward(TSource input) => _fwdPipe.Process(input);

        /// <inheritdoc />
        public TSource ProcessReverse(TTarget input) => _revPipe.Process(input);
    }
}