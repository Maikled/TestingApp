namespace TestingApp.Core.Models.Tests
{
    public class TestResult
    {
        public bool IsSuccess { get; }
        public List<string> Errors { get; }

        public TestResult(bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }
}
