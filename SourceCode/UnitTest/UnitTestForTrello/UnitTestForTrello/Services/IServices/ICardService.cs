using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ICardService
    {
        CardDetailDTO GetCardDetailByCardId(int cardId);
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
    }
}
