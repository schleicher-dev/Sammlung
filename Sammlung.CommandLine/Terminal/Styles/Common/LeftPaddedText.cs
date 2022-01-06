using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Terminal.Styles.Common
{
    internal class LeftPaddedText : StyledTextDecoratorBase
    {
        private readonly int _amount;

        public LeftPaddedText(StyledTextBase decorated, int amount) : base(decorated)
        {
            _amount = amount.RequireGreaterEqual(0, nameof(amount));
        }

        public override string GetText() => base.GetText()?.PadLeft(_amount);
    }
}