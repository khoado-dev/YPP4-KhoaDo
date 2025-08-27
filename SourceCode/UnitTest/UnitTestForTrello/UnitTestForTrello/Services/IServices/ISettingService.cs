using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Services.IServices
{
    public interface ISettingService
    {
        IEnumerable<SettingKeyOptionDTO> GetOptionsByOwnerType(OwnerType ownerType);
        IEnumerable<SettingKeyValueDTO> GetValuesByOwnerType(OwnerType ownerType, int ownerId);
    }
}