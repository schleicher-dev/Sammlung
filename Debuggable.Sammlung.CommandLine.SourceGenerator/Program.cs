using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Sammlung.CommandLine.Attributes;
using Sammlung.CommandLine.SourceGenerator.Execution;

namespace Debuggable.Sammlung.CommandLine.SourceGenerator
{
    public class Program
    {
        public static MetadataReference CreateMetadataReference(string assemblyString)
        {
            var assemblyName = new AssemblyName(assemblyString);
            var assembly = Assembly.Load(assemblyName);
            return MetadataReference.CreateFromFile(assembly.Location);
        }
        
        private static MetadataReference CreateMetadataReference<T>() => 
            MetadataReference.CreateFromFile(typeof(T).Assembly.Location);


        public static int Main(string[] args)
        {
            var source = File.ReadAllText("Parameters.cs");
            var trees = new[] { CSharpSyntaxTree.ParseText(source) };
            var references = new[]
            {
                CreateMetadataReference("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"),
                CreateMetadataReference("System.Runtime, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
                CreateMetadataReference<object>(),
                CreateMetadataReference<Attribute>(),
                CreateMetadataReference<CommandLineInterfaceAttribute>()
            };
            var compilation = CSharpCompilation.Create("ParametersTest", trees, references);

            var generatorExecutor = new GeneratorExecutor();
            var result = generatorExecutor.Execute(compilation);
            if (!result.ResultCode.DenotesSuccess) return result.ResultCode;

            using (var writer = File.CreateText(Path.Combine("..", "..", "..", "obj", "Debug", "Parameters.gen.cs")))
            {
                foreach (var sourceCodeFile in result.SourceCodeFiles)
                    writer.Write(sourceCodeFile.SourceText.ToString());
            }
            
            return result.ResultCode;
        }
    }
}