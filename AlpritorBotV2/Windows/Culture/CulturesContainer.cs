using System.Collections.Generic;

namespace AlpritorBotV2.Windows.Culture
{
    public class CulturesContainer
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string CultureName { get; set; }
        public static List<CulturesContainer> GetSupportedCultures()
        {
            return new List<CulturesContainer>(){ new() { Code = "GB", Name = "English", CultureName = "en-GB" }, new() { Code = "RU", Name = "Russia", CultureName = "ru-RU" }, new() { Code = "UA", Name = "Ukraine", CultureName = "uk-UA" } };
        }
    }
}
