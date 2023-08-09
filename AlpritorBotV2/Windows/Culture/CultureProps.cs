using System.Collections.Generic;

namespace AlpritorBotV2.Windows.Culture
{
    public class CultureProps
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string CultureName { get; set; }
        public string? ChannelName { get; set; }
        public static List<CultureProps> GetSupportedCultures()
        {
            return new List<CultureProps>(){ new() { Code = "GB", Name = "English", CultureName = "en-GB" }, new() { Code = "RU", Name = "Russia", CultureName = "ru-RU" }, new() { Code = "UA", Name = "Ukraine", CultureName = "uk-UA" } };
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
