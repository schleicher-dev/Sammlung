using Sammlung.Werkzeug;

namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="InvertedBiDiPipe{TSource,TTarget}"/> takes a <see cref="IBiDiPipe{TSource,TTarget}"/> and
    /// inverses the operations <see cref="IBiDiPipe{TSource,TTarget}.ProcessForward"/> and <see cref="IBiDiPipe{TSource,TTarget}.ProcessReverse"/>.
    /// </summary>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    internal class InvertedBiDiPipe<TSource, TTarget> : IBiDiPipe<TTarget, TSource>
    {
        private readonly IBiDiPipe<TSource, TTarget> _pipe;

        /// <summary>
        /// Creates a new <see cref="InvertedBiDiPipe{TSource,TTarget}"/> using an original pipe.
        /// </summary>
        /// <param name="pipe" />
        public InvertedBiDiPipe(IBiDiPipe<TSource, TTarget> pipe)
        {
            _pipe = pipe.RequireNotNull(nameof(pipe));
        }

        /// <inheritdoc />
        public TSource ProcessForward(TTarget input) => _pipe.ProcessReverse(input);

        /// <inheritdoc />
        public TTarget ProcessReverse(TSource input) => _pipe.ProcessForward(input);
    }
}