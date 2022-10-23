namespace LearnFast.Services.Data
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
        private readonly IMapper mapper;

        public LanguageService(
            IDeletableEntityRepository<Language> languageRepository,
            IMapper mapper)
        {
            this.languageRepository = languageRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAllLanguageAsync<T>()
        {
            return await this.languageRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<LanguageViewModel> GetLanguageByIdAsync(int id)
        {
            var language = await this.languageRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (language == null)
            {
                throw new NullReferenceException(GlobalExceptions.LanguageNullExceptionMessage);
            }

            var model = this.mapper.Map<LanguageViewModel>(language);

            return model;
        }

        public async Task<IEnumerable<SelectListItem>> GetLanguageListAsync()
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
