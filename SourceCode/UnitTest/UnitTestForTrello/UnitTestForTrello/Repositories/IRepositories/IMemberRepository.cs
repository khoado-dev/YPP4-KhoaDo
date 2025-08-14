using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IMemberRepository
    {
        IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId);
        IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId);
        IEnumerable<CardSelectableMemberDTO> GetSelectableMembersByCardId(int cardId);
    }
}
