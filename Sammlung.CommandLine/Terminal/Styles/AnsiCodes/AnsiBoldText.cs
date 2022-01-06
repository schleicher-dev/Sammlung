namespace Sammlung.CommandLine.Terminal.Styles.AnsiCodes
{
    public class AnsiBoldText : AnsiStyledTextDecoratorBase
    {
        public AnsiBoldText(StyledTextBase decorated) : base(decorated)
        {
        }

        public override string GetText() => GetAnsiSequence(1) + base.GetText() + GetAnsiSequence(0);
    }
}