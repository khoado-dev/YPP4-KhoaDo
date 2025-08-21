namespace UnitTestForTrello.Models.DTOs
{
    public class SettingKeyValueDTO
    {
        public string KeyName { get; set; } = string.Empty;
        public int Value { get; set; } // COALESCE(SettingContent, DefaultValue) -> int
    }
}
