using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sammlung.CommandLine.SourceGenerator
{
    public static class AttributeDataExtensions
    {
        public static T CreateAttributeOrDefault<T>(this IEnumerable<AttributeData> attributes) where T : Attribute
        {
            var attrName = typeof(T).FullName ?? throw new NullReferenceException($"Unknown class {typeof(T)}");
            var attrData = attributes.FirstOrDefault(a => attrName.Equals(a.AttributeClass?.ToDisplayString()));
            return attrData.Build<T>();
        }

    }
}