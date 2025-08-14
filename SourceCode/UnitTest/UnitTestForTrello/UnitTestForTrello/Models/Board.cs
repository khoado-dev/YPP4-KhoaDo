namespace UnitTestForTrello.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string? BoardName { get; set; }
        public string? BoardDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public string? BackgroundUrl { get; set; }
        public int? WorkspaceId { get; set; }
        public string? BoardStatus { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public Workspace? Workspace { get; set; }
    }
}
