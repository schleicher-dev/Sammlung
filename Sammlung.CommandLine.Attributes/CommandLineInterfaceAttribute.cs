using System;

namespace Sammlung.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandLineInterfaceAttribute : Attribute
    {
        public string ApplicationName { get; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }

        public CommandLineInterfaceAttribute(string applicationName)
        {
            ApplicationName = applicationName;
        }
    }
}