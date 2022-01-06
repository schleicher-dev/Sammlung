namespace Sammlung.CommandLine.Terminal.Styles.AnsiCodes
{
    public class AnsiStyleBuilderFactory : IStyleBuilderFactory
    {
        public IStyleBuilder Create(string text) => new AnsiStyleBuilder(this, text);
    }
}