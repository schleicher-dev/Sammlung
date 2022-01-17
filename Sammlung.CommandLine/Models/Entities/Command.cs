using System.Collections.Generic;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Terminal;
using Sammlung.CommandLine.Utilities;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities
{
    public class Command<TParentData, TData> : BindableCommandBase<TData>, IBindableTrait<TParentData>
    {
        private readonly BindableSetter<TParentData, TData> _bindableSetter;

        public Command(IEnumerable<string> keywords, ConstructorDelegate dataConstructor, BindableSetter<TParentData, TData> bindableSetter) : 
            base(keywords, dataConstructor)
        {
            _bindableSetter = bindableSetter.RequireNotNull(nameof(bindableSetter));
        }

        public override TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal = null)
        {
            var result = base.Parse(args, terminal);
            _bindableSetter.Value = Data;
            return result;
        }

        public void Bind(TParentData data) => _bindableSetter.Bind(data);
    }
}