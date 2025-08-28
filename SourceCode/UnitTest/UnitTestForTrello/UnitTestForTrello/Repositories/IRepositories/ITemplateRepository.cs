using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ITemplateRepository
    {
        IEnumerable<TemplateCategoryDTO>? GetTemplateCategories();
        TemplateDetailDTO? GetTemplateDetail(int templateId);
        IEnumerable<TemplateByCategoryDTO>? GetTemplatesByCategory(int categoryId);
    }
}
