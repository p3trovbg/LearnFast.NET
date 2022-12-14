namespace LearnFast.Services.Data.LanguageService
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
    using LearnFast.Web.ViewModels.Language;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class LanguageService : ILanguageService
    {
        private readonly IDeletableEntityRepository<Language> languageRepository;

        public LanguageService(
            IDeletableEntityRepository<Language> languageRepository)
        {
            this.languageRepository = languageRepository;
        }

        public async Task<IEnumerable<T>> GetAllLanguageAsync<T>()
        {
            return await this.languageRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<LanguageViewModel> GetLanguageByIdAsync(int id)
        {
            var language = await this.languageRepository
                .AllAsNoTracking()
                .To<LanguageViewModel>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (language == null)
            {
                throw new NullReferenceException(GlobalExceptions.LanguageNullExceptionMessage);
            }

            return language;
        }

        public async Task<IEnumerable<SelectListItem>> GetLanguagesAsSelectListItem()
        {
            var list = await this.GetAllLanguageAsync<LanguageViewModel>();
            return list.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
        }
    }
}
