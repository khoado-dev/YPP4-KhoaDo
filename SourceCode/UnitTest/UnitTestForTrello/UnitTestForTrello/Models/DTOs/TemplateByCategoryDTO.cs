namespace UnitTestForTrello.Models.DTOs
{
    public class TemplateByCategoryDTO
    {
        public int TemplateId { get; set; }
        public string TemplateTitle { get; set; } = string.Empty;
        public string TemplateDescription { get; set; } = string.Empty;
        public int Viewed { get; set; }
        public int Copied { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int TemplateCategoryId { get; set; }
        public string TemplateCategory { get; set; } = string.Empty;
    }

}
