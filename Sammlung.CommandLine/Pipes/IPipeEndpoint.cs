using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Pipes
{
    public interface IPipeEndpoint<in TData, in T> : IBindableTrait<TData>
    {
        void PushValue(T value);
    }
}