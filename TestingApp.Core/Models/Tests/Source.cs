namespace TestingApp.Core.Models.Tests
{
    public class Source
    {
        public Guid ID { get; set; }
        public required Guid OwnerID { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
    }
}
