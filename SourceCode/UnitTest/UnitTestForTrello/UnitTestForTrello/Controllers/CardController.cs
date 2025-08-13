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
    }
}
