namespace LearnFast.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using LearnFast.Data.Seeding.DTOs;
    using Newtonsoft.Json;

    public class CountrySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            var countries = await this.GetAllCountries();

            dbContext.Countries.AddRange(countries);

            dbContext.SaveChanges();
        }

        public async Task<List<Country>> GetAllCountries()
        {
            HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync($"https://restcountries.com/v3.1/all");

            var countriesModel = await response.Content.ReadFromJsonAsync<List<ImportCountryDTO>>();

            return countriesModel
                .Select(x => new Country
                {
                    Name = x.name.common,
                })
                .ToList();
        }
    }
}
