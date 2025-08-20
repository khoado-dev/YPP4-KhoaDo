using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;

namespace UnitTestForTrello.Services
{
    public class StickerService
    {
        private readonly StickerRepository _stickerRepository;

        public StickerService(StickerRepository stickerRepository)
        {
            _stickerRepository = stickerRepository;
        }

        public IEnumerable<StickerDTO> GetCustomStickersByUser(int userId)
        {
            return _stickerRepository.GetCustomStickersByUser(userId);
        }

        public IEnumerable<StickerDTO> GetNonCustomStickers()
        {
            return _stickerRepository.GetNonCustomStickers();
        }
    }
}