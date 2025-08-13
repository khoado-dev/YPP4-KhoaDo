using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ICardService
    {
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
    }
}
