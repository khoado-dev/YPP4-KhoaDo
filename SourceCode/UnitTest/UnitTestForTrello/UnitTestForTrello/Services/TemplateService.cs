using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;

namespace UnitTestForTrello.Services
{
    public class TemplateService
    {
        private readonly TemplateRepository _templateRepository;

        public TemplateService(TemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public IEnumerable<TemplateCategoryDTO>? GetAllCategories()
        {
            return _templateRepository.GetAllCategories();
        }

        public IEnumerable<TemplateByCategoryDTO>? GetTemplatesByCategory(int categoryId)
        {
            return _templateRepository.GetTemplatesByCategory(categoryId);
        }

        public TemplateDetailDTO? GetTemplateDetail(int templateId)
        {
            return _templateRepository.GetTemplateDetail(templateId);
        }
    }
}