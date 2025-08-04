using UnitTestForTrello.Models;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Tests;

namespace UnitTestForTrello;



[TestClass]
public class WorkspaceTest : DatabaseTestBase
{
    private WorkspaceRepository _workspaceRepository;
    [TestInitialize]
    public void Setup()
    {
        base.BeginTransaction(); 
        _workspaceRepository = new WorkspaceRepository(_connection!, _transaction!);
    }

    [TestMethod]
    public void CreateWorkspaceTest()
    {
        var workspace = new Workspace
        {
            WorkspaceName = "Test Workspace",
            WorkspaceDescription = "Test Desc",
            CategoryId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            LogoUrl = "http://logo"
        };

        int id = _workspaceRepository.CreateWorkspace(workspace);
        var newWorkspace = _workspaceRepository.GetWorkspaceById(id);
        Assert.IsNotNull(newWorkspace);
        Assert.IsTrue(newWorkspace?.WorkspaceName?.Equals(workspace.WorkspaceName));
    }

    [TestMethod]
    public void GetWorkspaceByIdTest()
    {
        var workspace = new Workspace
        {
            WorkspaceName = "GetById Workspace",
            WorkspaceDescription = "Desc",
            CategoryId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            LogoUrl = "http://logo"
        };
        int id = _workspaceRepository.CreateWorkspace(workspace);

        var result = _workspaceRepository.GetWorkspaceById(id);
        Assert.IsNotNull(result);
        Assert.AreEqual(workspace.WorkspaceName, result?.WorkspaceName);
    }

    [TestMethod]
    public void GetAllWorkspacesTest()
    {
        var before = _workspaceRepository.GetAllWorkspaces().Count;

        var workspace = new Workspace
        {
            WorkspaceName = "AllWorkspacesTest",
            WorkspaceDescription = "Desc",
            CategoryId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            LogoUrl = "http://logo"
        };
        _workspaceRepository.CreateWorkspace(workspace);

        var after = _workspaceRepository.GetAllWorkspaces().Count;

        Assert.IsTrue(after == before + 1);

    }

    [TestMethod]
    public void UpdateWorkspaceTest()
    {
        var workspace = new Workspace
        {
            WorkspaceName = "UpdateWorkspaceTest",
            WorkspaceDescription = "Desc",
            CategoryId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            LogoUrl = "http://logo"
        };
        int id = _workspaceRepository.CreateWorkspace(workspace);

        var toUpdate = _workspaceRepository.GetWorkspaceById(id);
        Assert.IsNotNull(toUpdate);

        toUpdate.WorkspaceName = "Updated Name";
        toUpdate.UpdatedAt = DateTime.UtcNow;
        toUpdate.UpdatedBy = 2;

        var updated = _workspaceRepository.UpdateWorkspace(toUpdate);

        Assert.IsTrue(updated);

        var afterUpdate = _workspaceRepository.GetWorkspaceById(id);
        Assert.AreEqual(toUpdate.WorkspaceName, afterUpdate?.WorkspaceName);
        Assert.AreEqual(toUpdate.UpdatedBy, afterUpdate?.UpdatedBy);
    }

    [TestMethod]
    public void DeleteWorkspaceTest()
    {
        var workspace = new Workspace
        {
            WorkspaceName = "DeleteWorkspaceTest",
            WorkspaceDescription = "Desc",
            CategoryId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            LogoUrl = "http://logo"
        };
        int id = _workspaceRepository.CreateWorkspace(workspace);

        var deleted = _workspaceRepository.DeleteWorkspace(id);

        Assert.IsTrue(deleted);

        var afterDelete = _workspaceRepository.GetWorkspaceById(id);
        Assert.IsNull(afterDelete);
    }
}
