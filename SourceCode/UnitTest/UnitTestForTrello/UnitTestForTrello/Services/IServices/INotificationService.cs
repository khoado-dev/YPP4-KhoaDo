using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface INotificationService
    {
        IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead);
    }
}
