namespace UnitTestForTrello.Models.DTOs
{
    public class WorkspaceDetailDTO
    {
        public int WorkspaceId { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public string WorkspaceName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string WorkspaceDescription { get; set; } = string.Empty;
    }

}
