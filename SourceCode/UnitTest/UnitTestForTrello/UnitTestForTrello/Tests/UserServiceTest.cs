using Moq;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class UserServiceTest
    {
        private const string ExistingEmail = "james85@booth-daniels.net";
        private const string MissingEmail = "missing@example.com";

        [TestMethod]
        public void GetUserByEmail_ShouldReturnUser_WhenEmailExists()
        {
            // Arrange
            var repo = new Mock<IUserRepository>(MockBehavior.Strict);

            var expected = new UserDTO
            {
                Id = 1,
                Email = ExistingEmail,
                Username = "james85",
                PictureUrl = "https://example.com/images/james85.png",
                Bio = "Software engineer and coffee lover."
            };

            repo.Setup(r => r.GetUserByEmail(ExistingEmail)).Returns(expected);

            var service = new UserService(repo.Object); // implements IUserService

            // Act
            var user = service.GetUserByEmail(ExistingEmail);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(ExistingEmail, user!.Email);
            Assert.AreEqual(expected.Id, user.Id);
            Assert.AreEqual(expected.Username, user.Username);

            repo.Verify(r => r.GetUserByEmail(ExistingEmail), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetUserByEmail_ShouldReturnNull_WhenEmailNotFound()
        {
            // Arrange
            var repo = new Mock<IUserRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetUserByEmail(MissingEmail)).Returns((UserDTO?)null);

            var service = new UserService(repo.Object);

            // Act
            var user = service.GetUserByEmail(MissingEmail);

            // Assert
            Assert.IsNull(user);

            repo.Verify(r => r.GetUserByEmail(MissingEmail), Times.Once);
            repo.VerifyNoOtherCalls();
        }
    }
}
