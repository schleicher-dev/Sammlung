using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases
{
    public abstract class ArgumentBase : EntityWithMultiplicityBase, IArityTrait
    {
        private List<string> _metaNames;
        protected int NumArity { get; set; }

        int IArityTrait.NumArity
        {
            get => NumArity;
            set => NumArity = value;
        }

        protected int Arity { get; set; }

        int IArityTrait.Arity
        {
            get => Arity;
            set => Arity = value.RequireGreaterEqual(1, nameof(value));
        }

        IEnumerable<string> IArityTrait.MetaNames
        {
            get => _metaNames;
            set => _metaNames = value.RequireNotNull(nameof(value)).ToList();
        }
    }
}