namespace UnitTestForTrello.Models.DTOs
{
    public class CardCustomFieldDTO
    {
        public int CardId { get; set; }
        public int BoardId { get; set; }
        public int CustomFieldId { get; set; }
        public string CustomFieldTitle { get; set; } = string.Empty;
        public string DataTypeValue { get; set; } = string.Empty;
        public int? FieldItemId { get; set; }
        public string FieldItemValue { get; set; } = string.Empty;
        public int Position { get; set; }
    }

}
