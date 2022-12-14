namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.LanguageService;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.Language;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class LanguageServiceTests : BaseServiceTests
    {
        private Mock<IDeletableEntityRepository<Language>> repository;

        public LanguageServiceTests()
        {
            this.repository = new Mock<IDeletableEntityRepository<Language>>();
        }

        [Fact]
        public async Task GetAllShouldReturnsAllLanguages()
        {
            var mockLanguages = GetLanguages();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(mockLanguages.AsQueryable().BuildMock());

            var service = new LanguageService(this.repository.Object);

            var result = await service.GetAllLanguageAsync<LanguageViewModel>();

            Assert.Equal(mockLanguages.Count(), result.Count());
            Assert.Equal(mockLanguages[0].Name, result.Where(x => x.Id == mockLanguages[0].Id).Select(x => x.Name).FirstOrDefault());
        }

        [Fact]
        public async Task GetLanguageByIdShouldReturnLanguageViewModelObject()
        {
            var mockLanguages = GetLanguages();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(mockLanguages.BuildMock());

            var service = new LanguageService(this.repository.Object);

            var result = await service.GetLanguageByIdAsync(1);

            Assert.Equal(mockLanguages[0].Name, result.Name);
            Assert.NotEqual(mockLanguages[1].Name, result.Name);
            Assert.NotEqual(mockLanguages[2].Name, result.Name);
        }

        [Fact]
        public async Task GetLanguageAsSelectListItem()
        {
            var mockLanguages = GetLanguages();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(mockLanguages.BuildMock());

            var service = new LanguageService(this.repository.Object);

            var result = await service.GetLanguagesAsSelectListItem();

            Assert.NotNull(result.FirstOrDefault(x => x.Text == mockLanguages[0].Name));
            Assert.NotNull(result.FirstOrDefault(x => x.Value == mockLanguages[0].Id.ToString()));
            Assert.Null(result.FirstOrDefault(x => x.Text == "BG"));
        }

        [Fact]
        public async Task GetLanguageByInvalidIdShouldThrowException()
        {
            var mockLanguages = GetLanguages();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(mockLanguages.BuildMock());

            var service = new LanguageService(this.repository.Object);

            var result = await service.GetLanguageByIdAsync(1);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetLanguageByIdAsync(10));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetLanguageByIdAsync(-1));

            Assert.Equal(GlobalExceptions.LanguageNullExceptionMessage, ex.Message);
        }

        public static List<Language> GetLanguages()
        {
            return new List<Language>
                                        {
                                            new Language { Id = 1, Name = "Bulgarian" },
                                            new Language { Id = 2, Name = "English" },
                                            new Language { Id = 3, Name = "Spanish" },
                                            new Language { Id = 4, Name = "Turkey" },
                                            new Language { Id = 5, Name = "Russian" },
                                        };
        }
    }
}
