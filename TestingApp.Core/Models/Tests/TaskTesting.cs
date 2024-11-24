using TestingApp.Core.Models.Identity;

namespace TestingApp.Core.Models.Tests
{
    public class TaskTesting
    {
        public Guid ID { get; set; }
        public required User OwnerUser { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IList<Test> Tests { get; set; } = new List<Test>();
    }
}