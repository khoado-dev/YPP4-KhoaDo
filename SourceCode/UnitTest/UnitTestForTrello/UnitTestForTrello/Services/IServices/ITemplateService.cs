using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ITemplateService
    {
        IEnumerable<TemplateCategoryDTO>? GetTemplateCategories();
        TemplateDetailDTO? GetTemplateDetail(int templateId);
        IEnumerable<TemplateByCategoryDTO>? GetTemplatesByCategory(int categoryId);
    }
}