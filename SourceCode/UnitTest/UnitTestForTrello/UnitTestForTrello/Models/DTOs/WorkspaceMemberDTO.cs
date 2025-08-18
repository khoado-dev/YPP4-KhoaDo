namespace UnitTestForTrello.Models.DTOs
{
    public class WorkspaceMemberDTO
    {
        // User info
        public string UserPicture { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime? UserLastActive { get; set; }

        // Permission info
        public string PermissionName { get; set; } = string.Empty;

        // Aggregate info
        public int BoardCount { get; set; }
    }
}
