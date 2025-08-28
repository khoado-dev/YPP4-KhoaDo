using Moq;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories; // ITemplateRepository
using UnitTestForTrello.Services;                   // TemplateService

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class TemplateServiceTest
    {
        private const int CategoryIdBusiness = 1; // Business Templates
        private const int CategoryIdEducation = 2; // Education Templates
        private const int NotFoundCategory = 999;

        private const int TemplateIdProjectPlan = 1; // "Project Plan"
        private const int NotFoundTemplate = 999;

        [TestMethod]
        public void GetAllCategories_ShouldReturnTwoCategories_WhenSeeded()
        {
            // Arrange
            var repo = new Mock<ITemplateRepository>(MockBehavior.Strict);

            var categories = new List<TemplateCategoryDTO>
            {
                new() { TemplateCategoryId = 1, DisplayValue = "Business Templates",  IconUrl = "https://example.com/icons/business.png" },
                new() { TemplateCategoryId = 2, DisplayValue = "Education Templates", IconUrl = "https://example.com/icons/education.png" }
            };

            repo.Setup(r => r.GetTemplateCategories()).Returns(categories);

            var service = new TemplateService(repo.Object); // implements ITemplateService

            // Act
            var result = service.GetTemplateCategories()!.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Business Templates", result[0].DisplayValue);
            Assert.AreEqual("Education Templates", result[1].DisplayValue);

            repo.Verify(r => r.GetTemplateCategories(), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetTemplatesByCategory_ShouldReturnTwo_WhenBusinessCategory()
        {
            // Arrange
            var repo = new Mock<ITemplateRepository>(MockBehavior.Strict);

            var templates = new List<TemplateByCategoryDTO>
            {
                new() { TemplateId = 1, TemplateTitle = "Project Plan",       TemplateDescription = "Template for project planning",     TemplateCategoryId = 1, TemplateCategory = "Business Templates",  Viewed = 10, Copied = 3 },
                new() { TemplateId = 2, TemplateTitle = "Marketing Campaign", TemplateDescription = "Template for marketing activities", TemplateCategoryId = 1, TemplateCategory = "Business Templates",  Viewed = 25, Copied = 5 }
            };

            repo.Setup(r => r.GetTemplatesByCategory(CategoryIdBusiness)).Returns(templates);

            var service = new TemplateService(repo.Object);

            // Act
            var result = service.GetTemplatesByCategory(CategoryIdBusiness)!.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].TemplateCategoryId);
            Assert.AreEqual("Project Plan", result[0].TemplateTitle);
            Assert.AreEqual(2, result[1].TemplateId);
            Assert.AreEqual("Marketing Campaign", result[1].TemplateTitle);

            repo.Verify(r => r.GetTemplatesByCategory(CategoryIdBusiness), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetTemplatesByCategory_ShouldReturnEmpty_WhenCategoryNotFound()
        {
            // Arrange
            var repo = new Mock<ITemplateRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetTemplatesByCategory(NotFoundCategory))
                .Returns(new List<TemplateByCategoryDTO>());

            var service = new TemplateService(repo.Object);

            // Act
            var result = service.GetTemplatesByCategory(NotFoundCategory)!.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);

            repo.Verify(r => r.GetTemplatesByCategory(NotFoundCategory), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetTemplateDetail_ShouldReturnDetail_WhenTemplateExists()
        {
            // Arrange
            var repo = new Mock<ITemplateRepository>(MockBehavior.Strict);

            var expected = new TemplateDetailDTO
            {
                TemplateId = 1,
                TemplateTitle = "Project Plan",
                TemplateDescription = "Template for project planning",
                Username = "james85",
                UserPicture = "https://example.com/images/james85.png",
                ViewedNumber = 10,
                CopiedNumber = 3,
                BoardId = 1
            };

            repo.Setup(r => r.GetTemplateDetail(TemplateIdProjectPlan)).Returns(expected);

            var service = new TemplateService(repo.Object);

            // Act
            var dto = service.GetTemplateDetail(TemplateIdProjectPlan);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(TemplateIdProjectPlan, dto!.TemplateId);
            Assert.AreEqual("Project Plan", dto.TemplateTitle);
            Assert.AreEqual("Template for project planning", dto.TemplateDescription);
            Assert.AreEqual(10, dto.ViewedNumber);
            Assert.AreEqual(3, dto.CopiedNumber);
            Assert.AreEqual(1, dto.BoardId);

            repo.Verify(r => r.GetTemplateDetail(TemplateIdProjectPlan), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetTemplateDetail_ShouldReturnNull_WhenTemplateNotFound()
        {
            // Arrange
            var repo = new Mock<ITemplateRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetTemplateDetail(NotFoundTemplate))
                .Returns((TemplateDetailDTO?)null);

            var service = new TemplateService(repo.Object);

            // Act
            var dto = service.GetTemplateDetail(NotFoundTemplate);

            // Assert
            Assert.IsNull(dto);

            repo.Verify(r => r.GetTemplateDetail(NotFoundTemplate), Times.Once);
            repo.VerifyNoOtherCalls();
        }
    }
}
