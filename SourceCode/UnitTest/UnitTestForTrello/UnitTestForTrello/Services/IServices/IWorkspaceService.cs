using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface IWorkspaceService
    {
        IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId);
        IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes();
    }
}