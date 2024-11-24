namespace TestingApp.Areas.Tasking.Models
{
    public class TestExecuteHistory
    {
        public Guid ID { get; set; }
        public Guid TestID { get; set; }
        public Guid OwnerUserID { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
