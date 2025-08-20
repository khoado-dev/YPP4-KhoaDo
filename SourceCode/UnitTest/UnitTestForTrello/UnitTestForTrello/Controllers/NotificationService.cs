using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;

namespace UnitTestForTrello.Controllers
{
    public class NotificationService
    {
        private readonly NotificationRepository _notificationRepository;
        public NotificationService(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead)
        {
            return _notificationRepository.GetNotificationByUser(userId, isRead);
        }
    }
}