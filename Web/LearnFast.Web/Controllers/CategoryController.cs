namespace LearnFast.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Services.Data;
    using LearnFast.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAll<CategoryViewModel>();

            var model = new CategoryListViewModel { Categories = categories };

            return this.View(model);
        }
    }
}
