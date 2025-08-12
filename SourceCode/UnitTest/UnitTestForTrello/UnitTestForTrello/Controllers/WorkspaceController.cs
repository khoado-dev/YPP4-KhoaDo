using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class WorkspaceController
    {
        private readonly IWorkspaceService _workspaceService;
        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        public IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId)
        {
            return _workspaceService.GetWorkspacesByUserId(userId);
        }
    }
}
