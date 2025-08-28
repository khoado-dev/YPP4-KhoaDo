using Moq;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class WorkspaceServiceTest
    {
        [TestMethod]
        public void GetWorkspacesByUserId_ShouldReturnTwoWorkspaces_WhenUserExists()
        {
            // Arrange
            const int userId = 1;
            var repo = new Mock<IWorkspaceRepository>(MockBehavior.Strict);

            var workspaces = new List<WorkspaceDTO>
            {
                new WorkspaceDTO { WorkspaceId = 1, WorkspaceName = "Workspace 1", LogoUrl = "logo1.png", UserId = userId, CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new WorkspaceDTO { WorkspaceId = 2, WorkspaceName = "Workspace 2", LogoUrl = "logo2.png", UserId = userId, CreatedAt = DateTime.UtcNow.AddDays(-10) }
            };

            repo.Setup(r => r.GetWorkspacesByUserId(userId)).Returns(workspaces);

            var service = new WorkspaceService(repo.Object); // implements IWorkspaceService

            // Act
            var result = service.GetWorkspacesByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].WorkspaceId);
            Assert.AreEqual("Workspace 1", result[0].WorkspaceName);
            Assert.AreEqual("logo1.png", result[0].LogoUrl);
            Assert.AreEqual(2, result[1].WorkspaceId);
            Assert.AreEqual("Workspace 2", result[1].WorkspaceName);
            Assert.AreEqual("logo2.png", result[1].LogoUrl);

            repo.Verify(r => r.GetWorkspacesByUserId(userId), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetWorkspacesByUserId_ShouldReturnEmpty_WhenUserHasNoWorkspaces()
        {
            // Arrange
            const int userId = 123;
            var repo = new Mock<IWorkspaceRepository>(MockBehavior.Strict);

            repo.Setup(r => r.GetWorkspacesByUserId(userId)).Returns(new List<WorkspaceDTO>());

            var service = new WorkspaceService(repo.Object);

            // Act
            var result = service.GetWorkspacesByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);

            repo.Verify(r => r.GetWorkspacesByUserId(userId), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetWorkspaceTypes_ShouldReturnNineTypes_WhenTypesAreSeeded()
        {
            // Arrange
            var repo = new Mock<IWorkspaceRepository>(MockBehavior.Strict);

            var types = new List<WorkspaceTypeDTO>
            {
                new() { WorkspaceTypeId = 1, TypeValue = "business",        DisplayValue = "Business" },
                new() { WorkspaceTypeId = 2, TypeValue = "sales_crm",       DisplayValue = "Sales CRM" },
                new() { WorkspaceTypeId = 3, TypeValue = "engineering_it",  DisplayValue = "Engineering-IT" },
                new() { WorkspaceTypeId = 4, TypeValue = "small_business",  DisplayValue = "Small Business" },
                new() { WorkspaceTypeId = 5, TypeValue = "education",       DisplayValue = "Education" },
                new() { WorkspaceTypeId = 6, TypeValue = "human_resources", DisplayValue = "Human Resources" },
                new() { WorkspaceTypeId = 7, TypeValue = "operations",      DisplayValue = "Operations" },
                new() { WorkspaceTypeId = 8, TypeValue = "marketing",       DisplayValue = "Marketing" },
                new() { WorkspaceTypeId = 9, TypeValue = "other",           DisplayValue = "Other" },
            };

            repo.Setup(r => r.GetWorkspaceTypes()).Returns(types);

            var service = new WorkspaceService(repo.Object);

            // Act
            var result = service.GetWorkspaceTypes().ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result.Count);
            var picks = result.Where(t => t.WorkspaceTypeId is 1 or 2 or 3 or 9)
                              .Select(t => t.DisplayValue)
                              .ToArray();
            CollectionAssert.AreEquivalent(new[] { "Business", "Sales CRM", "Engineering-IT", "Other" }, picks);

            repo.Verify(r => r.GetWorkspaceTypes(), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetWorkspaceDetailById_ShouldReturnFullDetail_WhenWorkspaceExists()
        {
            // Arrange
            const int workspaceId = 1;
            var repo = new Mock<IWorkspaceRepository>(MockBehavior.Strict);

            var detail = new WorkspaceDetailDTO
            {
                WorkspaceId = 1,
                WorkspaceName = "Workspace 1",
                ShortName = "WS1",
                Website = "https://workspace1.com",
                WorkspaceDescription = "Description for Workspace 1",
                LogoUrl = "logo1.png"
            };

            repo.Setup(r => r.GetWorkspaceDetailById(workspaceId)).Returns(detail);

            var service = new WorkspaceService(repo.Object);

            // Act
            var result = service.GetWorkspaceDetailById(workspaceId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result!.WorkspaceId);
            Assert.AreEqual("Workspace 1", result.WorkspaceName);
            Assert.AreEqual("WS1", result.ShortName);
            Assert.AreEqual("https://workspace1.com", result.Website);
            Assert.AreEqual("Description for Workspace 1", result.WorkspaceDescription);
            Assert.AreEqual("logo1.png", result.LogoUrl);

            repo.Verify(r => r.GetWorkspaceDetailById(workspaceId), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetWorkspaceDetailById_ShouldReturnNull_WhenWorkspaceDoesNotExist()
        {
            // Arrange
            const int notFoundId = 999;
            var repo = new Mock<IWorkspaceRepository>(MockBehavior.Strict);

            repo.Setup(r => r.GetWorkspaceDetailById(notFoundId)).Returns((WorkspaceDetailDTO?)null);

            var service = new WorkspaceService(repo.Object);

            // Act
            var result = service.GetWorkspaceDetailById(notFoundId);

            // Assert
            Assert.IsNull(result);

            repo.Verify(r => r.GetWorkspaceDetailById(notFoundId), Times.Once);
            repo.VerifyNoOtherCalls();
        }
    }
}
