using System.Collections.Generic;

namespace AlpritorBotV2.Windows.Culture
{
    public class CultureInfo
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string CultureName { get; set; }
        public static List<CultureInfo> GetSupportedCultures()
        {
            return new List<CultureInfo>(){ new() { Code = "GB", Name = "English", CultureName = "en-GB" }, new() { Code = "RU", Name = "Russia", CultureName = "ru-RU" }, new() { Code = "UA", Name = "Ukraine", CultureName = "uk-UA" } };
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
