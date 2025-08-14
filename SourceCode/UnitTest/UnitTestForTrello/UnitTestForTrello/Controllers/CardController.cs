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

        public IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId)
        {
            return _cardService.GetCardSummariesByBoardId(boardId);
        }

        public CardDetailDTO? GetCardDetailByCardId(int cardId)
        {
            return _cardService.GetCardDetailByCardId(cardId);
        }

        public IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId)
        {
            return _cardService.GetCardDetailByBoardId(boardId);
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
    }
}
