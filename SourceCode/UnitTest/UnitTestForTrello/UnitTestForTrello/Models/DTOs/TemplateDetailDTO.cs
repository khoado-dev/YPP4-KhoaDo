namespace UnitTestForTrello.Models.DTOs
{
    public class TemplateDetailDTO
    {
        public int TemplateId { get; set; }
        public string UserPicture { get; set; } = string.Empty;
        public string TemplateDescription { get; set; } = string.Empty;
        public string TemplateTitle { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int CopiedNumber { get; set; }
        public int ViewedNumber { get; set; }
        public int BoardId { get; set; }
    }

}
