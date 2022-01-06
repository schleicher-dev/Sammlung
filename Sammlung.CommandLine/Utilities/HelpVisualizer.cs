using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sammlung.CommandLine.Models.Entities.Bases;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Resources;
using Sammlung.CommandLine.Terminal;
using Sammlung.CommandLine.Terminal.Styles;
using Sammlung.CommandLine.Terminal.Styles.Neutral;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Utilities
{
    internal class CompositeTextStyles
    {
        private const int NumIndentSpaces = 4;
        
        private readonly IStyleBuilderFactory _styleBuilderFactory;

        public CompositeTextStyles(IStyleBuilderFactory styleBuilderFactory)
        {
            _styleBuilderFactory = styleBuilderFactory ?? new NeutralStyleBuilderFactory();
        }
        
        public string SectionHeader(string text) => 
            _styleBuilderFactory.Create(text).Bold().Build();

        public string IndentedText(string text, int numLevels = 1) => 
            _styleBuilderFactory.Create(text).Indent(NumIndentSpaces * numLevels).Build();

        public string TwoColumnsText(string lhs, string rhs, int lhsPadding, int numLevels) =>
            _styleBuilderFactory
                .Create(lhs)
                .PadRight((ushort)(lhsPadding + NumIndentSpaces))
                .Append(rhs)
                .Indent(NumIndentSpaces + numLevels)
                .Build();
    }
    
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
        }
    }
}