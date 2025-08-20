
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services;

namespace UnitTestForTrello.Controllers
{
    public class StickerController
    {
        private readonly StickerService _stickerService;
        public StickerController(StickerService stickerService)
        {
            _stickerService = stickerService;
        }
        public IEnumerable<StickerDTO> GetCustomStickersByUser(int userId)
        {
            return _stickerService.GetCustomStickersByUser(userId);
        }

        public IEnumerable<StickerDTO> GetNonCustomStickers()
        {
            return _stickerService.GetNonCustomStickers();
        }
    }
}