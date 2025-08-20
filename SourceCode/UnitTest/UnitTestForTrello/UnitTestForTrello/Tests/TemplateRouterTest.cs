using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class TemplateRouterTest
    {
        private Router _router = null!;
        private const int categoryId = 1;  // matches seed: Business Templates
        private const int templateId = 1;  // matches seed: Project Plan

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }

        [TestMethod]
        public void GetTemplateCategories_ReturnsAll()
        {
            int expectedCount = 2; // from SeedTemplateCategories()

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = "/template-categories"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<TemplateCategoryDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsTrue(result.All(c => c.TemplateCategoryId > 0));
            Assert.IsTrue(result.All(c => !string.IsNullOrWhiteSpace(c.DisplayValue)));
        }

        [TestMethod]
        public void GetTemplatesByCategory_ReturnsOnlyThatCategory()
        {
            int expectedCount = 2; // from SeedTemplates(): 2 items in categoryId=1

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/templates/by-category?categoryId={categoryId}"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<TemplateByCategoryDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsTrue(result.All(t => t.TemplateCategoryId == categoryId));
            Assert.IsTrue(result.All(t => t.TemplateId > 0));
        }

        [TestMethod]
        public void GetTemplateDetail_ReturnsExpectedTemplate()
        {
            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/templates/detail?templateId={templateId}"
            };

            var res = _router.Handle(req);

            var dto = (TemplateDetailDTO)res.Data!;
            Assert.AreEqual(templateId, dto.TemplateId);
        }
    }
}
