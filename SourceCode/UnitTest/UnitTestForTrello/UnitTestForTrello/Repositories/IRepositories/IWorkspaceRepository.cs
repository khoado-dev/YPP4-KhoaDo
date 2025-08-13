using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IWorkspaceRepository
    {
        WorkspaceDetailDTO GetWorkspaceDetailById(int workspaceId);
        IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId);
        IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes();
    }
}