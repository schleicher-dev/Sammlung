using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases
{
    public abstract class EntityWithMultiplicityBase : EntityBase, IMultiplicityTrait, IParseTrait
    {
        protected int NumOccurrences { get; set; }

        int IMultiplicityTrait.NumOccurrences
        {
            get => NumOccurrences;
            set => NumOccurrences = value;
        }

        public int MinOccurrences { get; private set; }
        public int? MaxOccurrences { get; private set; }

        protected EntityWithMultiplicityBase()
        {
            AssignMultiplicity(1, 1);
        }

        /// <inheritdoc />
        public void AssignMultiplicity(int minOccurrences, int? maxOccurrences)
        {
            var maximumValue = maxOccurrences.GetValueOrDefault(minOccurrences);
            MinOccurrences = minOccurrences.RequireGreaterEqual(0, nameof(minOccurrences))
                .RequireLessEqual(maximumValue, nameof(minOccurrences));
            MaxOccurrences = maxOccurrences?.RequireGreaterEqual(0, nameof(maxOccurrences)) ?? default;
        }

        /// <inheritdoc />
        public abstract IParseStateMachine ParseStateMachine { get; }
    }
}