namespace UnitTestForTrello.Models.DTOs
{
    public class CardActivityDTO
    {
        public int UserId { get; set; }
        public string UserPicture { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int ActivityId { get; set; }
        public string ActivityDescription { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string Category { get; set; } = string.Empty;
        public int CardId { get; set; }
    }

}
