using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Entities.Bases.Commands;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Resources;
using Sammlung.CommandLine.Terminal;
using Sammlung.CommandLine.Terminal.Styles;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Utilities
{
    public class HelpVisualizer
    {
        private readonly IOutputWriter _writer;
        private readonly IEntityFormatter _formatter;
        private readonly CompositeTextStyles _textStyles;

        private static string GetApplicationName(CommandBase command) =>
            (command as IApplicationNameTrait)?.ApplicationName ??
            Assembly.GetEntryAssembly()?.GetName().Name ??
            throw new InvalidOperationException(Lang.ApplicationNameNotFound);

        public HelpVisualizer(IOutputWriter writer, IEntityFormatter formatter = null,
            IStyleBuilderFactory styleBuilderFactory = null)
        {
            _writer = writer.RequireNotNull(nameof(writer));
            _formatter = formatter ?? new EntityFormatter();
            _textStyles = new CompositeTextStyles(styleBuilderFactory);
        }

        private IEnumerable<string> GetDescriptionSection<T>(T command) where T : IDescriptionTrait
        {
            yield return command.Description ?? "No description";
        }

        private string GetUsageHint<TData>(BindableCommandBase<TData> command)
        {
            IEnumerable<string> Lines()
            {
                yield return "$";
                yield return GetApplicationName(command);
                foreach (var argument in command.Arguments)
                    yield return _formatter.FormatMultiplicity(argument);
                foreach (var option in command.Options)
                    yield return _formatter.FormatMultiplicity(option);
                if (!command.Commands.Any()) yield break;
                yield return _formatter.FormatVariableName("Command");
            }

            return string.Join(" ", Lines());
        }

        private IEnumerable<string> GetUsageSection<TData>(BindableCommandBase<TData> command)
        {
            yield return _textStyles.SectionHeader("USAGE");
            yield return _textStyles.IndentedText(GetUsageHint(command));
        }
        
        private IEnumerable<string> GetSection<T>(string header, List<T> entities) where T : IDisplayFormatTrait, IDescriptionTrait
        {
            if (!entities.Any()) yield break;
            
            yield return _textStyles.SectionHeader(header);

            var padding = entities.Select(e => _formatter.Format(e)).Aggregate(0, (v, s) => Math.Max(v, s.Length));
            foreach (var entity in entities)
            {
                var entityFormat = _formatter.Format(entity);
                var description = entity.Description ?? "No description";
                yield return _textStyles.TwoColumnsText(entityFormat, description, padding, 1);
            }
        }

        private IEnumerable<IEnumerable<string>> GetAllSections<TData>(BindableCommandBase<TData> command)
        {
            yield return GetDescriptionSection(command);
            yield return new[] { string.Empty };
            yield return GetUsageSection(command);
            yield return new[] { string.Empty };
            yield return GetSection("ARGUMENTS", command.Arguments);
            yield return new[] { string.Empty };
            yield return GetSection("OPTIONS", command.Options);
            yield return new[] { string.Empty };
            yield return GetSection("COMMANDS", command.Commands);
        }

        public void ShowHelp<TData>(BindableCommandBase<TData> command)
        {
            foreach (var line in GetAllSections(command).SelectMany(e => e))
                _writer.WriteLine(line);
        }

        public void ShowException(Exception exception)
        {
            if (exception == null) return;

            foreach (var line in GetExceptionSection(exception.ToString()))
                _writer.WriteLine(line);
        }

        private IEnumerable<string> GetExceptionSection(string exString)
        {
            yield return _textStyles.SectionHeader("EXCEPTION");
            yield return exString;
            yield return string.Empty;
        }
    }
}