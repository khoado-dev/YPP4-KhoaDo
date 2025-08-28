using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateService(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public IEnumerable<TemplateCategoryDTO>? GetTemplateCategories()
        {
            return _templateRepository.GetTemplateCategories();
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