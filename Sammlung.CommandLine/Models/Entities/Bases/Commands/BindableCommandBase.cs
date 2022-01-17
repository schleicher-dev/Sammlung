using System;
using System.Collections.Generic;
using System.Linq;
using Sammlung.CommandLine.Models.Entities.Bases.Arguments;
using Sammlung.CommandLine.Models.Entities.Bases.Options;
using Sammlung.CommandLine.Models.Parsing;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Terminal;
using Sammlung.CommandLine.Utilities;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Models.Entities.Bases.Commands
{
    public abstract class BindableCommandBase<TData> : CommandBase, IBindableTrait<TData>
    {
        public delegate TData ConstructorDelegate();
        
        private readonly ConstructorDelegate _dataConstructor;
        
        public TData Data { get; private set; }

        public override IEnumerable<CommandBase> Commands => QualifiedCommands;
        public override IEnumerable<OptionBase> Options => QualifiedOptions.OfType<OptionBase>();
        public override IEnumerable<ArgumentBase> Arguments => QualifiedArguments.OfType<ArgumentBase>();
        
        public List<CommandBase> QualifiedCommands { get; } = new List<CommandBase>();
        public List<BindableOptionBase<TData>> QualifiedOptions { get; } = new List<BindableOptionBase<TData>>();
        public List<Argument<TData>> QualifiedArguments { get; } = new List<Argument<TData>>();
        
        public BindableCommandBase(IEnumerable<string> keywords, ConstructorDelegate dataConstructor) : base(keywords)
        {
            _dataConstructor = dataConstructor.RequireNotNull(nameof(dataConstructor));
        }

        internal void PushCommand<TChildData>(Command<TData, TChildData> command)
        {
            command = command.RequireNotNull(nameof(command));
            if (command.Parent != null)
                throw new InvalidOperationException("Cannot add command which already has a parent.");
                    
            var knownKeywords = Commands.SelectMany(c => c.Keywords).Concat(Reservations.CommandHelpKeywords);
            RequireUniqueNames(knownKeywords, command.Keywords.ToList());
            
            QualifiedCommands.Add(command);
            command.Parent = this;
        }
        
        internal void PushArgument(Argument<TData> argument) => QualifiedArguments.Add(argument);

        internal void PushOption(BindableOptionBase<TData> bindableOption)
        {
            var knownKeywords = Options.SelectMany(o => o.Keywords).Concat(Reservations.OptionHelpKeywords);
            RequireUniqueNames(knownKeywords, bindableOption.Keywords.ToList());
            QualifiedOptions.Add(bindableOption);
        }

        public override TerminationInfo ShowHelp(HelpVisualizer helpVisualizer, Exception ex = null)
        {
            helpVisualizer = helpVisualizer.RequireNotNull(nameof(helpVisualizer));
            helpVisualizer.ShowException(ex);
            helpVisualizer.ShowHelp(this);
            return TerminationInfo.DisplayingHelp;
        }


        public override TerminationInfo Parse(IEnumerable<string> args, ITerminal terminal = null)
        {
            Bind(Data = _dataConstructor.Invoke());

            var parser = new CommandParser<TData>(this, Data, QualifiedArguments, QualifiedOptions, QualifiedCommands);
            var result = parser.Parse(args, terminal ?? new DefaultTerminal());
            Data = parser.Data;
            
            return result;
        }

        public void Bind(TData data)
        {
            foreach (var option in QualifiedOptions)
                option.Bind(data);
            foreach (var argument in QualifiedArguments)
                argument.Bind(data);
            foreach (var bindableTrait in QualifiedCommands.OfType<IBindableTrait<TData>>())
                bindableTrait.Bind(data);
        }
    }
}