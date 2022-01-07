using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Fluent;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases.Options
{
    public abstract class OptionBase : MultiParserEntityBase, IKeywordsTrait
    {
        private readonly List<string> _keywords;

        public IEnumerable<string> Keywords => _keywords;

        protected OptionBase(IEnumerable<string> keywords)
        {
            _keywords = keywords.RequireNotNull(nameof(keywords)).ToList().RequireNumElements(1, nameof(keywords));
            this.SetMultiplicity(0, 1);
        }
    }
}