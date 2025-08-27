using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead)
        {
            return _notificationRepository.GetNotificationByUser(userId, isRead);
        }
    }
}