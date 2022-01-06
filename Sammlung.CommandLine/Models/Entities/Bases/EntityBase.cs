using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Entities.Bases
{
    public abstract class EntityBase : IEntity
    {
        string IDescriptionTrait.Description { get; set; }
        
        public abstract string Format(IEntityFormatter formatter);
    }
}