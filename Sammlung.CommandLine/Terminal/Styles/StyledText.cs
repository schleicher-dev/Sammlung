namespace Sammlung.CommandLine.Terminal.Styles
{
    public class StyledText : StyledTextBase
    {
        private readonly string _text;

        public StyledText(string text)
        {
            _text = text;
        }
        
        public override string GetText() => _text;
    }
}