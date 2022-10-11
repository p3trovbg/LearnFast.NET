using LearnFast.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFast.Data.Seeding
{
    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new string[]
            {
                "Development",
                "Business",
                "IT & Software",
                "Personal Development",
                "Design",
                "Marketing",
                "Lifestyle",
            };

            var list = new List<Category>();
            for (int i = 0; i < categories.Length; i++)
            {
                var category = categories[i];
                list.Add(new Category
                {
                    Name = category,
                });
            }

            await dbContext.Categories.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();
        }
    }
}
