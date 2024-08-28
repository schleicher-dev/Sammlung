using System;
using Sammlung.Werkzeug;

namespace Sammlung.Functional
{
    /// <summary>
    /// The <see cref="Maybe"/> class is a helper to create the <see cref="Maybe{T}"/> monad.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class Maybe
    {
        /// <summary>
        /// Creates a new <see cref="Maybe{T}"/> from the passed value.
        /// </summary>
        /// <param name="value">the value to create the monad from</param>
        /// <returns>the instance</returns>
        public static Maybe<T> From<T>(T value) => Maybe<T>.From(value);
    }
    
    /// <summary>
    /// The <see cref="Maybe{T}"/> type is a monad lend from the functional programming realm which avoids
    /// null values in their entirety.
    /// </summary>
    /// <typeparam name="T">the inner type of the monad</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public readonly struct Maybe<T>
    {
        /// <summary>
        /// Creates a new <see cref="Maybe{T}"/> from the passed value.
        /// </summary>
        /// <param name="value">the value to create the monad from</param>
        /// <returns>the instance</returns>
        public static Maybe<T> From(T value) => value is null ? new Maybe<T>() : new Maybe<T>(value);
        
        /// <summary>
        /// Implicitly converts the value to the <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="value">the value to create the monad from</param>
        /// <returns>the instance</returns>
        public static implicit operator Maybe<T>(T value) => From(value);
        
        /// <summary>
        /// The instance which represents nil.
        /// </summary>
        public static Maybe<T> Nil { get; }
        
        private readonly T _value;
        
        /// <summary>
        /// Indicates if the <see cref="Maybe{T}"/> has a value.
        /// </summary>
        public bool HasValue { get; }

        private Maybe(T value)
        {
            HasValue = true;
            _value = value;
        }

        /// <summary>
        /// Exposes the value if and only if the monad is not nil.
        /// </summary>
        /// <param name="value">the value if set</param>
        /// <returns>true if there is a value or false otherwise</returns>
        public bool TryGetValue(out T value) => Out.Result(HasValue, _value, out value);

        /// <summary>
        /// Returns the value encapsulated in the monad or the default value.
        /// </summary>
        /// <param name="defaultValue">the value if the encapsulated value is not set</param>
        /// <returns>either the encapsulated value or the default value</returns>
        public T GetOrDefault(T defaultValue = default) => GetOrDefault(x => x, defaultValue);
        
        /// <summary>
        /// Returns the value encapsulated in the monad or the default value.
        /// </summary>
        /// <param name="defaultFunction">the function which is called if the monad hasn't a value</param>
        /// <returns>either the encapsulated value or the default value</returns>
        public T GetOrDefault(Func<T> defaultFunction) => GetOrDefault(x => x, defaultFunction);

        /// <summary>
        /// Returns the value encapsulated in the monad or the default value.
        /// </summary>
        /// <param name="getFunction">the function which converts to the desired return type</param>
        /// <param name="defaultValue">the value if the encapsulated value is not set</param>
        /// <returns>either the encapsulated value or the default value</returns>
        public TReturn GetOrDefault<TReturn>(Func<T, TReturn> getFunction, TReturn defaultValue) =>
            HasValue ? getFunction.RequireNotNull(nameof(getFunction)).Invoke(_value) : defaultValue;
        
        /// <summary>
        /// Invokes the <see cref="valueFunction"/> if the value is set or otherwise the <see cref="defaultFunction"/>.
        /// </summary>
        /// <param name="valueFunction">the function which is called if the monad has a value</param>
        /// <param name="defaultFunction">the function which is called if the monad hasn't a value</param>
        /// <returns>the respective return value of each execution paths</returns>
        public TReturn GetOrDefault<TReturn>(Func<T, TReturn> valueFunction, Func<TReturn> defaultFunction) =>
            HasValue
                ? valueFunction.RequireNotNull(nameof(valueFunction)).Invoke(_value)
                : defaultFunction.RequireNotNull(nameof(defaultFunction)).Invoke();

        /// <summary>
        /// Maps the result of the <see cref="mapFunction"/> to a new <see cref="Maybe{TReturn}"/> monad.
        /// </summary>
        /// <param name="mapFunction">the function which transform <see cref="T"/> to <see cref="TReturn"/></param>
        /// <typeparam name="TReturn">the type argument of the returned monad</typeparam>
        /// <returns>the monad encapsulating the transformed value</returns>
        public Maybe<TReturn> Map<TReturn>(Func<T, TReturn> mapFunction) =>
            HasValue ? mapFunction.RequireNotNull(nameof(mapFunction)).Invoke(_value) : new Maybe<TReturn>();

        /// <summary>
        /// Binds the result of the <see cref="bindFunction"/> to a new <see cref="Maybe{TReturn}"/> monad.
        /// </summary>
        /// <param name="bindFunction">the function which transform <see cref="T"/> to <see cref="Maybe{TReturn}"/></param>
        /// <typeparam name="TReturn">the type argument of the returned monad</typeparam>
        /// <returns>the bound monad</returns>
        public Maybe<TReturn> Bind<TReturn>(Func<T, Maybe<TReturn>> bindFunction) =>
            HasValue ? bindFunction.Invoke(_value) : new Maybe<TReturn>();

        /// <summary>
        /// Invokes the <see cref="valueFunction"/> or does nothing.
        /// </summary>
        /// <param name="valueFunction">the function which is called if the monad has a value</param>
        public void InvokeOrNothing(Action<T> valueFunction) => InvokeOrDefault(valueFunction);

        /// <summary>
        /// Invokes the <see cref="valueFunction"/> if the value is set or otherwise the <see cref="defaultFunction"/>.
        /// </summary>
        /// <param name="valueFunction">the function which is called if the monad has a value</param>
        /// <param name="defaultFunction">the function which is called if the monad hasn't a value</param>
        public void InvokeOrDefault(Action<T> valueFunction, Action defaultFunction = default)
        {
            if (HasValue) valueFunction.RequireNotNull(nameof(valueFunction)).Invoke(_value);
            else defaultFunction?.Invoke();
        }
    }
}