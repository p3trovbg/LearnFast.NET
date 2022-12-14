namespace LearnFast.Services.Data.CategoryService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    {
        Task<T> GetCategoryById<T>(int id);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<SelectListItem>> GetCategoryList();

        string GetCategoryName(int? id);
    }
}
