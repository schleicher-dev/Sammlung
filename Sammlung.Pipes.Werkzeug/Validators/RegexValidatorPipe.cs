using System.Text.RegularExpressions;
using Sammlung.Pipes.Werkzeug.Exceptions;
using Sammlung.Pipes.Werkzeug.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.Pipes.Werkzeug.Validators
{
    /// <summary>
    /// The <see cref="RegexValidatorPipe"/> validates if a particular value matches a regular expression.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class RegexValidatorPipe : ValidatorPipeBase<string>
    {
        private readonly Regex _regex;

        /// <summary>
        /// Creates a new <see cref="RegexValidatorPipe"/> using the regular expression.
        /// </summary>
        /// <param name="regex">the regular expression</param>
        public RegexValidatorPipe(Regex regex)
        {
            _regex = regex.RequireNotNull(nameof(regex));
        }
        
        /// <inheritdoc />
        protected override PipelineValidationException GetException(string value)
        {
            var message = string.Format(Lang.Validation_Interval_Exc_Reason, value, _regex);
            return new PipelineValidationException(message);
        }

        /// <inheritdoc />
        protected override bool TryValidate(string value) => value != null && _regex.IsMatch(value);
    }
}