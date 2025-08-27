using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IStickerRepository
    {
        IEnumerable<StickerDTO> GetCustomStickersByUser(int userId);
        IEnumerable<StickerDTO> GetNonCustomStickers();
    }
}
