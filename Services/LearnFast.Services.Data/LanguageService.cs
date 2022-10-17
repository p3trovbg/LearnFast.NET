﻿namespace LearnFast.Services.Data
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

        public IEnumerable<T> GetAllLanguage<T>()
        {
            return this.languageRepository.AllAsNoTracking().To<T>().ToList();
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
    }
}
