namespace LearnFast.Web.Controllers
{
    using System.Threading.Tasks;

    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(
            ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllAsync<CategoryViewModel>();

            var model = new CategoryListViewModel { Categories = categories };

            return this.View(model);
        }

        public IActionResult Empty()
        {
            return this.View();
        }
    }
}
