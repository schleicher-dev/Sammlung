using System.Collections.Generic;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Reflection;
using Sammlung.CommandLine.Terminal;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities
{
    public class Command<TParentData, TData> : BindableCommandBase<TData>, IBindableTrait<TParentData>
    {
        private readonly Property<TParentData, TData> _property;

        public Command(IEnumerable<string> keywords, ConstructorDelegate dataConstructor, Property<TParentData, TData> property) : 
            base(keywords, dataConstructor)
        {
            _property = property.RequireNotNull(nameof(property));
        }

        public override TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal = null)
        {
            var result = base.Parse(args, terminal);
            _property.Value = Data;
            return result;
        }

        public void Bind(TParentData data) => _property.Bind(data);
    }
}