using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbConnection _con;
        public NotificationRepository()
        {
            _con = TestStartup.Conn!;
        }

        public IEnumerable<NotificationDTO> GetNotificationByUser(int userId, bool isRead)
        {
            const string sql = @"
            SELECT 
              noti.Id AS NotificationId, 
              us.Id UserId, 
              us.PictureUrl AS UserPicture, 
              us.Username, 
              ac.ActivityDescription AS ActivityDescription, 
              noti.IsRead, 
              owt.OwnerTypeValue, 
              ac.OwnerId 
            FROM 
              [Notification] noti 
              JOIN Activity ac ON ac.Id = noti.ActivityId 
              JOIN OwnerType owt ON owt.Id = ac.OwnerTypeId 
              JOIN [Users] us ON us.Id = ac.UserId 
            WHERE 
              ac.UserId = @UserId 
              AND noti.IsRead = @IsRead;
            ";

            return _con.Query<NotificationDTO>(sql, new 
            { 
                UserId = userId,
                IsRead = isRead ? 1 : 0
            });
        }
    }
}
