namespace LearnFast.Services.Data
{
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Category;

    public interface ICategoryService
    {
        Task<CategoryViewModel> GetCategoryById(int id);
    }
}
