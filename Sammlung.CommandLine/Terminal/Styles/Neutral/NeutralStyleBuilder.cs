using Sammlung.CommandLine.Terminal.Styles.Common;

namespace Sammlung.CommandLine.Terminal.Styles.Neutral
{
    internal class NeutralStyleBuilder : StyleBuilderBase
    {
        public NeutralStyleBuilder(IStyleBuilderFactory factory, string text) : base(factory, text)
        {
        }

        public override IStyleBuilder Bold() => this;

        public override IStyleBuilder Italic() => this;

        public override IStyleBuilder Underlined() => this;

    }
}