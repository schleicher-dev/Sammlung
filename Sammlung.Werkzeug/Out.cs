namespace Sammlung.Werkzeug;

/// <summary>
/// The <see cref="Out"/> class is a helper class to make functions with out-parameters one-liners.
/// </summary>
[JetBrains.Annotations.PublicAPI]
public static class Out
{
    /// <summary>
    /// Creates a positive result and assigns the value to the outValue.
    /// </summary>
    /// <param name="value">the value to assign to the outValue</param>
    /// <param name="outValue"/>
    /// <typeparam name="T">the type of the outValue</typeparam>
    /// <returns>true in every case</returns>
    public static bool True<T>(T value, out T outValue) => Result(true, value, out outValue);

    /// <summary>
    /// Creates a positive result if and only if the value is not null and assigns the value to the outValue.
    /// </summary>
    /// <param name="value">the value to assign to the outValue</param>
    /// <param name="outValue"/>
    /// <typeparam name="T">the type of the outValue</typeparam>
    /// <returns>true in every case</returns>
    public static bool TrueIfNotNull<T>(T value, out T outValue) where T : class =>
        Result(value is not null, value, out outValue);

    /// <summary>
    /// Creates a negative result and assigns the value to the outValue.
    /// </summary>
    /// <param name="outValue"/>
    /// <typeparam name="T">the type of the outValue</typeparam>
    /// <returns>false in every case</returns>
    public static bool False<T>(out T outValue) => Result(false, default, out outValue);

    /// <summary>
    /// Creates a result with the respective return value and assigns the passed value to the outValue.
    /// </summary>
    /// <param name="returnValue">the returned value</param>
    /// <param name="value">the value to assign to the outValue</param>
    /// <param name="outValue"/>
    /// <typeparam name="T">the type of the outValue</typeparam>
    /// <typeparam name="TReturn">the type of the returnValue</typeparam>
    /// <returns>the return value</returns>
    public static TReturn Result<T, TReturn>(TReturn returnValue, T value, out T outValue)
    {
        outValue = value;
        return returnValue;
    }
}