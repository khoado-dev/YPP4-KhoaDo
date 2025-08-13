namespace UnitTestForTrello.Models.DTOs
{
    public class BoardWithWorkspaceDTO
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; }          
        public string BackgroundUrl { get; set; }       
        public string WorkspaceName { get; set; }
        public int WorkspaceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
