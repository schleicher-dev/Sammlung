namespace Sammlung.CommandLine
{
    /// <summary>
    /// <see cref="TerminationInfo"/> holds the result information for the termination of the application.
    /// </summary>
    public class TerminationInfo
    {
        /// <summary>
        /// Implicitly converts the <see cref="TerminationInfo"/> to an integer.
        /// </summary>
        /// <param name="info">the information</param>
        /// <returns>the exit code</returns>
        public static implicit operator int(TerminationInfo info) => info.ExitCode;
        
        /// <summary>
        /// Implicitly converts an integer to an <see cref="TerminationInfo"/>.
        /// </summary>
        /// <param name="exitCode">the exit code</param>
        /// <returns>the information value</returns>
        public static implicit operator TerminationInfo(int exitCode) =>
            exitCode switch
            {
                0 => RegularTermination,
                _ => new TerminationInfo(exitCode, null)
            };

        public static readonly TerminationInfo RegularTermination =
            new TerminationInfo(0, "Terminated regularly");
        
        public static readonly TerminationInfo DisplayingHelp =
            new TerminationInfo(1, "Showing help");

        /// <summary>
        /// The exit code.
        /// </summary>
        public int ExitCode { get; }
        
        /// <summary>
        /// The description of the error.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates a new <see cref="TerminationInfo"/> using the exit code and the description.
        /// </summary>
        /// <param name="exitCode">the exit code</param>
        /// <param name="description">the description</param>
        public TerminationInfo(int exitCode, string description)
        {
            ExitCode = exitCode;
            Description = !string.IsNullOrEmpty(description) ? description : "No description";
        }

        public override string ToString()
        {
            return $"[{nameof(TerminationInfo)} | ExitCode={ExitCode}, Description={Description}]";
        }
    }
}