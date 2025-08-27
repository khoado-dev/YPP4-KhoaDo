using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class TemplateController
    {
        private readonly ITemplateService _templateService;
        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public IEnumerable<TemplateCategoryDTO>? GetAllCategories()
        {
            return _templateService.GetAllCategories();
        }

        public IEnumerable<TemplateByCategoryDTO>? GetTemplatesByCategory(int categoryId)
        {
            return _templateService.GetTemplatesByCategory(categoryId);
        }

        public TemplateDetailDTO? GetTemplateDetail(int templateId)
        {
            return _templateService.GetTemplateDetail(templateId);
        }
    }
}