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

        public IEnumerable<CardDetailDTO> GetCardDetailsByBoardId(int boardId)
        {
            return _cardRepository.GetCardDetailsByBoardId(boardId);
        }

        public CardDetailDTO? GetCardDetailByCardId(int cardId)
        {
            return _cardRepository.GetCardDetailsByCardId(cardId);
        }

        public IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId)
        {
            return _cardRepository.GetCardLabelsByCardId(cardId);
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
