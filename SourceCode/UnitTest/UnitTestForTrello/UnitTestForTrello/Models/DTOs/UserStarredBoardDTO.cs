using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class UserStarredBoardDTO
    {
        public int UserId { get; set; }
        public int BoardId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool StarredBoardsStatus { get; set; }
    }
}
