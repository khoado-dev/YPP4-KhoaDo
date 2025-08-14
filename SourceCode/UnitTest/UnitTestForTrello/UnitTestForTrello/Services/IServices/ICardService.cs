using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ICardService
    {
        IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId);
        CardDetailDTO? GetCardDetailByCardId(int cardId);
        IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId);
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
    }
}
