namespace LearnFast.Services.Data.CourseService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface ICourseService
    {
        Task AddCourseAsync(ImportCourseModel course);

        Task DeleteCourseByIdAsync(int courseId, string userId);

        Task UpdateAsync(ImportCourseModel model, string userId, int courseId);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<int> GetCountAsync();
    }
}
