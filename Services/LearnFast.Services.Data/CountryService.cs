﻿namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class CountryService : ICountryService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountryService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.countryRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetAllOrderByAlphabetical<T>()
        {
            return this.countryRepository.All().OrderBy(x => x.Name).To<T>().ToList();

        }
    }
}
