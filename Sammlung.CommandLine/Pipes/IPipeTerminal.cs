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
        void Execute(T1 input);
    }
}