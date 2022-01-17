using System;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, Inherited = false)]
    public class DescriptionAttribute : Attribute
    {
        public string Key { get; }
        public string DefaultDescription { get; }


        public DescriptionAttribute(string key, string defaultDescription)
        {
            Key = key.RequireNotNullOrEmpty(nameof(key));
            DefaultDescription = defaultDescription.RequireNotNullOrEmpty(nameof(defaultDescription));
        }
    }
}