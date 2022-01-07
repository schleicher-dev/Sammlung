using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<BindableOptionBase<TData>> Options { get; } = new List<BindableOptionBase<TData>>();
        public List<Argument<TData>> Arguments { get; } = new List<Argument<TData>>();
        
        public BindableCommandBase(IEnumerable<string> keywords, ConstructorDelegate dataConstructor) : base(keywords)
        {
            _dataConstructor = dataConstructor.RequireNotNull(nameof(dataConstructor));
        }

        internal void PushArgument(Argument<TData> argument) => Arguments.Add(argument);

        internal void PushOption(BindableOptionBase<TData> bindableOption)
        {
            var knownKeywords = Options.SelectMany(o => o.Keywords).Concat(Reservations.OptionKeywords);
            RequireUniqueNames(knownKeywords, bindableOption.Keywords.ToList());
            Options.Add(bindableOption);
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

            var parser = new CommandParser<TData>(Data, Arguments, Options, Commands);
            var result = parser.Parse(args, terminal ?? new DefaultTerminal());
            Data = parser.Data;
            
            return result;
        }

        public void Bind(TData data)
        {
            foreach (var option in Options)
                option.Bind(data);
            foreach (var argument in Arguments)
                argument.Bind(data);
        }
    }
}