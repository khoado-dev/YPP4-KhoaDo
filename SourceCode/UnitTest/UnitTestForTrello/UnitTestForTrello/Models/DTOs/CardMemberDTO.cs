using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class CardMemberDTO
    {
        public int UserId { get; set; }
        public string UserPicture { get; set; }
        public int CardId { get; set; }
    }

}
