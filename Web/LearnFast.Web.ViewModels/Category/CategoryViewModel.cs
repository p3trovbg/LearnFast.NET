namespace LearnFast.Web.ViewModels.Category
{
    using LearnFast.Data.Models;

    using LearnFast.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
