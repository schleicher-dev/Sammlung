namespace Sammlung.CommandLine.Terminal.Styles
{
    public interface IStyleBuilderFactory
    {
        IStyleBuilder Create(string text);
    }
}