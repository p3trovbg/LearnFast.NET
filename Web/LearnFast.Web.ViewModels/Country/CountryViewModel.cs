namespace LearnFast.Web.ViewModels.Country
{
    using LearnFast.Data.Models;

    using LearnFast.Services.Mapping;

    public class CountryViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
