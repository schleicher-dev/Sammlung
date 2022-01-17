using System;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Utilities
{
    public class BindableSetter<T1, T2> : IBindableTrait<T1>
    {
        private readonly Action<T1, T2> _setter;
        private T1 _data;

        public BindableSetter(Action<T1, T2> setter)
        {
            _setter = setter.RequireNotNull(nameof(setter));
        }

        private void RequireBinding()
        {
            if (_data == null)
                throw new GenericException(Lang.ObjectIsUnbound);
        }

        public void Bind(T1 data) => _data = data;

        public T2 Value
        {
            set
            {
                RequireBinding();
                _setter.Invoke(_data, value);
            }
        }
    }
}