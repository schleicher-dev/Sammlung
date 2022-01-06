using System;
using System.Linq;

namespace Sammlung.CommandLine.Terminal.Styles.AnsiCodes
{
    public abstract class AnsiStyledTextDecoratorBase : StyledTextDecoratorBase
    {

        protected AnsiStyledTextDecoratorBase(StyledTextBase decorated) : base(decorated)
        {
        }

        protected string GetAnsiSequence(params byte[] values) => 
            $"\x1b[{string.Join(";", values.Select(v => Convert.ToString(v)))}m";
    }
}