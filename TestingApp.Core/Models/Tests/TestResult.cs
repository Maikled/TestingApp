namespace TestingApp.Core.Models.Tests
{
    public class TestResult
    {
        public bool IsSuccess { get; }
        public IEnumerable<string> Errors { get; }

        public TestResult(bool isSuccess, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }
}
