using Sammlung.Pipes.Werkzeug.Exceptions;

namespace Sammlung.Pipes.Werkzeug.Validators
{
    /// <summary>
    /// The <see cref="ValidatorPipeBase{T}"/> type may be used as the default base class of pipeline validators.
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public abstract class ValidatorPipeBase<T> : IBiDiPipe<T, T>
    {
        /// <summary>
        /// Validates the value.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if the value is valid else false</returns>
        protected abstract bool TryValidate(T value);
        
        /// <summary>
        /// Constructs a proper exception using the input value.
        /// </summary>
        /// <param name="value">the input value</param>
        /// <returns>the exception</returns>
        protected abstract PipelineValidationException GetException(T value);
        
        /// <summary>
        /// Validates the input value.
        /// </summary>
        /// <param name="value">the input value</param>
        /// <returns>the unaltered value</returns>
        /// <exception cref="PipelineValidationException">When the input value is not valid.</exception>
        protected virtual T Validate(T value) => TryValidate(value) ? value : throw GetException(value);
        
        /// <inheritdoc />
        public virtual T ProcessForward(T input) => Validate(input);
        
        /// <inheritdoc />
        public virtual T ProcessReverse(T input) => Validate(input);
    }
}