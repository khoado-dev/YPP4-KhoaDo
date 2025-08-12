using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class StarredBoardDTO
    {
        public int UserId { get; set; }
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public string BackgroundUrl { get; set; }
        public string BoardStatus { get; set; }
        public bool StarredBoardsStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
