using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Pipes;

namespace Sammlung.CommandLine.Models.Fluent
{
    public static class CommandExtensions
    {
        public static TCmd AddFlags<TCmd, TData>(this TCmd parent, params Flag<TData>[] flags)
            where TCmd : BindableCommandBase<TData> =>
            parent.AddFlags(flags.AsEnumerable());

        public static TCmd AddFlags<TCmd, TData>(this TCmd parent, IEnumerable<Flag<TData>> flags)
            where TCmd : BindableCommandBase<TData>
        {
            foreach (var flag in flags)
                parent.PushOption(flag);
            return parent;
        }

        public static TCmd AddOptions<TCmd, TData>(this TCmd parent, params Option<TData>[] options)
            where TCmd : BindableCommandBase<TData> =>
            parent.AddOptions(options.AsEnumerable());

        public static TCmd AddOptions<TCmd, TData>(this TCmd parent, IEnumerable<Option<TData>> options)
            where TCmd : BindableCommandBase<TData>
        {
            foreach (var option in options)
                parent.PushOption(option);
            return parent;
        }

        public static TCmd AddArguments<TCmd, TData>(this TCmd parent, params Argument<TData>[] arguments)
            where TCmd : BindableCommandBase<TData>
        {
            return parent.AddArguments(arguments.AsEnumerable());
        }        
        
        public static TCmd AddArguments<TCmd, TData>(this TCmd parent, IEnumerable<Argument<TData>> arguments)
            where TCmd : BindableCommandBase<TData>
        {
            foreach (var argument in arguments)
                parent.PushArgument(argument);
            return parent;
        }

        public static TParentCmd AddCommand<TParentCmd, TParentData, TData>(this TParentCmd parent, Command<TParentData, TData> command) 
            where TParentCmd : BindableCommandBase<TParentData>
        {
            parent.PushCommand(command);
            return parent;
        }

        public static Flag<TData> BuildFlag<TData>(this IPipeTerminal<TData, string> terminal,
            params string[] keywords) => new Flag<TData>(keywords, terminal);

        public static Option<TData> BuildOption<TData>(this IPipeTerminal<TData, string> terminal,
            params string[] keywords) => new Option<TData>(keywords, terminal);

        public static Argument<TData> BuildArgument<TData>(this IPipeTerminal<TData, string> terminal) =>
            new Argument<TData>(terminal);
    }
}