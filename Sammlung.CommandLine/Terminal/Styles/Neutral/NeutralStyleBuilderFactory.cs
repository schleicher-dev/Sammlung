namespace Sammlung.CommandLine.Terminal.Styles.Neutral
{
    public class NeutralStyleBuilderFactory : IStyleBuilderFactory
    {
        public IStyleBuilder Create(string text) => new NeutralStyleBuilder(this, text);
    }
}