using Sammlung.Werkzeug;

namespace Sammlung.Pipes.SpecialPipes
{
    /// <summary>
    /// The <see cref="ConcatenatedUnDiPipe{TSource,TIntermediate,TTarget}"/> type combines two
    /// <see cref="IUnDiPipe{TSource,TTarget}"/> instances where the second type argument of the first pipe
    /// is the same as the fist type argument of the second pipe.
    /// </summary>
    /// <typeparam name="TSource">the source type</typeparam>
    /// <typeparam name="TIntermediate">the intermediate type</typeparam>
    /// <typeparam name="TTarget">the target type</typeparam>
    internal class ConcatenatedUnDiPipe<TSource, TIntermediate, TTarget> : IUnDiPipe<TSource, TTarget>
    {
        private readonly IUnDiPipe<TSource, TIntermediate> _fstPipe;
        private readonly IUnDiPipe<TIntermediate, TTarget> _sndPipe;

        /// <summary>
        /// Creates a new <see cref="ConcatenatedUnDiPipe{TSource,TIntermediate,TTarget}"/> using the two pipes to
        /// handle as concatenated.
        /// </summary>
        /// <param name="lhsPipe" />
        /// <param name="rhsPipe" />
        public ConcatenatedUnDiPipe(IUnDiPipe<TSource, TIntermediate> lhsPipe, IUnDiPipe<TIntermediate, TTarget> rhsPipe)
        {
            _fstPipe = lhsPipe.RequireNotNull(nameof(lhsPipe));
            _sndPipe = rhsPipe.RequireNotNull(nameof(rhsPipe));
        }

        /// <inheritdoc />
        public TTarget Process(TSource input)
        {
            var intermediate = _fstPipe.Process(input);
            return _sndPipe.Process(intermediate);
        }
    }
}