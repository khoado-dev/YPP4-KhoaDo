using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;
namespace UnitTestForTrello.Repositories
{
    public class SettingRepository
    {
        private readonly IDbConnection _con;

        public SettingRepository()
        {
            _con = TestStartup.Conn!;
        }

        public IEnumerable<SettingKeyOptionDTO> GetOptionsByOwnerType(OwnerType ownerType)
        {
            const string sql = @"
            SELECT 
              sk.KeyName AS SettingKey, 
              sk.SettingKeyDescription, 
              so.DisplayValue AS SettingOptionDisplayValue 
            FROM 
              SettingKey sk 
              JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId 
              JOIN SettingKeySettingOption sso ON sso.SettingKeyId = sk.Id 
              JOIN SettingOption so ON so.Id = sso.SettingOptionId 
            WHERE 
              OwnerTypeValue = @OwnerType 
            ORDER BY 
              sk.KeyName;

            ";
            return _con.Query<SettingKeyOptionDTO>(sql, new 
            {
                OwnerType = ownerType.ToString() 
            });
        }

        public IEnumerable<SettingKeyValueDTO> GetValuesByOwnerType(OwnerType ownerType, int ownerId)
        {
            const string sql = @"
            SELECT 
              sk.KeyName, 
              COALESCE(
                sv.SettingContent, sk.DefaultValue
              ) AS Value 
            FROM 
              SettingKey sk 
              JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId 
              LEFT JOIN SettingValue sv ON sv.SettingKeyId = sk.Id 
              AND sv.OwnerId = @OwnerId
            WHERE 
              owt.OwnerTypeValue = @OwnerType;
            ";
            return _con.Query<SettingKeyValueDTO>(sql, new
            {
                OwnerId = ownerId,
                OwnerType = ownerType.ToString()
            });
        }
    }
}
