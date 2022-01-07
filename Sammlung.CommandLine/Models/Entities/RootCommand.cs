using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Terminal;

namespace Sammlung.CommandLine.Models.Entities
{
    public class RootCommand<TData> : BindableCommandBase<TData>, IApplicationNameTrait
    {
        string IApplicationNameTrait.ApplicationName { get; set; }

        public RootCommand(ConstructorDelegate dataConstructor) : 
            base(Enumerable.Empty<string>(), dataConstructor)
        {
        }

        public override TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal = null)
        {
            terminal ??= new DefaultTerminal();
            
            try
            {
                return base.Parse(args, terminal);
            }
            catch (Exception ex)
            {
                return ShowHelp(terminal, ex);
            }
        }
    }
}