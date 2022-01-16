using System;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Models.Traits;
using Sammlung.CommandLine.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Reflection
{
    public class Property<T1, T2> : IBindableTrait<T1>
    {
        
        private readonly Func<T1, T2> _getter;
        private readonly Action<T1, T2> _setter;
        private T1 _data;

        public Property(Func<T1, T2> getter, Action<T1, T2> setter)
        {
            _getter = getter.RequireNotNull(nameof(getter));
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
            get
            {
                RequireBinding();
                return _getter.Invoke(_data);
            }
            set
            {
                RequireBinding();
                _setter.Invoke(_data, value);
            }
        }
    }
}