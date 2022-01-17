using System.Collections.Generic;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Utilities;

namespace Sammlung.CommandLine.Models.Entities.Factories
{
    public static class CommandFactory
    {
        public static Command<TData, TSubData> Create<TData, TSubData>(IEnumerable<string> keywords,
            BindableSetter<TData, TSubData> bindableSetter) where TSubData : new() =>
            Create(keywords, () => new TSubData(), bindableSetter);

        public static Command<TData, TSubData> Create<TData, TSubData>(IEnumerable<string> keywords,
            BindableCommandBase<TSubData>.ConstructorDelegate paramsConstructor,
            BindableSetter<TData, TSubData> bindableSetter) where TSubData : new() =>
            new Command<TData, TSubData>(keywords, paramsConstructor, bindableSetter);
    }
}