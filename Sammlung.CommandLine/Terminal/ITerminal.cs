namespace Sammlung.CommandLine.Terminal
{
    public interface ITerminal
    {
        IOutputWriter Pipe { get; }
        IOutputWriter Message { get; }
    }
}