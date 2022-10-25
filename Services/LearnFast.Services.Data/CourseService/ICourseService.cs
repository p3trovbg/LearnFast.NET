namespace LearnFast.Services.Data.CourseService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;

    public interface ICourseService
    {
        IQueryable<T> GetAllAsQueryAble<T>();

        Task GetAllWithFilter(FilterViewModel model);

        Task<int> AddCourseAsync(ImportCourseModel course);

        Task DeleteCourseByIdAsync(int courseId, string userId);

        Task UpdateAsync(ImportCourseModel model, string userId);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<int> GetCountAsync();
    }
}