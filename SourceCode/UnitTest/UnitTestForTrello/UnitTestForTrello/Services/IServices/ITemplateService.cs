using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ITemplateService
    {
        IEnumerable<TemplateCategoryDTO>? GetAllCategories();
        TemplateDetailDTO? GetTemplateDetail(int templateId);
        IEnumerable<TemplateByCategoryDTO>? GetTemplatesByCategory(int categoryId);
    }
}