namespace UnitTestForTrello.Models.DTOs
{
    public sealed class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string UserPicture { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ActivityDescription { get; set; } = string.Empty;
        public int IsRead { get; set; }
        public string OwnerTypeValue { get; set; } = string.Empty;
        public int OwnerId { get; set; }
    }

}
