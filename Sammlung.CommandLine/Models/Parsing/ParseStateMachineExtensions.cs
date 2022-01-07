namespace Sammlung.CommandLine.Models.Parsing
{
    public static class ParseStateMachineExtensions
    {
        public static bool HasState(this IParseStateMachine stateMachine, ParseState state) =>
            stateMachine?.CurrentState == state;
    }
}