using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface IStickerService
    {
        IEnumerable<StickerDTO> GetCustomStickersByUser(int userId);
        IEnumerable<StickerDTO> GetNonCustomStickers();
    }
}