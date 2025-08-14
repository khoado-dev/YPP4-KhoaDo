using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class CardLabelDTO
    {
        public int CardId { get; set; }
        public int LabelId { get; set; }
        public string? LabelTitle { get; set; }
        public string? ColorName { get; set; }
        public string? LabelIcon{ get; set; }
    }

}
