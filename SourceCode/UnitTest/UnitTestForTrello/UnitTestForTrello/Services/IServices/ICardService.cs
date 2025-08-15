using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ICardService
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
