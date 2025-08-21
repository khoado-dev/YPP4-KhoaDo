namespace UnitTestForTrello.Models.DTOs
{
    public sealed class SettingKeyOptionDTO
    {
        public string SettingKey { get; set; } = string.Empty;             // sk.KeyName
        public string SettingKeyDescription { get; set; } = string.Empty;  // sk.SettingKeyDescription
        public string SettingOptionDisplayValue { get; set; } = string.Empty; // so.DisplayValue
    }
}
