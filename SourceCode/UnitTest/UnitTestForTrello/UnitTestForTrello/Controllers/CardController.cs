using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class CardController
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        public CardDetailDTO? GetCardDetailByCardId(int cardId)
        {
            return _cardService.GetCardDetailByCardId(cardId);
        }

        public IEnumerable<CardDetailDTO> GetCardDetailsByBoardId(int boardId)
        {
            return _cardService.GetCardDetailsByBoardId(boardId);
        }

        public IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId)
        {
            return _cardService.GetCardLabelsByCardId(cardId);
        }

        public IEnumerable<CardCommentWithReactionCountDTO> GetCardCommentsAndReactionsCountByCardId(int cardId)
        {
            return _cardService.GetCardCommentsAndReactionsCountByCardId(cardId);
        }

        public IEnumerable<CardActivityDTO> GetActivitiesByCardId(int cardId)
        {
            return _cardService.GetActivitiesByCardId(cardId);
        }

        public IEnumerable<CardCustomFieldDTO> GetCustomFieldsByCardId(int cardId)
        {
            return _cardService.GetCustomFieldsByCardId(cardId);
        }

        public IEnumerable<CardCustomFieldValueDTO> GetCustomFieldValuesByCardId(int cardId)
        {
            return _cardService.GetCustomFieldValuesByCardId(cardId);
        }

        public IEnumerable<CardAttachmentDTO> GetAttachmentsByCardId(int cardId)
        {
            return _cardService.GetAttachmentsByCardId(cardId);
        }
    }
}
