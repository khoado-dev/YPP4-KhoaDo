namespace UnitTestForTrello.Models.DTOs
{
    public class RecentBoardDTO
    {
        public int BoardId { get; set; }
        public string? BoardName { get; set; }
        public string? BackgroundUrl { get; set; }
        public DateTime? AccessedAt { get; set; }
        public string? BoardStatus { get; set; }
    }
}
