namespace UnitTestForTrello.Models.DTOs
{
    public class WorkspaceDTO
    {
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
