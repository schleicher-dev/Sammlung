namespace Sammlung.CommandLine.Terminal.Styles.Common
{
    internal class CombinedText : StyledTextBase
    {
        private readonly StyledTextBase _lhsText;
        private readonly StyledTextBase _rhsText;

        public CombinedText(StyledTextBase lhsText, StyledTextBase rhsText)
        {
            _lhsText = lhsText;
            _rhsText = rhsText;
        }

        public override string GetText() => $"{_lhsText.GetText()}{_rhsText.GetText()}";

    }
}