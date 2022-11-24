
namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.Category;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class CategoryServiceTests
    {
        private Mock<IDeletableEntityRepository<Category>> mockRepository;

        public CategoryServiceTests()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("LearnFast.Web.ViewModels"));
            this.mockRepository = new Mock<IDeletableEntityRepository<Category>>();
        }

        [Fact]
        public void ShouldReturnCategoryNameById()
        {
            this.mockRepository.Setup(r => r.AllAsNoTracking()).Returns(GetCategoryList().AsQueryable());

            var service = new CategoryService(this.mockRepository.Object, null);

            var category = service.GetCategoryName(1);
            var category1 = service.GetCategoryName(2);
            var category2 = service.GetCategoryName(3);

            Assert.Equal("IT", category);
            Assert.Equal("Software", category1);
            Assert.Equal("Design", category2);
        }

        [Fact]
        public async Task GetCategoryByInvalidIdShouldThrowsException()
        {
            this.mockRepository.Setup(r => r.AllAsNoTracking()).Returns(GetCategoryList().AsQueryable().BuildMock());

            var service = new CategoryService(this.mockRepository.Object, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetCategoryById<CategoryViewModel>(-5));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetCategoryById<CategoryViewModel>(20));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetCategoryById<CategoryViewModel>(0));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetCategoryById<CategoryViewModel>(1));

            Assert.Equal(GlobalExceptions.CategoryNullExceptionMessage, ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(4)]
        [InlineData(2)]
        public async Task GetCategoryByIdShouldReturnsCorrectCategory(int id)
        {
            var categoryList = GetCategoryList();
            this.mockRepository.Setup(r => r.AllAsNoTracking()).Returns(categoryList.AsQueryable().BuildMock());

            var service = new CategoryService(this.mockRepository.Object, null);
            var result = await service.GetCategoryById<CategoryViewModel>(id);

            var expectedCategory = categoryList.First(x => x.Id == id);
            Assert.Equal(expectedCategory.Id, result.Id);
            Assert.Equal(expectedCategory.Name, result.Name);
        }

        [Fact]
        public async Task GetAllShouldReturnsAllListWithCategories()
        {
            var categoryList = GetCategoryList();
            this.mockRepository.Setup(r => r.AllAsNoTracking()).Returns(categoryList.AsQueryable().BuildMock());

            var service = new CategoryService(this.mockRepository.Object, null);
            var result = await service.GetAllAsync<CategoryViewModel>();

            Assert.Equal(categoryList.Count(), result.Count());
            Assert.Equal(categoryList[0].Name, result.Where(x => x.Id == categoryList[0].Id).Select(x => x.Name).FirstOrDefault());
            Assert.Equal(categoryList[2].Name, result.Where(x => x.Id == categoryList[2].Id).Select(x => x.Name).FirstOrDefault());
            Assert.Equal(categoryList[3].Name, result.Where(x => x.Id == categoryList[3].Id).Select(x => x.Name).FirstOrDefault());
        }

        [Fact]
        public async Task GetCategoriesAsSelectListItems()
        {
            var categoryList = GetCategoryList();
            this.mockRepository.Setup(r => r.AllAsNoTracking()).Returns(categoryList.BuildMock());

            var service = new CategoryService(this.mockRepository.Object, null);

            var result = await service.GetCategoryList();

            Assert.NotNull(result.FirstOrDefault(x => x.Text == categoryList[0].Name));
            Assert.NotNull(result.FirstOrDefault(x => x.Value == categoryList[0].Id.ToString()));
            Assert.NotNull(result.FirstOrDefault(x => x.Text == categoryList[4].Name));
            Assert.NotNull(result.FirstOrDefault(x => x.Value == categoryList[4].Id.ToString()));
            Assert.Null(result.FirstOrDefault(x => x.Text == "BG"));
        }

        private static List<Category> GetCategoryList()
        {
            return new List<Category>
                                                        {
                                                            new Category { Id = 1, Name = "IT" },
                                                            new Category { Id = 2, Name = "Software" },
                                                            new Category { Id = 3, Name = "Design" },
                                                            new Category { Id = 4, Name = "Business" },
                                                            new Category { Id = 5, Name = "UI" },
                                                        };
        }
    }
}
