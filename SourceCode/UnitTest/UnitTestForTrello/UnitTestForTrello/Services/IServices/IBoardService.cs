using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface IBoardService
    {
        public IEnumerable<StarredBoardDTO> GetStarredBoards(int userId);
        public IEnumerable<RecentlyBoardDTO> GetRecentlyBoards(int userId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsMemberInWorkspace(int userId, int workspaceId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsOwnerInWorkspace(int userId, int workspaceId);
    }
}
