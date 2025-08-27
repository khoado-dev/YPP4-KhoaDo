using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class StickerController
    {
        private readonly IStickerService _stickerService;
        public StickerController(IStickerService stickerService)
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