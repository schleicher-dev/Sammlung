using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Terminal.Styles.Common
{
    internal class IndentedText : StyledTextDecoratorBase
    {
        private readonly int _amount;

        public IndentedText(StyledTextBase decorated, int amount) : base(decorated)
        {
            _amount = amount.RequireGreaterEqual(0, nameof(amount));
        }

        public override string GetText() => $"{new string(' ', _amount)}{base.GetText()}";

    }
}