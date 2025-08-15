using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ICardRepository
    {
        IEnumerable<CardActivityDTO> GetActivitiesByCardId(int cardId);
        IEnumerable<CardAttachmentDTO> GetAttachmentsByCardId(int cardId);
        IEnumerable<CardCommentWithReactionCountDTO> GetCardCommentsAndReactionsCountByCardId(int cardId);
        IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId);
        CardDetailDTO? GetCardDetailByCardId(int cardId);
        IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId);
        IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId);
        IEnumerable<CardCustomFieldDTO> GetCustomFieldsByCardId(int cardId);
        IEnumerable<CardCustomFieldValueDTO> GetCustomFieldValuesByCardId(int cardId);
    }
}