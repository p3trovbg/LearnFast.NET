namespace LearnFast.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

            var languages = this.GetAllLanguages();

            await dbContext.Languages.AddRangeAsync(languages);

            await dbContext.SaveChangesAsync();
        }

        public List<Language> GetAllLanguages()
        {
            using (StreamReader r = new StreamReader("path.json"))
            {
                string json = r.ReadToEnd();
                List<ImportLanguageDTO> items = JsonConvert.DeserializeObject<List<ImportLanguageDTO>>(json);

                return items
                    .Select(x => new Language
                    {
                        Name = x.LanguageName,
                    })
                      .ToList();
            }
        }
    }
}
