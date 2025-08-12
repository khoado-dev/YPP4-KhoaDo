using UnitTestForTrello.Models;

namespace UnitTestForTrello.Models.DTOs
{
    public class BoardDTO
    {
        public int BoardId { get; set; }
        public string? BoardName { get; set; }
        public string? BoardDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? BackgroundUrl { get; set; }
        public string? BoardStatus { get; set; }
        public int? WorkspaceId { get; set; }
    }

}
