using UnitTestForTrello.Models;

namespace UnitTestForTrello;

[TestClass]
public class WorkspaceTest
{
    [TestMethod]
    public void TestCreateWorkspaceWithValidData()
    {
        string workspaceName = "Workspace 1";
        Workspace workspace = new(workspaceName);
        Assert.AreEqual(workspaceName, workspace.Title);

    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestCreateWorkspaceNullTitle()
    {
        string workspaceName = "";
        Workspace workspace = new(workspaceName);
    }
}
