using Sammlung.Werkzeug;

namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="ConcatenatedBiDiPipe{TSource,TIntermediate,TTarget}"/> type combines two
    /// <see cref="IBiDiPipe{TSource,TTarget}"/> instances where the second type argument of the first pipe
    /// is the same as the fist type argument of the second pipe.
    /// </summary>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TIntermediate">the intermediate type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    internal class ConcatenatedBiDiPipe<TSource, TIntermediate, TTarget> : IBiDiPipe<TSource, TTarget>
    {
        private readonly IBiDiPipe<TSource, TIntermediate> _fstPipe;
        private readonly IBiDiPipe<TIntermediate, TTarget> _sndPipe;

        /// <summary>
        /// Creates a new <see cref="ConcatenatedBiDiPipe{TSource,TIntermediate,TTarget}"/> using the two pipes to
        /// handle as concatenated.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        public ConcatenatedBiDiPipe(IBiDiPipe<TSource, TIntermediate> lhsPipe,
            IBiDiPipe<TIntermediate, TTarget> rhsPipe)
        {
            _fstPipe = lhsPipe.RequireNotNull(nameof(lhsPipe));
            _sndPipe = rhsPipe.RequireNotNull(nameof(rhsPipe));
        }

        /// <inheritdoc />
        public TTarget ProcessForward(TSource input)
        {
            var intermediate = _fstPipe.ProcessForward(input);
            return _sndPipe.ProcessForward(intermediate);
        }

        /// <inheritdoc />
        public TSource ProcessReverse(TTarget input)
        {
            var intermediate = _sndPipe.ProcessReverse(input);
            return _fstPipe.ProcessReverse(intermediate);
        }
    }
}