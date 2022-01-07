using System.Collections.Generic;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;

namespace Sammlung.CommandLine.Models.Entities
{
    public class Command<TData> : BindableCommandBase<TData>
    {
        public Command(IEnumerable<string> keywords, ConstructorDelegate dataConstructor) : 
            base(keywords, dataConstructor)
        {
        }
    }
}