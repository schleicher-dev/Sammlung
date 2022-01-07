using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases
{
    /// <summary>
    /// The <see cref="MultiParserEntityBase"/> is the base type for all entities which may be used many times in the parser.
    /// </summary>
    public abstract class MultiParserEntityBase : IParserEntity, IMultiplicityTrait, IParseTrait
    {
        /// <inheritdoc />
        public string Description { get; set; }
        
        /// <inheritdoc />
        public abstract string Format(IEntityFormatter formatter);
        
        /// <inheritdoc />
        public int MinOccurrences { get; private set; } = 1;

        /// <inheritdoc />
        public int? MaxOccurrences { get; private set; } = 1;

        /// <inheritdoc />
        public void AssignMultiplicity(int minOccurrences, int? maxOccurrences)
        {
            var maximumValue = maxOccurrences.GetValueOrDefault(minOccurrences);
            MinOccurrences = minOccurrences.RequireGreaterEqual(0, nameof(minOccurrences))
                .RequireLessEqual(maximumValue, nameof(minOccurrences));
            MaxOccurrences = maxOccurrences?.RequireGreaterEqual(0, nameof(maxOccurrences)) ?? default;
        }

        public abstract IParseStateMachine ParseStateMachine { get; }
    }
}