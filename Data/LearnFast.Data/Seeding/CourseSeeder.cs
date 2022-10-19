namespace LearnFast.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using Newtonsoft.Json;

    public class CourseSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var courses = this.GetCourses();

            await dbContext.Courses.AddRangeAsync(courses);

            await dbContext.SaveChangesAsync();
        }

        private List<Course> GetCourses()
        {
            using (StreamReader r = new StreamReader("C:\\Users\\Georgi Petrov\\source\\LearnFastProject\\LearnFastProject\\Data\\LearnFast.Data\\Seeding\\coursesJson.json"))
            {
                string json = r.ReadToEnd();
                List<Course> items = JsonConvert.DeserializeObject<List<Course>>(json);

                return items;
            }
        }
    }
}
