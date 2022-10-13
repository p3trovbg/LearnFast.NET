namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface ICourseService
    {
        Task AddCourse(ImportCourseModel course);

        Task DeleteCourseById(int courseId, string userId);

        Task UpdateAsync(ImportCourseModel model, string userId, int courseId);

        Task<IEnumerable<T>> GetAll<T>();

        Task<int> Count();
    }
}
