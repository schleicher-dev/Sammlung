using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Terminal.Styles.Common
{
    internal class RightPaddedText : StyledTextDecoratorBase
    {
        private readonly int _amount;

        public RightPaddedText(StyledTextBase decorated, int amount) : base(decorated)
        {
            _amount = amount.RequireGreaterEqual(0, nameof(amount));
        }

        public override string GetText() => base.GetText()?.PadRight(_amount);
    }
}