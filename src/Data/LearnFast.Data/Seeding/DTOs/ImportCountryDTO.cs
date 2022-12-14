namespace LearnFast.Data.Seeding.DTOs
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    [JsonObject]
    public class ImportCountryDTO
    {
        public Name name { get; set; }

        public string[] Tld { get; set; }

        public string cca2 { get; set; }

        public string ccn3 { get; set; }

        public string cca3 { get; set; }

        public string cioc { get; set; }

        public bool independent { get; set; }

        public string status { get; set; }

        public bool unMember { get; set; }

        public Currencies currencies { get; set; }

        public Idd idd { get; set; }

        public string[] capital { get; set; }

        public string[] altSpellings { get; set; }

        public string region { get; set; }

        public string subregion { get; set; }

        public Languages languages { get; set; }

        public Translations translations { get; set; }

        public decimal[] latlng { get; set; }

        public Demonyms demonyms { get; set; }

        public bool landlocked { get; set; }

        public string[] borders { get; set; }

        public decimal area { get; set; }

        public string[] callingCodes { get; set; }

        public string flag { get; set; }
    }

    public class Name
    {
        public string common { get; set; }

        public string official { get; set; }

        public Native native { get; set; }
    }

    public class Native
    {
        public Bar bar { get; set; }
    }

    public class Bar
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Currencies
    {
        public EUR EUR { get; set; }
    }

    public class EUR
    {
        public string name { get; set; }

        public string symbol { get; set; }
    }

    public class Idd
    {
        public string root { get; set; }

        public string[] suffixes { get; set; }
    }

    [JsonObject("languages")]
    public class Languages
    {
        [JsonPropertyName(nameof(ImportCountryDTO.cca2))]
        public string bar { get; set; }
    }

    public class Translations
    {
        public Cym cym { get; set; }

        public Deu deu { get; set; }

        public Fra fra { get; set; }

        public Hrv hrv { get; set; }

        public Ita ita { get; set; }

        public Jpn jpn { get; set; }

        public Nld nld { get; set; }

        public Por por { get; set; }

        public Rus rus { get; set; }

        public Spa spa { get; set; }
    }

    public class Cym
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Deu
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Fra
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Hrv
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Ita
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Jpn
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Nld
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Por
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Rus
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Spa
    {
        public string official { get; set; }

        public string common { get; set; }
    }

    public class Demonyms
    {
        public Fra1 fra { get; set; }

        public Spa1 spa { get; set; }
    }

    public class Fra1
    {
        public string f { get; set; }

        public string m { get; set; }
    }

    public class Spa1
    {
        public string f { get; set; }

        public string m { get; set; }
    }
}
