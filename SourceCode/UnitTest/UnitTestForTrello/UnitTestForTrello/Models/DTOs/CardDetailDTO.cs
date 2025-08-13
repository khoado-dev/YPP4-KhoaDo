using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class CardDetailDTO
    {
        public int CardId { get; set; }
        public string? CardTitle { get; set; }
        public string? CardDescription { get; set; }
        public string? CardLocation { get; set; }
        public string? StageTitle { get; set; }
    }
}
