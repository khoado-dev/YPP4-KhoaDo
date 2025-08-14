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

        //Board Screen
        public int? CardPosition { get; set; }
        public int? StagePosition { get; set; }
        public string? CardCover { get; set; }
        public int? NumberOfComments { get; set; }
        public int? NumberOfCheckListItem { get; set; }
        public int? NumberOfAttachment { get; set; }
        public int? StageId { get; set; }
        public string? StageColor { get; set; }
        public int? BoardId { get; set; }
        public string? BoardName { get; set; }
    }
}
