using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface INotificationRepository
    {
        IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead);
    }
}
