namespace LearnFast.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;
    using System.Linq;

    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Country;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Language;

    public class IndexViewModel
    {
        public int UsersCount => this.Users.Count();

        public int CountriesCount => this.Countries.Count();

        public int CategoriesCount => this.Categories.Count();

        public int LanguagesCount => this.Languages.Count();

        public int CoursesCount { get; set; }

        public int ReviewsCount { get; set; }

        public IEnumerable<LanguageViewModel> Languages { get; set; }

        public IEnumerable<BaseCourseViewModel> Courses { get; set; }

        public IEnumerable<CountryViewModel> Countries { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public IEnumerable<BaseUserViewModel> Users { get; set; }
    }
}
