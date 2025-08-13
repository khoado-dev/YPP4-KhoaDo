using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ICardRepository
    {
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
    }
}