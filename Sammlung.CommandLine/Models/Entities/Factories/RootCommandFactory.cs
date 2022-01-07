using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;

namespace Sammlung.CommandLine.Models.Entities.Factories
{
    public static class RootCommandFactory
    {
        public static RootCommand<T> Create<T>() where T : new() => new RootCommand<T>(() => new T());

        public static RootCommand<T> Create<T>(BindableCommandBase<T>.ConstructorDelegate paramsConstructor) =>
            new RootCommand<T>(paramsConstructor);
    }
}