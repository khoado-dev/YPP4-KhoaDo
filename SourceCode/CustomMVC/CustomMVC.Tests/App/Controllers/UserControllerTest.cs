using CustomMVC.App.Controllers;
using CustomMVC.App.Models;
using CustomMVC.App.Service.IService;
using CustomMVC.Core.Mvc.Results;
using Moq;
using System.Reflection;

namespace CustomMVC.Tests;

[TestClass]
public class UserControllerTest
{
    private static T? GetPrivateField<T>(object obj, string fieldName)
    {
        var f = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (T?)f?.GetValue(obj);
    }
    [TestMethod]
    public void GetUsers_ShouldReturn_ListUsers_View_WithUsers()
    {
        // Arrange
        var users = new List<UserDTO>
            {
                new UserDTO { Id = 1, Name = "A", Email = "a@ex.com" },
                new UserDTO { Id = 2, Name = "B", Email = "b@ex.com" }
            };
        var service = new Mock<IUserService>(MockBehavior.Strict);
        service.Setup(s => s.GetAllUsers()).Returns(users);

        var controller = new UsersController(service.Object);

        // Act
        var result = controller.GetUsers();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));

        var view = (ViewResult)result;
        // verify viewName & model via reflection (fields: _viewName, _model)
        var viewName = GetPrivateField<string>(view, "_viewName");
        var model = GetPrivateField<object>(view, "_model");

        Assert.AreEqual("Users/ListUsers", viewName, "Expected explicit view name 'Users/ListUsers'.");
        Assert.IsNotNull(model);

        // model is anonymous type containing property Users
        var usersProp = model.GetType().GetProperty("Users");
        Assert.IsNotNull(usersProp);

        var modelUsers = usersProp!.GetValue(model) as IEnumerable<UserDTO>;
        CollectionAssert.AreEquivalent(users.ToList(), modelUsers!.ToList());

        service.Verify(s => s.GetAllUsers(), Times.Once);
    }
}
