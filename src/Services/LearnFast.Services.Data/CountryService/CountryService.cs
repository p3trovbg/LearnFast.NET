namespace LearnFast.Services.Data.CountryService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CountryService : ICountryService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountryService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.countryRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByAlphabeticalAsync<T>()
        {
            return await this.countryRepository.All().OrderBy(x => x.Name).To<T>().ToListAsync();
        }
    }
}
