namespace LearnFast.Services.Data.CourseService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface ICourseService
    {
        IQueryable<T> GetAllAsQueryAble<T>();

        Task AddCourseAsync(ImportCourseModel course);

        Task DeleteCourseByIdAsync(int courseId, string userId);

        Task UpdateAsync(ImportCourseModel model, string userId);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<int> GetCountAsync();
    }
}