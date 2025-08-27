using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Services
{
    public class StickerService : IStickerService
    {
        private readonly IStickerRepository _stickerRepository;

        public StickerService(IStickerRepository stickerRepository)
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