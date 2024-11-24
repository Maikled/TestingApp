using TestingApp.Core.Models.Tests;

namespace TestingApp.Areas.Tasking.Models
{
    public class TaskModel
    {
        public required TaskTesting Task { get; set; }
        public Source? Source { get; set; }
        public IEnumerable<TestExecuteHistory>? TestExecuteHistories { get; set; }
    }
}
