namespace Sammlung.CommandLine.Models.Parsing
{
    /// <summary>
    /// The parse state.
    /// </summary>
    public enum ParseState
    {
        RequiresNextOccurrence,
        ExpectNextOccurrence,
        ExpectNextToken,
        Finalized
    }
}