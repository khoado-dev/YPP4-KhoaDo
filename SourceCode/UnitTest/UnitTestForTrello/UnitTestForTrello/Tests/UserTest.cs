using UnitTestForTrello.Models;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Tests;

namespace UnitTestForTrello;

[TestClass]
public class UserTest : DatabaseTestBase
{
    private UserRepository _userRepository;

    [TestInitialize]
    public void Setup()
    {
        base.BeginTransaction();
        _userRepository = new UserRepository(_connection!, _transaction!);
    }

    [TestMethod]
    public void CreateUserTest()
    {
        var user = new User
        {
            Username = "testuser",
            Bio = "Test bio",
            Email = "test@example.com",
            LastActive = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PictureUrl = "http://picture"
        };

        int id = _userRepository.Create(user);
        var newUser = _userRepository.GetById(id);
        Assert.IsNotNull(newUser);
        Assert.AreEqual(user.Username, newUser?.Username);
    }

    [TestMethod]
    public void GetUserByIdTest()
    {
        var user = new User
        {
            Username = "getbyiduser",
            Bio = "Bio",
            Email = "getbyid@example.com",
            LastActive = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PictureUrl = "http://picture"
        };
        int id = _userRepository.Create(user);

        var result = _userRepository.GetById(id);
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Username, result?.Username);
    }

    [TestMethod]
    public void GetAllUsersTest()
    {
        var before = _userRepository.GetAll().Count;

        var user = new User
        {
            Username = "alluserstest",
            Bio = "Bio",
            Email = "allusers@example.com",
            LastActive = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PictureUrl = "http://picture"
        };
        _userRepository.Create(user);

        var after = _userRepository.GetAll().Count;

        Assert.IsTrue(after == before + 1);
    }

    [TestMethod]
    public void UpdateUserTest()
    {
        var user = new User
        {
            Username = "updateusertest",
            Bio = "Bio",
            Email = "updateuser@example.com",
            LastActive = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PictureUrl = "http://picture"
        };
        int id = _userRepository.Create(user);

        var toUpdate = _userRepository.GetById(id);
        Assert.IsNotNull(toUpdate);

        toUpdate.Username = "updateduser";
        toUpdate.UpdatedAt = DateTime.UtcNow;

        var updated = _userRepository.Update(toUpdate);

        Assert.IsTrue(updated);

        var afterUpdate = _userRepository.GetById(id);
        Assert.AreEqual(toUpdate.Username, afterUpdate?.Username);
    }

    [TestMethod]
    public void DeleteUserTest()
    {
        var user = new User
        {
            Username = "deleteusertest",
            Bio = "Bio",
            Email = "deleteuser@example.com",
            LastActive = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PictureUrl = "http://picture"
        };
        int id = _userRepository.Create(user);

        var deleted = _userRepository.Delete(id);

        Assert.IsTrue(deleted);

        var afterDelete = _userRepository.GetById(id);
        Assert.IsNull(afterDelete);
    }
}
