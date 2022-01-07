using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Formatting
{
    public interface IEntityFormatter
    {
        /// <summary>
        /// The <see cref="Format(IDisplayFormatTrait)"/> method is the entry point for the visitor pattern.
        /// </summary>
        /// <param name="entity">the entity with the trait</param>
        /// <returns>the representation of the entity</returns>
        string Format(IDisplayFormatTrait entity);
        
        /// <summary>
        /// Wraps the variable name in a uniform layout.
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>the resulting representation</returns>
        string FormatVariableName(string name);
        
        /// <summary>
        /// Formats an entity which has the <see cref="IMultiplicityTrait"/>.
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <typeparam name="T">the type of the entity</typeparam>
        /// <returns>the representation of the entity</returns>
        string FormatMultiplicity<T>(T entity) where T : IParserEntity, IMultiplicityTrait;
        
        /// <summary>
        /// Formats an <see cref="Argument{TData}"/>.
        /// </summary>
        /// <param name="argument">the argument</param>
        /// <returns>the representation of the entity</returns>
        string FormatArgument<TData>(Argument<TData> argument);
        
        /// <summary>
        /// Formats a <see cref="Flag{TData}"/>.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <typeparam name="TData">the data type</typeparam>
        /// <returns>the representation of the entity</returns>
        string FormatFlag<TData>(Flag<TData> flag);

        /// <summary>
        /// Formats a <see cref="Option{TData}"/>.
        /// </summary>
        /// <param name="option">the option</param>
        /// <typeparam name="TData">the data type</typeparam>
        /// <returns>the representation of the entity</returns>
        string FormatOption<TData>(Option<TData> option);
        
        /// <summary>
        /// Formats a <see cref="CommandBase"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>the representation of the entity</returns>
        string FormatCommand(CommandBase command);
    }
}