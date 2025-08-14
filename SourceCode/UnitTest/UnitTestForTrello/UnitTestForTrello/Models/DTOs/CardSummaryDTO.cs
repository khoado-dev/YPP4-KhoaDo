namespace UnitTestForTrello.Models.DTOs
{
    public class CardSummaryDTO
    {
        public int CardPosition { get; set; }
        public int StagePosition { get; set; }
        public int CardId { get; set; }
        public string CardTitle { get; set; }
        public string CardLocation { get; set; }
        public string CardCover { get; set; }
        public int NumberOfComments { get; set; } = 0;
        public int NumberOfCheckListItem { get; set; } = 0;
        public int NumberOfAttachment { get; set; } = 0;
        public int StageId { get; set; }
        public string StageTitle { get; set; }
        public string StageColor { get; set; }
        public int BoardId { get; set; }
        public string BoardName { get; set; }
    }
}
