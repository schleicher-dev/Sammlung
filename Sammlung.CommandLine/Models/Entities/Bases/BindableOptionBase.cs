using System.Collections.Generic;
using Sammlung.CommandLine.Models.Formatting;
using Sammlung.CommandLine.Models.Traits;

namespace Sammlung.CommandLine.Models.Entities.Bases
{
    public abstract class BindableOptionBase<TData> : OptionBase, IBindableTrait<TData>
    {
        protected BindableOptionBase(IEnumerable<string> keywords) : base(keywords) { }

        public abstract void Bind(TData data);
        
    }
}