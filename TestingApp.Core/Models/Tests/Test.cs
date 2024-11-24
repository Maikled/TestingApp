namespace TestingApp.Core.Models.Tests
{
    public class Test
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public required string InputData { get; set; }
        public required string OutputData { get; set; }
    }
}
