using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sammlung.CommandLine.SourceGenerator
{
    public static class AttributeBuilder
    {
        private static object ConstructConstant(Type type, TypedConstant constant)
        {
            if (!type.IsArray) return constant.Value;

            var elementType = type.GetElementType() ?? throw new NullReferenceException("No element type");
            var array = Array.CreateInstance(elementType, constant.Values.Length);
            for (var i = 0; i < constant.Values.Length; ++i)
                array.SetValue(ConstructConstant(elementType, constant.Values[i]), i);
            return array;
        }
        
        public static T Build<T>(this AttributeData attributeData) where T : Attribute
        {
            if (attributeData == null) return null;
            
            var type = typeof(T);
            var ctorArgs = attributeData.ConstructorArguments.Select(c => c.Value);
            var instance = (T)Activator.CreateInstance(typeof(T), ctorArgs.ToArray());
            foreach (var argument in attributeData.NamedArguments)
            {
                var name = argument.Key;
                var constant = argument.Value;
                
                var propertyInfo = typeof(T).GetProperty(name) ??
                                   throw new NullReferenceException(
                                       $"Could not find property {name} on type {type.FullName}.");
                
                propertyInfo.SetValue(instance, ConstructConstant(propertyInfo.PropertyType, constant));
            }

            return instance;
        }
    }
}