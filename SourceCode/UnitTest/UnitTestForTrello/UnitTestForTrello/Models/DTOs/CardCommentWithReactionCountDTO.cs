namespace UnitTestForTrello.Models.DTOs
{
    public class CardCommentWithReactionCountDTO
    {
        public int UserId { get; set; }
        public string UserPicture { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CommentId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CardId { get; set; }
        public int ReactionId { get; set; }
        public string ReactionName { get; set; } = string.Empty;
        public int ReactionCount { get; set; }
    }

}
