namespace TestingApp.Core.Models.Tests
{
    public class TestData
    {
        public string SourceCode { get; }
        public string InputData { get; }
        public string OutputData { get; }

        public TestData(string sourceCode, string inputData, string outputData)
        {
            SourceCode = sourceCode;
            InputData = inputData;
            OutputData = outputData;
        }
    }
}