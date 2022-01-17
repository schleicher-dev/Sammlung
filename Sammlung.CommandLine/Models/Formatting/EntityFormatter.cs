using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Entities;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Formatting
{
    public class EntityFormatter : IEntityFormatter
    {
        private static string FormatKeywords(IEnumerable<string> keywords) => string.Join("|", keywords.ToArray());

        private IEnumerable<string> FormatArityVariables(IArityTrait arityTrait, string defaultPrefix)
        {
            using var metaNamesEnumerator = (arityTrait.MetaNames ?? Enumerable.Empty<string>()).GetEnumerator();
            for (var index = 0; index < arityTrait.Arity; ++index)
            {
                var name = metaNamesEnumerator.MoveNext() ? metaNamesEnumerator.Current : $"{defaultPrefix}{index}";
                yield return FormatVariableName(name);
            }
        }

        /// <inheritdoc />
        public string FormatVariableName(string variableName) => 
            $"<{variableName ?? "!!!NULL!!!"}>";

        /// <inheritdoc />
        public string FormatMultiplicity<T>(T entity) where T : IParserEntity, IMultiplicityTrait
        {
            var display = Format(entity);
            var minOccurs = entity.MinOccurrences;
            var maxOccurs = entity.MaxOccurrences;

            if (maxOccurs == 1)
                return minOccurs != 0 ? display : $"[{display}]";
            
            display = $"{display} [...]";
            return minOccurs != 0 ? display : $"[{display}]";
        }
        
        /// <inheritdoc />
        public string FormatArgument<TData>(Argument<TData> argument) => 
            string.Join(" ", FormatArityVariables(argument, "ARG").ToArray());
        
        /// <inheritdoc />
        public string FormatFlag<TData>(Flag<TData> flag) => FormatKeywords(flag.Keywords);

        /// <inheritdoc />
        public string FormatOption<TData>(Option<TData> option)
        {
            var keywords = FormatKeywords(option.Keywords);

            var optionArgs = string.Join(" ", FormatArityVariables(option, "ARG").ToArray());
            return string.Join(" ", new[] { keywords, optionArgs });
        }


        /// <inheritdoc />
        public string FormatCommand(CommandBase command) => FormatKeywords(command.Keywords);

        /// <inheritdoc />
        public string Format(IDisplayFormatTrait entity) => entity.Format(this);

    }
}