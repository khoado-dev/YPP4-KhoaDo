namespace UnitTestForTrello.Models.DTOs
{
    public sealed class StickerDTO
    {
        public int StickerId { get; set; }
        public string StickerName { get; set; } = string.Empty;
        public string StickerUrl { get; set; } = string.Empty;

        public int StickerCategoryId { get; set; }
        public string DisplayValue { get; set; } = string.Empty;

        public int? CreatedBy { get; set; } // chỉ có với custom stickers
    }

}
