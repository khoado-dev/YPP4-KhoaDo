
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Controllers
{
    public class NotificationController
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead)
        {
           return _notificationService.GetNotificationByUser(userId, isRead);
        }

    }
}
