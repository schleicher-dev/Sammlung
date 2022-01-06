using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Terminal.Styles.Common
{
    public abstract class StyleBuilderBase : IStyleBuilder
    {
        public IStyleBuilderFactory Factory { get; }
        protected StyledTextBase CurrentStyle { get; set; }

        protected StyleBuilderBase(IStyleBuilderFactory factory, string text)
        {
            Factory = factory.RequireNotNull(nameof(factory));
            CurrentStyle = new StyledText(text);
        }

        public abstract IStyleBuilder Bold();
        public abstract IStyleBuilder Italic();
        public abstract IStyleBuilder Underlined();
        public virtual IStyleBuilder Indent(int amount)
        {
            CurrentStyle = new IndentedText(CurrentStyle, amount);
            return this;
        }

        public virtual IStyleBuilder PadLeft(int amount)
        {
            CurrentStyle = new LeftPaddedText(CurrentStyle, amount);
            return this;
        }
        
        public virtual IStyleBuilder PadRight(int amount)
        {
            CurrentStyle = new RightPaddedText(CurrentStyle, amount);
            return this;
        }

        public virtual IStyleBuilder Append(StyledTextBase text)
        {
            CurrentStyle = new CombinedText(CurrentStyle, text);
            return this;
        }
        
        public virtual StyledTextBase Build() => CurrentStyle;
    }
}