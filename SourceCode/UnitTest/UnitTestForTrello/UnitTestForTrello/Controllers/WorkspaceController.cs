using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests;

namespace UnitTestForTrello.Controllers
{
    public class WorkspaceController
    {
        private readonly WorkspaceService _workspaceService;
        public WorkspaceController(WorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        public IEnumerable<WorkspaceDTO> GetWorkspacesByUserId(int userId)
        {
            return _workspaceService.GetWorkspacesByUserId(userId);
        }

        public IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes()
        {
            return _workspaceService.GetWorkspaceTypes();
        }

        public WorkspaceDetailDTO GetWorkspaceDetailById(int workspaceId)
        {
            return _workspaceService.GetWorkspaceDetailById(workspaceId);
        }
    }
}
