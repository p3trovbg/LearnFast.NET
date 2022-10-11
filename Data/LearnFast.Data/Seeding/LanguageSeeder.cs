﻿namespace LearnFast.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using LearnFast.Data.Seeding.DTOs;
    using Newtonsoft.Json;

    public class LanguageSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Languages.Any())
            {
                return;
            }

            var languages = await this.GetAllLanguages();

            dbContext.Languages.AddRange(languages);

            dbContext.SaveChanges();
        }

        public async Task<List<Language>> GetAllLanguages()
        {
            using (StreamReader r = new StreamReader("filepath"))
            {
                string json = r.ReadToEnd();
                List<ImportLanguageDTO> items = JsonConvert.DeserializeObject<List<ImportLanguageDTO>>(json);

                return items
                    .Select(x => new Language
                    {
                        Nationality = x.LanguageName,
                    })
                      .ToList();
            }
        }
    }
}
