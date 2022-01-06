namespace Sammlung.CommandLine.Terminal.Styles
{
    public interface IStyleBuilder
    {
        public IStyleBuilderFactory Factory { get; }
        IStyleBuilder Bold();
        IStyleBuilder Italic();
        IStyleBuilder Underlined();
        StyledTextBase Build();
        IStyleBuilder Indent(int amount);
        IStyleBuilder PadLeft(int amount);
        IStyleBuilder PadRight(int amount);
        IStyleBuilder Append(StyledTextBase text);
    }
}