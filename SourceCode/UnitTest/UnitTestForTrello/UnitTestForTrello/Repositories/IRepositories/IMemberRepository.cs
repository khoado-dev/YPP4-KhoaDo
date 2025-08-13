using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IMemberRepository
    {
        IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId);
    }
}
