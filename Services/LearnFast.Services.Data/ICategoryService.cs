namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    {
        Task<CategoryViewModel> GetCategoryById(int id);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<SelectListItem>> GetCategoryList();

        string GetCategoryName(int? id);
    }
}
