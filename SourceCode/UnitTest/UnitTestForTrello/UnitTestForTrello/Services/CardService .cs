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

        public IEnumerable<CardActivityDTO> GetActivitiesByCardId(int cardId)
        {
            return _cardRepository.GetActivitiesByCardId(cardId);
        }

        public IEnumerable<CardAttachmentDTO> GetAttachmentsByCardId(int cardId)
        {
            return _cardRepository.GetAttachmentsByCardId(cardId);
        }

        public IEnumerable<CardCommentWithReactionCountDTO> GetCardCommentsAndReactionsCountByCardId(int cardId)
        {
            return _cardRepository.GetCardCommentsAndReactionsCountByCardId(cardId);
        }

        public IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId)
        {
            return _cardRepository.GetCardDetailByBoardId(boardId);
        }

        public CardDetailDTO? GetCardDetailByCardId(int cardId)
        {
            return _cardRepository.GetCardDetailByCardId(cardId);
        }

        public IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId)
        {
            return _cardRepository.GetCardLabelsByCardId(cardId);
        }

        public IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId)
        {
            return _cardRepository.GetCardSummariesByBoardId(boardId);
        }

        public IEnumerable<CardCustomFieldDTO> GetCustomFieldsByCardId(int cardId)
        {
            return _cardRepository.GetCustomFieldsByCardId(cardId);
        }

        public IEnumerable<CardCustomFieldValueDTO> GetCustomFieldValuesByCardId(int cardId)
        {
            return _cardRepository.GetCustomFieldValuesByCardId(cardId);
        }
    }
}
