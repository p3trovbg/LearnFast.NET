namespace LearnFast.Data.Seeding.DTOs
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    [JsonObject]
    public class ImportLanguageDTO
    {
        [JsonProperty("english_name")]
        public string LanguageName { get; set; }
    }
}
