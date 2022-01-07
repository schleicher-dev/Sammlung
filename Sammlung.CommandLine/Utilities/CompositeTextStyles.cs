using Sammlung.CommandLine.Terminal.Styles;
using Sammlung.CommandLine.Terminal.Styles.Neutral;

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
                .Indent(NumIndentSpaces * numLevels)
                .Build();
    }
}