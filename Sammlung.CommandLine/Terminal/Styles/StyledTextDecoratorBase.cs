using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Terminal.Styles
{
    public class StyledTextDecoratorBase : StyledTextBase
    {
        private readonly StyledTextBase _decorated;

        protected StyledTextDecoratorBase(StyledTextBase decorated)
        {
            _decorated = decorated.RequireNotNull(nameof(decorated));
        }

        public override string GetText() => _decorated.GetText();
    }
}