namespace Sammlung.CommandLine.Terminal
{
    public interface IOutputWriter
    {
        string NewLine { get; }
        void Write(string content);
    }
}