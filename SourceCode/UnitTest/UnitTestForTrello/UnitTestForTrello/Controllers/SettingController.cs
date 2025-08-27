using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Controllers
{
    public class SettingController
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IEnumerable<SettingKeyOptionDTO> GetOptionsByOwnerType(OwnerType ownerType)
        {
            return _settingService.GetOptionsByOwnerType(ownerType);
        }

        public IEnumerable<SettingKeyValueDTO> GetValuesByOwnerType(OwnerType ownerType, int ownerId)
        {
            return _settingService.GetValuesByOwnerType(ownerType, ownerId);
        }
    }
}
