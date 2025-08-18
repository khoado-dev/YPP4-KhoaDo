using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IMemberRepository
    {
        IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId);
        IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId);
        IEnumerable<WorkspaceMemberDTO> GetMembersByWorkspaceId(int workspaceId);
        IEnumerable<CardSelectableMemberDTO> GetSelectableMembersByCardId(int cardId);
    }
}
