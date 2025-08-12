using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IWorkspaceRepository
    {
        IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId);
    }
}