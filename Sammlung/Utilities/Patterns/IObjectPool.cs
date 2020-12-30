namespace Sammlung.Utilities.Patterns
{
    internal interface IObjectPool<T> where T : class
    {
        T Get();
        void Return(T instance);
        void Reset(T instance);
    }
}