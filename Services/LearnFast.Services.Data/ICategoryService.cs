namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Category;

    public interface ICategoryService
    {
        Task<CategoryViewModel> GetCategoryById(int id);

        Task<IEnumerable<T>> GetAll<T>();
    }
}
