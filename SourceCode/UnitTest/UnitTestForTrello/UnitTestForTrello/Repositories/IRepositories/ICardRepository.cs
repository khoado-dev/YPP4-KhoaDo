using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ICardRepository
    {
        IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId);
        CardDetailDTO? GetCardDetailByCardId(int cardId);
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
    }
}