namespace SqlCoreByHand;

[TestClass]
public class AggregateTest
{
    private List<User> users;
    private SqlService<User, Workspace> service;

    [TestInitialize]
    public void Setup()
    {
        users = new List<User>
            {
                new User { Id = 1, Username = "Alice", Age = 25 },
                new User { Id = 2, Username = "Bob", Age = 30 },
                new User { Id = 3, Username = "Charlie", Age = 35 }
            };

        service = new SqlService<User, Workspace>();
        service.RegisterJoinCondition("UserWorkspace", (u, w) => u.Id == w.CreatedBy);
    }

    [TestMethod]
    public void TestSum()
    {
        int result = service.Aggregate(users, u => u.Age, OperationType.Sum);
        int expectedResult = 90; // Sum age is 90
        Assert.AreEqual(expectedResult, result); // 25 + 30 + 35
    }

    [TestMethod]
    public void TestAverage()
    {
        int result = service.Aggregate(users, u => u.Age, OperationType.Avg);
        int expectedResult = 30; // Average age is 30
        Assert.AreEqual(expectedResult, result); // (25 + 30 + 35) / 3
    }

    [TestMethod]
    public void TestMin()
    {
        int result = service.Aggregate(users, u => u.Age, OperationType.Min);
        int expectedResult = 25; // Minimum age is 25
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void TestMax()
    {
        int result = service.Aggregate(users, u => u.Age, OperationType.Max);
        int expectedResult = 35; // Max age is 35
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void TestCount()
    {
        int result = service.Aggregate(users, u => u.Age, OperationType.Count);
        int expectedResult = 3; // Count age is 3
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void TestEmptyList()
    {
        var emptyUsers = new List<User>();
        int result = service.Aggregate(emptyUsers, u => u.Age, OperationType.Sum);
        int expectedResult = 0; // SUM of EmptyList is 0
        Assert.AreEqual(expectedResult, result);
    }
}
