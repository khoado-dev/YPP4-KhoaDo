
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class NotificationController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead)
        {
            return _notificationService.GetNotificationByUser(userId, isRead);
        }

    }
}
