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
            List<string> errors = new List<string>();

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
                        
                        var realOutputWriter = new StringWriter();

                        replacer.ReplaceIO(assembly, inputReader, realOutputWriter);

                        if (assembly.EntryPoint != null)
                        {
                            var parameters = assembly.EntryPoint.GetParameters().Length == 0 ? null : new object[] { };
                            assembly.EntryPoint.Invoke(null, parameters);
                        }

                        var realOutputData = realOutputWriter.ToString();
                        var expectedOutputData = _testData.OutputData;
                        
                        if (realOutputData != expectedOutputData)
                        {
                            throw new Exception($"Тест не пройден. Ожидаемый вывод: \"{expectedOutputData}\", реальный вывод: \"{realOutputData}\"");
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
