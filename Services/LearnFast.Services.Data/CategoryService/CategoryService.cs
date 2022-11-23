namespace LearnFast.Services.Data.CategoryService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await categoryRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<T> GetCategoryById<T>(int id)
        {
            var category = await categoryRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new NullReferenceException(GlobalExceptions.CategoryNullExceptionMessage);
            }

            return mapper.Map<T>(category);
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoryList()
        {
            var categories = await GetAllAsync<CategoryViewModel>();

            return categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
        }

        public string GetCategoryName(int? id)
        {
            return this.categoryRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == id).Name;
        }
    }
}
