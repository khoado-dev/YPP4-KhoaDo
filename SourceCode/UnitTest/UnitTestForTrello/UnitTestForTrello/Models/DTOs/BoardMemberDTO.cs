namespace UnitTestForTrello.Models.DTOs
{
    public class BoardMemberDTO
    {
        public int UserId { get; set; }
        public string UserPicture { get; set; } = string.Empty;
        public string OwnerTypeValue { get; set; } = string.Empty;
        public int BoardId { get; set; }
    }
}
