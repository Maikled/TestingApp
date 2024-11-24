using TestingApp.Core.Models.Tests;

namespace TestingApp.Areas.Tasking.Models
{
    public class TestModel
    {
        public Guid TestID { get; set; }
        public required Source Source { get; set; }
    }
}