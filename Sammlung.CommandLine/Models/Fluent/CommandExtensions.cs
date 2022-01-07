using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Pipes;

namespace Sammlung.CommandLine.Models.Fluent
{
    public static class CommandExtensions
    {
        public static TCmd AddFlag<TCmd, TData>(this TCmd parent, Flag<TData> flag)
            where TCmd : BindableCommandBase<TData>
        {
            parent.PushOption(flag);
            return parent;
        }

        public static TCmd AddOption<TCmd, TData>(this TCmd parent, Option<TData> option)
            where TCmd : BindableCommandBase<TData>
        {
            parent.PushOption(option);
            return parent;
        }

        public static TCmd AddArgument<TCmd, TData>(this TCmd parent, Argument<TData> argument)
            where TCmd : BindableCommandBase<TData>
        {
            parent.PushArgument(argument);
            return parent;
        }

        public static TCmd AddCommand<TCmd>(this TCmd parent, CommandBase command) where TCmd : CommandBase
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