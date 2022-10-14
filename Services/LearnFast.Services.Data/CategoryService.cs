namespace LearnFast.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Category;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(
            IDeletableEntityRepository<Category> categoryRepository,
            IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            var category = await this.categoryRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new NullReferenceException(GlobalExceptions.CategoryNullExceptionMessage);
            }

            return this.mapper.Map<CategoryViewModel>(category);
        }
    }
}
