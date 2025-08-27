using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ISettingRepository
    {
        IEnumerable<SettingKeyOptionDTO> GetOptionsByOwnerType(OwnerType ownerType);
        IEnumerable<SettingKeyValueDTO> GetValuesByOwnerType(OwnerType ownerType, int ownerId);
    }
}
