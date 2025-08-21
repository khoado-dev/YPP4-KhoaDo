using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Services.IServices
{
    public class SettingService
    {
        private readonly SettingRepository _settingRepository;

        public SettingService(SettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public IEnumerable<SettingKeyOptionDTO> GetOptionsByOwnerType(OwnerType ownerType)
        {
            return _settingRepository.GetOptionsByOwnerType(ownerType);
        }

        public IEnumerable<SettingKeyValueDTO> GetValuesByOwnerType(OwnerType ownerType, int ownerId)
        {
            return _settingRepository.GetValuesByOwnerType(ownerType, ownerId);
        }
    }
}
