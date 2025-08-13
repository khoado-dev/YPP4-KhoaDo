using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Services.IServices
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public CardDetailDTO GetCardDetailByCardId(int cardId)
        {
            return _cardRepository.GetCardDetailByCardId(cardId);
        }

        public IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId)
        {
            return _cardRepository.GetCardSummariesByBoardId(boardId);
        }
    }
}
