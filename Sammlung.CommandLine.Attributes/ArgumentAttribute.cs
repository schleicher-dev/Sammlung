using System;
using System.Collections.Generic;

namespace Sammlung.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ArgumentAttribute : Attribute
    {
        public int Position { get; set; } = 0;
        public uint Arity { get; set; } = 1;
        public string[] MetaNames { get; set; }
        public Type ConversionPipe { get; set; }
    }
}