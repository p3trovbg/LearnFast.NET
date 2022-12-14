namespace LearnFast.Web.ViewModels.Language
{
    using LearnFast.Data.Models;

    using LearnFast.Services.Mapping;

    public class LanguageViewModel : IMapFrom<Language>, IMapTo<LanguageViewModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
