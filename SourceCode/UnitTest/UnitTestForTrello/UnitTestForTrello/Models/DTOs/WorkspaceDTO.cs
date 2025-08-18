namespace UnitTestForTrello.Models.DTOs
{
    public class WorkspaceDTO
    {
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public string LogoUrl { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
