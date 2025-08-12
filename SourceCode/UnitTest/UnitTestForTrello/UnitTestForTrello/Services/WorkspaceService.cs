using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Tests
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceRepository _workspaceRepository;

        public WorkspaceService(IWorkspaceRepository workspaceRepository)
        {
            _workspaceRepository = workspaceRepository;
        }

        public IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId)
        {
            return _workspaceRepository.GetWorkspacesByUserId(userId);
        }
    }
}