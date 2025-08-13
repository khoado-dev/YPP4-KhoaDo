using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface IWorkspaceService
    {
        WorkspaceDetailDTO GetWorkspaceDetailById(int workspaceId);
        IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId);
        IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes();
    }
}