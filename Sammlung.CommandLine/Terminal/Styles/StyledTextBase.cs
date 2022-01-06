namespace Sammlung.CommandLine.Terminal.Styles
{
    public abstract class StyledTextBase
    {
        public static implicit operator string(StyledTextBase text) => text.GetText();
        
        public abstract string GetText();
    }
}