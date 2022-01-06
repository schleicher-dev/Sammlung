namespace Sammlung.CommandLine.Pipes
{
    public delegate void PushValueDelegate<in TData, in T>(TData data, T value);
}