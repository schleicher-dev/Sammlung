using System;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.SourceGenerator
{
    public sealed class ScopedAction : IDisposable
    {
        private readonly Action _exitScope;

        public ScopedAction(Action enterScope, Action exitScope)
        {
            enterScope = enterScope.RequireNotNull(nameof(enterScope));
            _exitScope = exitScope.RequireNotNull(nameof(exitScope));
            
            enterScope.Invoke();
        }

        public void Dispose()
        {
            _exitScope.Invoke();
        }
    }
}