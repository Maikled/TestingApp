namespace TestingApp.Core.Models.Tests
{
    public class Source
    {
        public Guid ID { get; set; }
        public required Guid OwnerTaskID { get; set; }
        public required Guid OwnerUserID { get; set; }
        public required string Code { get; set; }
    }
}
