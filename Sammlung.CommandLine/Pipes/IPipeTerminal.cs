using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Pipes
{
    /// <summary>
    /// The <see cref="IPipeTerminal{TData,T1}"/> is an interface which should get control over an underlying pipe.
    /// </summary>
    /// <typeparam name="TData">the data type</typeparam>
    /// <typeparam name="T1">the input type</typeparam>
    public interface IPipeTerminal<in TData, in T1> : IBindableTrait<TData>
    {
        /// <summary>
        /// Executes the conversion stage of the terminal on the input parameter and writes the result to the
        /// bound <see cref="TData"/>.
        /// </summary>
        /// <param name="input">the input</param>
        void ExecuteConversionStage(T1 input);

        /// <summary>
        /// Executes the validation stage of the terminal on the value in the <see cref="TData"/> object.
        /// </summary>
        void ExecuteValidationStage();

        /// <summary>
        /// Executes all stages of the terminal on the input parameter and writes the result to the
        /// bound <see cref="TData"/>.
        /// </summary>
        /// <param name="input">the input</param>
        /// <remarks>
        /// Executes the <see cref="ExecuteConversionStage"/> and then the <see cref="ExecuteValidationStage"/>.
        /// </remarks>
        void ExecuteAll(T1 input);
    }
}