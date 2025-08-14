namespace UnitTestForTrello.Models.DTOs
{
    public class CardSelectableMemberDTO
    {
        public int UserId { get; set; }
        public string? UserPicture { get; set; } 
        public string? Username { get; set; }
        public string? OwnerTypeValue { get; set; } // CARD / BOARD / WORKSPACE
        public DateTime JoinedAt { get; set; }
    }

}
