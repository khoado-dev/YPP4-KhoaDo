using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ICardRepository
    {
        IEnumerable<CardCommentWithReactionCountDTO> GetCardCommentsAndReactionsCountByCardId(int cardId);
        IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId);
        CardDetailDTO? GetCardDetailByCardId(int cardId);
        IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId);
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
    }
}