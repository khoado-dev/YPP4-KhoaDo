using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface IMemberService
    {
        IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId);
        IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId);
    }
}