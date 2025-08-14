using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class CardCustomFieldValueDTO
    {
        public int CardId { get; set; }
        public int BoardId { get; set; }
        public int CustomFieldId { get; set; }
        public string CustomFieldTitle { get; set; } = string.Empty;
        public string DataTypeValue { get; set; } = string.Empty;
        public string FieldValue { get; set; } = string.Empty;
        public string FieldItemValue { get; set; } = string.Empty;
        public int Position { get; set; }
    }

}
