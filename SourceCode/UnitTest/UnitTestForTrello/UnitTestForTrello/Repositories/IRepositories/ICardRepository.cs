using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ICardRepository
    {
        IEnumerable<CardActivityDTO> GetActivitiesByCardId(int cardId);
        IEnumerable<CardAttachmentDTO> GetAttachmentsByCardId(int cardId);
        IEnumerable<CardCommentWithReactionCountDTO> GetCardCommentsAndReactionsCountByCardId(int cardId);
        IEnumerable<CardDetailDTO> GetCardDetailsByBoardId(int boardId);
        CardDetailDTO? GetCardDetailsByCardId(int cardId);
        IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId);
        IEnumerable<CardCustomFieldDTO> GetCustomFieldsByCardId(int cardId);
        IEnumerable<CardCustomFieldValueDTO> GetCustomFieldValuesByCardId(int cardId);
    }
}