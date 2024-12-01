using TestingApp.Core.Models.Tests;
using TestingApp.Core.Processing.Compillers.CSharp;
using TestingApp.Core.Processing.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace TestingApp.Core.Processing.CSharp
{
    public class CSharpCompiller : ICompiller
    {
        private TestData _testData;

        public CSharpCompiller(TestData testData)
        {
            _testData = testData;
        }

        public TestResult Run()
        {
            var errors = new List<string>();

            try
            {
                var replacer = new CSharpReplacer();
                var sourceCode = replacer.ReplaceSourceCode(_testData.SourceCode);

                var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

                var references = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                    .Select(a => MetadataReference.CreateFromFile(a.Location))
                    .ToList();

                var compilation = CSharpCompilation.Create($"DynamicAssembly_{Guid.NewGuid()}")
                    .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
                    .AddReferences(references)
                    .AddSyntaxTrees(syntaxTree);

                using(var memoryStream = new MemoryStream())
                {
                    var result = compilation.Emit(memoryStream);
                    if(!result.Success)
                    {
                        errors.AddRange(result.Diagnostics.Select(p => p.ToString()));
                    }
                    else
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var assembly = Assembly.Load(memoryStream.ToArray());

                        var inputReader = new StringReader(_testData.InputData);               
                        var outputWriter = new StringWriter();

                        replacer.ReplaceIO(assembly, inputReader, outputWriter);

                        if (assembly.EntryPoint != null)
                        {
                            var args = new List<object>();
                            foreach(var parameter in assembly.EntryPoint.GetParameters())
                            {
                                if (parameter.ParameterType.IsArray)
                                {
                                    var elementType = parameter.ParameterType.GetElementType();
                                    if(elementType != null)
                                    {
                                        args.Add(Array.CreateInstance(elementType, 0));
                                    }
                                }
                                else
                                {
                                    args.Add(new object());
                                }
                            }

                            var finalArgs = args.Count == 0 ? null : args.ToArray();
                            assembly.EntryPoint.Invoke(null, finalArgs);

                            var realOutputData = outputWriter.ToString().Replace("\r\n", "\n").Trim('\n', '\r');
                            var expectedOutputData = _testData.OutputData.Replace("\r\n", "\n").Trim('\n', '\r');

                            if (realOutputData != expectedOutputData)
                            {
                                throw new Exception($"Тест не пройден. Ожидаемый вывод: \"{expectedOutputData}\", реальный вывод: \"{realOutputData}\"");
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException("Отсутствует доступная точка входа в программе");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            return new TestResult(errors.Count == 0, errors);
        }
    }
}