using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases.Arguments
{
    public abstract class ArgumentBase : MultiParserEntityBase, IArityTrait
    {
        private List<string> _metaNames;
        private int _arity;

        public int Arity
        {
            get => _arity;
            set => _arity = value.RequireGreaterEqual(1, nameof(value));
        }

        public IEnumerable<string> MetaNames
        {
            get => _metaNames;
            set => _metaNames = value.RequireNotNull(nameof(value)).ToList();
        }
    }
}