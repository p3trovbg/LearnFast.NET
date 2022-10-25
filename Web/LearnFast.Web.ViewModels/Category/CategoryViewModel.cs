namespace LearnFast.Web.ViewModels.Category
{
    using System.Collections.Generic;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.Course;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<BaseCourseViewModel> Courses { get; set; }
    }
}
