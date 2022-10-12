namespace LearnFast.Services.Data
{
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface ICourseService
    {
        Task AddCourse(ImportCourseModel course);

        Task DeleteCourseById(int courseId, string userId);
    }
}
