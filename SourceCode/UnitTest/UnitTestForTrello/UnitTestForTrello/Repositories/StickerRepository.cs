using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Repositories
{
    public class StickerRepository
    {
        private readonly IDbConnection _con;
        private readonly ICustomCache _cache;

        public StickerRepository()
        {
            _con = TestStartup.Conn!;
            _cache = TestStartup.Cache;
        }
        public IEnumerable<StickerDTO> GetCustomStickersByUser(int userId)
        {
            string customType = "Custom Stickers";
            const string sql = @"
            SELECT 
              stk.Id StickerId, 
              stk.StickerName, 
              stk.StickerUrl, 
              skc.Id StickerCategoryId, 
              skc.DisplayValue, 
              stk.CreatedBy 
            FROM 
              Sticker stk 
              JOIN StickerCategory skc ON skc.Id = stk.CategoryId 
            WHERE 
              DisplayValue = @DisplayValue 
              AND stk.CreatedBy = @UserId;
            ";
            return _con.Query<StickerDTO>(sql, new 
            {
                UserId = userId,
                DisplayValue = customType
            });
        }

        public IEnumerable<StickerDTO> GetNonCustomStickers()
        {
            string customType = "Custom Stickers";
            const string sql = @"
            SELECT 
              stk.Id StickerId, 
              stk.StickerName, 
              stk.StickerUrl, 
              skc.Id StickerCategoryId, 
              skc.DisplayValue 
            FROM 
              Sticker stk 
              JOIN StickerCategory skc ON skc.Id = stk.CategoryId 
            WHERE 
              DisplayValue != @DisplayValue 
            ORDER BY 
              skc.DisplayValue;
            ";
            return _con.Query<StickerDTO>(sql, new
            {
                DisplayValue = customType
            });
        }
    }
}