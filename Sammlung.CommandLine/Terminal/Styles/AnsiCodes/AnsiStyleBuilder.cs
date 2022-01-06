using Sammlung.CommandLine.Terminal.Styles.Common;

namespace Sammlung.CommandLine.Terminal.Styles.AnsiCodes
{
    public class AnsiStyleBuilder : StyleBuilderBase
    {
        public AnsiStyleBuilder(IStyleBuilderFactory factory, string text) : base(factory, text)
        {
        }
        public override IStyleBuilder Bold()
        {
            CurrentStyle = new AnsiBoldText(CurrentStyle);
            return this;
        }

        public override IStyleBuilder Italic() => this;

        public override IStyleBuilder Underlined() => this;

    }
}