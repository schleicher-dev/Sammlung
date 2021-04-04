namespace Sammlung.Utilities.Math
{
    /// <summary>
    /// The <see cref="IOperationProvider{T}"/> is an attempt to generalize some arithmetic operations from the
    /// underlying type. Such that these operations can be done using arbitrary types.
    /// </summary>
    internal interface IOperationProvider<T> where T : struct
    {
        /// <summary>
        /// The number of bits of the given type.
        /// </summary>
        public short NumBits { get; }

        /// <summary>
        /// Adds two values.
        /// </summary>
        /// <param name="lhs">the left hand side</param>
        /// <param name="rhs">the right hand side</param>
        /// <returns>the result</returns>
        public T Add(T lhs, long rhs);
        
        /// <summary>
        /// Subtracts two values.
        /// </summary>
        /// <param name="lhs">the left hand side</param>
        /// <param name="rhs">the right hand side</param>
        /// <returns>the result</returns>
        public T Subtract(T lhs, long rhs);
        
        /// <summary>
        /// Shifts the value by the given places to the right.
        /// </summary>
        /// <param name="lhs">the left hand side</param>
        /// <param name="rhs">the right hand side</param>
        /// <returns>the result</returns>
        public T ShiftRight(T lhs, long rhs);
        
        /// <summary>
        /// Does an or operation on the given values.
        /// </summary>
        /// <param name="lhs">the left hand side</param>
        /// <param name="rhs">the right hand side</param>
        /// <returns>the result</returns>
        public T Or(T lhs, T rhs);
    }
}